using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.Promotion.GetHotelPromotionSuggestionSettingByCity
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetHotelPromotionSuggestionSettingByCity($cityId: Int!) {
  PromotionSuggestionSettingsByCityPromotionType(cityId: $cityId) {
    id
    cityId
    promotionTypeId
    startDate
    endDate
    startMonth
    endMonth
    discountValue
    promotionDiscountTypeId
    minAdvPurchase
    maxAdvPurchase
    stayOn
    minNightsStay
    bookTimeFrom
    bookTimeTo
    isActive
    lastUpdatedBy
    lastUpdatedWhen
    PromotionSuggestion {
      cityId
      promotionTypeId
      rank
      lastUpdatedBy
      lastUpdatedWhen
    }
  }
}";

        public int CityId { get; }

        public Query(int cityId, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            CityId = cityId;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "cityId", CityId }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("PromotionSuggestionSettingsByCityPromotionType")]
        public List<PromotionSuggestionSettingsByCityPromotionType> PromotionSuggestionSettingsByCityPromotionType { get; set; }
    }
    
    public sealed class PromotionSuggestion 
    {
        [JsonProperty("cityId")]
        public int CityId { get; set; }
        [JsonProperty("promotionTypeId")]
        public int PromotionTypeId { get; set; }
        [JsonProperty("rank")]
        public int Rank { get; set; }
        [JsonProperty("lastUpdatedBy")]
        public string LastUpdatedBy { get; set; }
        [JsonProperty("lastUpdatedWhen")]
        public DateTime? LastUpdatedWhen { get; set; }
    }
    
    public sealed class PromotionSuggestionSettingsByCityPromotionType 
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("cityId")]
        public int CityId { get; set; }
        [JsonProperty("promotionTypeId")]
        public int PromotionTypeId { get; set; }
        [JsonProperty("startDate")]
        public int StartDate { get; set; }
        [JsonProperty("endDate")]
        public int EndDate { get; set; }
        [JsonProperty("startMonth")]
        public int StartMonth { get; set; }
        [JsonProperty("endMonth")]
        public int EndMonth { get; set; }
        [JsonProperty("discountValue")]
        public double DiscountValue { get; set; }
        [JsonProperty("promotionDiscountTypeId")]
        public int PromotionDiscountTypeId { get; set; }
        [JsonProperty("minAdvPurchase")]
        public int? MinAdvPurchase { get; set; }
        [JsonProperty("maxAdvPurchase")]
        public int? MaxAdvPurchase { get; set; }
        [JsonProperty("stayOn")]
        public string StayOn { get; set; }
        [JsonProperty("minNightsStay")]
        public int? MinNightsStay { get; set; }
        [JsonProperty("bookTimeFrom")]
        public DateTime? BookTimeFrom { get; set; }
        [JsonProperty("bookTimeTo")]
        public DateTime? BookTimeTo { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("lastUpdatedBy")]
        public string LastUpdatedBy { get; set; }
        [JsonProperty("lastUpdatedWhen")]
        public DateTime? LastUpdatedWhen { get; set; }
        [JsonProperty("PromotionSuggestion")]
        public PromotionSuggestion PromotionSuggestion { get; set; }
    }
}
