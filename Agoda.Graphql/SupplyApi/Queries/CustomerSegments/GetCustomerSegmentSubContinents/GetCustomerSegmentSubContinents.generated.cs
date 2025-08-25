using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.CustomerSegments.GetCustomerSegmentSubContinents
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetCustomerSegmentSubContinents {
  CustomerSegmentSubContinents {
    subContinentId
    name
    continentId
    isActive
    sortOrder
    cmsItemId
  }
}";



        public Query(IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {

        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {

        };
    }

    public sealed class Data
    {        
        [JsonProperty("CustomerSegmentSubContinents")]
        public List<CustomerSegmentSubContinents> CustomerSegmentSubContinents { get; set; }
    }
    
    public sealed class CustomerSegmentSubContinents 
    {
        [JsonProperty("subContinentId")]
        public int SubContinentId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("continentId")]
        public int ContinentId { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }
        [JsonProperty("cmsItemId")]
        public int CmsItemId { get; set; }
    }
}
