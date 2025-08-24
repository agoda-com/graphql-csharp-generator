import { Kind, OperationDefinitionNode, TypeNode, visit } from "graphql";
import { Types } from "@graphql-codegen/plugin-helpers";
import { SCALAR_TYPES } from "./constants";

export const getOperationsDefinitions = async (documents: Types.DocumentFile[]): Promise<OperationDefinitionNode[]> => {
    const parseDocumentTasks = documents.map(document => {
        return new Promise<OperationDefinitionNode | undefined>((resolve, reject) => {
            if (!document.document) return resolve(undefined);
            visit(document.document, {
                OperationDefinition: (node: OperationDefinitionNode) => {
                    resolve(node);
                }
            });
        });
    });
    return (await Promise.all(parseDocumentTasks)).filter(x => x !== undefined) as OperationDefinitionNode[];
}

export const extractTypeName = (typeNode: TypeNode): string => {
    if (typeNode.kind === Kind.NAMED_TYPE) {
      return typeNode.name.value;
    } else if (typeNode.kind === Kind.NON_NULL_TYPE) {
      return extractTypeName(typeNode.type);
    } else if (typeNode.kind === Kind.LIST_TYPE) {
      return extractTypeName(typeNode.type);
    }
    return 'Unknown';
};

export const isScalarType = (typeName: string): boolean => {
    return SCALAR_TYPES.includes(typeName);
};