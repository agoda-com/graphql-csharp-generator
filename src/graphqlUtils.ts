import { Kind, TypeNode, GraphQLSchema, isEnumType } from "graphql";
import { SCALAR_TYPES } from "./constants";

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

export const isEnumTypeFromSchema = (schema: GraphQLSchema, typeName: string): boolean => {
    try {
        const graphqlType = schema.getType(typeName);
        return !!(graphqlType && isEnumType(graphqlType));
    } catch (error) {
        return false;
    }
};