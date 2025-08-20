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
        public List<BookingDetailsByBookingIds> BookingDetailsByBookingIds { get; set; }
    }
    
    public sealed class BaseBooking 
    {        
        [JsonProperty("bookingId")]
        public long BookingId { get; set; }
    }
    
    public sealed class BookingHotelRooms 
    {        
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    public sealed class Guests 
    {        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        
        [JsonProperty("guestNo")]
        public int GuestNo { get; set; }
    }
    
    public sealed class Property 
    {        
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
    }
    
    public sealed class Dmc 
    {        
        [JsonProperty("dmcId")]
        public int? DmcId { get; set; }
    }
    
    public sealed class SmartFlexBooking 
    {        
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
    }
    
    public sealed class ReplacementBooking 
    {        
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
    }
    
    public sealed class SmartFlex 
    {        
        [JsonProperty("smartFlexBooking")]
        public SmartFlexBooking SmartFlexBooking { get; set; }
        
        [JsonProperty("replacementBooking")]
        public ReplacementBooking ReplacementBooking { get; set; }
    }
    
    public sealed class ResellBookingFeature 
    {        
        [JsonProperty("smartFlex")]
        public SmartFlex SmartFlex { get; set; }
    }
    
    public sealed class Acknowledge 
    {        
        [JsonProperty("ackTypeId")]
        public int AckTypeId { get; set; }
    }
    
    public sealed class Occupancy 
    {        
        [JsonProperty("numberOfAdults")]
        public int NumberOfAdults { get; set; }
        
        [JsonProperty("numberOfChildren")]
        public int NumberOfChildren { get; set; }
    }
    
    public sealed class Guests 
    {        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
    
    public sealed class GuestInfo 
    {        
        [JsonProperty("guests")]
        public List<Guests> Guests { get; set; }
    }
    
    public sealed class ResellBooking 
    {        
        [JsonProperty("resellBookingId")]
        public int? ResellBookingId { get; set; }
        
        [JsonProperty("resellStatusId")]
        public int ResellStatusId { get; set; }
        
        [JsonProperty("resellTypeId")]
        public int? ResellTypeId { get; set; }
        
        [JsonProperty("guestInfo")]
        public GuestInfo GuestInfo { get; set; }
    }
    
    public sealed class PropertyBooking 
    {        
        [JsonProperty("checkinDate")]
        public LocalDateTime? CheckinDate { get; set; }
        
        [JsonProperty("checkoutDate")]
        public LocalDateTime? CheckoutDate { get; set; }
        
        [JsonProperty("bookingHotelRooms")]
        public List<BookingHotelRooms> BookingHotelRooms { get; set; }
        
        [JsonProperty("paymentModel")]
        public int? PaymentModel { get; set; }
        
        [JsonProperty("guests")]
        public List<Guests> Guests { get; set; }
        
        [JsonProperty("property")]
        public Property Property { get; set; }
        
        [JsonProperty("dmc")]
        public Dmc Dmc { get; set; }
        
        [JsonProperty("resellBookingFeature")]
        public ResellBookingFeature ResellBookingFeature { get; set; }
        
        [JsonProperty("acknowledge")]
        public Acknowledge Acknowledge { get; set; }
        
        [JsonProperty("occupancy")]
        public Occupancy Occupancy { get; set; }
        
        [JsonProperty("resellBooking")]
        public ResellBooking ResellBooking { get; set; }
    }
    
    public sealed class BookingDetailsByBookingIds 
    {        
        [JsonProperty("baseBooking")]
        public BaseBooking BaseBooking { get; set; }
        
        [JsonProperty("propertyBooking")]
        public PropertyBooking PropertyBooking { get; set; }
    }
}
