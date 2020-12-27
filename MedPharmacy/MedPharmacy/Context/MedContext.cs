using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MedPharmacy.Models;

namespace MedPharmacy.Context
{
    public class MedContext : DbContext
    {
        public DbSet<Pharmacy> PharmacyInfo { get; set; }
        public DbSet<MedicineInformation> MedicineInformations { get; set; }
        public DbSet<Contact> ContactInfo { get; set; }
        public DbSet<OrderList> Orderinfo { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<PharmacyMedicine> PharmacyMedicines { get; set; }
        public DbSet<Problems> Problemses { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
    }
}