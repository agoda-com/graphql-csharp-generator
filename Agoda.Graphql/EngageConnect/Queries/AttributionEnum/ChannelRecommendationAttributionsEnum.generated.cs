using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.EngageConnect.Queries.AttributionEnum
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query ChannelRecommendationAttributionsEnum($propertyId: Int!, $whiteLabelId: Int!, $includeInactive: Boolean!, $recommendationChannel: RecommendationChannel!, $productCategoryNames: [String!]) {
  findProductRecommendations(
    hotelId: $propertyId
    whiteLabelId: $whiteLabelId
    includeInactive: $includeInactive
    recommendationChannel: $recommendationChannel
    productCategoryNames: $productCategoryNames
  ) {
    recommendationId
    channelRecommendationAttributions {
      recommendationChannel
    }
  }
}";

        public int PropertyId { get; }
        public int WhiteLabelId { get; }
        public bool IncludeInactive { get; }
        public RecommendationChannel RecommendationChannel { get; }
        public List<string>? ProductCategoryNames { get; }

        public Query(int propertyId, int whiteLabelId, bool includeInactive, RecommendationChannel recommendationChannel, List<string>? productCategoryNames, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            PropertyId = propertyId;
            WhiteLabelId = whiteLabelId;
            IncludeInactive = includeInactive;
            RecommendationChannel = recommendationChannel;
            ProductCategoryNames = productCategoryNames;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "propertyId", PropertyId },
            { "whiteLabelId", WhiteLabelId },
            { "includeInactive", IncludeInactive },
            { "recommendationChannel", RecommendationChannel },
            { "productCategoryNames", ProductCategoryNames }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("findProductRecommendations")]
        public List<FindProductRecommendations> FindProductRecommendations { get; set; }
    }
    
    public sealed class ChannelRecommendationAttributions 
    {
        [JsonProperty("recommendationChannel")]
        public RecommendationChannel RecommendationChannel { get; set; }
    }
    
    public sealed class FindProductRecommendations 
    {
        [JsonProperty("recommendationId")]
        public long? RecommendationId { get; set; }
        [JsonProperty("channelRecommendationAttributions")]
        public List<ChannelRecommendationAttributions> ChannelRecommendationAttributions { get; set; }
    }
}
