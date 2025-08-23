import { OperationDefinitionNode, visit } from "graphql";
import { Types } from "@graphql-codegen/plugin-helpers";

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