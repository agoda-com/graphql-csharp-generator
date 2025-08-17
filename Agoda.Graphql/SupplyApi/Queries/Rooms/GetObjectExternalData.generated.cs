using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.GetObjectExternalData
{
    public partial class Query : QueryBase<Data>
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

        public Query(long id, string type, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            Id = id;
            Type = type;
        }
        
        public long Id { get; }
        public string Type { get; }
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
    
    /// <summary>Inner Model</summary> 
    public sealed class ObjectExternalData 
    {
                
        
        [JsonProperty("objectId")]
        public BigInt ObjectId { get; set; }
        
        
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
