using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

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
        public PaginationPropertyYcsBooking PropertyBookingByYCSCriteria { get; set; }
    }
    
    public sealed class PropertyGuest 
    {
        [JsonProperty("guestNo")]
        public int GuestNo { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
    
    public sealed class PropertyRoom 
    {
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    public sealed class PropertyInfo 
    {
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        [JsonProperty("noOfAdults")]
        public int NoOfAdults { get; set; }
        [JsonProperty("noOfChildren")]
        public int NoOfChildren { get; set; }
        [JsonProperty("bookingHotelRooms")]
        public List<PropertyRoom> BookingHotelRooms { get; set; }
    }
    
    public sealed class PropertyBookingAcknowledge 
    {
        [JsonProperty("ackTypeId")]
        public int AckTypeId { get; set; }
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
    
    public sealed class PropertyDmc 
    {
        [JsonProperty("dmcId")]
        public int? DmcId { get; set; }
    }
    
    public sealed class PropertyBookingSummary 
    {
        [JsonProperty("whitelabelId")]
        public int? WhitelabelId { get; set; }
        [JsonProperty("stayType")]
        public int? StayType { get; set; }
    }
    
    public sealed class PropertyBooking 
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
        public List<PropertyGuest> Guests { get; set; }
        [JsonProperty("property")]
        public PropertyInfo Property { get; set; }
        [JsonProperty("acknowledge")]
        public PropertyBookingAcknowledge Acknowledge { get; set; }
        [JsonProperty("resellBookingFeature")]
        public ResellBookingFeatureQueryInput ResellBookingFeature { get; set; }
        [JsonProperty("resellBooking")]
        public ResellBookingDetails ResellBooking { get; set; }
        [JsonProperty("dmc")]
        public PropertyDmc Dmc { get; set; }
        [JsonProperty("summary")]
        public PropertyBookingSummary Summary { get; set; }
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
    
    public sealed class PaginationPropertyYcsBooking 
    {
        [JsonProperty("properties")]
        public List<PropertyBooking> Properties { get; set; }
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
        public DateRange? BookingDatePeriod { get; set; }
        [JsonProperty("bookingId")]
        public int? BookingId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        [JsonProperty("lastUpdateDatePeriod")]
        public DateRange? LastUpdateDatePeriod { get; set; }
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
        public DateRange? StayDatePeriod { get; set; }
        [JsonProperty("whitelistDmcIds")]
        public List<int>? WhitelistDmcIds { get; set; }
    }
}
