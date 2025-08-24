export const toPascalCase = (str: string): string => {
    if (!str) return str;
    return str.charAt(0).toUpperCase() + str.slice(1);
};

export const mapGraphQLTypeToCSharp = (typeName: string): string => {
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
        case 'Json': csharpType = 'JToken'; break;
        default: 
            csharpType = toPascalCase(typeName); 
            break;
    }
    return csharpType;
};