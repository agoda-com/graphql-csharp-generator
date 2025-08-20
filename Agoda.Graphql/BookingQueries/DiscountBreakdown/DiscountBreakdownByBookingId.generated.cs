using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries.DiscountBreakdown
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetDiscountBreakdownByBookingId($bookingId: Long!) {
  DiscountBreakdownByBookingIds(bookingIds: [$bookingId]) {
    bookingId
    discountId
    discountType
    discountName
    discountRateType
    discountRate
    appliedDate
  }
}";

        public long BookingId { get; }

        public Query(long bookingId, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            BookingId = bookingId;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "bookingId", BookingId }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("DiscountBreakdownByBookingIds")]
        public List<DiscountBreakdownByBookingIds> DiscountBreakdownByBookingIds { get; set; }
    }
    
    public sealed class DiscountBreakdownByBookingIds 
    {
        [JsonProperty("bookingId")]
        public long BookingId { get; set; }
        [JsonProperty("discountId")]
        public int DiscountId { get; set; }
        [JsonProperty("discountType")]
        public int DiscountType { get; set; }
        [JsonProperty("discountName")]
        public string DiscountName { get; set; }
        [JsonProperty("discountRateType")]
        public int DiscountRateType { get; set; }
        [JsonProperty("discountRate")]
        public decimal DiscountRate { get; set; }
        [JsonProperty("appliedDate")]
        public List<string> AppliedDate { get; set; }
    }

}
