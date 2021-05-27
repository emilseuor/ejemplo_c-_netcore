using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Helpers
{
    public class MovieObj
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double RentalPrice { get; set; }
        public double SalePrice { get; set; }
        public string Img { get; set; }
        public int Stock { get; set; }
        public bool Availability { get; set; }
        public int CountLikes { get; set; }

    }
}
