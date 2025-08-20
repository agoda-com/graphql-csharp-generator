using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries.PropertyBooking
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetBookingDetailsByBookingId($bookingIds: [Int!]!) {
  BookingDetailsByBookingIds(bookingIds: $bookingIds) {
    propertyBooking {
      bookingId
      acknowledge {
        ackTypeId
        paymentModel
      }
      guests {
        guestNo
        firstName
        lastName
        nationalityName
      }
      summary {
        stayType
      }
      checkinDate
      checkoutDate
      bookingDate
      recCreatedWhen
      recModifiedWhen
      property {
        hotelId
        noOfRooms
        noOfAdults
        noOfChildren
        noOfExtraBeds
        bookingHotelRooms {
          roomTypeId
        }
        otherSpecialNeeds
        propertyDetails {
          country {
            countryId
            gmtOffset
          }
        }
      }
      benefits {
        benefitList: benefits {
          benefitId
          displayText
        }
      }
      memberId
      resellBooking {
        resellStatusId
        resellBookingId
        bookingId
        guestInfo {
          guests {
            firstName
            lastName
          }
        }
      }
      originalResellBooking {
        resellStatusId
        resellBookingId
        bookingId
      }
      resellBookingFeature {
        smartFlex {
          smartFlexBooking {
            smartFlexScenario
            liabilityStatus {
              replacedBookingId
              dateOfStay
              reason
            }
          }
          replacementBooking {
            smartFlexScenario
            originalSmartFlexBooking {
              bookingId
            }
          }
        }
      }
      enigmaAPI {
        contact {
          phoneNumber
        }
      }
      workflow {
        workflowStateId
      }
      dmc {
        dmcId
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
    
    public sealed class PropertyBookingAcknowledge 
    {
        [JsonProperty("ackTypeId")]
        public int AckTypeId { get; set; }
        [JsonProperty("paymentModel")]
        public int PaymentModel { get; set; }
    }
    
    public sealed class PropertyGuest 
    {
        [JsonProperty("guestNo")]
        public int GuestNo { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("nationalityName")]
        public string NationalityName { get; set; }
    }
    
    public sealed class PropertyBookingSummary 
    {
        [JsonProperty("stayType")]
        public int? StayType { get; set; }
    }
    
    public sealed class PropertyRoom 
    {
        [JsonProperty("roomTypeId")]
        public int? RoomTypeId { get; set; }
    }
    
    public sealed class Country 
    {
        [JsonProperty("countryId")]
        public int CountryId { get; set; }
        [JsonProperty("gmtOffset")]
        public int? GmtOffset { get; set; }
    }
    
    public sealed class ProductHotel 
    {
        [JsonProperty("country")]
        public Country Country { get; set; }
    }
    
    public sealed class PropertyInfo 
    {
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        [JsonProperty("noOfRooms")]
        public int NoOfRooms { get; set; }
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
    
    public sealed class Benefit 
    {
        [JsonProperty("benefitId")]
        public int BenefitId { get; set; }
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
    }
    
    public sealed class BookingBenefits 
    {
        [JsonProperty("benefits")]
        public List<Benefit> Benefits { get; set; }
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
        [JsonProperty("resellStatusId")]
        public int ResellStatusId { get; set; }
        [JsonProperty("resellBookingId")]
        public int? ResellBookingId { get; set; }
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        [JsonProperty("guestInfo")]
        public EnigmaGuestInfo GuestInfo { get; set; }
    }
    
    public sealed class ResellBookingDetails 
    {
        [JsonProperty("resellStatusId")]
        public int ResellStatusId { get; set; }
        [JsonProperty("resellBookingId")]
        public int? ResellBookingId { get; set; }
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
    }
    
    public sealed class LiabilityStatus 
    {
        [JsonProperty("replacedBookingId")]
        public int? ReplacedBookingId { get; set; }
        [JsonProperty("dateOfStay")]
        public string DateOfStay { get; set; }
        [JsonProperty("reason")]
        public int? Reason { get; set; }
    }
    
    public sealed class SmartFlexBookingQueryInput 
    {
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
        [JsonProperty("liabilityStatus")]
        public List<LiabilityStatus> LiabilityStatus { get; set; }
    }
    
    public sealed class ResellBookingDetails 
    {
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
    }
    
    public sealed class ReplacementBooking 
    {
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
        [JsonProperty("originalSmartFlexBooking")]
        public List<ResellBookingDetails> OriginalSmartFlexBooking { get; set; }
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
    
    public sealed class CustomerContact 
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
    
    public sealed class EnigmaGuestInfo 
    {
        [JsonProperty("contact")]
        public CustomerContact Contact { get; set; }
    }
    
    public sealed class PropertyWorkflow 
    {
        [JsonProperty("workflowStateId")]
        public int? WorkflowStateId { get; set; }
    }
    
    public sealed class PropertyDmc 
    {
        [JsonProperty("dmcId")]
        public int? DmcId { get; set; }
    }
    
    public sealed class PropertyBooking 
    {
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        [JsonProperty("acknowledge")]
        public PropertyBookingAcknowledge Acknowledge { get; set; }
        [JsonProperty("guests")]
        public List<PropertyGuest> Guests { get; set; }
        [JsonProperty("summary")]
        public PropertyBookingSummary Summary { get; set; }
        [JsonProperty("checkinDate")]
        public DateTime? CheckinDate { get; set; }
        [JsonProperty("checkoutDate")]
        public DateTime? CheckoutDate { get; set; }
        [JsonProperty("bookingDate")]
        public DateTime? BookingDate { get; set; }
        [JsonProperty("recCreatedWhen")]
        public DateTime? RecCreatedWhen { get; set; }
        [JsonProperty("recModifiedWhen")]
        public DateTime? RecModifiedWhen { get; set; }
        [JsonProperty("property")]
        public PropertyInfo Property { get; set; }
        [JsonProperty("benefits")]
        public BookingBenefits Benefits { get; set; }
        [JsonProperty("memberId")]
        public int? MemberId { get; set; }
        [JsonProperty("resellBooking")]
        public ResellBookingDetails ResellBooking { get; set; }
        [JsonProperty("originalResellBooking")]
        public ResellBookingDetails OriginalResellBooking { get; set; }
        [JsonProperty("resellBookingFeature")]
        public ResellBookingFeatureQueryInput ResellBookingFeature { get; set; }
        [JsonProperty("enigmaAPI")]
        public EnigmaGuestInfo EnigmaAPI { get; set; }
        [JsonProperty("workflow")]
        public PropertyWorkflow Workflow { get; set; }
        [JsonProperty("dmc")]
        public PropertyDmc Dmc { get; set; }
    }
    
    public sealed class CrossProductBookingMeta 
    {
        [JsonProperty("propertyBooking")]
        public PropertyBooking PropertyBooking { get; set; }
    }

}
