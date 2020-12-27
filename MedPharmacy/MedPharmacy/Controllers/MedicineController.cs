using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedPharmacy.Context;
using MedPharmacy.Models;

namespace MedPharmacy.Controllers
{
    public class MedicineController : Controller
    {
        SecurityController security = new SecurityController();
        MedContext ctx = new MedContext();
        //
        // GET: /Medicine/
         public ActionResult HomeIndex()
        {
            ViewBag.HomeIndex = "active";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Contact = "active";
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            ViewBag.Contact = "active";
            using (var ctx = new MedContext())
            {
                ctx.ContactInfo.Add(contact);
                ctx.SaveChanges();
            }
            ViewBag.Yes = '1';
            return View();
        }

        public ActionResult MedicineSearch()
        {
            ViewBag.MedicineSearch = "active";
            List<MedicineInformation> info;
            using (var ctx = new MedContext())
            {
                info = ctx.MedicineInformations.ToList();
            }
            return View(info);
        }
        public ActionResult Informations()
        {
            ViewBag.Informations = "active";
            return View();
        }
        public ActionResult Category()
        {
            ViewBag.Category = "active";
            return View();
        }
        public ActionResult Medicineshow()
        {
            ViewBag.Medicineshow = "active";
            return View();
        }
        public ActionResult Order(string data, string data2)
        {
            ViewBag.Order = "active";
            ViewBag.Data = data;
            ViewBag.Data2 = data2;
            return View();
        }
        [HttpPost]
        public ActionResult Order(OrderList orderList)
        {
            
           string a = (orderList.MedicineAmount*Convert.ToInt32(orderList.Price)).ToString();

            ViewBag.Order = "active";
            using (var ctx = new MedContext())
            {
                orderList.TotalPrice = a +" "+"BDT";
                ctx.Orderinfo.Add(orderList);
                ctx.SaveChanges();
            }
            ViewBag.Yes = '1';
            return View();
        }
        public ActionResult Profile()
        {
            ViewBag.Profile = "active";
            Register pharmacyuser = new Register();
            int pharmacyid = Convert.ToInt32(Session["PharmacyId"]);
            using (var db = new MedContext())
            {
                var u = db.Registers.Where(k => k.Id == pharmacyid).Select(c => new { c.PharmacyName, c.Address, c.PhoneNo });
                foreach (var j in u)
                {
                    pharmacyuser.PharmacyName = security.Decrypt(j.PharmacyName);
                    pharmacyuser.Address = security.Decrypt(j.Address);
                   
                    pharmacyuser.PhoneNo = security.Decrypt(j.PhoneNo);
                }
                ViewBag.Patient = pharmacyuser;
                return View();
            }
        }
        public ActionResult ProfileUpdate()
        {
            ViewBag.ProfileUpdate = "active";
            Register pharmacyuser = new Register();
            int pharmacyid = Convert.ToInt32(Session["PharmacyId"]);
            using (var db = new MedContext())
            {
                var u = db.Registers.Where(k => k.Id == pharmacyid).Select(c => new { c.PharmacyName, c.Address, c.PhoneNo });
                foreach (var j in u)
                {
                    pharmacyuser.PharmacyName = security.Decrypt(j.PharmacyName);
                    pharmacyuser.Address = security.Decrypt(j.Address);

                    pharmacyuser.PhoneNo = security.Decrypt(j.PhoneNo);
                }
                ViewBag.Patient = pharmacyuser;
                return View();
            }
        }
        public ActionResult Update(Register pharmacyuser)
        {

            int id = Convert.ToInt32(Session["PharmacyId"]);
            string useremail = security.Encrypt(pharmacyuser.Email);
            using (var db = new MedContext())
            {
                Register pa = db.Registers.Single(e => e.Id == id);
                if (pa.Email == useremail)
                {
                    pa.PharmacyName = security.Encrypt(pharmacyuser.PharmacyName);
                    pa.Address = security.Encrypt(pharmacyuser.Address);
                    pa.PhoneNo = security.Encrypt(pharmacyuser.PhoneNo);
                    db.SaveChanges();
                    return RedirectToAction("Profile", "Medicine");
                }

                return RedirectToAction("Profile", "Medicine");

            }
        }
        public ActionResult AddMedicine()
        {
            ViewBag.AddMedicine = "active";
            return View();
        }
        [HttpPost]
        public ActionResult AddMedicine(PharmacyMedicine medicine)
        {
            ViewBag.AddMedicine = "active";
            int id = Convert.ToInt32(Session["PharmacyId"]);
            using (var ctx = new MedContext())
            {
                medicine.PharmacyId = id;
                ctx.PharmacyMedicines.Add(medicine);
                ctx.SaveChanges();
            }
            ViewBag.Yes = '1';
            return View();
        }

        //suggetion
        public ActionResult MedicineSuggestion()
        {
            ViewBag.MedicineSuggestion = "active";
            List<Problems> pro = new List<Problems>();
            using (var ctx = new MedContext())
            {
                pro = ctx.Problemses.ToList();
                ViewBag.Problems = pro;
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult MedicineSuggestion(int[] sym)
        {
            List<Medicine> ld = new List<Medicine>();
            Dictionary<string, double> result = new Dictionary<string, double>();
            ld = ctx.Medicines.ToList();

            foreach (Medicine d in ld)
            {
                string[] values = d.Problems.Split(',');
                int[] v = Array.ConvertAll(values, int.Parse);
                int cont = 0;

                foreach (int i in v)
                {
                    foreach (int j in sym)
                    {
                        if (i == j)
                        {
                            cont += 1;
                            break;
                        }
                    }
                }
                result[d.Name] = (cont / 2.0) * 100;
            }
            ViewBag.Message = result.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
    
            List<Problems> pro = new List<Problems>();
            pro = ctx.Problemses.ToList();
            ViewBag.Problems = pro;

            return View();
        }

	}
	
}