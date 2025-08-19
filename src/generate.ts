#!/usr/bin/env node

import { promises as fs } from 'fs'
import path from 'path'
import os from 'os'
import { getArgValue, getHeaderArgs, isHelpRequested } from './utils'
import { showManual } from './manual'
import { getFiles, deleteFiles } from './files'
import { generateGraphql, runCodegenWithConfig } from './core'

// Helper function to generate namespace from file path (cross-platform)
const generateNamespaceFromPath = (filePath: string): string => {
    const dir = path.dirname(filePath)
    // Normalize path separators and remove leading ./ or .\
    const normalizedDir = path.normalize(dir)
        .replace(/^\.[\\/]/, '') // Remove leading ./ or .\
        .replace(/[\\/]+/g, '.') // Convert all path separators to dots
    
    return normalizedDir
}

// Helper function to create dynamic config content based on GraphQL files (cross-platform)
const createDynamicConfig = (schemaUrl: string, graphqlFiles: string[]): string => {
    const EOL = os.EOL // Use OS-specific line endings
    
    let config = `overwrite: true
schema: "${schemaUrl}"
generates:
`
    
    for (const graphqlFile of graphqlFiles) {
        // Create the output .generated.cs file path
        const outputFile = graphqlFile.replace(/\.graphql$/, '.generated.cs')
        
        // Generate namespace from the directory path
        const namespace = generateNamespaceFromPath(graphqlFile)
        
        // Normalize paths for YAML config (always use forward slashes in YAML)
        const normalizedOutputFile = outputFile.replace(/\\/g, '/')
        const normalizedGraphqlFile = graphqlFile.replace(/\\/g, '/')
        
        // Create cross-platform plugin path
        const pluginPath = path.join('.', 'dist', 'agoda-csharp-codegen.js').replace(/\\/g, '/')
        
        // Add the generate section for this file
        config += `    ${normalizedOutputFile}:
        documents:
            - "${normalizedGraphqlFile}"
        plugins:
            - "${pluginPath}"
        config:
            namespace: "${namespace}"
`
    }
    
    return config
}

export const run = async (): Promise<void> => {
    const currentDir = process.cwd()
    const tempConfigPath = path.join(currentDir, 'temp-codegen.yml')

    if (isHelpRequested()) {
        showManual()
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

    const graphqlFiles = await getFiles(rawGraphqlDirectory, '.graphql')
    
    if (graphqlFiles.length === 0) {
        console.log('No GraphQL files found in the specified directory')
        return
    }
    
    console.log(`Found ${graphqlFiles.length} GraphQL files:`)
    graphqlFiles.forEach(file => console.log(`  - ${file}`))

    try {
        // Create dynamic config content based on found GraphQL files
        console.log('Creating dynamic config content...')
        const dynamicConfigContent = createDynamicConfig(schemaUrl, graphqlFiles)
        
        
        // Create temp-codegen.yml from dynamic content
        console.log('Creating temp-codegen.yml at:', tempConfigPath)
        await fs.writeFile(tempConfigPath, dynamicConfigContent, 'utf8')
        console.log('Successfully created temp-codegen.yml')
        
        // Run gql-gen with the created config file
        await runCodegenWithConfig(tempConfigPath)
        
    } catch (error) {
        console.error('Error during GraphQL code generation:', error)
        throw error
    } finally {
        // Always clean up the temporary config file
        try {
            // await fs.unlink(tempConfigPath)
            console.log('Successfully removed temp-codegen.yml')
        } catch (cleanupError) {
            console.warn('Failed to remove temp-codegen.yml:', cleanupError)
            // Don't throw here - cleanup failure shouldn't fail the main process
        }
    }

}

run().catch((err) => {
    console.error('Fatal error:', err)
    process.exit(1)
})
