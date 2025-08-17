using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.HotelCompetitorByHotelId
{
    public partial class Query : QueryBase<Data>
    {
        private const string _query = @"query HotelCompetitorByHotelId($id: Int!, $languageId: Int!) {
  HotelCompetitorByHotelId(id: $id) {
    competitorId
    orderNo
    settingDate
    recStatus
    Property {
      id
      name
      cityId
      ProductHotelsLanguage(languageId: $languageId) {
        hotelName
      }
    }
  }
}";

        public int Id { get; }
        public int LanguageId { get; }

        public Query(int id, int languageId, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            Id = id;
            LanguageId = languageId;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "id", Id },
            { "languageId", LanguageId }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("HotelCompetitorByHotelId")]
        public List<HotelCompetitorByHotelId> HotelCompetitorByHotelId { get; set; }
    }
    
    public sealed class ProductHotelsLanguage 
    {        
        [JsonProperty("hotelName")]
        public string HotelName { get; set; }
    }
    
    public sealed class Property 
    {        
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("cityId")]
        public int? CityId { get; set; }
        
        [JsonProperty("ProductHotelsLanguage")]
        public ProductHotelsLanguage ProductHotelsLanguage { get; set; }
    }
    
    public sealed class HotelCompetitorByHotelId 
    {        
        [JsonProperty("competitorId")]
        public int CompetitorId { get; set; }
        
        [JsonProperty("orderNo")]
        public int OrderNo { get; set; }
        
        [JsonProperty("settingDate")]
        public DateTime SettingDate { get; set; }
        
        [JsonProperty("recStatus")]
        public int RecStatus { get; set; }
        
        [JsonProperty("Property")]
        public Property Property { get; set; }
    }
}
