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