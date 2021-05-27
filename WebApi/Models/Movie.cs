using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double RentalPrice { get; set; }

        [Required]
        public double SalePrice { get; set; }


        public string Img { get; set; }
        
        public int Stock { get; set; }
        
        public bool Availability { get; set; }

        public virtual IEnumerable<MovieLikes> likes { get; set; }

    }
}
