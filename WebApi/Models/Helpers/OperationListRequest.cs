using System;
using WebApi.Models;

namespace WebApi.Models.Helpers
{
    public class OperationListRequest
    {
        public string UserId { get; set; }
        
        public string Type { get; set; }
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
        
        public int MovieId { get; set; }
        public string Status { get; set; }
        
    }
}
