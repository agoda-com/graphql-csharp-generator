const { visit, Kind, isNonNullType, isListType, getNamedType, typeFromAST } = require('graphql');
const path = require('path');

const toPascalCase = (str) => {
  if (!str || typeof str !== 'string') return '';
  return str.charAt(0).toUpperCase() + str.slice(1);
};

const getOperationCSharpClassName = (operation) => {
  switch (operation.type) {
    case 'query': return 'Query';
    case 'mutation': return 'Mutation';
    case 'subscription': return 'Subscription';
  }
};

const convertGraphQLTypeToCSharp = (input, schema = null) => {
  let schemaType;
  
  // Handle both AST types and schema types
  if (input && typeof input === 'object' && input.kind) {
    // This is an AST type, convert to schema type
    if (!schema) {
      throw new Error('Schema is required when converting AST types');
    }
    schemaType = typeFromAST(schema, input);
    
    if (!schemaType) {
      // Fallback to old method if conversion fails
      const typeName = input.kind === 'NamedType' ? input.name.value : 'object';
      return toPascalCase(typeName);
    }
  } else {
    // This is already a schema type
    schemaType = input;
  }
  
  let currentType = schemaType;
  let isRequired = false;
  let isList = false;
  let isInnerRequired = false;
  
  // Handle NonNull types (required fields marked with !)
  if (isNonNullType(currentType)) {
    isRequired = true;
    currentType = currentType.ofType;
  }
  
  // Handle List types (arrays marked with [])
  if (isListType(currentType)) {
    isList = true;
    currentType = currentType.ofType;
    
    // Handle NonNull inside List
    if (isNonNullType(currentType)) {
      isInnerRequired = true;
      currentType = currentType.ofType;
    }
  }
  
  const typeName = getNamedType(currentType).name;
  
  // Map GraphQL scalar types to C# types
  let csharpType;
  switch (typeName) {
    case 'String': csharpType = 'string'; break;
    case 'Int': csharpType = 'int'; break;
    case 'BigInt': csharpType = 'long'; break;
    case 'Long': csharpType = 'long'; break;
    case 'Float': csharpType = 'double'; break;
    case 'Boolean': csharpType = 'bool'; break;
    case 'Date': csharpType = 'DateTime'; break;
    case 'DateTime': csharpType = 'DateTime'; break;
    case 'ID': csharpType = 'string'; break;
    default: 
      csharpType = toPascalCase(typeName); 
      break;
  }
  
  // Handle nullability and lists
  if (isList) {
    const innerType = (csharpType === 'string' || isInnerRequired) ? csharpType : `${csharpType}?`;
    csharpType = `List<${innerType}>`;
    csharpType += isRequired ? '' : '?';
  } else if (!isRequired && csharpType !== 'string') {
    csharpType += '?';
  }
  
  return csharpType;
};

const getOperationsDefinitions = (documents) => {
  const operations = [];
  documents.forEach(doc => {
    visit(doc.document, {
      OperationDefinition: (node) => {
        operations.push({
          name: node.name?.value,
          type: node.operation,
          variables: node.variableDefinitions || [],
          document: doc,
        });
      }
    });
  });
  
  return operations;
};

// Helper function to get field type from schema
const getFieldTypeFromSchema = (parentType, fieldName) => {
  try {
    if (!parentType || !parentType.getFields) {
      // console.log(`Parent type ${parentType?.name || 'unknown'} has no getFields method`);
      return null;
    }
    
    const fields = parentType.getFields();
    const field = fields[fieldName];
    
    if (!field) {
      // console.log(`Field ${fieldName} not found in type ${parentType.name}`);
      return null;
    }
    
    return field.type;
  } catch (error) {
    // console.log(`Error getting field type for ${fieldName}:`, error.message);
    return null;
  }
};

// Helper function to generate nested class code
const generateNestedClass = (className, properties) => {
  const classProperties = properties.map(prop => 
    `        
    [JsonProperty("${prop.name}")]
    public ${prop.type} ${prop.pascalName} { get; set; }`
  ).join('\n');
  
  return `    
  public sealed class ${className} 
  {${classProperties}
  }`;
};

