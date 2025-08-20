using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries.DiscountBreakdown
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query BookingDisountsByBookingIds($bookingIds: [Int!]!) {
  result: BookingDetailsByBookingIds(bookingIds: $bookingIds) {
    bookingId
    propertyBooking {
      bookingDiscounts {
        bookingId
        discountType
        discountId
        discountRateType
        discountRate
        discountName
        appliedDate
      }
    }
  }
}";

        public List<int> BookingIds { get; }

        public Query(List<int> bookingIds, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            BookingIds = bookingIds;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "bookingIds", BookingIds }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("BookingDetailsByBookingIds")]
        public List<CrossProductBookingMeta> BookingDetailsByBookingIds { get; set; }
    }
    
    public sealed class BookingDiscountBreakdown 
    {
        [JsonProperty("bookingId")]
        public long BookingId { get; set; }
        [JsonProperty("discountType")]
        public int DiscountType { get; set; }
        [JsonProperty("discountId")]
        public int DiscountId { get; set; }
        [JsonProperty("discountRateType")]
        public int DiscountRateType { get; set; }
        [JsonProperty("discountRate")]
        public decimal DiscountRate { get; set; }
        [JsonProperty("discountName")]
        public string DiscountName { get; set; }
        [JsonProperty("appliedDate")]
        public string AppliedDate { get; set; }
    }
    
    public sealed class PropertyBooking 
    {
        [JsonProperty("bookingDiscounts")]
        public List<BookingDiscountBreakdown> BookingDiscounts { get; set; }
    }
    
    public sealed class CrossProductBookingMeta 
    {
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        [JsonProperty("propertyBooking")]
        public PropertyBooking PropertyBooking { get; set; }
    }

}
