using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.Rooms.GetObjectExternalData
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetObjectExternalData($id: BigInt!, $type: String!) {
  ObjectExternalData(objectId: $id, objectType: $type) {
    objectId
    recStatus
    externalDataId
    objectType
    recModifyWhen
  }
}";

        public long Id { get; }
        public string Type { get; }

        public Query(long id, string type, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            Id = id;
            Type = type;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "id", Id },
            { "type", Type }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("ObjectExternalData")]
        public List<ObjectExternalData> ObjectExternalData { get; set; }
    }
    
    public sealed class ObjectExternalData 
    {
        [JsonProperty("objectId")]
        public long ObjectId { get; set; }
        [JsonProperty("recStatus")]
        public int RecStatus { get; set; }
        [JsonProperty("externalDataId")]
        public int ExternalDataId { get; set; }
        [JsonProperty("objectType")]
        public string ObjectType { get; set; }
        [JsonProperty("recModifyWhen")]
        public DateTime RecModifyWhen { get; set; }
    }
}
