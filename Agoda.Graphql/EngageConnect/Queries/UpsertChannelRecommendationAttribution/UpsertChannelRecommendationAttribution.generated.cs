using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.EngageConnect.Queries.UpsertChannelRecommendationAttribution
{
    public class Mutation : QueryBase<Data>
    {
        private const string _query = @"mutation UpsertChannelRecommendationAttribution($recommendationTraceInput: [RecommendationTraceInput!]!, $channelStatus: ChannelStatus!, $recommendationChannel: RecommendationChannel!) {
  upsertChannelRecommendationAttributions(
    recommendationChannel: $recommendationChannel
    channelStatus: $channelStatus
    recommendationTraces: $recommendationTraceInput
  ) {
    recommendationId
    channelStatus
    recommendationTraceId
  }
}";

        public List<RecommendationTraceInput> RecommendationTraceInput { get; }
        public ChannelStatus ChannelStatus { get; }
        public RecommendationChannel RecommendationChannel { get; }

        public Mutation(List<RecommendationTraceInput> recommendationTraceInput, ChannelStatus channelStatus, RecommendationChannel recommendationChannel, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            RecommendationTraceInput = recommendationTraceInput;
            ChannelStatus = channelStatus;
            RecommendationChannel = recommendationChannel;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "recommendationTraceInput", RecommendationTraceInput },
            { "channelStatus", ChannelStatus },
            { "recommendationChannel", RecommendationChannel }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("upsertChannelRecommendationAttributions")]
        public List<UpsertChannelRecommendationAttributions> UpsertChannelRecommendationAttributions { get; set; }
    }
    
    public sealed class UpsertChannelRecommendationAttributions 
    {
        [JsonProperty("recommendationId")]
        public long? RecommendationId { get; set; }
        [JsonProperty("channelStatus")]
        public ChannelStatus ChannelStatus { get; set; }
        [JsonProperty("recommendationTraceId")]
        public string RecommendationTraceId { get; set; }
    }
}
