using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;
using Agoda.Graphql.BookingQueries;

namespace Agoda.Graphql.BookingQueries.PropertyBooking
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetPropertyBookingByYCSCriteria($ycsCriteria: YCSCriteria!) {
  PropertyBookingByYCSCriteria(YCSCriteria: $ycsCriteria) {
    properties {
      bookingId
      checkinDate
      checkoutDate
      paymentModel
      guests {
        guestNo
        firstName
        lastName
      }
      property {
        hotelId
        noOfAdults
        noOfChildren
        bookingHotelRooms {
          roomTypeId
        }
      }
      acknowledge {
        ackTypeId
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
      dmc {
        dmcId
      }
      summary {
        whitelabelId
        stayType
      }
    }
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
    
    public sealed class Guests 
    {
        [JsonProperty("guestNo")]
        public int GuestNo { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
    
    public sealed class BookingHotelRooms 
    {
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    public sealed class Property 
    {
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        [JsonProperty("noOfAdults")]
        public int NoOfAdults { get; set; }
        [JsonProperty("noOfChildren")]
        public int NoOfChildren { get; set; }
        [JsonProperty("bookingHotelRooms")]
        public List<BookingHotelRooms> BookingHotelRooms { get; set; }
    }
    
    public sealed class Acknowledge 
    {
        [JsonProperty("ackTypeId")]
        public int AckTypeId { get; set; }
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
    
    public sealed class GuestInfoGuests 
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
    
    public sealed class GuestInfo 
    {
        [JsonProperty("guests")]
        public List<GuestInfoGuests> Guests { get; set; }
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
    
    public sealed class Dmc 
    {
        [JsonProperty("dmcId")]
        public int? DmcId { get; set; }
    }
    
    public sealed class Summary 
    {
        [JsonProperty("whitelabelId")]
        public int? WhitelabelId { get; set; }
        [JsonProperty("stayType")]
        public int? StayType { get; set; }
    }
    
    public sealed class Properties 
    {
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        [JsonProperty("checkinDate")]
        public DateTime? CheckinDate { get; set; }
        [JsonProperty("checkoutDate")]
        public DateTime? CheckoutDate { get; set; }
        [JsonProperty("paymentModel")]
        public int? PaymentModel { get; set; }
        [JsonProperty("guests")]
        public List<Guests> Guests { get; set; }
        [JsonProperty("property")]
        public Property Property { get; set; }
        [JsonProperty("acknowledge")]
        public Acknowledge Acknowledge { get; set; }
        [JsonProperty("resellBookingFeature")]
        public ResellBookingFeature ResellBookingFeature { get; set; }
        [JsonProperty("resellBooking")]
        public ResellBooking ResellBooking { get; set; }
        [JsonProperty("dmc")]
        public Dmc Dmc { get; set; }
        [JsonProperty("summary")]
        public Summary Summary { get; set; }
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
        [JsonProperty("properties")]
        public List<Properties> Properties { get; set; }
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
