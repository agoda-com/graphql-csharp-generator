using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;

namespace Agoda.Graphql.BookingQueries
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
        public List<BookingDetailsByBookingIds> BookingDetailsByBookingIds { get; set; }
    }
    
    public sealed class Acknowledge 
    {
        [JsonProperty("ackTypeId")]
        public int AckTypeId { get; set; }
        [JsonProperty("paymentModel")]
        public int PaymentModel { get; set; }
    }
    
    public sealed class Guests 
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
    
    public sealed class Summary 
    {
        [JsonProperty("stayType")]
        public int? StayType { get; set; }
    }
    
    public sealed class BookingHotelRooms 
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
    
    public sealed class PropertyDetails 
    {
        [JsonProperty("country")]
        public Country Country { get; set; }
    }
    
    public sealed class Property 
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
        public List<BookingHotelRooms> BookingHotelRooms { get; set; }
        [JsonProperty("otherSpecialNeeds")]
        public string OtherSpecialNeeds { get; set; }
        [JsonProperty("propertyDetails")]
        public PropertyDetails PropertyDetails { get; set; }
    }
    
    public sealed class BenefitList 
    {
        [JsonProperty("benefitId")]
        public int BenefitId { get; set; }
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
    }
    
    public sealed class Benefits 
    {
        [JsonProperty("benefits")]
        public List<BenefitList> BenefitList { get; set; }
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
        [JsonProperty("resellStatusId")]
        public int ResellStatusId { get; set; }
        [JsonProperty("resellBookingId")]
        public int? ResellBookingId { get; set; }
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        [JsonProperty("guestInfo")]
        public GuestInfo GuestInfo { get; set; }
    }
    
    public sealed class OriginalResellBooking 
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
    
    public sealed class SmartFlexBooking 
    {
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
        [JsonProperty("liabilityStatus")]
        public List<LiabilityStatus> LiabilityStatus { get; set; }
    }
    
    public sealed class OriginalSmartFlexBooking 
    {
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
    }
    
    public sealed class ReplacementBooking 
    {
        [JsonProperty("smartFlexScenario")]
        public int? SmartFlexScenario { get; set; }
        [JsonProperty("originalSmartFlexBooking")]
        public List<OriginalSmartFlexBooking> OriginalSmartFlexBooking { get; set; }
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
    
    public sealed class Contact 
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
    
    public sealed class EnigmaAPI 
    {
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
    
    public sealed class Workflow 
    {
        [JsonProperty("workflowStateId")]
        public int? WorkflowStateId { get; set; }
    }
    
    public sealed class Dmc 
    {
        [JsonProperty("dmcId")]
        public int? DmcId { get; set; }
    }
    
    public sealed class PropertyBooking 
    {
        [JsonProperty("bookingId")]
        public int BookingId { get; set; }
        [JsonProperty("acknowledge")]
        public Acknowledge Acknowledge { get; set; }
        [JsonProperty("guests")]
        public List<Guests> Guests { get; set; }
        [JsonProperty("summary")]
        public Summary Summary { get; set; }
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
        public Property Property { get; set; }
        [JsonProperty("benefits")]
        public Benefits Benefits { get; set; }
        [JsonProperty("memberId")]
        public int? MemberId { get; set; }
        [JsonProperty("resellBooking")]
        public ResellBooking ResellBooking { get; set; }
        [JsonProperty("originalResellBooking")]
        public OriginalResellBooking OriginalResellBooking { get; set; }
        [JsonProperty("resellBookingFeature")]
        public ResellBookingFeature ResellBookingFeature { get; set; }
        [JsonProperty("enigmaAPI")]
        public EnigmaAPI EnigmaAPI { get; set; }
        [JsonProperty("workflow")]
        public Workflow Workflow { get; set; }
        [JsonProperty("dmc")]
        public Dmc Dmc { get; set; }
    }
    
    public sealed class BookingDetailsByBookingIds 
    {
        [JsonProperty("propertyBooking")]
        public PropertyBooking PropertyBooking { get; set; }
    }

}
