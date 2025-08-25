using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.CustomerSegments.GetCustomerSegments
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetCustomerSegments {
  CustomerSegments {
    customerSegmentId
    customerSegmentGroupId
    languageId
    countryIso2
    isActive
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
        [JsonProperty("CustomerSegments")]
        public List<CustomerSegments> CustomerSegments { get; set; }
    }
    
    public sealed class CustomerSegments 
    {
        [JsonProperty("customerSegmentId")]
        public int CustomerSegmentId { get; set; }
        [JsonProperty("customerSegmentGroupId")]
        public int CustomerSegmentGroupId { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
        [JsonProperty("countryIso2")]
        public string CountryIso2 { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}
