# agoda-graphql-csharp-generator
## it's graphql plugin. it will work together with -> https://the-guild.dev/graphql/codegen/docs/getting-started

## installation
```node
npm install agoda-graphql-csharp-generator @graphql-codegen/cli graphql
```

## how to use
- create codegen config -> https://the-guild.dev/graphql/codegen/docs/config-reference/codegen-config
- use plugin -> `agoda-graphql-csharp-generator`

# Notes:
- automate job to update semver version
- automate job to create release and tags


Found the Solution:
- create custom plugin to generate share type class from schema
    - pass list of graphql files
    - create share type and use in each generated file
- create custom plugin to generate query or mutation class using that share type
- fix npm package to export 2 files
- test with BAPI and SMAPI
- make sure it's working with json converter
