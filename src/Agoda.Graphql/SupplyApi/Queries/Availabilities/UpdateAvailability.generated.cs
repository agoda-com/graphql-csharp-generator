using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.Availabilities
{
    public class Mutation : QueryBase<Data>
    {
        private const string _query = @"mutation UpdateAvailability($dmcId: Int!, $hotelId: String!, $roomTypeId: String!, $userId: String!, $startDate: Date!, $endDate: Date!, $regularAllotment: Int, $guaranteedAllotment: Int, $dayOfWeek: [Int!]) {
  AvailabilityMutation(
    dmcId: $dmcId
    hotelId: $hotelId
    roomTypeId: $roomTypeId
    endDate: $endDate
    startDate: $startDate
    userId: $userId
    regularAllotment: $regularAllotment
    guaranteedAllotment: $guaranteedAllotment
    dayOfWeek: $dayOfWeek
  ) {
    regularAllotment
    guaranteedAllotment
    guaranteedAllotmentUsed
    regularAllotmentUsed
    date
    propertyId
    dmcId
    roomId
    createdWhen
    createdBy
    modifiedWhen
    modifiedBy
  }
}";

        public int DmcId { get; }
        public string HotelId { get; }
        public string RoomTypeId { get; }
        public string UserId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int? RegularAllotment { get; }
        public int? GuaranteedAllotment { get; }
        public List<int>? DayOfWeek { get; }

        public Mutation(int dmcId, string hotelId, string roomTypeId, string userId, DateTime startDate, DateTime endDate, int? regularAllotment, int? guaranteedAllotment, List<int>? dayOfWeek, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            DmcId = dmcId;
            HotelId = hotelId;
            RoomTypeId = roomTypeId;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
            RegularAllotment = regularAllotment;
            GuaranteedAllotment = guaranteedAllotment;
            DayOfWeek = dayOfWeek;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "dmcId", DmcId },
            { "hotelId", HotelId },
            { "roomTypeId", RoomTypeId },
            { "userId", UserId },
            { "startDate", StartDate.ToString("yyyy-MM-dd") },
            { "endDate", EndDate.ToString("yyyy-MM-dd") },
            { "regularAllotment", RegularAllotment },
            { "guaranteedAllotment", GuaranteedAllotment },
            { "dayOfWeek", DayOfWeek }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("AvailabilityMutation")]
        public List<AvailabilityMutation> AvailabilityMutation { get; set; }
    }
    
    public sealed class AvailabilityMutation 
    {        
        [JsonProperty("regularAllotment")]
        public int? RegularAllotment { get; set; }
        
        [JsonProperty("guaranteedAllotment")]
        public int? GuaranteedAllotment { get; set; }
        
        [JsonProperty("guaranteedAllotmentUsed")]
        public int? GuaranteedAllotmentUsed { get; set; }
        
        [JsonProperty("regularAllotmentUsed")]
        public int? RegularAllotmentUsed { get; set; }
        
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        
        [JsonProperty("propertyId")]
        public string PropertyId { get; set; }
        
        [JsonProperty("dmcId")]
        public int DmcId { get; set; }
        
        [JsonProperty("roomId")]
        public string RoomId { get; set; }
        
        [JsonProperty("createdWhen")]
        public DateTime CreatedWhen { get; set; }
        
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        
        [JsonProperty("modifiedWhen")]
        public DateTime? ModifiedWhen { get; set; }
        
        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }
    }
}
