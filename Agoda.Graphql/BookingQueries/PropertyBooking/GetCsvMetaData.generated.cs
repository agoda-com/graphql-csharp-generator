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
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
