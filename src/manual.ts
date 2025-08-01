export const showManual = (): void => {
    console.log(`
GraphQL C# Code Generator

DESCRIPTION:
    A tool to generate C# classes from GraphQL schema files. This script recursively
    searches for .graphql files in a specified directory, generates corresponding
    C# classes using graphql-code-generator, and post-processes the output to
    match Agoda's coding standards.

USAGE:
    npm run generate -- --graphql-dir <directory> --schema-url <url> [--header <name:value>]
    node src/generate.ts --graphql-dir <directory> --schema-url <url> [--header <name:value>]

REQUIRED ARGUMENTS:
    --graphql-dir <directory>    Directory containing .graphql files to process
                                 The script will recursively search this directory
                                 for all .graphql files

    --schema-url <url>           GraphQL schema endpoint URL
                                 Used to validate and generate types from the schema

OPTIONAL ARGUMENTS:
    --header <name:value>        HTTP header to include in GraphQL schema requests
                                 Can be specified multiple times for multiple headers
                                 Format: "Header-Name: header-value"
                                 Example: --header "API-Key: your-api-key" --header "Client-ID: xxxx"

    --help                       Show this help message and exit

BEHAVIOR:
    1. Removes all existing .generated.cs files in the target directory
    2. Processes each .graphql file found recursively
    3. Generates C# classes using graphql-code-generator

EXAMPLES:
    # Basic usage
    graphql-csharp-generator --graphql-dir "./Agoda.Graphql" --schema-url "https://api.example.com/graphql"
    
    # With custom headers
    graphql-csharp-generator --graphql-dir "./Agoda.Graphql" --schema-url "https://api.example.com/graphql" --header "API-Key: your-key" --header "Client-ID: xxxx"

OUTPUT:
    For each .graphql file, a corresponding .generated.cs file will be created
    in the same directory with the generated C# classes.
`)
}