using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedPharmacy.Models
{
    public class OrderList
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string MedicineName { get; set; }
        public int MedicineAmount { get; set; }
        public string Price { get; set; }
        public string TotalPrice { get; set; }
        public string Date { get; set; }
    }
}