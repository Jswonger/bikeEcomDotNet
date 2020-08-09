using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mBikeEcommerce.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        
        public string imgPath { get; set; }

        [Required]
        public double price { get; set; }

        [Required]
        [StringLength(2028)]
        public string description { get; set; }

    }
}