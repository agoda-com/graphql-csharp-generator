#!/usr/bin/env node

import { promises as fs } from 'fs'
import * as path from 'path'
import { exec } from 'child_process'

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

// Helper to get argument value from CLI
const getArgValue = (argName: string): string | undefined => {
    const args = process.argv.slice(2)
    const index = args.indexOf(`--${argName}`)
    if (index !== -1 && args[index + 1]) {
        let value = args[index + 1]
        // Remove surrounding quotes if they exist
        if ((value.startsWith("'") && value.endsWith("'")) || (value.startsWith('"') && value.endsWith('"'))) {
            value = value.slice(1, -1)
        }
        return value
    }
    return undefined
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
    const folder = path.dirname(filePath).replace(/\//g, '.')
    const namespace = `${folder}.${fileName}`

    let content = await fs.readFile(filePath, 'utf8')
    content = content.replace(/#region input types[\s\S]*?#endregion[^\n]*\n?/g, '')
    content = content.replace(/using Agoda\.CodeGen\.GraphQL/g, 'using Agoda.Graphql')
    content = content.replace(`namespace Generated.${fileName}`, `namespace ${namespace}`)
    content = content.replace(/sumary/g, 'summary')

    await fs.writeFile(filePath, content, 'utf8')
}

// Generate GraphQL code and post-process
const generateGraphql = async (schemaUrl: string, filePath: string): Promise<void> => {
    const folder = path.dirname(filePath)
    const gqlGenCommand = getGqlGenCommand()

    await new Promise<void>((resolve, reject) => {
        console.warn(`Generating code for ${filePath}...`)
        exec(
            `${gqlGenCommand} --schema "${schemaUrl}" --template agoda-graphql-codegen-csharp --out "${folder}" "${filePath}"`,
            (error, stdout, stderr) => {
                if (error) {
                    console.error(`Error: ${error.message}`)
                    reject(error)
                    return
                }
                if (stderr) {
                    console.warn(`Warning: ${stderr}`)
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
    const rawGraphqlDirectory = getArgValue('graphql-dir')
    const schemaUrl = getArgValue('schema-url')

    if (!rawGraphqlDirectory || !schemaUrl) {
        console.error('Usage: script --graphql-dir <dir> --schema-url <url>')
        process.exit(1)
    }

    console.log('raw graphql directory: ', rawGraphqlDirectory)
    console.log('graphql schema url: ', schemaUrl)

    const oldGeneratedFiles = await getGraphqlFiles(rawGraphqlDirectory, '.generated.cs')
    console.log('removing generated files...')
    await deleteFiles(oldGeneratedFiles)

    const graphqlFiles = await getGraphqlFiles(rawGraphqlDirectory, '.graphql')

    for (const f of graphqlFiles) {
        console.log(`Processing file: ${f}`)
        await generateGraphql(schemaUrl, f)
    }
}

run().catch((err) => {
    console.error('Fatal error:', err)
    process.exit(1)
})
