#!/usr/bin/env node

import { promises as fs } from 'fs'
import * as path from 'path'
import { exec } from 'child_process'
import { log } from 'console'

// Helper to find the gql-gen binary
const getGqlGenCommand = (): string => {
    try {
        // Find the graphql-code-generator package that provides gql-gen
        const gqlGenPath = require.resolve('graphql-code-generator/dist/cli.js')
        return `node "${gqlGenPath}"`
    } catch (error) {
        // Fallback to npx with correct package name
        console.warn('Could not resolve gql-gen binary, falling back to npx')
        return 'npx graphql-code-generator'
    }
}

// Display help documentation
const showHelp = (): void => {
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

// Helper to get argument value from CLI
const getArgValue = (argName: string): string | undefined => {
    const args = process.argv.slice(2)
    const index = args.indexOf(`--${argName}`)
    if (index !== -1 && args[index + 1]) {
        let value = args[index + 1]
        if ((value.startsWith("'") && value.endsWith("'")) || (value.startsWith('"') && value.endsWith('"'))) {
            value = value.slice(1, -1)
        }
        return value
    }
    return undefined
}

// Helper to get all header arguments from CLI
const getHeaderArgs = (): string[] => {
    const args = process.argv.slice(2)
    const headers: string[] = []
    
    for (let i = 0; i < args.length; i++) {
        if (args[i] === '--header' && args[i + 1]) {
            let headerValue = args[i + 1]
            // Remove quotes if present
            if ((headerValue.startsWith("'") && headerValue.endsWith("'")) || 
                (headerValue.startsWith('"') && headerValue.endsWith('"'))) {
                headerValue = headerValue.slice(1, -1)
            }
            headers.push(headerValue)
            i++ // Skip the next argument as it's the header value
        }
    }
    
    return headers
}

// Check if help is requested
const isHelpRequested = (): boolean => {
    const args = process.argv.slice(2)
    return args.includes('--help') || args.includes('-h')
}

// Delete files asynchronously
const deleteFiles = async (listFiles: string[]): Promise<void> => {
    await Promise.all(
        listFiles.map(async (file) => {
            try {
                await fs.unlink(file)
                console.log(`Deleted: ${file}`)
            } catch (err) {
                console.error(`Failed to delete ${file}:`, err)
            }
        })
    )
}

// Remove input types region and update namespace/usings
async function removeInputTypesRegion(filePath: string): Promise<void> {
    const fileName = path.parse(filePath).name.replace('.generated', '')
    const folder = path.dirname(filePath).replace(/[/\\]/g, '.')
    const namespace = `${folder}.${fileName}`

    let content = await fs.readFile(filePath, 'utf8')
    
    content = content.replace(/#region input types[\s\S]*?#endregion[^\n]*\n?/g, '')
    content = content.replace(/using Agoda\.CodeGen\.GraphQL/g, 'using Agoda.Graphql')
    content = content.replace(/namespace Generated\..*/g, `namespace ${namespace}`)
    content = content.replace(/sumary/g, 'summary')

    await fs.writeFile(filePath, content, 'utf8')
}

// Generate GraphQL code and post-process
const generateGraphql = async (schemaUrl: string, filePath: string, headers: string[] = []): Promise<void> => {
    const folder = path.dirname(filePath)
    const gqlGenCommand = getGqlGenCommand()

    // Build header arguments only if headers are provided
    const headerArgs = headers.length > 0 
        ? headers.map(header => `--header "${header}"`).join(' ') + ' '
        : ''

    await new Promise<void>((resolve, reject) => {
        exec(
            `${gqlGenCommand} --schema "${schemaUrl}" ${headerArgs}--template agoda-graphql-codegen-csharp --out "${folder}" "${filePath}"`,
            (error, stdout, stderr) => {
                if (error) {
                    console.error(`Error: ${error.message}`)
                    reject(error)
                    return
                }
                resolve()
            }
        )
    })

    const oldPath = path.join(folder, 'Classes.cs')
    const fileName = `${path.parse(filePath).name}.generated.cs`
    const newPath = path.join(folder, fileName)

    await fs.rename(oldPath, newPath)
    await removeInputTypesRegion(newPath)
    console.log('generated: ', newPath)
}

// Recursively get files with a specific extension
const getGraphqlFiles = async (dir: string, extensionName: string, files: string[] = []): Promise<string[]> => {
    const entries = await fs.readdir(dir, { withFileTypes: true })
    for (const entry of entries) {
        const fullPath = path.join(dir, entry.name)
        if (entry.isDirectory()) {
            await getGraphqlFiles(fullPath, extensionName, files)
        } else if (entry.isFile() && entry.name.endsWith(extensionName)) {
            files.push(fullPath)
        }
    }
    return files
}

// Main runner
const run = async (): Promise<void> => {
    if (isHelpRequested()) {
        showHelp()
        process.exit(0)
    }

    const rawGraphqlDirectory = getArgValue('graphql-dir')
    const schemaUrl = getArgValue('schema-url')
    const headers = getHeaderArgs()

    if (!rawGraphqlDirectory || !schemaUrl) {
        console.error('Usage: script --graphql-dir <dir> --schema-url <url>')
        process.exit(1)
    }

    console.log('raw graphql directory: ', rawGraphqlDirectory)
    console.log('graphql schema url: ', schemaUrl)
    if (headers.length > 0) {
        console.log('headers: ', headers)
    }

    const oldGeneratedFiles = await getGraphqlFiles(rawGraphqlDirectory, '.generated.cs')
    console.log('removing generated files...')
    await deleteFiles(oldGeneratedFiles)

    const graphqlFiles = await getGraphqlFiles(rawGraphqlDirectory, '.graphql')

    for (const f of graphqlFiles) {
        await generateGraphql(schemaUrl, f, headers)
    }
}

run().catch((err) => {
    console.error('Fatal error:', err)
    process.exit(1)
})
