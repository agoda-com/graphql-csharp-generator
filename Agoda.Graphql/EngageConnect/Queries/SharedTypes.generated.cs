using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.EngageConnect.Queries
{
    public enum RecommendationChannel 
    {
        DEFAULT,
        PCOMM,
        SDM_EMAIL,
        SDM_POPUP,
        YCS,
        YCS_PROMO_PAGE_2
    }

    public sealed class PaginationInput 
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
    }

    public sealed class RecommendationTraceInput 
    {
        [JsonProperty("recommendationId")]
        public long? RecommendationId { get; set; }
        [JsonProperty("recommendationRank")]
        public int? RecommendationRank { get; set; }
        [JsonProperty("recommendationTraceId")]
        public string RecommendationTraceId { get; set; }
        [JsonProperty("ticketId")]
        public long? TicketId { get; set; }
    }

    public enum ChannelStatus 
    {
        ACTIVATED,
        DISPATCHED,
        READ,
        RECOMMENDED,
        TOUCHED
    }
}
