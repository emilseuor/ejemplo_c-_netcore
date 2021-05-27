using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Helpers
{
    public class RoleRequest
    {
        [Required]
        public string Name { get; set; }
        
        public List<string> Permissions { get; set; }
    }
    public class RoleUpdateRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
}
