import { promises as fs } from 'fs'
import * as path from 'path'

export const getFiles = async (dir: string, extensionName: string, files: string[] = []): Promise<string[]> => {
    const entries = await fs.readdir(dir, { withFileTypes: true })
    for (const entry of entries) {
        const fullPath = path.join(dir, entry.name)
        if (entry.isDirectory()) {
            await getFiles(fullPath, extensionName, files)
        } else if (entry.isFile() && entry.name.endsWith(extensionName)) {
            files.push(fullPath)
        }
    }
    return files
}

export const deleteFiles = async (listFiles: string[]): Promise<void> => {
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