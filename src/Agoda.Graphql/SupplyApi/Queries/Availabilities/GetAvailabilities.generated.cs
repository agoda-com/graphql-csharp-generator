using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.Availabilities
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetAvailabilities($propertyId: String!, $dmcId: Int!, $roomTypeId: String, $startDate: Date!, $endDate: Date!) {
  Availabilities(
    hotelId: $propertyId
    dmcId: $dmcId
    roomTypeId: $roomTypeId
    startDate: $startDate
    endDate: $endDate
  ) {
    propertyId
    roomId
    dmcId
    date
    regularAllotment
    regularAllotmentUsed
    guaranteedAllotment
    guaranteedAllotmentUsed
    createdBy
    createdWhen
    modifiedBy
    modifiedWhen
  }
}";

        public string PropertyId { get; }
        public int DmcId { get; }
        public string RoomTypeId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public Query(string propertyId, int dmcId, string roomTypeId, DateTime startDate, DateTime endDate, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            PropertyId = propertyId;
            DmcId = dmcId;
            RoomTypeId = roomTypeId;
            StartDate = startDate;
            EndDate = endDate;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "propertyId", PropertyId },
            { "dmcId", DmcId },
            { "roomTypeId", RoomTypeId },
            { "startDate", StartDate.ToString("yyyy-MM-dd") },
            { "endDate", EndDate.ToString("yyyy-MM-dd") }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("Availabilities")]
        public List<Availabilities> Availabilities { get; set; }
    }
    
    public sealed class Availabilities 
    {        
        [JsonProperty("propertyId")]
        public string PropertyId { get; set; }
        
        [JsonProperty("roomId")]
        public string RoomId { get; set; }
        
        [JsonProperty("dmcId")]
        public int DmcId { get; set; }
        
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        
        [JsonProperty("regularAllotment")]
        public int? RegularAllotment { get; set; }
        
        [JsonProperty("regularAllotmentUsed")]
        public int? RegularAllotmentUsed { get; set; }
        
        [JsonProperty("guaranteedAllotment")]
        public int? GuaranteedAllotment { get; set; }
        
        [JsonProperty("guaranteedAllotmentUsed")]
        public int? GuaranteedAllotmentUsed { get; set; }
        
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        
        [JsonProperty("createdWhen")]
        public DateTime CreatedWhen { get; set; }
        
        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }
        
        [JsonProperty("modifiedWhen")]
        public DateTime? ModifiedWhen { get; set; }
    }
}
