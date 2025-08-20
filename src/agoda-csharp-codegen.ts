import { 
  visit, 
  Kind, 
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
  TypeNode
} from 'graphql';
import { PluginFunction, Types } from '@graphql-codegen/plugin-helpers'

// Constants
const SCALAR_TYPES = ['String', 'Int', 'Float', 'Boolean', 'ID', 'Date', 'DateTime', 'LocalDate', 'LocalDateTime'];

// Helper function to check if a type is a scalar or enum
const isScalarOrEnum = (schema: GraphQLSchema, typeName: string): boolean => {
  if (SCALAR_TYPES.includes(typeName)) {
    return true;
  }
  
  try {
    const graphqlType = schema.getType(typeName);
    if (graphqlType && 'getValues' in graphqlType) {
      return true; // It's an enum
    }
  } catch (error) {
    // Ignore errors
  }
  
  return false;
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
  let csharpType;
  switch (typeName) {
    case 'String': csharpType = 'string'; break;
    case 'Int': csharpType = 'int'; break;
    case 'BigInt': csharpType = 'long'; break;
    case 'BigDecimal': csharpType = 'decimal'; break;
    case 'Long': csharpType = 'long'; break;
    case 'Float': csharpType = 'double'; break;
    case 'Boolean': csharpType = 'bool'; break;
    case 'Date': csharpType = 'DateTime'; break;
    case 'DateTime': csharpType = 'DateTime'; break;
    case 'ID': csharpType = 'string'; break;
    case 'LocalDate': csharpType = 'DateTime'; break;
    case 'LocalDateTime': csharpType = 'DateTime'; break;
    default: 
      // Check if it's an enum type
      try {
        const graphqlType = schema?.getType(typeName);
        if (graphqlType && graphqlType.astNode && graphqlType.astNode.kind === 'EnumTypeDefinition') {
          csharpType = toPascalCase(typeName);
        } else {
          csharpType = toPascalCase(typeName); 
        }
      } catch (error) {
        csharpType = toPascalCase(typeName); 
      }
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

// Helper function to extract type name from TypeNode
const extractTypeName = (typeNode: TypeNode): string => {
  if (typeNode.kind === 'NamedType') {
    return typeNode.name.value;
  } else if (typeNode.kind === 'NonNullType') {
    return extractTypeName(typeNode.type);
  } else if (typeNode.kind === 'ListType') {
    return extractTypeName(typeNode.type);
  }
  return 'Unknown';
};

// Function to generate C# enum from GraphQL enum
const generateEnumFromGraphQLType = (schema: GraphQLSchema, typeName: string): string => {
  try {
    const enumType = schema.getType(typeName);
    if (!enumType || !('getValues' in enumType)) {
      return '';
    }
    
    const enumValues = (enumType as any).getValues();
    const enumValueNames = enumValues.map((value: any) => value.name).join(',\n        ');
    
    return `    
    public enum ${toPascalCase(typeName)} 
    {
        ${enumValueNames}
    }`;
  } catch (error) {
    console.log(`Error generating enum for ${typeName}:`, error);
    return '';
  }
};

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
    
    // Handle enum types
    if ('getValues' in graphqlType) {
      const enumDefinition = generateEnumFromGraphQLType(schema, typeName);
      if (enumDefinition) {
        result.push(enumDefinition);
      }
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
      const csharpType = convertGraphQLTypeToCSharp(field.type, schema);
      
      // Check if this field uses another custom type that needs to be generated
      let currentType = field.type;
      // Unwrap NonNull and List types to get the base type
      while (isNonNullType(currentType) || isListType(currentType)) {
        currentType = currentType.ofType;
      }
      const namedTypeName = currentType.name;
      
      // If it's a custom type, generate it recursively
      if (!isScalarOrEnum(schema, namedTypeName) && !processedTypes.has(namedTypeName)) {
        const nestedTypes = generateClassFromGraphQLType(schema, namedTypeName, processedTypes, isInputType);
        result.push(...nestedTypes);
      } else if (isScalarOrEnum(schema, namedTypeName) && !SCALAR_TYPES.includes(namedTypeName) && !processedTypes.has(namedTypeName)) {
        // Generate enum types that are referenced
        const enumDefinition = generateEnumFromGraphQLType(schema, namedTypeName);
        if (enumDefinition) {
          result.push(enumDefinition);
          processedTypes.add(namedTypeName);
        }
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
const parseSelectionSet = (selectionSet: SelectionSetNode | undefined, parentType: GraphQLType | null = null, responseClasses: Set<string> = new Set(), schema: GraphQLSchema | null = null): Property[] => {
  if (!selectionSet || !selectionSet.selections) return [];
  const properties: Property[] = [];
  
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
        // Get the actual GraphQL type name from schema, not the field name
        let nestedClassName = pascalFieldName; // fallback
        let nestedType = null;
        
        if (fieldType) {
          nestedType = getNamedType(fieldType);
          if (nestedType && nestedType.name) {
            nestedClassName = toPascalCase(nestedType.name);
          }
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
        
        // Parse nested selection set recursively to only generate used classes
        if (selection.selectionSet) {
          const nestedProperties = parseSelectionSet(selection.selectionSet, nestedType, responseClasses, schema);
          
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
        name: fieldName,
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
    const operationName = operation.name;
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
    const inputTypeClasses = new Set<string>();
    
    // Collect input types from variables
    operationsDefinitions.forEach(op => {
      if (!op.document.document) return;
      visit(op.document.document, {
        OperationDefinition: (node) => {
          if (node.variableDefinitions) {
            node.variableDefinitions.forEach(variableDef => {
              const typeName = extractTypeName(variableDef.type);
              
              // Check if this is a custom input type (not a scalar or enum)
              if (!isScalarOrEnum(schema, typeName)) {
                console.log(`Found custom input type: ${typeName}`);
                
                // Generate input type dynamically from schema
                const generatedInputTypes = generateClassFromGraphQLType(schema, typeName, new Set(), true);
                generatedInputTypes.forEach((inputTypeClass: string) => {
                  inputTypeClasses.add(inputTypeClass);
                });
              }
            });
          }
        }
      });
    });
    
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
            mainDataProperties = parseSelectionSet(node.selectionSet, rootType, responseClasses, schema);
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
${Array.from(inputTypeClasses).join('\n')}
}
`;
}