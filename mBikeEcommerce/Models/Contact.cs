using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mBikeEcommerce.Models
{
    public class Contact
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string name { get; set; }

        [Required]
        [StringLength(64)]
        public string email { get; set; }

        [Required]
        [StringLength(2028)]
        public string description { get; set; }
    }
}