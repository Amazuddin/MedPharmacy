using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedPharmacy.Models
{
    public class Register
    {
        public int Id { get; set; }
        public string PharmacyName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}