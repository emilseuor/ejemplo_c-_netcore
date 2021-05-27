using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApi.Models
{
    public class Operation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Movie Movie { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string OperatorUserId { get; set; }

        [Required]
        [ValidValues("PURCHASE", "RENT")]
        public string Type { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ValidValues("PAID", "TO_PAY","FAIL", "CANCELLED", "RETURNED", "PENALTY_PAID")]
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public double PenaltyPrice { get; set; }
        public string Details { get; set; }
    }

    public class ValidValuesAttribute : ValidationAttribute
    {
        string[] _args;

        public ValidValuesAttribute(params string[] args)
        {
            _args = args;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_args.Contains((string)value))
                return ValidationResult.Success;
            return new ValidationResult("Invalid value.");
        }
    }

}
