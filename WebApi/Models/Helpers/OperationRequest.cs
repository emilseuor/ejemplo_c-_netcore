using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Helpers
{
    public class OperationRequest
    {
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public string UserId { get; set; }
        public int DaysBeforeDueDate { get; set; }

        public string OperatorUserId { get; set; }

        [ValidValues("PURCHASE", "RENT")]
        public string Type { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        [ValidValues("PAID", "TO_PAY", "FAILED", "CANCELLED", "RETURNED", "PENALTY_PAID")]
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public double PenaltyPrice { get; set; }
        public string Details { get; set; }
    }
}
