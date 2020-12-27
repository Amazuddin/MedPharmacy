using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using MedPharmacy.Models;
using MedPharmacy.Context;
using Newtonsoft.Json;

namespace MedPharmacy.Controllers
{
    public class PharmacyAdminController : Controller
    {
        private MedContext db = new MedContext();
        SecurityController security = new SecurityController();

        // GET: /PharmacyAdmin/
        public ActionResult Index()
        {
            ViewBag.Index = "active";
            List<Register> pharmacyreg = db.Registers.ToList();
            foreach (Register phar in pharmacyreg)
            {
                phar.PharmacyName = security.Decrypt(phar.PharmacyName);
                phar.Address = security.Decrypt(phar.Address);
                phar.Email = security.Decrypt(phar.Email);
                phar.PhoneNo = security.Decrypt(phar.PhoneNo);


            }
            return View(pharmacyreg);
        }

        // GET: /PharmacyAdmin/Details/5
       //public ActionResult Details(int? id)
       // {
       //     if (id == null)
       //     {
       //         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
       //     }
       //     Register register = db.Registers.Find(id);
       //     if (register == null)
       //     {
       //         return HttpNotFound();
       //     }
       //     return View(register);
       // }

        // GET: /PharmacyAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /PharmacyAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PharmacyName,Address,PhoneNo,Email,Password")] Register register)
        {
            if (ModelState.IsValid)
            {
                db.Registers.Add(register);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(register);
        }

        // GET: /PharmacyAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            register.PharmacyName = security.Decrypt(register.PharmacyName);
            register.Address = security.Decrypt(register.Address);
            register.Email = security.Decrypt(register.Email);
            register.PhoneNo = security.Decrypt(register.PhoneNo);
            return View(register);
        }

        // POST: /PharmacyAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,PharmacyName,Address,PhoneNo,Email")] Register register)
        {
            if (ModelState.IsValid)
            {
                register.PharmacyName = security.Encrypt(register.PharmacyName);
                register.Address = security.Encrypt(register.Address);
                register.Email = security.Encrypt(register.Email);
                register.PhoneNo = security.Encrypt(register.PhoneNo);
                db.Entry(register).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Update Values Successfully";
            return View(register);
        }

        // GET: /PharmacyAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            register.PharmacyName = security.Decrypt(register.PharmacyName);
            register.Address = security.Decrypt(register.Address);
            register.Email = security.Decrypt(register.Email);
            register.PhoneNo = security.Decrypt(register.PhoneNo);
            return View(register);
        }

        // POST: /PharmacyAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Register register = db.Registers.Find(id);
            db.Registers.Remove(register);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Notification()
        {
            ViewBag.Notification = "active";
            List<Contact> noti = new List<Contact>();
            using (var db = new MedContext())
            {
                noti = db.ContactInfo.ToList();
            }

            return View(noti);
        }
        public ActionResult CustomerOrderList()
        {
            ViewBag.CustomerOrderList = "active";
            List<OrderList> noti = new List<OrderList>();
            using (var db = new MedContext())
            {
                noti = db.Orderinfo.ToList();
            }

            return View(noti);
        }

        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var senderEmail = new MailAddress("md.sayemhossain.85@gmail.com", "MedToDoor");
                    var senderEmail = new MailAddress("medtodoorbgc@gmail.com", "MedToDoor");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    //var password = "123123Aa";
                    var password = "@a123456z";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    //  return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
            }
            return View();
        }

        public ActionResult Graph()
        {
            ViewBag.Graph = "active";
            return View();
        }
        [HttpPost]
        public ActionResult Graph(string dt)
        {
            string ll = dt + "/01/20";
            string hl = dt + "/31/20";
            int[] ar = new int[32];
            for (int i = 1; i <= 31; i++)
                ar[i] = 0;



            using (var ctx = new MedContext())
            {


                string sql = "SELECT Count(a.Id) as val,a.Date FROM OrderLists as a " +
                                                        "WHERE a.Date>='" + ll + "' AND a.Date<='" + hl + "' GROUP BY a.Date";
                SqlConnection cn = new SqlConnection(ctx.Database.Connection.ConnectionString);
                SqlCommand command = new SqlCommand(sql, cn);

                cn.Open();


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int a = Convert.ToInt32(reader["val"]);
                        string b = reader["Date"].ToString();
                        string kl = b[3].ToString() + b[4].ToString();
                        int dd = Convert.ToInt32(kl);
                        ar[dd - 1] = a;
                    }
                }
                cn.Close();
            }

            string dataStr = JsonConvert.SerializeObject(ar, Formatting.None);
            ViewBag.Data = dataStr;
            ViewBag.mon = dt;
            return View();

        }

    }
}
