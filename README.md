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


TODO:
- create script to generate codegen.yml that accept
```
--schema-url 'https://smapi-qa-http.privatecloud.qa.agoda.is/v2/graphql' --graphql-dir '../../src/Agoda.SupplyReporting.GraphQl/SupplyApi/Queries/DMCs' --graphql-project 'Agoda.SupplyReporting.GraphQl' --output-name
```
- in target project just run with gql-gen
