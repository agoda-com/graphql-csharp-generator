using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Agoda.Graphql;




#region GetCsvMetaData 

namespace Agoda.Graphql.BookingQueries.PropertyBooking.GetCsvMetaData
{

    /// <summary>Operation Type</summary>
    public partial class Query : QueryBase<Data>
    { 
        private const string _query = @"query GetCsvMetaData($ycsCriteria: YCSCriteria!) {
  PropertyBookingByYCSCriteria(YCSCriteria: $ycsCriteria) {
    pagination {
      page
      pageSize
      totalPages
      totalRecords
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
        
        
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
    
    /// <summary>Inner Model</summary> 
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
}

#endregion

