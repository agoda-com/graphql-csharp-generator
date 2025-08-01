#!/usr/bin/env node

import { getArgValue, getHeaderArgs, isHelpRequested } from './utils'
import { showManual } from './manual'
import { getFiles, deleteFiles } from './files'
import { generateGraphql } from './core'

export const run = async (): Promise<void> => {
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

    const oldGeneratedFiles = await getFiles(rawGraphqlDirectory, '.generated.cs')
    console.log('removing generated files...')
    await deleteFiles(oldGeneratedFiles)

    const graphqlFiles = await getFiles(rawGraphqlDirectory, '.graphql')

    for (const f of graphqlFiles) {
        await generateGraphql(schemaUrl, f, headers)
    }
}

run().catch((err) => {
    console.error('Fatal error:', err)
    process.exit(1)
})
