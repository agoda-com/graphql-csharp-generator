using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;




#region GetCsvDataWithoutPagination 

namespace Agoda.Graphql.BookingQueries.PropertyBooking.GetCsvDataWithoutPagination
{

    /// <summary>Operation Type</summary>
    public partial class Query : QueryBase<Data>
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

        public Query(YcsCriteria ycsCriteria, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            YcsCriteria = ycsCriteria;
        }
        
        public YcsCriteria YcsCriteria { get; }
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "ycsCriteria", YcsCriteria },
        };        
    }

    public sealed class Data
    {
        
        
        [JsonProperty("PropertyBookingByYCSCriteria")]
        public PropertyBookingByYcsCriteria PropertyBookingByYcsCriteria { get; set; }
    }

    
    /// <summary>Inner Model</summary> 
    public sealed class PropertyBookingByYcsCriteria 
    {
        
        
        [JsonProperty("properties")]
        public List<Properties> Properties { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class Properties 
    {
        
        
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        
        
        [JsonProperty("bookingDate")]
        public LocalDateTime? BookingDate { get; set; }
        
        
        [JsonProperty("checkinDate")]
        public LocalDateTime? CheckinDate { get; set; }
        
        
        [JsonProperty("checkoutDate")]
        public LocalDateTime? CheckoutDate { get; set; }
        
        
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
    
    /// <summary>Inner Model</summary> 
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
    
    /// <summary>Inner Model</summary> 
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
    
    /// <summary>Inner Model</summary> 
    public sealed class BookingHotelRooms 
    {
        
        
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class PropertyDetails 
    {
        
        
        [JsonProperty("countryId")]
        public int? CountryId { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
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
    
    /// <summary>Inner Model</summary> 
    public sealed class ResellBookingFeature 
    {
        
        
        [JsonProperty("smartFlex")]
        public SmartFlex SmartFlex { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class SmartFlex 
    {
        
        
        [JsonProperty("smartFlexBooking")]
        public SmartFlexBooking SmartFlexBooking { get; set; }
        
        
        [JsonProperty("replacementBooking")]
        public ReplacementBooking ReplacementBooking { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class SmartFlexBooking 
    {
        
        
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class ReplacementBooking 
    {
        
        
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class ResellBooking 
    {
        
        
        [JsonProperty("resellBookingId")]
        public int? ResellBookingId { get; set; }
        
        
        [JsonProperty("resellStatusId")]
        public int ResellStatusId { get; set; }
        
        
        [JsonProperty("guestInfo")]
        public GuestInfo GuestInfo { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class GuestInfo 
    {
        
        
        [JsonProperty("guests")]
        public List<_Guests> Guests { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class _Guests 
    {
        
        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        
        
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class RateCategory 
    {
        
        
        [JsonProperty("rateCategoryId")]
        public int RateCategoryId { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class SupplierPayments 
    {
        
        
        [JsonProperty("dailyRates")]
        public List<DailyRates> DailyRates { get; set; }
        
        
        [JsonProperty("totalNetInclusive")]
        public float? TotalNetInclusive { get; set; }
        
        
        [JsonProperty("totalSellInclusive")]
        public float? TotalSellInclusive { get; set; }
        
        
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class DailyRates 
    {
        
        
        [JsonProperty("withHoldingTax")]
        public float? WithHoldingTax { get; set; }
        
        
        [JsonProperty("normalAndWithholdingTax")]
        public float? NormalAndWithholdingTax { get; set; }
        
        
        [JsonProperty("refSell")]
        public float? RefSell { get; set; }
        
        
        [JsonProperty("refComm")]
        public float? RefComm { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class Cancellation 
    {
        
        
        [JsonProperty("policyCode")]
        public string PolicyCode { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class Benefits 
    {
        
        
        [JsonProperty("benefitList")]
        public List<BenefitList> BenefitList { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
    public sealed class BenefitList 
    {
        
        
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
    }
}

#endregion

