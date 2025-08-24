import { 
  visit,
  isNonNullType, 
  isListType, 
  getNamedType, 
  typeFromAST,
  GraphQLSchema,
  SelectionSetNode,
  VariableDefinitionNode,
  GraphQLType,
  GraphQLObjectType,
  isObjectType,
  isEnumType,
  TypeNode
} from 'graphql';
import { PluginFunction, Types } from '@graphql-codegen/plugin-helpers'
import { isScalarType } from './graphqlUtils';
import { mapGraphQLTypeToCSharp } from './naming';

// Helper function to check if a type is an enum
const isEnumTypeFromSchema = (schema: GraphQLSchema, typeName: string): boolean => {
  try {
    const graphqlType = schema.getType(typeName);
    return !!(graphqlType && isEnumType(graphqlType));
  } catch (error) {
    return false;
  }
};

// Plugin configuration interface
interface AgodaCSharpCodegenConfig {
  namespace?: string;
}

// Type definitions
interface Operation {
  name: string | undefined;
  type: 'query' | 'mutation' | 'subscription';
  variables: readonly VariableDefinitionNode[];
  document: Types.DocumentFile;
}

interface Property {
  name: string;
  pascalName: string;
  type: string;
}

const toPascalCase = (str: string): string => {
  if (!str || typeof str !== 'string') return '';
  return str.charAt(0).toUpperCase() + str.slice(1);
};

const getOperationCSharpClassName = (operation: Operation): string => {
  switch (operation.type) {
    case 'query': return 'Query';
    case 'mutation': return 'Mutation';
    case 'subscription': return 'Subscription';
  }
};

