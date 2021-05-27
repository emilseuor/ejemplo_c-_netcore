using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Helpers
{
    public class UserRequestModel
    {
        [Required]
        [EmailAddress, StringLength(100)]
        public string Email { get; set; }
        public List<string> Roles { get; set; }

        [Required]
        [MinLength(5), StringLength(100)]
        public string Password { get; set; }

        [MinLength(5), StringLength(100)]
        public string ConfirmPassword { get; set; }

    }
}
