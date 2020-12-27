using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedPharmacy.Models
{
    public class MedicineInformation
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string Price { get; set; }
        public string Disease { get; set; }
    }
}