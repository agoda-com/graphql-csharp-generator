using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries.PropertyBooking
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetCsvMetaData($ycsCriteria: YCSCriteria!) {
  PropertyBookingByYCSCriteria(YCSCriteria: $ycsCriteria) {
    pagination {
      page
      pageSize
      totalPages
      totalRecords
    }
  }
}";

        public YCSCriteria YcsCriteria { get; }

        public Query(YCSCriteria ycsCriteria, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            YcsCriteria = ycsCriteria;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "ycsCriteria", YcsCriteria }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("PropertyBookingByYCSCriteria")]
        public PropertyBookingByYCSCriteria PropertyBookingByYCSCriteria { get; set; }
    }
    
    public sealed class Pagination 
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }
    }
    
    public sealed class PropertyBookingByYCSCriteria 
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
    
    public enum AcknowledgementRequestType 
    {
        All,
        AmendBooking,
        CancelBooking,
        ConfirmBooking
    }
    public sealed class DateRange 
    {
        [JsonProperty("from")]
        public DateTime From { get; set; }
        [JsonProperty("to")]
        public DateTime To { get; set; }
    }
    public sealed class YCSCriteria 
    {
        [JsonProperty("ackRequestTypes")]
        public List<AcknowledgementRequestType>? AckRequestTypes { get; set; }
        [JsonProperty("acknowledgementId")]
        public string AcknowledgementId { get; set; }
        [JsonProperty("blacklistDmcIds")]
        public List<int>? BlacklistDmcIds { get; set; }
        [JsonProperty("bookingDatePeriod")]
        public DateRange BookingDatePeriod { get; set; }
        [JsonProperty("bookingId")]
        public int? BookingId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        [JsonProperty("lastUpdateDatePeriod")]
        public DateRange LastUpdateDatePeriod { get; set; }
        [JsonProperty("pageIndex")]
        public int? PageIndex { get; set; }
        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }
        [JsonProperty("rateChannel")]
        public int? RateChannel { get; set; }
        [JsonProperty("ratePlan")]
        public int? RatePlan { get; set; }
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
        [JsonProperty("sorting")]
        public string Sorting { get; set; }
        [JsonProperty("stayDatePeriod")]
        public DateRange StayDatePeriod { get; set; }
        [JsonProperty("whitelistDmcIds")]
        public List<int>? WhitelistDmcIds { get; set; }
    }
}