// Helper function to parse selection set recursively
const parseSelectionSet = (selectionSet, parentType = null, responseClasses = new Set()) => {
  if (!selectionSet || !selectionSet.selections) return [];
  const properties = [];
  
  selectionSet.selections.forEach(selection => {
    if (selection.kind === 'Field') {
      const fieldName = selection.name.value;
      const pascalFieldName = toPascalCase(fieldName);
      
      // Get actual field type from schema
      let csharpType = 'string'; // fallback
      let fieldType = null;
      
      if (parentType) {
        fieldType = getFieldTypeFromSchema(parentType, fieldName);            
        if (fieldType) {
          // Convert the actual GraphQL schema type to C#
          csharpType = convertGraphQLTypeToCSharp(fieldType);
        }
      }
      
      // If this field has a selection set, it's a complex type
      if (selection.selectionSet) {
        const nestedClassName = pascalFieldName;
        
        // Get the nested type from schema
        let nestedType = null;
        if (fieldType) {
          nestedType = getNamedType(fieldType);
        }
        
        // Determine if it's a list and set the correct C# type
        if (fieldType) {
          let tempType = fieldType;
          let isList = false;
          
          // Unwrap NonNull
          if (isNonNullType(tempType)) {
            tempType = tempType.ofType;
          }
          
          // Check if it's a list
          if (isListType(tempType)) {
            isList = true;
          }
          
          csharpType = isList ? `List<${nestedClassName}>` : nestedClassName;
        }
        
        // Parse nested selection set
        const nestedProperties = parseSelectionSet(selection.selectionSet, nestedType, responseClasses);
        
        // Generate the nested class
        const nestedClass = generateNestedClass(nestedClassName, nestedProperties);
        responseClasses.add(nestedClass);
      }
      
      properties.push({
        name: fieldName,
        pascalName: pascalFieldName,
        type: csharpType
      });
    }
  });
  
  return properties;
};

module.exports = {
  plugin(schema, documents, config, { outputFile }) {  
    const operationsDefinitions = getOperationsDefinitions(documents);
    
    if (operationsDefinitions.length === 0) {
      return 'No operations found';
    }

    const operation = operationsDefinitions[0];
    const operationName = operation.name;
    const variables = operation.variables;
    const rawQuery = operation.document.rawSDL.replace(/"/g, '""');

    const operationClassName = getOperationCSharpClassName(operation);
    
    // Generate constructor parameters
    const constructorParams = variables.map(variable => {
      const name = variable.variable.name.value;
      const csharpType = convertGraphQLTypeToCSharp(variable.type, schema);
      return `${csharpType} ${name}`;
    });
    
    const constructorParamsString = [
      ...constructorParams,
      'IResultProcessor<Data> resultProcessor = null'
    ].join(', ');
    
    // Generate constructor body (property assignments)
    const constructorBody = variables.map(variable => {
      const name = variable.variable.name.value;
      const pascalName = toPascalCase(name);
      return `            ${pascalName} = ${name};`;
    }).join('\n');
    
    // Generate properties
    const declareProperties = variables.map(variable => {
      const name = variable.variable.name.value;
      const csharpType = convertGraphQLTypeToCSharp(variable.type, schema);
      const pascalName = toPascalCase(name);
      return `        public ${csharpType} ${pascalName} { get; }`;
    }).join('\n');
    
    // Generate Variables dictionary
    const variablesDict = variables.length > 0 ? variables.map(variable => {
      const name = variable.variable.name.value;
      const pascalName = toPascalCase(name);
      const csharpType = convertGraphQLTypeToCSharp(variable.type, schema);
      
      // Handle DateTime formatting for Date types
      let value = pascalName;
      if (csharpType.includes('DateTime')) {
        value = `${pascalName}.ToString("yyyy-MM-dd")`;
      }
      
      return `            { "${name}", ${value} }`;
    }).join(',\n') : '';

    // Parse selection set to generate response classes
    const responseClasses = new Set();
    
    // Parse the main query operation
    let mainDataProperties = [];
    operationsDefinitions.forEach(op => {
      visit(op.document.document, {
        OperationDefinition: (node) => {
          if (node.selectionSet) {
            let rootType = null;
            try {
              switch (node.operation) {
                case 'query':
                  rootType = schema.getQueryType();
                  break;
                case 'mutation':
                  rootType = schema.getMutationType();
                  break;
                case 'subscription':
                  rootType = schema.getSubscriptionType();
                  break;
              }
            } catch (error) {
              // console.log('Error getting root type from schema:', error.message);
            }
            mainDataProperties = parseSelectionSet(node.selectionSet, rootType, responseClasses);
          }
        }
      });
    });
    
    // Generate Data class properties
    const dataProperties = mainDataProperties.map(prop => 
      `        
        [JsonProperty("${prop.name}")]
        public ${prop.type} ${prop.pascalName} { get; set; }`
    ).join('\n');

    return `using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.${operationName}
{
    public partial class ${operationClassName} : QueryBase<Data>
    {
        private const string _query = @"${rawQuery}";

${declareProperties}

        public ${operationClassName}(${constructorParamsString}) : base(resultProcessor)
        {
${constructorBody}
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
${variablesDict}
        };
    }

    public sealed class Data
    {${dataProperties}
    }
${Array.from(responseClasses).join('\n')}
}
`;
  }
};