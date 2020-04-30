using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediPrime.Models;
using System.Data.Entity;

namespace MediPrime.Controllers
{
    public class DoctorPatientController : Controller
    {
        // GET: DoctorPatient
        public ActionResult Index()
        {
            if(Session["doctorid"] == null)
            {
                return RedirectToAction("DoctorLogin", "Home");
            }
            return View();
        }



        public ActionResult GetData()
        {
            int did = Convert.ToInt32(Session["doctorid"]);
            using (MediPrimeDBEntities2 db = new MediPrimeDBEntities2())
            {

                List<Doctor_Patient> doctorpatients = db.Doctor_Patient.Where(m => m.Doctor_Id == did).ToList();
                return Json(new { data = doctorpatients }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddorEdit(int id = 0)
        {
            int did = Convert.ToInt32(Session["doctorid"]);
            MediPrimeDBEntities6 db1 = new MediPrimeDBEntities6();
            Doctor doctor = db1.Doctors.Where(m => m.Doctor_Id == did).FirstOrDefault();
            string dtype = doctor.Type;
            Session["doctortype"] = dtype;
            if (id == 0)
                return View(new Doctor_Patient());
            else
            {
                using (MediPrimeDBEntities2 db = new MediPrimeDBEntities2())
                {
                    return View(db.Doctor_Patient.Where(x => x.Doctor_Patient_Id == id).FirstOrDefault<Doctor_Patient>());
                }
            }

        }

        [HttpGet]
        public ActionResult AddVisit(int id = 0)
        {
            int did = Convert.ToInt32(Session["doctorid"]);
            MediPrimeDBEntities6 db1 = new MediPrimeDBEntities6();
            Doctor doctor = db1.Doctors.Where(m => m.Doctor_Id == did).FirstOrDefault();
            string dtype = doctor.Type;
            Session["doctortype"] = dtype;

            using (MediPrimeDBEntities2 db = new MediPrimeDBEntities2())
                {
                    Doctor_Patient dp = db.Doctor_Patient.Where(x => x.Doctor_Patient_Id == id).FirstOrDefault<Doctor_Patient>();
                    dp.Doctor_Patient_Id = 0;
                    return View(dp);
                }
        
        }

        [HttpPost]
        public ActionResult AddorEdit(Doctor_Patient dp)
        {
            using (MediPrimeDBEntities2 db = new MediPrimeDBEntities2())
            {
                int did = Convert.ToInt32(Session["doctorid"]);
                dp.Doctor_Id = did;
                if (dp.Doctor_Patient_Id == 0)
                {
                    db.Doctor_Patient.Add(dp);
                    db.SaveChanges();
                    return Json(new { success = "true", message = "Saved Successfully" });
                }
                else
                {
                    db.Entry(dp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = "true", message = "Updated Successfully" });

                }
            }



        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (MediPrimeDBEntities2 db = new MediPrimeDBEntities2())
            {
                Doctor_Patient dp = db.Doctor_Patient.Where(x => x.Doctor_Patient_Id == id).FirstOrDefault<Doctor_Patient>();
                db.Doctor_Patient.Remove(dp);
                db.SaveChanges();
                return Json(new { success = "true", message = "Deleted Successfully" });

            }
        }
    }
}