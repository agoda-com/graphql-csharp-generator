using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.Rooms.GetListRoomByPropertyId
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query getList($hotelId: Int!, $recStatusCore: String!, $recStatusYcs: String!) {
  RoomsByPropertyAndRecStatus(
    propertyId: $hotelId
    recStatusCore: $recStatusCore
    recStatusYcs: $recStatusYcs
  ) {
    id
    dmcRoomId
    allotmentAutoTopup
    hotelRoomTypeId
    noOfRoom
    hotelId
    hotelRoomTypeYcsFlag
    masterRoomTypeId
    maxOccupancy
    maxAdultsOccupancy
    inventoryTypeId
  }
}";

        public int HotelId { get; }
        public string RecStatusCore { get; }
        public string RecStatusYcs { get; }

        public Query(int hotelId, string recStatusCore, string recStatusYcs, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            HotelId = hotelId;
            RecStatusCore = recStatusCore;
            RecStatusYcs = recStatusYcs;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "hotelId", HotelId },
            { "recStatusCore", RecStatusCore },
            { "recStatusYcs", RecStatusYcs }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("RoomsByPropertyAndRecStatus")]
        public List<RoomsByPropertyAndRecStatus> RoomsByPropertyAndRecStatus { get; set; }
    }
    
    public sealed class RoomsByPropertyAndRecStatus 
    {        
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("dmcRoomId")]
        public string DmcRoomId { get; set; }
        
        [JsonProperty("allotmentAutoTopup")]
        public int? AllotmentAutoTopup { get; set; }
        
        [JsonProperty("hotelRoomTypeId")]
        public int HotelRoomTypeId { get; set; }
        
        [JsonProperty("noOfRoom")]
        public int? NoOfRoom { get; set; }
        
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        
        [JsonProperty("hotelRoomTypeYcsFlag")]
        public int? HotelRoomTypeYcsFlag { get; set; }
        
        [JsonProperty("masterRoomTypeId")]
        public int? MasterRoomTypeId { get; set; }
        
        [JsonProperty("maxOccupancy")]
        public int? MaxOccupancy { get; set; }
        
        [JsonProperty("maxAdultsOccupancy")]
        public int? MaxAdultsOccupancy { get; set; }
        
        [JsonProperty("inventoryTypeId")]
        public int? InventoryTypeId { get; set; }
    }
}
