using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

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
        [JsonProperty("middleName")]
        public string MiddleName { get; set; }
        [JsonProperty("nationalityName")]
        public string NationalityName { get; set; }
    }
    
    public sealed class PropertyRoom 
    {
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    public sealed class ProductHotel 
    {
        [JsonProperty("countryId")]
        public int? CountryId { get; set; }
    }
    
    public sealed class PropertyInfo 
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
        public List<PropertyRoom> BookingHotelRooms { get; set; }
        [JsonProperty("otherSpecialNeeds")]
        public string OtherSpecialNeeds { get; set; }
        [JsonProperty("propertyDetails")]
        public ProductHotel PropertyDetails { get; set; }
    }
    
    public sealed class PropertyBookingAcknowledge 
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
        [JsonProperty("guestInfo")]
        public EnigmaGuestInfo GuestInfo { get; set; }
    }
    
    public sealed class PropertyRateCategory 
    {
        [JsonProperty("rateCategoryId")]
        public int RateCategoryId { get; set; }
    }
    
    public sealed class RateDailyInfo 
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
    
    public sealed class PropertySupplierPayment 
    {
        [JsonProperty("dailyRates")]
        public List<RateDailyInfo> DailyRates { get; set; }
        [JsonProperty("totalNetInclusive")]
        public double? TotalNetInclusive { get; set; }
        [JsonProperty("totalSellInclusive")]
        public double? TotalSellInclusive { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
    
    public sealed class PropertyCancellation 
    {
        [JsonProperty("policyCode")]
        public string PolicyCode { get; set; }
    }
    
    public sealed class Benefit 
    {
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
    }
    
    public sealed class BookingBenefits 
    {
        [JsonProperty("benefits")]
        public List<Benefit> Benefits { get; set; }
    }
    
    public sealed class PropertyBooking 
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
        public List<PropertyGuest> Guests { get; set; }
        [JsonProperty("property")]
        public PropertyInfo Property { get; set; }
        [JsonProperty("acknowledge")]
        public PropertyBookingAcknowledge Acknowledge { get; set; }
        [JsonProperty("resellBookingFeature")]
        public ResellBookingFeatureQueryInput ResellBookingFeature { get; set; }
        [JsonProperty("resellBooking")]
        public ResellBookingDetails ResellBooking { get; set; }
        [JsonProperty("rateCategory")]
        public PropertyRateCategory RateCategory { get; set; }
        [JsonProperty("supplierPayments")]
        public PropertySupplierPayment SupplierPayments { get; set; }
        [JsonProperty("cancellation")]
        public PropertyCancellation Cancellation { get; set; }
        [JsonProperty("benefits")]
        public BookingBenefits Benefits { get; set; }
        [JsonProperty("ycsPromotionText")]
        public string YcsPromotionText { get; set; }
    }
    
    public sealed class PaginationPropertyYcsBooking 
    {
        [JsonProperty("properties")]
        public List<PropertyBooking> Properties { get; set; }
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
