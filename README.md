# agoda-graphql-csharp-generator
## it's graphql plugin. it will work together with -> https://the-guild.dev/graphql/codegen/docs/getting-started

## installation
```node
npm install agoda-graphql-csharp-generator @graphql-codegen/cli graphql
```

## how to use
- create codegen config -> https://the-guild.dev/graphql/codegen/docs/config-reference/codegen-config
- generated codegen config
```
generate-codegen-config --schema-url 'https://graphql-schema.com' --graphql-dir './Agoda.Graphql/SupplyApi/Queries' --graphql-project 'Agoda.Graphql' --yml-out './codegen.yml'
```
- it will generated codegen.yml in this file will be all config
- then use `gql-gen --config codegen.yml` to generated graphql files

# Notes:
- automate job to update semver version
- automate job to create release and tags

