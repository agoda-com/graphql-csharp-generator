using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.GetRoomById
{
    public partial class Query : QueryBase<Data>
    {
        private const string _query = @"query GetRoomById($roomTypeId: Int!, $fetchFromCdb: Boolean) {
  Room(roomTypeID: $roomTypeId, fetchFromCDB: $fetchFromCdb) {
    id
    dmcRoomId
    allotmentAutoTopup
    noOfRoom
    hotelRoomTypeId
    inventoryTypeId
  }
}";

        public int RoomTypeId { get; }
        public bool? FetchFromCdb { get; }

        public Query(int roomTypeId, bool? fetchFromCdb, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            RoomTypeId = roomTypeId;
            FetchFromCdb = fetchFromCdb;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "roomTypeId", RoomTypeId },
            { "fetchFromCdb", FetchFromCdb }
        };
    }

    public sealed class Data
    {        
        
        [JsonProperty("Room")]
        public Room Room { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class Room 
    {
                
        
        [JsonProperty("id")]
        public int Id { get; set; }
        
        
        [JsonProperty("dmcRoomId")]
        public string DmcRoomId { get; set; }
        
        
        [JsonProperty("allotmentAutoTopup")]
        public int? AllotmentAutoTopup { get; set; }
        
        
        [JsonProperty("noOfRoom")]
        public int? NoOfRoom { get; set; }
        
        
        [JsonProperty("hotelRoomTypeId")]
        public int HotelRoomTypeId { get; set; }
        
        
        [JsonProperty("inventoryTypeId")]
        public int? InventoryTypeId { get; set; }
    }
}
