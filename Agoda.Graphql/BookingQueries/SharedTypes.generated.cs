using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries
{
    public sealed class BookingDisountsByBookingIds
    {
        [JsonProperty("BookingDetailsByBookingIds")]
        public BookingDetailsByBookingIds BookingDetailsByBookingIds { get; set; }

    }

    public sealed class BookingDetailsByBookingIds
    {
        [JsonProperty("bookingId")]
        public String BookingId { get; set; }

        [JsonProperty("propertyBooking")]
        public PropertyBooking PropertyBooking { get; set; }

    }

    public sealed class PropertyBooking
    {
        [JsonProperty("bookingDiscounts")]
        public BookingDiscounts BookingDiscounts { get; set; }

    }

    public sealed class BookingDiscounts
    {
        [JsonProperty("bookingId")]
        public String BookingId { get; set; }

        [JsonProperty("discountType")]
        public String DiscountType { get; set; }

        [JsonProperty("discountId")]
        public String DiscountId { get; set; }

        [JsonProperty("discountRateType")]
        public String DiscountRateType { get; set; }

        [JsonProperty("discountRate")]
        public String DiscountRate { get; set; }

        [JsonProperty("discountName")]
        public String DiscountName { get; set; }

        [JsonProperty("appliedDate")]
        public String AppliedDate { get; set; }

    }


}
