using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedPharmacy.Models
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }

        public string PharmacyName { get; set; }
        public string Adress { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}