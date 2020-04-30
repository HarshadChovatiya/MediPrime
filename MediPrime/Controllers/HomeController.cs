using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediPrime.Models;
using System.Data.Entity;

namespace MediPrime.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(Admin admin)
        {
            using (MediPrimeDBEntities4 db = new MediPrimeDBEntities4())
            {
                Admin admin1 = db.Admins.Where(m => m.Name.Equals(admin.Name) && m.Password.Equals(admin.Password)).FirstOrDefault();
                if (admin1 == null)
                {
                    string errormsg = "Invalid Name or Password";
                    ViewBag.msg = errormsg;
                    return View();
                }
                else
                {
                    Session["adminname"] = admin1.Name;
                    return RedirectToAction("Index", "Medicine");
                }
            }
        }

        [HttpGet]
        public ActionResult DoctorLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoctorLogin(Doctor doctor)
        {
            using (MediPrimeDBEntities6 db = new MediPrimeDBEntities6())
            {
                Doctor doctor1 = db.Doctors.Where(m => m.Username.Equals(doctor.Username) && m.Password.Equals(doctor.Password)).FirstOrDefault();
                if (doctor1 == null)
                {
                    string errormsg = "Invalid Username or Password";
                    ViewBag.msg = errormsg;
                    return View();
                }
                else
                {
                    Session["doctorid"] = doctor1.Doctor_Id; 
                    return RedirectToAction("Index", "DoctorPatient");
                }
            }
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View(new Doctor());
         
        }

        [HttpPost]
        public ActionResult Registration(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                using (MediPrimeDBEntities6 db = new MediPrimeDBEntities6())
                {
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    return Json(new { success = "true", message = "Registration Done Successfully" });
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult PatientLogin()
        {
              return View();
        }

        [HttpPost]
        public ActionResult PatientLogin(Patient patient)
        {               
            using (MediPrimeDBEntities5 db = new MediPrimeDBEntities5())
            {
                Patient patient1 = db.Patients.Where(m => m.username.Equals(patient.username) && m.password.Equals(patient.password)).FirstOrDefault();
                if (patient1 == null)
                {
                     string errormsg = "Invalid Username or Password";
                     ViewBag.msg = errormsg;
                     return View();
                }
                else
                {
                    Session["patientname"] = patient1.Name;
                    return RedirectToAction("PatientDetails", "Home");
                }
            }
        }

        [HttpGet]
        public ActionResult PatientRegistration()
        {
            return View(new Patient());

        }

        [HttpPost]
        public ActionResult PatientRegistration(Patient patient)
        {
            if (ModelState.IsValid)
            {
                using (MediPrimeDBEntities5 db = new MediPrimeDBEntities5())
                {
                    db.Patients.Add(patient);
                    db.SaveChanges();
                    return Json(new { success = "true", message = "Registration Done Successfully" });
                }
            }
            return View();
        }

        public ActionResult PatientDetails()
        {

            if(Session["patientname"] == null)
            {
                return RedirectToAction("PatientLogin");
            }
            string pname = (string)Session["patientname"];

            //using (
            MediPrimeDBEntities2 db = new MediPrimeDBEntities2();//)
            //{
                List<Doctor_Patient> doctor_Patients = db.Doctor_Patient.Where(m => m.PatientName == pname).ToList();
                ViewBag.patientDetails = doctor_Patients;
                ViewBag.patientname = pname;

             List<int?> doctoridlist = db.Doctor_Patient.Where(m => m.PatientName.Equals(pname)).Select(m => m.Doctor_Id).ToList();
            //}
            //using (
            MediPrimeDBEntities6 db1 = new MediPrimeDBEntities6();
            //{
                List<Doctor> doctorlist = db1.Doctors.Where(m => doctoridlist.Contains(m.Doctor_Id)).ToList();
            ViewBag.doctordetail = doctorlist;
            //}

           /* MediPrimeDBEntities5 db5 = new MediPrimeDBEntities5();
            int patientid = db5.Patients.Where(m => m.username.Equals(pname)).Select(m => m.Patient_Id).First();
            ViewBag.loginpatientid = patientid;
             */   return View();
        }


        public ActionResult AdminLogout()
        {
            Session["adminname"] = null; 
            return RedirectToAction("AdminLogin");
        }
        public ActionResult PatientLogout()
        {
            Session["patientname"] = null;
            return RedirectToAction("PatientLogin");
        }

        public ActionResult DoctorLogout()
        {
            Session["doctorid"] = null;
            Session["doctortype"] = null;
            return RedirectToAction("DoctorLogin");
        }
    }
}