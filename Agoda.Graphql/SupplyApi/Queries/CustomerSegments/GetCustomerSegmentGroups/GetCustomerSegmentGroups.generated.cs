using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.CustomerSegments.GetCustomerSegmentGroups
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetCustomerSegmentGroups {
  CustomerSegmentGroups {
    customerSegmentGroupId
    name
    subContinentId
    cmsItemId
    isActive
    isLocal
    customerSegmentType
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
        [JsonProperty("CustomerSegmentGroups")]
        public List<CustomerSegmentGroups> CustomerSegmentGroups { get; set; }
    }
    
    public sealed class CustomerSegmentGroups 
    {
        [JsonProperty("customerSegmentGroupId")]
        public int CustomerSegmentGroupId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("subContinentId")]
        public int SubContinentId { get; set; }
        [JsonProperty("cmsItemId")]
        public int CmsItemId { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("isLocal")]
        public bool IsLocal { get; set; }
        [JsonProperty("customerSegmentType")]
        public CustomerSegmentType CustomerSegmentType { get; set; }
    }
}
