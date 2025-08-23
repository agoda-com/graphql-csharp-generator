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
        private const string _query = @"query GetCsvDataWithoutPagination($ycsCriteria: YCSCriteria!) {
  PropertyBookingByYCSCriteria(YCSCriteria: $ycsCriteria) {
    properties {
      bookingId
      bookingDate
      checkinDate
      checkoutDate
      paymentModel
      guests {
        guestNo
        firstName
        lastName
        middleName
        nationalityName
      }
      property {
        hotelId
        noOfAdults
        noOfChildren
        noOfExtraBeds
        bookingHotelRooms {
          roomTypeId
        }
        otherSpecialNeeds
        propertyDetails {
          countryId
        }
      }
      acknowledge {
        isAcknowledged
        ackId
        ackTypeId
        rateChannelId
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
        guestInfo {
          guests {
            firstName
            lastName
          }
        }
      }
      rateCategory {
        rateCategoryId
      }
      supplierPayments {
        dailyRates {
          withHoldingTax
          normalAndWithholdingTax
          refSell
          refComm
        }
        totalNetInclusive
        totalSellInclusive
        currencyCode
      }
      cancellation {
        policyCode
      }
      benefits {
        benefitList: benefits {
          displayText
        }
      }
      ycsPromotionText
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
        [JsonProperty("middleName")]
        public string MiddleName { get; set; }
        [JsonProperty("nationalityName")]
        public string NationalityName { get; set; }
    }
    
    public sealed class BookingHotelRooms 
    {
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    public sealed class PropertyDetails 
    {
        [JsonProperty("countryId")]
        public int? CountryId { get; set; }
    }
    
    public sealed class Property 
    {
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        [JsonProperty("noOfAdults")]
        public int NoOfAdults { get; set; }
        [JsonProperty("noOfChildren")]
        public int NoOfChildren { get; set; }
        [JsonProperty("noOfExtraBeds")]
        public int? NoOfExtraBeds { get; set; }
        [JsonProperty("bookingHotelRooms")]
        public List<BookingHotelRooms> BookingHotelRooms { get; set; }
        [JsonProperty("otherSpecialNeeds")]
        public string OtherSpecialNeeds { get; set; }
        [JsonProperty("propertyDetails")]
        public PropertyDetails PropertyDetails { get; set; }
    }
    
    public sealed class Acknowledge 
    {
        [JsonProperty("isAcknowledged")]
        public bool IsAcknowledged { get; set; }
        [JsonProperty("ackId")]
        public string AckId { get; set; }
        [JsonProperty("ackTypeId")]
        public int AckTypeId { get; set; }
        [JsonProperty("rateChannelId")]
        public int? RateChannelId { get; set; }
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
        [JsonProperty("guestInfo")]
        public GuestInfo GuestInfo { get; set; }
    }
    
    public sealed class RateCategory 
    {
        [JsonProperty("rateCategoryId")]
        public int RateCategoryId { get; set; }
    }
    
    public sealed class DailyRates 
    {
        [JsonProperty("withHoldingTax")]
        public double? WithHoldingTax { get; set; }
        [JsonProperty("normalAndWithholdingTax")]
        public double? NormalAndWithholdingTax { get; set; }
        [JsonProperty("refSell")]
        public double? RefSell { get; set; }
        [JsonProperty("refComm")]
        public double? RefComm { get; set; }
    }
    
    public sealed class SupplierPayments 
    {
        [JsonProperty("dailyRates")]
        public List<DailyRates> DailyRates { get; set; }
        [JsonProperty("totalNetInclusive")]
        public double? TotalNetInclusive { get; set; }
        [JsonProperty("totalSellInclusive")]
        public double? TotalSellInclusive { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
    
    public sealed class Cancellation 
    {
        [JsonProperty("policyCode")]
        public string PolicyCode { get; set; }
    }
    
    public sealed class BenefitList 
    {
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
    }
    
    public sealed class Benefits 
    {
        [JsonProperty("benefitList")]
        public List<BenefitList> BenefitList { get; set; }
    }
    
    public sealed class Properties 
    {
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        [JsonProperty("bookingDate")]
        public DateTime? BookingDate { get; set; }
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
        [JsonProperty("rateCategory")]
        public RateCategory RateCategory { get; set; }
        [JsonProperty("supplierPayments")]
        public SupplierPayments SupplierPayments { get; set; }
        [JsonProperty("cancellation")]
        public Cancellation Cancellation { get; set; }
        [JsonProperty("benefits")]
        public Benefits Benefits { get; set; }
        [JsonProperty("ycsPromotionText")]
        public string YcsPromotionText { get; set; }
    }
    
    public sealed class PropertyBookingByYCSCriteria 
    {
        [JsonProperty("properties")]
        public List<Properties> Properties { get; set; }
    }
}
