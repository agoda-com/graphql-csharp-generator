import { 
    GraphQLSchema, OperationDefinitionNode, SelectionNode, SelectionSetNode,
} from 'graphql';
import { PluginFunction, Types } from '@graphql-codegen/plugin-helpers'

import { getOperationsDefinitions } from './graphqlUtils';

interface AgodaCSharpSharedConfig {
    namespace?: string;
}

const toPascalCase = (str: string): string => {
    return str.charAt(0).toUpperCase() + str.slice(1);
};

const mapGraphQLTypeToCSharp = (typeName: string): string => {
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
            csharpType = toPascalCase(typeName); 
            break;
    }
    return csharpType;
};

const generateCSharpClassFromSelectionSet = (selectionSet: SelectionSetNode, className: string): string => {
    let classCode = `    public sealed class ${className}\n    {\n`;
    
    // Process each field in the selection set
    selectionSet.selections?.forEach((selection: SelectionNode) => {
        if (selection.kind === 'Field') {
            const fieldName = selection.name.value;
            const propertyName = fieldName.charAt(0).toUpperCase() + fieldName.slice(1); // Pascal case
            const jsonPropertyName = fieldName;
            
            if (selection.selectionSet) {
                // This is an object type, reference the separate class
                const nestedClassName = propertyName;
                classCode += `        [JsonProperty("${jsonPropertyName}")]\n`;
                classCode += `        public ${nestedClassName} ${propertyName} { get; set; }\n\n`;
            } else {
                // This is a scalar type - we need to determine the type from the schema
                // For now, we'll use a default mapping or you can enhance this with schema lookup
                console.log(selection);
                
                const csharpType = mapGraphQLTypeToCSharp('string'); // Default to string, you can enhance this
                classCode += `        [JsonProperty("${jsonPropertyName}")]\n`;
                classCode += `        public ${csharpType} ${propertyName} { get; set; }\n\n`;
            }
        }
    });
    
    classCode += `    }\n`;
    return classCode;
};

const generateAllClassesFromSelectionSet = (selectionSet: SelectionSetNode, className: string): string => {
    let allClasses = '';
    const generatedClasses = new Set<string>();
    
    const processSelectionSet = (selSet: SelectionSetNode, clsName: string): void => {
        if (generatedClasses.has(clsName)) return;
        generatedClasses.add(clsName);
        
        // Generate the current class
        allClasses += generateCSharpClassFromSelectionSet(selSet, clsName);
        allClasses += '\n';
        
        // Process nested selection sets to generate separate classes
        selSet.selections?.forEach((selection: SelectionNode) => {
            if (selection.kind === 'Field' && selection.selectionSet) {
                const nestedClassName = selection.name.value.charAt(0).toUpperCase() + selection.name.value.slice(1);
                processSelectionSet(selection.selectionSet, nestedClassName);
            }
        });
    };
    
    processSelectionSet(selectionSet, className);
    return allClasses;
};

const generateCSharpClassesFromOperations = (operations: OperationDefinitionNode[]): string => {
    let classesCode = '';
    
    operations.forEach(operation => {
        if (operation.selectionSet) {
            const className = operation.name?.value || 'QueryResult';
            classesCode += generateAllClassesFromSelectionSet(operation.selectionSet, className);
        }
    });
    
    return classesCode;
};

export const plugin: PluginFunction<AgodaCSharpSharedConfig> = async (
    schema: GraphQLSchema,
    documents: Types.DocumentFile[],
    config: AgodaCSharpSharedConfig
): Promise<string> => {
    const operations = await getOperationsDefinitions(documents);
    // console.log('operations', operations);
    
    const generatedClasses = generateCSharpClassesFromOperations(operations);
    
    return `using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace ${config.namespace}
{
${generatedClasses}
}
`;

}