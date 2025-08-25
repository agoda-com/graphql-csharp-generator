using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Agoda.Graphql;

namespace Agoda.Graphql.SupplyApi.Queries.Promotion.GetHotelPromotionsByHotel
{
    public class Query : QueryBase<Data>
    {
        private const string _query = @"query GetHotelPromotionsByHotel($hotelId: Int!, $isActive: Boolean, $getExpired: Boolean, $bookDateToAfter: DateTime, $languageId: Int) {
  HotelPromotionsByHotel(
    hotelId: $hotelId
    isActive: $isActive
    getExpired: $getExpired
    bookDateToAfter: $bookDateToAfter
  ) {
    id
    hotelId
    promotionTypeId
    minAdvPurchase
    maxAdvPurchase
    bookOn
    stayOn
    checkInOn
    minNightsStay
    maxNightsStay
    bookDateFrom
    bookDateTo
    bookTimeFrom
    bookTimeTo
    stayDateFrom
    stayDateTo
    minRooms
    promotionDiscountTypeId
    discValueMon1
    discValueTue2
    discValueWed3
    discValueThu4
    discValueFri5
    discValueSat6
    discValueSun7
    cancellationPolicyId
    isActive
    isRecurringBenefit
    specificNight
    externalCode
    description
    isStackable
    createdWhen
    createdBy
    lastUpdatedWhen
    lastUpdatedBy
    refSourceId
    promotionSourceTypeId
    isStackableCombine
    stackableDiscountType
    isIncludeCustomerSegment
    isIncludeChannel
    isIncludeRatecategory
    isIncludeRoom
    isApplyChannelDiscount
    isOfflineType
    PromotionType {
      id
      name
      cmsIdHotel
      cmsIdWebsite
      cmsIdPartners
      recStatus
      recCreatedBy
      recCreatedWhen
      recModifiedBy
      recModifiedWhen
      isShortcut
      isSpecial
      icon
      isDisplayExpiration
      isNha
    }
    PromotionRooms {
      hotelPromotionRoomTypeId
      hotelPromotionId
      hotelRoomTypeId
    }
    PromotionRatecategories {
      hotelPromotionRatecategoryId
      hotelPromotionId
      hotelRatecategoryId
    }
    PromotionCustomerSegments {
      hotelPromotionCustomerSegmentId
      hotelPromotionId
      customerSegmentGroupId
    }
    PromotionChannels {
      hotelPromotionChannelId
      hotelPromotionId
      channelId
    }
    PromotionBlackouts {
      hotelPromotionBlackoutId
      hotelPromotionId
      stayDateFrom
      stayDateTo
    }
    CancellationPolicyForYCS {
      id
      code
      name
      description
      noticeDay1
      chargeType1
      chargeValue1
      noticeDay2
      chargeType2
      chargeValue2
      chargeNoShowType
      chargeNoShowValue
      recStatus
      recCreatedBy
      recCreatedWhen
      recModifyBy
      recModifyWhen
      amendmentType
      amendmentValue
      locked
      cmsItemId
      nhaNameCmsItemId
      nhaDescriptionCmsItemId
      isNhaEnabled
      isDefault
      propertyId
      isMultiLevel
      cancellationPolicyLanguage(languageId: $languageId) {
        id
        languageId
        cancellationPolicyDescription
        recStatus
        recCreatedBy
        recCreatedWhen
        recModifyBy
        recModifyWhen
      }
    }
  }
}";

        public int HotelId { get; }
        public bool? IsActive { get; }
        public bool? GetExpired { get; }
        public DateTime? BookDateToAfter { get; }
        public int? LanguageId { get; }

        public Query(int hotelId, bool? isActive, bool? getExpired, DateTime? bookDateToAfter, int? languageId, IResultProcessor<Data> resultProcessor = null) : base(resultProcessor)
        {
            HotelId = hotelId;
            IsActive = isActive;
            GetExpired = getExpired;
            BookDateToAfter = bookDateToAfter;
            LanguageId = languageId;
        }
        
