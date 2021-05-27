using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class MovieLikes
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public string UserId { get; set; }

        [Required]
        public Movie Movie { get; set; }
    }
}
