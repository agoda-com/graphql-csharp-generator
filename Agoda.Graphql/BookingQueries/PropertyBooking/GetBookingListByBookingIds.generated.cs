using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries.PropertyBooking
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetBookingListByBookingIds($bookingIds: [Int!]!) {
  BookingDetailsByBookingIds(bookingIds: $bookingIds) {
    baseBooking {
      bookingId
    }
    propertyBooking {
      checkinDate
      checkoutDate
      bookingHotelRooms {
        roomTypeId
      }
      paymentModel
      guests {
        firstName
        lastName
        guestNo
      }
      property {
        hotelId
      }
      dmc {
        dmcId
      }
      resellBookingFeature {
        smartFlex {
          smartFlexBooking {
            smartFlexScenario
          }
          replacementBooking {
            smartFlexScenario
          }
        }
      }
      acknowledge {
        ackTypeId
      }
      occupancy {
        numberOfAdults
        numberOfChildren
      }
      resellBooking {
        resellBookingId
        resellStatusId
        resellTypeId
        guestInfo {
          guests {
            firstName
            lastName
          }
        }
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
    
    public sealed class BaseBookingInfo 
    {
        [JsonProperty("bookingId")]
        public long BookingId { get; set; }
    }
    
    public sealed class PropertyRoom 
    {
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    public sealed class PropertyGuest 
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("guestNo")]
        public int GuestNo { get; set; }
    }
    
    public sealed class PropertyInfo 
    {
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
    }
    
    public sealed class PropertyDmc 
    {
        [JsonProperty("dmcId")]
        public int? DmcId { get; set; }
    }
    
    public sealed class SmartFlexBookingQueryInput 
    {
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
    }
    
    public sealed class ReplacementBooking 
    {
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
    }
    
    public sealed class SmartFlexQueryInput 
    {
        [JsonProperty("smartFlexBooking")]
        public SmartFlexBookingQueryInput SmartFlexBooking { get; set; }
        [JsonProperty("replacementBooking")]
        public ReplacementBooking ReplacementBooking { get; set; }
    }
    
    public sealed class ResellBookingFeatureQueryInput 
    {
        [JsonProperty("smartFlex")]
        public SmartFlexQueryInput SmartFlex { get; set; }
    }
    
    public sealed class PropertyBookingAcknowledge 
    {
        [JsonProperty("ackTypeId")]
        public int AckTypeId { get; set; }
    }
    
    public sealed class OccupancyInfo 
    {
        [JsonProperty("numberOfAdults")]
        public int NumberOfAdults { get; set; }
        [JsonProperty("numberOfChildren")]
        public int NumberOfChildren { get; set; }
    }
    
    public sealed class BookingGuest 
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
    
    public sealed class EnigmaGuestInfo 
    {
        [JsonProperty("guests")]
        public List<BookingGuest> Guests { get; set; }
    }
    
    public sealed class ResellBookingDetails 
    {
        [JsonProperty("resellBookingId")]
        public int? ResellBookingId { get; set; }
        [JsonProperty("resellStatusId")]
        public int ResellStatusId { get; set; }
        [JsonProperty("resellTypeId")]
        public int? ResellTypeId { get; set; }
        [JsonProperty("guestInfo")]
        public EnigmaGuestInfo GuestInfo { get; set; }
    }
    
    public sealed class PropertyBooking 
    {
        [JsonProperty("checkinDate")]
        public DateTime? CheckinDate { get; set; }
        [JsonProperty("checkoutDate")]
        public DateTime? CheckoutDate { get; set; }
        [JsonProperty("bookingHotelRooms")]
        public List<PropertyRoom> BookingHotelRooms { get; set; }
        [JsonProperty("paymentModel")]
        public int? PaymentModel { get; set; }
        [JsonProperty("guests")]
        public List<PropertyGuest> Guests { get; set; }
        [JsonProperty("property")]
        public PropertyInfo Property { get; set; }
        [JsonProperty("dmc")]
        public PropertyDmc Dmc { get; set; }
        [JsonProperty("resellBookingFeature")]
        public ResellBookingFeatureQueryInput ResellBookingFeature { get; set; }
        [JsonProperty("acknowledge")]
        public PropertyBookingAcknowledge Acknowledge { get; set; }
        [JsonProperty("occupancy")]
        public OccupancyInfo Occupancy { get; set; }
        [JsonProperty("resellBooking")]
        public ResellBookingDetails ResellBooking { get; set; }
    }
    
    public sealed class CrossProductBookingMeta 
    {
        [JsonProperty("baseBooking")]
        public BaseBookingInfo BaseBooking { get; set; }
        [JsonProperty("propertyBooking")]
        public PropertyBooking PropertyBooking { get; set; }
    }

}