const convertGraphQLTypeToCSharp = (input: GraphQLType | TypeNode, schema: GraphQLSchema | null = null): string => {
  let schemaType: GraphQLType;
  
  // Handle both AST types and schema types
  if (input && typeof input === 'object' && 'kind' in input) {
    // This is an AST type, convert to schema type
    if (!schema) {
      throw new Error('Schema is required when converting AST types');
    }
    const convertedType = typeFromAST(schema, input as TypeNode);
    
    if (!convertedType) {
      // Fallback to old method if conversion fails
      const typeName = input.kind === 'NamedType' ? (input as any).name.value : 'object';
      return toPascalCase(typeName);
    }
    schemaType = convertedType;
  } else {
    // This is already a schema type
    schemaType = input as GraphQLType;
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
  let csharpType = mapGraphQLTypeToCSharp(typeName);
  
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

const getOperationsDefinitions = (documents: Types.DocumentFile[]): Operation[] => {
  const operations: Operation[] = [];
  documents.forEach(doc => {
    if (!doc.document) return;
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
const getFieldTypeFromSchema = (parentType: GraphQLType | null, fieldName: string): GraphQLType | null => {
  try {
    if (!parentType || !isObjectType(parentType)) {
      // console.log(`Parent type ${parentType?.toString() || 'unknown'} is not an object type`);
      return null;
    }
    
    const fields = parentType.getFields();
    const field = fields[fieldName];
    
    if (!field) {
      // console.log(`Field ${fieldName} not found in type ${parentType.name}`);
      return null;
    }
    
    return field.type;
  } catch (error: any) {
    // console.log(`Error getting field type for ${fieldName}:`, error.message);
    return null;
  }
};

// Note: Enum generation is handled by the shared types plugin

// Unified function to generate C# class from GraphQL type
const generateClassFromGraphQLType = (
  schema: GraphQLSchema, 
  typeName: string, 
  processedTypes: Set<string> = new Set(),
  isInputType: boolean = false
): string[] => {
  const result: string[] = [];
  
  // Avoid infinite recursion
  if (processedTypes.has(typeName)) {
    return result;
  }
  processedTypes.add(typeName);
  
  try {
    const graphqlType = schema.getType(typeName);
    if (!graphqlType) {
      return result;
    }
    
    // Handle enum types - now handled by shared types plugin
    if ('getValues' in graphqlType) {
      // Enums are generated by the shared types plugin
      return result;
    }
    
    // Handle object types (classes)
    if (!('getFields' in graphqlType)) {
      return result;
    }
    
    const fields = (graphqlType as any).getFields();
    const properties: string[] = [];
    
    for (const fieldName in fields) {
      const field = fields[fieldName];
      const pascalFieldName = toPascalCase(fieldName);
      
      // Convert the GraphQL type to C# type
      // For complex types, use field-based naming to avoid conflicts
      let csharpType = convertGraphQLTypeToCSharp(field.type, schema);
      
      // For input types, use schema type names; for response types, use field-based naming
      let fieldType = field.type;
      while (isNonNullType(fieldType) || isListType(fieldType)) {
        fieldType = fieldType.ofType;
      }
      const fieldTypeName = fieldType.name;
      
      if (!isScalarType(fieldTypeName) && !isEnumTypeFromSchema(schema, fieldTypeName)) {
        if (isInputType) {
          // For input types, use schema type names to maintain consistency
          const schemaTypeName = toPascalCase(fieldTypeName);
          if (isListType(field.type)) {
            csharpType = `List<${schemaTypeName}>`;
          } else {
            csharpType = schemaTypeName;
          }
        } else {
          // For response types, use field-based naming to avoid conflicts
          const fieldBasedTypeName = toPascalCase(fieldName);
          if (isListType(field.type)) {
            csharpType = `List<${fieldBasedTypeName}>`;
          } else {
            csharpType = fieldBasedTypeName;
          }
        }
      }
      
      // Check if this field uses another custom type that needs to be generated
      let currentType = field.type;
      // Unwrap NonNull and List types to get the base type
      while (isNonNullType(currentType) || isListType(currentType)) {
        currentType = currentType.ofType;
      }
      const namedTypeName = currentType.name;
      
      // If it's a custom type (not scalar and not enum), generate it recursively
      if (!isScalarType(namedTypeName) && !isEnumTypeFromSchema(schema, namedTypeName) && !processedTypes.has(namedTypeName)) {
        const nestedTypes = generateClassFromGraphQLType(schema, namedTypeName, processedTypes, isInputType);
        result.push(...nestedTypes);
      }
      
      properties.push(`        [JsonProperty("${fieldName}")]
        public ${csharpType} ${pascalFieldName} { get; set; }`);
    }
    
    const classDefinition = `    public sealed class ${toPascalCase(typeName)} 
    {
${properties.join('\n')}
    }`;
    
    result.push(classDefinition);
    
  } catch (error) {
    console.log(`Error generating class for ${typeName}:`, error);
  }
  
  return result;
};

// Helper function to parse selection set recursively
const parseSelectionSet = (selectionSet: SelectionSetNode | undefined, parentType: GraphQLType | null = null, responseClasses: Set<string> = new Set(), schema: GraphQLSchema | null = null, parentFieldName: string = '', existingClassNames: Set<string> = new Set()): Property[] => {
  if (!selectionSet || !selectionSet.selections) return [];
  const properties: Property[] = [];
  
  selectionSet.selections.forEach(selection => {
    if (selection.kind === 'Field') {
      const fieldName = selection.name.value;
      // Use alias if available, otherwise use field name
      const className = selection.alias ? selection.alias.value : fieldName;
      const pascalFieldName = toPascalCase(className);
      
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
        // Generate unique class name by combining parent field name if there's a duplicate
        let nestedClassName = pascalFieldName;
        let nestedType = null;

        if (fieldType) {
          nestedType = getNamedType(fieldType);
        }
        
        // Check if this class name already exists and combine with parent field name if needed
        if (existingClassNames.has(nestedClassName) && parentFieldName) {
          nestedClassName = toPascalCase(parentFieldName) + nestedClassName;
        }
        existingClassNames.add(nestedClassName);
        
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
        
        // Parse nested selection set recursively to only generate used classes
        if (selection.selectionSet) {
          const nestedProperties = parseSelectionSet(selection.selectionSet, nestedType, responseClasses, schema, fieldName, existingClassNames);
          
          // Generate only the specific class for this selection set
          const classProperties = nestedProperties.map(prop => 
            `        [JsonProperty("${prop.name}")]
        public ${prop.type} ${prop.pascalName} { get; set; }`
          ).join('\n');
          
          const nestedClass = `    
    public sealed class ${nestedClassName} 
    {
${classProperties}
    }`;
          
          responseClasses.add(nestedClass);
        }
      }
      
      properties.push({
        name: selection.alias ? selection.alias.value : fieldName,
        pascalName: pascalFieldName,
        type: csharpType
      });
    }
  });
  
  return properties;
};

export const plugin: PluginFunction<AgodaCSharpCodegenConfig> = (
  schema: GraphQLSchema,
  documents: Types.DocumentFile[],
  config: AgodaCSharpCodegenConfig
): string => {
    if (!config.namespace) {
      throw new Error('namespace is required');
    }

    const operationsDefinitions = getOperationsDefinitions(documents);
    
    if (operationsDefinitions.length === 0) {
      return 'No operations found';
    }

    const operation = operationsDefinitions[0];
    const variables = operation.variables;
    const rawQuery = (operation.document.rawSDL || '').replace(/"/g, '""');

    const operationClassName = getOperationCSharpClassName(operation);
    const namespace = config.namespace;
    
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
    const responseClasses = new Set<string>();
    
    // Note: Input types and enums are now generated by a separate shared types plugin
    // This plugin only generates response classes and query classes
    
    // Parse the main query operation and generate response classes
    let mainDataProperties: Property[] = [];
    operationsDefinitions.forEach(op => {
      if (!op.document.document) return;
      visit(op.document.document, {
        OperationDefinition: (node) => {
          if (node.selectionSet) {
            let rootType: GraphQLObjectType | null = null;
            try {
              switch (node.operation) {
                case 'query':
                  rootType = schema.getQueryType() ?? null;
                  break;
                case 'mutation':
                  rootType = schema.getMutationType() ?? null;
                  break;
                case 'subscription':
                  rootType = schema.getSubscriptionType() ?? null;
                  break;
              }
            } catch (error) {
              // console.log('Error getting root type from schema:', error.message);
            }
            
            // Get properties for Data class and generate response classes
            mainDataProperties = parseSelectionSet(node.selectionSet, rootType, responseClasses, schema, '', new Set());
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
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace ${namespace}
{
    public class ${operationClassName} : QueryBase<Data>
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