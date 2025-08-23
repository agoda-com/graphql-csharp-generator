using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries
{
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
