import { exec } from "child_process"
import path from "path"
import { promises as fs } from 'fs'

export const removeInputTypesRegion = async (filePath: string): Promise<void> => {
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

// Helper to find the gql-gen binary
export const getGqlGenCommand = (): string => {
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

// Run gql-gen with a config file (cross-platform)
export const runCodegenWithConfig = async (configFilePath: string): Promise<void> => {
    console.log(`Running gql-gen with config: ${configFilePath}`)
    
    // Properly quote the config file path to handle spaces and special characters
    const quotedConfigPath = `"${configFilePath}"`
    
    return new Promise<void>((resolve, reject) => {
        exec(
            `gql-gen --config ${quotedConfigPath}`,
            (error, stdout, stderr) => {
                if (stdout) {
                    console.log('stdout:', stdout)
                }
                if (stderr) {
                    console.log('stderr:', stderr)
                }
                if (error) {
                    console.error(`Error: ${error.message}`)
                    reject(error)
                    return
                }
                console.log('GraphQL code generation completed successfully')
                resolve()
            }
        )
    })
}

// Generate GraphQL code and post-process
export const generateGraphql = async (schemaUrl: string, filePath: string, headers: string[] = []): Promise<void> => {
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