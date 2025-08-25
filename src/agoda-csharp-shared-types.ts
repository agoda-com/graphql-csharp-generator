import { 
    visit, 
    isNonNullType, 
    isListType, 
    getNamedType, 
    typeFromAST,
    GraphQLSchema,
    GraphQLType,
    GraphQLObjectType,
    isObjectType,
    TypeNode,
    Kind
} from 'graphql';
import { PluginFunction, Types } from '@graphql-codegen/plugin-helpers'
import { isScalarType, isEnumTypeFromSchema } from './graphqlUtils';
import { mapGraphQLTypeToCSharp, toPascalCase } from './naming';
import { extractTypeName } from './graphqlUtils';

// Plugin configuration interface
interface AgodaCSharpSharedConfig {
    namespace?: string;
}

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
            const typeName = input.kind === Kind.NAMED_TYPE ? (input as any).name.value : 'object';
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

// Function to generate C# enum from GraphQL enum
const generateEnumFromGraphQLType = (schema: GraphQLSchema, typeName: string): string => {
    try {
        const enumType = schema.getType(typeName);
        if (!enumType || !('getValues' in enumType)) {
            return '';
        }
        
        const enumValues = (enumType as any).getValues();
        const enumValueNames = enumValues.map((value: any) => value.name).join(',\n        ');
        
        return `    public enum ${toPascalCase(typeName)} 
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
    isInputType: boolean = true
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
            let csharpType = convertGraphQLTypeToCSharp(field.type, schema);
            
            // For input types, use schema type names to maintain consistency
            let fieldType = field.type;
            while (isNonNullType(fieldType) || isListType(fieldType)) {
                fieldType = fieldType.ofType;
            }
            const fieldTypeName = fieldType.name;
            
            if (!isScalarType(fieldTypeName) && !isEnumTypeFromSchema(schema, fieldTypeName)) {
                // For input types, use schema type names to maintain consistency
                const schemaTypeName = toPascalCase(fieldTypeName);
                if (isListType(field.type)) {
                    csharpType = `List<${schemaTypeName}>`;
                } else {
                    csharpType = schemaTypeName;
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
            } else if (isEnumTypeFromSchema(schema, namedTypeName) && !processedTypes.has(namedTypeName)) {
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

// Helper function to get field type from schema
const getFieldTypeFromSchema = (parentType: GraphQLType | null, fieldName: string): GraphQLType | null => {
    try {
        if (!parentType || !isObjectType(parentType)) {
            return null;
        }
        
        const fields = parentType.getFields();
        const field = fields[fieldName];
        
        if (!field) {
            return null;
        }
        
        return field.type;
    } catch (error) {
        return null;
    }
};

export const plugin: PluginFunction<AgodaCSharpSharedConfig> = (
    schema: GraphQLSchema,
    documents: Types.DocumentFile[],
    config: AgodaCSharpSharedConfig
): string => {
    if (!config.namespace) {
        throw new Error('namespace is required');
    }

    const allGeneratedTypes: string[] = [];
    const processedTypes = new Set<string>();
    
    // Process all documents to collect input types and enums
    documents.forEach(doc => {
        if (!doc.document) return;
        
        visit(doc.document, {
            OperationDefinition: (node) => {
                // Process variable definitions (input types and enums)
                if (node.variableDefinitions) {
                    node.variableDefinitions.forEach(variableDef => {
                        const typeName = extractTypeName(variableDef.type);
                        
                        // Check if this is a custom input type (not a scalar and not an enum)
                        if (!isScalarType(typeName) && !isEnumTypeFromSchema(schema, typeName)) {
                            console.log(`Found custom input type: ${typeName}`);
                            
                            // Generate input type dynamically from schema
                            const generatedInputTypes = generateClassFromGraphQLType(schema, typeName, processedTypes, true);
                            generatedInputTypes.forEach((inputTypeClass: string) => {
                                if (!allGeneratedTypes.includes(inputTypeClass)) {
                                    allGeneratedTypes.push(inputTypeClass);
                                }
                            });
                        } else if (isEnumTypeFromSchema(schema, typeName) && !processedTypes.has(typeName)) {
                            console.log(`Found enum type in variables: ${typeName}`);
                            
                            // Generate enum type
                            const enumDefinition = generateEnumFromGraphQLType(schema, typeName);
                            if (enumDefinition && !allGeneratedTypes.includes(enumDefinition)) {
                                allGeneratedTypes.push(enumDefinition);
                                processedTypes.add(typeName);
                            }
                        }
                    });
                }
                
                // Process selection sets (response types and enums)
                if (node.selectionSet) {
                    // Recursive function to process fields at any nesting level
                    const processFieldRecursively = (selectionSet: any, parentType: GraphQLObjectType | null) => {
                        visit(selectionSet, {
                            Field: (fieldNode) => {
                                if (parentType) {
                                    const fieldType = getFieldTypeFromSchema(parentType, fieldNode.name.value);
                                    if (fieldType) {
                                        const namedType = getNamedType(fieldType);
                                        const typeName = (namedType as any).name;
                                        
                                        if (isEnumTypeFromSchema(schema, typeName) && !processedTypes.has(typeName)) {
                                            console.log(`Found enum type in response: ${typeName}`);
                                            
                                            // Generate enum type
                                            const enumDefinition = generateEnumFromGraphQLType(schema, typeName);
                                            if (enumDefinition && !allGeneratedTypes.includes(enumDefinition)) {
                                                allGeneratedTypes.push(enumDefinition);
                                                processedTypes.add(typeName);
                                            }
                                        }
                                        
                                        // Recursively process nested fields if this field has a selection set
                                        if (fieldNode.selectionSet) {
                                            const nestedType = getNamedType(fieldType);
                                            if (isObjectType(nestedType)) {
                                                processFieldRecursively(fieldNode.selectionSet, nestedType);
                                            }
                                        }
                                    }
                                }
                            }
                        });
                    };
                    
                    // Get the root type based on operation
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
                    
                    // Start recursive processing from the root selection set
                    processFieldRecursively(node.selectionSet, rootType);
                }
            }
        });
    });
    
    return `using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace ${config.namespace}
{
${allGeneratedTypes.join('\n\n')}
}
`;
};