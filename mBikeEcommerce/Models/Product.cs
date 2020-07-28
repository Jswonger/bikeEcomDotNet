using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace mBikeEcommerce.Models
{
    public class Product
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string brand { get; set; }

        [Required]
        [StringLength(64)]
        public string type { get; set; }

        [Required]
        [StringLength(64)]
        public string name { get; set; }

        [Required]
        [StringLength(128)]
        public string imgPath { get; set; }

        [Required]
        public double price { get; set; }

        [Required]
        [StringLength(2028)]
        public string description { get; set; }
    }
}