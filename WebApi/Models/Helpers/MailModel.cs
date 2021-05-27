using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Helpers
{
    public class MailModel
    {
        [Required]
        public string fromemail { get; set; }

        [Required]
        public string toemail { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string content { get; set; }
    }
}
