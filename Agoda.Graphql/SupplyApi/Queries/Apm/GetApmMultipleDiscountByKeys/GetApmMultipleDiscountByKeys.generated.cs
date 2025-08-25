using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.Apm.GetApmMultipleDiscountByKeys
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetApmMultipleDiscountByKeys($apmMultipleDiscountKeys: [getApmMultipleDiscount!]!) {
  ApmMultipleDiscount(apmMultipleDiscountKeys: $apmMultipleDiscountKeys) {
    apmAdjustmentDiscount {
      ApmAdjustmentDiscountId
      ProgramId
      AdjustmentChannelId
      AdjustmentDiscountPercent
      CategoryName
      StartDate
      EndDate
    }
    apmCommissionDiscount {
      ApmCommissionDiscountId
      ProgramId
      CommissionChannelId
      CommissionDiscountPercent
      CategoryName
      StartDate
      EndDate
    }
  }
}";

        public List<GetApmMultipleDiscount> ApmMultipleDiscountKeys { get; }

        public Query(List<GetApmMultipleDiscount> apmMultipleDiscountKeys, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            ApmMultipleDiscountKeys = apmMultipleDiscountKeys;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "apmMultipleDiscountKeys", ApmMultipleDiscountKeys }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("ApmMultipleDiscount")]
        public ApmMultipleDiscount ApmMultipleDiscount { get; set; }
    }
    
    public sealed class ApmAdjustmentDiscount 
    {
        [JsonProperty("ApmAdjustmentDiscountId")]
        public int ApmAdjustmentDiscountId { get; set; }
        [JsonProperty("ProgramId")]
        public int ProgramId { get; set; }
        [JsonProperty("AdjustmentChannelId")]
        public int AdjustmentChannelId { get; set; }
        [JsonProperty("AdjustmentDiscountPercent")]
        public double AdjustmentDiscountPercent { get; set; }
        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }
        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("EndDate")]
        public DateTime EndDate { get; set; }
    }
    
    public sealed class ApmCommissionDiscount 
    {
        [JsonProperty("ApmCommissionDiscountId")]
        public int ApmCommissionDiscountId { get; set; }
        [JsonProperty("ProgramId")]
        public int ProgramId { get; set; }
        [JsonProperty("CommissionChannelId")]
        public int CommissionChannelId { get; set; }
        [JsonProperty("CommissionDiscountPercent")]
        public double CommissionDiscountPercent { get; set; }
        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }
        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("EndDate")]
        public DateTime EndDate { get; set; }
    }
    
    public sealed class ApmMultipleDiscount 
    {
        [JsonProperty("apmAdjustmentDiscount")]
        public List<ApmAdjustmentDiscount> ApmAdjustmentDiscount { get; set; }
        [JsonProperty("apmCommissionDiscount")]
        public List<ApmCommissionDiscount> ApmCommissionDiscount { get; set; }
    }
}
