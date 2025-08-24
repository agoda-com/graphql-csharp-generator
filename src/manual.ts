export const showManual = (): void => {
    console.log(`
GraphQL C# Code Generator

DESCRIPTION:
    This script generates a codegen config file that can be used to generate C# classes using @graphql-codegen/cli - gql-gen.

USAGE:
    generate-codegen-config --graphql-dir <directory> --schema-url <url> [--graphql-project <project-name>] [--yml-out <output-path>] [--header <name:value>]

REQUIRED ARGUMENTS:
    --graphql-dir <directory>           Directory containing .graphql files to process
                                        The script will recursively search this directory
                                        for all .graphql files

    --schema-url <url>                  GraphQL schema endpoint URL
                                        Used to validate and generate types from the schema

    --graphql-project <project-name>    Base project name for namespace generation
                                        When provided, the namespace will be constructed as:
                                        <project-name>.<path-after-project>
                                        Example: --graphql-project "Agoda.Graphql"

OPTIONAL ARGUMENTS:
    --yml-out <output-path>             Custom output path for the generated YAML config file
                                        If not specified, defaults to 'temp-codegen.yml' in current directory
                                        Example: --yml-out "./config/codegen.yml"
                                        default is 'codegen.yml' in current directory

    --header <name:value>               HTTP header to include in GraphQL schema requests
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
    generate-codegen-config --graphql-dir "./Graphql" --schema-url "https://api.example.com/graphql" --graphql-project "Graphql"

    # With custom header
    generate-codegen-config --graphql-dir "./Graphql" --schema-url "https://api.example.com/graphql" --header "API-Key: your-key" --header "Client-ID: xxxx" --graphql-project "Graphql"
`)
}