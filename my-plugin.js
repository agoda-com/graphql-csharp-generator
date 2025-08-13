const { visit, Kind, isNonNullType, isListType, getNamedType } = require('graphql');
const path = require('path');

const toPascalCase = (str) => {
  if (!str || typeof str !== 'string') return '';
  return str.charAt(0).toUpperCase() + str.slice(1);
};

module.exports = {
  plugin(schema, documents, config, { outputFile }) {
    // Helper functions
    
    const convertGraphQLTypeToCSharp = (type) => {
      console.log('=== Type Conversion Debug ===');
      console.log('Input type:', JSON.stringify(type, null, 2));
      console.log('Type kind:', type.kind);
      
      let currentType = type;
      let isRequired = false;
      let isList = false;
      
      // Handle NonNullType (required fields marked with !)
      if (currentType.kind === 'NonNullType') {
        console.log('Found NonNullType (required field)');
        isRequired = true;
        currentType = currentType.type;
        console.log('After unwrapping NonNull:', currentType);
      }
      
      // Handle ListType (arrays marked with [])
      if (currentType.kind === 'ListType') {
        console.log('Found ListType (array field)');
        isList = true;
        currentType = currentType.type;
        console.log('After unwrapping List:', currentType);
        
        // Handle NonNullType inside ListType  
        if (currentType.kind === 'NonNullType') {
          currentType = currentType.type;
          console.log('After unwrapping inner NonNull:', currentType);
        }
      }
      
      // Get the actual type name
      let typeName;
      if (currentType.kind === 'NamedType') {
        typeName = currentType.name.value;
      } else {
        console.log('Unexpected type structure:', currentType);
        typeName = 'object'; // fallback
      }
      
      console.log('Final type name:', typeName);
      console.log('isRequired:', isRequired);
      console.log('isList:', isList);
      
      // Map GraphQL scalar types to C# types
      let csharpType;
      switch (typeName) {
        case 'String': csharpType = 'string'; break;
        case 'Int': csharpType = 'int'; break;
        case 'Float': csharpType = 'double'; break;
        case 'Boolean': csharpType = 'bool'; break;
        case 'Date': csharpType = 'DateTime'; break;
        case 'DateTime': csharpType = 'DateTime'; break;
        case 'ID': csharpType = 'string'; break;
        default: 
          console.log('Unknown type, using PascalCase:', typeName);
          csharpType = toPascalCase(typeName); 
          break;
      }
      
      // Handle nullability and lists
      if (isList) {
        const innerType = (csharpType === 'string' || isRequired) ? csharpType : `${csharpType}?`;
        csharpType = `List<${innerType}>`;
      } else if (!isRequired && csharpType !== 'string') {
        csharpType += '?';
      }
      
      console.log('Final C# type:', csharpType);
      console.log('=== End Debug ===\n');
      
      return csharpType;
    };

    // Extract operation info from the GraphQL document
    const operations = [];
    
    documents.forEach(doc => {
      visit(doc.document, {
        OperationDefinition: (node) => {
          operations.push({
            name: node.name?.value,
            type: node.operation,
            variables: node.variableDefinitions || [],
            document: doc
          });
        }
      });
    });

    if (operations.length === 0) {
      return 'No operations found';
    }

    const operation = operations[0];
    const operationName = operation.name;
    const operationType = operation.type;
    const variables = operation.variables;
    
    // Determine the C# class name based on operation type
    const operationClassName = operationType === 'mutation' ? 'Mutation' : 'Query';
    
    // Extract the raw GraphQL query
    const rawQuery = operation.document.rawSDL || operation.document.content;
    
    // Generate constructor parameters
    
    const constructorParams = variables.map(variable => {
      const name = variable.variable.name.value;
      
      const csharpType = convertGraphQLTypeToCSharp(variable.type);
      return `${csharpType} ${name}`;
    });
    
    // Add the result processor parameter
    constructorParams.push('IResultProcessor<Data> resultProcessor = null');
    const constructorParamsString = constructorParams.join(', ');
    
    // Generate constructor body (property assignments)
    const constructorBody = variables.map(variable => {
      const name = variable.variable.name.value;
      const pascalName = toPascalCase(name);
      return `            ${pascalName} = ${name};`;
    }).join('\n');
    
    // Generate properties
    const properties = variables.map(variable => {
      const name = variable.variable.name.value;
      const csharpType = convertGraphQLTypeToCSharp(variable.type);
      const pascalName = toPascalCase(name);
      return `        public ${csharpType} ${pascalName} { get; }`;
    }).join('\n');
    
    // Generate Variables dictionary
    const variablesDict = variables.length > 0 ? variables.map(variable => {
      const name = variable.variable.name.value;
      const pascalName = toPascalCase(name);
      const csharpType = convertGraphQLTypeToCSharp(variable.type);
      
      // Handle DateTime formatting for Date types
      let value = pascalName;
      if (csharpType.includes('DateTime')) {
        value = `${pascalName}.ToString("yyyy-MM-dd")`;
      }
      
      return `            { "${name}", ${value} }`;
    }).join(',\n') : '';

    // Parse selection set to generate response classes
    const responseClasses = new Set();
    const dataClassProperties = [];
    
    // Helper function to get field type from schema
    const getFieldTypeFromSchema = (parentType, fieldName) => {
      try {
        if (!parentType || !parentType.getFields) {
          console.log(`Parent type ${parentType?.name || 'unknown'} has no getFields method`);
          return null;
        }
        
        const fields = parentType.getFields();
        const field = fields[fieldName];
        
        if (!field) {
          console.log(`Field ${fieldName} not found in type ${parentType.name}`);
          return null;
        }
        
        console.log(`Found field ${fieldName} in ${parentType.name}:`, field.type);
        return field.type;
      } catch (error) {
        console.log(`Error getting field type for ${fieldName}:`, error.message);
        return null;
      }
    };
    
    // Helper function to convert GraphQL schema types directly to C#
    const convertGraphQLTypeFromSchema = (graphqlType) => {
      console.log('Converting schema type:', graphqlType.toString());
      
      let currentType = graphqlType;
      let isRequired = false;
      let isList = false;
      
      // Handle NonNull types (required fields)
      if (isNonNullType(currentType)) {
        isRequired = true;
        currentType = currentType.ofType;
      }
      
      // Handle List types 
      if (isListType(currentType)) {
        isList = true;
        currentType = currentType.ofType;
        
        // Handle NonNull inside List
        if (isNonNullType(currentType)) {
          currentType = currentType.ofType;
        }
      }
      
      // Get the named type
      const namedType = getNamedType(currentType);
      const typeName = namedType.name;
      
      console.log('Schema type details:', {
        originalType: graphqlType.toString(),
        typeName,
        isRequired,
        isList
      });
      
      // Map GraphQL types to C#
      let csharpType;
      switch (typeName) {
        case 'String': csharpType = 'string'; break;
        case 'Int': csharpType = 'int'; break;
        case 'Float': csharpType = 'double'; break;
        case 'Boolean': csharpType = 'bool'; break;
        case 'Date': csharpType = 'DateTime'; break;
        case 'DateTime': csharpType = 'DateTime'; break;
        case 'ID': csharpType = 'string'; break;
        default:
          // Custom types - use PascalCase
          csharpType = toPascalCase(typeName);
          break;
      }
      
      // Handle lists and nullability
      if (isList) {
        const innerType = (csharpType === 'string' || isRequired) ? csharpType : `${csharpType}?`;
        csharpType = `List<${innerType}>`;
      } else if (!isRequired && csharpType !== 'string') {
        csharpType += '?';
      }
      
      console.log('Final C# type from schema:', csharpType);
      return csharpType;
    };
    
    // Helper function to parse selection set recursively
    const parseSelectionSet = (selectionSet, parentTypeName = null, parentType = null) => {
      if (!selectionSet || !selectionSet.selections) return [];
      
      const properties = [];
      
      selectionSet.selections.forEach(selection => {
        console.log("selection", selection);
        
        if (selection.kind === 'Field') {
          const fieldName = selection.name.value;
          const pascalFieldName = toPascalCase(fieldName);
          
          // Get actual field type from schema
          let csharpType = 'string'; // fallback
          let fieldType = null;
          
          if (parentType) {
            fieldType = getFieldTypeFromSchema(parentType, fieldName);
            if (fieldType) {
              // Convert the actual GraphQL type to C# using our existing function
              csharpType = convertGraphQLTypeFromSchema(fieldType);
            } else {
              // Fallback to field name inference
              csharpType = inferCSharpTypeFromFieldName(fieldName);
            }
          } else {
            // Fallback to field name inference when no parent type
            csharpType = inferCSharpTypeFromFieldName(fieldName);
          }
          
          // If this field has a selection set, it's a complex type
          if (selection.selectionSet) {
            const nestedClassName = pascalFieldName;
            
            // Get the nested type from schema
            let nestedType = null;
            if (fieldType) {
              nestedType = getNamedType(fieldType);
              console.log(`Nested type for ${fieldName}:`, nestedType?.name);
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
            } else {
              // Fallback - assume it's a list if field name suggests it (plural form)
              csharpType = fieldName.endsWith('s') || fieldName.toLowerCase().includes('list') 
                ? `List<${nestedClassName}>` 
                : nestedClassName;
            }
            
            // Parse nested selection set
            const nestedProperties = parseSelectionSet(selection.selectionSet, nestedClassName, nestedType);
            
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
    
    // Helper function to infer C# type from field name (basic heuristics)
    const inferCSharpTypeFromFieldName = (fieldName) => {
      console.log(fieldName);
      
      if (fieldName.toLowerCase().includes('id')) return 'string';
      if (fieldName.toLowerCase().includes('date') || fieldName.toLowerCase().includes('when')) return 'DateTime';
      if (fieldName.toLowerCase().includes('count') || fieldName.toLowerCase().includes('used') || fieldName.toLowerCase().includes('allotment')) return 'int?';
      if (fieldName.toLowerCase().includes('by')) return 'string';
      if (fieldName.toLowerCase().includes('name') || fieldName.toLowerCase().includes('title')) return 'string';
      return 'string'; // default fallback
    };
    
    // Helper function to generate nested class code
    const generateNestedClass = (className, properties) => {
      const classProperties = properties.map(prop => 
        `        
        
        [JsonProperty("${prop.name}")]
        public ${prop.type} ${prop.pascalName} { get; set; }`
      ).join('\n');
      
      return `    
    /// <summary>Inner Model</summary> 
    public sealed class ${className} 
    {
        ${classProperties}
    }`;
    };
    
    // Parse the main query operation
    let mainDataProperties = [];
    operations.forEach(op => {
      visit(op.document.document, {
        OperationDefinition: (node) => {
          if (node.selectionSet) {
            // Get the root operation type from schema
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
              console.log(`Using root type for ${node.operation}:`, rootType?.name);
            } catch (error) {
              console.log('Error getting root type from schema:', error.message);
            }
            
            mainDataProperties = parseSelectionSet(node.selectionSet, 'Root', rootType);
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
        private const string _query = @"${rawQuery.replace(/"/g, '""')}";

        public ${operationClassName}(${constructorParamsString}) : base(resultProcessor)
        {
${constructorBody}
        }
        
${properties}
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