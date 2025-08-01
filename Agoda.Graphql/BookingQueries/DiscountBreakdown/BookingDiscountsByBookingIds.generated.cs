using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;




#region BookingDisountsByBookingIds 

namespace Agoda.Graphql.BookingQueries.DiscountBreakdown.BookingDiscountsByBookingIds
{

    /// <summary>Operation Type</summary>
    public partial class Query : QueryBase<Data>
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

        public Query(List<int> bookingIds, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            BookingIds = bookingIds;
        }
        
        public List<int> BookingIds { get; }
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "bookingIds", BookingIds },
        };        
    }

    public sealed class Data
    {
        
        
        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }

    
    /// <summary>Inner Model</summary> 
    public sealed class Result 
    {
        
        
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        
        
        [JsonProperty("propertyBooking")]
        public PropertyBooking PropertyBooking { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class PropertyBooking 
    {
        
        
        [JsonProperty("bookingDiscounts")]
        public List<BookingDiscounts> BookingDiscounts { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class BookingDiscounts 
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
}

#endregion

