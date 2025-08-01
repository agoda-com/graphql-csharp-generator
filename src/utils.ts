export const getArgValue = (argName: string): string | undefined => {
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

export const getHeaderArgs = (): string[] => {
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

export const isHelpRequested = (): boolean => {
    const args = process.argv.slice(2)
    return args.includes('--help') || args.includes('-h')
}