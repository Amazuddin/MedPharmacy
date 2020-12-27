using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MedPharmacy.Context;
using MedPharmacy.Models;

namespace MedPharmacy.Controllers
{
    public class UserLoginController : Controller
    {
        SecurityController security=new SecurityController();
        //
        // GET: /UserLogin/
        public ActionResult Registration()
        {
            ViewBag.Registration = "active";
            return View();
        }
        [HttpPost]
        public ActionResult Registration(Register register)
        {
            using (var db = new MedContext())
            {
                register.Email = security.Encrypt(register.Email);
                var e = db.Registers.Where(c => c.Email == register.Email).ToList().Count;
                if (e == 0)
                {
                    register.Password = EncodePassword(register.Password);
                    register.PharmacyName = security.Encrypt(register.PharmacyName);
                    register.Address = security.Encrypt(register.Address);
                    register.PhoneNo = security.Encrypt(register.PhoneNo);
                    db.Registers.Add(register);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Error = "Already Registered";
                }
            }
            ViewBag.Yes = '1';
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Login = "active";
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogIn logIn)
        {
            string password = EncodePassword(logIn.Password);
            string email = security.Encrypt(logIn.Email);
            using (var ctx = new MedContext())
            {

                var p = ctx.Registers.Where(c => c.Email == email && c.Password == password).Select(c => new { c.Id, c.Email }).ToList();
                if (p.Any())
                {
                    foreach (var k in p)
                    {
                        Session["PharmacyId"] = k.Id;
                        Session["PharmacyEmail"] = k.Email;

                    }
                    return RedirectToAction("HomeIndex", "Medicine");
                }
                else
                {
                    ViewBag.Error = "Login Failed";
                }



            }
            return View();
        }
        public ActionResult PharmacyLogout()
        {
            Session["PharmacyId"] = null;
            Session["PharmacyEmail"] = null;
            return RedirectToAction("Login", "UserLogin");
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(Admin admin)
        {
            string password = EncodePassword(admin.Password);
            string email = security.Encrypt(admin.Email);
            using (var ctx = new MedContext())
            {

                var p = ctx.Admins.Where(c => c.Email == email && c.Password == password).Select(c => new { c.Id, c.Email }).ToList();
                if (p.Any())
                {
                    foreach (var k in p)
                    {
                        Session["AdminId"] = k.Id;
                        Session["AdminEmail"] = k.Email;

                    }
                    return RedirectToAction("HomeIndex", "Medicine");
                }
                else
                {
                    ViewBag.Error = "Login Failed";
                }



            }
            return View();
        }
        public ActionResult AdminLogout()
        {
            Session["AdminId"] = null;
            Session["AdminEmail"] = null;
            return RedirectToAction("AdminLogin", "UserLogin");
        }
        public static string EncodePassword(string password)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 a;
            a = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(password);
            encodedBytes = a.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }

	}
}