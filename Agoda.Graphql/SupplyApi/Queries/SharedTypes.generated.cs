using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries
{
    public sealed class GetApmMultipleDiscount 
    {
        [JsonProperty("adjustmentChannelId")]
        public int? AdjustmentChannelId { get; set; }
        [JsonProperty("commissionChannelId")]
        public int? CommissionChannelId { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("programId")]
        public int ProgramId { get; set; }
    }

    public enum CustomerSegmentType 
    {
        IpOnly,
        Language,
        Origin,
        Unknown,
        VipLevel
    }
}
