# graphql-csharp-generator

## installation
```node
npm install -g agoda-graphql-csharp-generator
```

## how to use
```bash
graphql-csharp-generator --schema-url '$graphqlUrl' --graphql-dir '$rawGraphqlDirectory'
```
- example
```bash
graphql-csharp-generator --schema-url 'https://smapi-qa-http.privatecloud.qa.agoda.is/v2/graphql' --graphql-dir './Agoda.Graphql/SupplyApi'
```

# Notes:
- as of now, publish npm package will be trigger once release created
  - it's using free account so it can be deployed once a day
- need to improve
  - automate job to update semver version
  - automate job to create release and tags