        protected override string QueryText => _query;

        protected override Dictionary<string, object> Variables => new Dictionary<string, object>
        {
            { "hotelId", HotelId },
            { "isActive", IsActive },
            { "getExpired", GetExpired },
            { "bookDateToAfter", BookDateToAfter?.ToString("yyyy-MM-dd") },
            { "languageId", LanguageId }
        };
    }

    public sealed class Data
    {        
        [JsonProperty("HotelPromotionsByHotel")]
        public List<HotelPromotionsByHotel> HotelPromotionsByHotel { get; set; }
    }
    
    public sealed class PromotionType 
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cmsIdHotel")]
        public int? CmsIdHotel { get; set; }
        [JsonProperty("cmsIdWebsite")]
        public int? CmsIdWebsite { get; set; }
        [JsonProperty("cmsIdPartners")]
        public int? CmsIdPartners { get; set; }
        [JsonProperty("recStatus")]
        public int RecStatus { get; set; }
        [JsonProperty("recCreatedBy")]
        public string RecCreatedBy { get; set; }
        [JsonProperty("recCreatedWhen")]
        public DateTime RecCreatedWhen { get; set; }
        [JsonProperty("recModifiedBy")]
        public string RecModifiedBy { get; set; }
        [JsonProperty("recModifiedWhen")]
        public DateTime? RecModifiedWhen { get; set; }
        [JsonProperty("isShortcut")]
        public bool IsShortcut { get; set; }
        [JsonProperty("isSpecial")]
        public bool IsSpecial { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("isDisplayExpiration")]
        public bool IsDisplayExpiration { get; set; }
        [JsonProperty("isNha")]
        public bool IsNha { get; set; }
    }
    
    public sealed class PromotionRooms 
    {
        [JsonProperty("hotelPromotionRoomTypeId")]
        public int HotelPromotionRoomTypeId { get; set; }
        [JsonProperty("hotelPromotionId")]
        public int HotelPromotionId { get; set; }
        [JsonProperty("hotelRoomTypeId")]
        public int HotelRoomTypeId { get; set; }
    }
    
    public sealed class PromotionRatecategories 
    {
        [JsonProperty("hotelPromotionRatecategoryId")]
        public int HotelPromotionRatecategoryId { get; set; }
        [JsonProperty("hotelPromotionId")]
        public int HotelPromotionId { get; set; }
        [JsonProperty("hotelRatecategoryId")]
        public int HotelRatecategoryId { get; set; }
    }
    
    public sealed class PromotionCustomerSegments 
    {
        [JsonProperty("hotelPromotionCustomerSegmentId")]
        public int HotelPromotionCustomerSegmentId { get; set; }
        [JsonProperty("hotelPromotionId")]
        public int HotelPromotionId { get; set; }
        [JsonProperty("customerSegmentGroupId")]
        public int CustomerSegmentGroupId { get; set; }
    }
    
    public sealed class PromotionChannels 
    {
        [JsonProperty("hotelPromotionChannelId")]
        public int HotelPromotionChannelId { get; set; }
        [JsonProperty("hotelPromotionId")]
        public int HotelPromotionId { get; set; }
        [JsonProperty("channelId")]
        public int ChannelId { get; set; }
    }
    
    public sealed class PromotionBlackouts 
    {
        [JsonProperty("hotelPromotionBlackoutId")]
        public int HotelPromotionBlackoutId { get; set; }
        [JsonProperty("hotelPromotionId")]
        public int HotelPromotionId { get; set; }
        [JsonProperty("stayDateFrom")]
        public DateTime StayDateFrom { get; set; }
        [JsonProperty("stayDateTo")]
        public DateTime StayDateTo { get; set; }
    }
    
    public sealed class CancellationPolicyLanguage 
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
        [JsonProperty("cancellationPolicyDescription")]
        public string CancellationPolicyDescription { get; set; }
        [JsonProperty("recStatus")]
        public int RecStatus { get; set; }
        [JsonProperty("recCreatedBy")]
        public string RecCreatedBy { get; set; }
        [JsonProperty("recCreatedWhen")]
        public DateTime? RecCreatedWhen { get; set; }
        [JsonProperty("recModifyBy")]
        public string RecModifyBy { get; set; }
        [JsonProperty("recModifyWhen")]
        public DateTime? RecModifyWhen { get; set; }
    }
    
    public sealed class CancellationPolicyForYCS 
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("noticeDay1")]
        public int NoticeDay1 { get; set; }
        [JsonProperty("chargeType1")]
        public int ChargeType1 { get; set; }
        [JsonProperty("chargeValue1")]
        public decimal ChargeValue1 { get; set; }
        [JsonProperty("noticeDay2")]
        public int? NoticeDay2 { get; set; }
        [JsonProperty("chargeType2")]
        public int? ChargeType2 { get; set; }
        [JsonProperty("chargeValue2")]
        public decimal? ChargeValue2 { get; set; }
        [JsonProperty("chargeNoShowType")]
        public int? ChargeNoShowType { get; set; }
        [JsonProperty("chargeNoShowValue")]
        public decimal? ChargeNoShowValue { get; set; }
        [JsonProperty("recStatus")]
        public int RecStatus { get; set; }
        [JsonProperty("recCreatedBy")]
        public string RecCreatedBy { get; set; }
        [JsonProperty("recCreatedWhen")]
        public DateTime? RecCreatedWhen { get; set; }
        [JsonProperty("recModifyBy")]
        public string RecModifyBy { get; set; }
        [JsonProperty("recModifyWhen")]
        public DateTime? RecModifyWhen { get; set; }
        [JsonProperty("amendmentType")]
        public int? AmendmentType { get; set; }
        [JsonProperty("amendmentValue")]
        public decimal? AmendmentValue { get; set; }
        [JsonProperty("locked")]
        public bool? Locked { get; set; }
        [JsonProperty("cmsItemId")]
        public int? CmsItemId { get; set; }
        [JsonProperty("nhaNameCmsItemId")]
        public int? NhaNameCmsItemId { get; set; }
        [JsonProperty("nhaDescriptionCmsItemId")]
        public int? NhaDescriptionCmsItemId { get; set; }
        [JsonProperty("isNhaEnabled")]
        public bool? IsNhaEnabled { get; set; }
        [JsonProperty("isDefault")]
        public bool? IsDefault { get; set; }
        [JsonProperty("propertyId")]
        public int? PropertyId { get; set; }
        [JsonProperty("isMultiLevel")]
        public bool IsMultiLevel { get; set; }
        [JsonProperty("cancellationPolicyLanguage")]
        public CancellationPolicyLanguage CancellationPolicyLanguage { get; set; }
    }
    
    public sealed class HotelPromotionsByHotel 
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("hotelId")]
        public int HotelId { get; set; }
        [JsonProperty("promotionTypeId")]
        public int PromotionTypeId { get; set; }
        [JsonProperty("minAdvPurchase")]
        public int? MinAdvPurchase { get; set; }
        [JsonProperty("maxAdvPurchase")]
        public int? MaxAdvPurchase { get; set; }
        [JsonProperty("bookOn")]
        public string BookOn { get; set; }
        [JsonProperty("stayOn")]
        public string StayOn { get; set; }
        [JsonProperty("checkInOn")]
        public string CheckInOn { get; set; }
        [JsonProperty("minNightsStay")]
        public int? MinNightsStay { get; set; }
        [JsonProperty("maxNightsStay")]
        public int? MaxNightsStay { get; set; }
        [JsonProperty("bookDateFrom")]
        public DateTime BookDateFrom { get; set; }
        [JsonProperty("bookDateTo")]
        public DateTime BookDateTo { get; set; }
        [JsonProperty("bookTimeFrom")]
        public string BookTimeFrom { get; set; }
        [JsonProperty("bookTimeTo")]
        public string BookTimeTo { get; set; }
        [JsonProperty("stayDateFrom")]
        public DateTime? StayDateFrom { get; set; }
        [JsonProperty("stayDateTo")]
        public DateTime? StayDateTo { get; set; }
        [JsonProperty("minRooms")]
        public int MinRooms { get; set; }
        [JsonProperty("promotionDiscountTypeId")]
        public int PromotionDiscountTypeId { get; set; }
        [JsonProperty("discValueMon1")]
        public decimal? DiscValueMon1 { get; set; }
        [JsonProperty("discValueTue2")]
        public decimal? DiscValueTue2 { get; set; }
        [JsonProperty("discValueWed3")]
        public decimal? DiscValueWed3 { get; set; }
        [JsonProperty("discValueThu4")]
        public decimal? DiscValueThu4 { get; set; }
        [JsonProperty("discValueFri5")]
        public decimal? DiscValueFri5 { get; set; }
        [JsonProperty("discValueSat6")]
        public decimal? DiscValueSat6 { get; set; }
        [JsonProperty("discValueSun7")]
        public decimal? DiscValueSun7 { get; set; }
        [JsonProperty("cancellationPolicyId")]
        public int? CancellationPolicyId { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("isRecurringBenefit")]
        public bool IsRecurringBenefit { get; set; }
        [JsonProperty("specificNight")]
        public int? SpecificNight { get; set; }
        [JsonProperty("externalCode")]
        public string ExternalCode { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("isStackable")]
        public bool IsStackable { get; set; }
        [JsonProperty("createdWhen")]
        public DateTime? CreatedWhen { get; set; }
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        [JsonProperty("lastUpdatedWhen")]
        public DateTime? LastUpdatedWhen { get; set; }
        [JsonProperty("lastUpdatedBy")]
        public string LastUpdatedBy { get; set; }
        [JsonProperty("refSourceId")]
        public int? RefSourceId { get; set; }
        [JsonProperty("promotionSourceTypeId")]
        public int? PromotionSourceTypeId { get; set; }
        [JsonProperty("isStackableCombine")]
        public bool IsStackableCombine { get; set; }
        [JsonProperty("stackableDiscountType")]
        public int StackableDiscountType { get; set; }
        [JsonProperty("isIncludeCustomerSegment")]
        public bool? IsIncludeCustomerSegment { get; set; }
        [JsonProperty("isIncludeChannel")]
        public bool? IsIncludeChannel { get; set; }
        [JsonProperty("isIncludeRatecategory")]
        public bool? IsIncludeRatecategory { get; set; }
        [JsonProperty("isIncludeRoom")]
        public bool? IsIncludeRoom { get; set; }
        [JsonProperty("isApplyChannelDiscount")]
        public bool IsApplyChannelDiscount { get; set; }
        [JsonProperty("isOfflineType")]
        public bool? IsOfflineType { get; set; }
        [JsonProperty("PromotionType")]
        public PromotionType PromotionType { get; set; }
        [JsonProperty("PromotionRooms")]
        public List<PromotionRooms> PromotionRooms { get; set; }
        [JsonProperty("PromotionRatecategories")]
        public List<PromotionRatecategories> PromotionRatecategories { get; set; }
        [JsonProperty("PromotionCustomerSegments")]
        public List<PromotionCustomerSegments> PromotionCustomerSegments { get; set; }
        [JsonProperty("PromotionChannels")]
        public List<PromotionChannels> PromotionChannels { get; set; }
        [JsonProperty("PromotionBlackouts")]
        public List<PromotionBlackouts> PromotionBlackouts { get; set; }
        [JsonProperty("CancellationPolicyForYCS")]
        public CancellationPolicyForYCS CancellationPolicyForYCS { get; set; }
    }
}
