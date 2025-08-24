using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.EngageConnect.Queries.GetProductRecommendations
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetProductRecommendations($propertyId: Int!, $whiteLabelId: Int!, $includeInactive: Boolean!, $recommendationChannel: RecommendationChannel!, $productNames: [String!], $pagination: PaginationInput!) {
  findProductRecommendations(
    hotelId: $propertyId
    whiteLabelId: $whiteLabelId
    includeInactive: $includeInactive
    recommendationChannel: $recommendationChannel
    productNames: $productNames
    pagination: $pagination
  ) {
    recommendationId
    productName
    productCategoryName
    recommendationScore
    recommendationTraceId
    activationParameters
  }
}";

        public int PropertyId { get; }
        public int WhiteLabelId { get; }
        public bool IncludeInactive { get; }
        public RecommendationChannel RecommendationChannel { get; }
        public List<string>? ProductNames { get; }
        public PaginationInput Pagination { get; }

        public Query(int propertyId, int whiteLabelId, bool includeInactive, RecommendationChannel recommendationChannel, List<string>? productNames, PaginationInput pagination, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            PropertyId = propertyId;
            WhiteLabelId = whiteLabelId;
            IncludeInactive = includeInactive;
            RecommendationChannel = recommendationChannel;
            ProductNames = productNames;
            Pagination = pagination;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "propertyId", PropertyId },
            { "whiteLabelId", WhiteLabelId },
            { "includeInactive", IncludeInactive },
            { "recommendationChannel", RecommendationChannel },
            { "productNames", ProductNames },
            { "pagination", Pagination }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("findProductRecommendations")]
        public List<FindProductRecommendations> FindProductRecommendations { get; set; }
    }
    
    public sealed class FindProductRecommendations 
    {
        [JsonProperty("recommendationId")]
        public long? RecommendationId { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("productCategoryName")]
        public string ProductCategoryName { get; set; }
        [JsonProperty("recommendationScore")]
        public decimal RecommendationScore { get; set; }
        [JsonProperty("recommendationTraceId")]
        public string RecommendationTraceId { get; set; }
        [JsonProperty("activationParameters")]
        public JToken? ActivationParameters { get; set; }
    }
}
