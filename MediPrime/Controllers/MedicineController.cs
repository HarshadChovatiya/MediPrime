using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediPrime.Models;
using System.Data.Entity;

namespace MediPrime.Controllers
{
    public class MedicineController : Controller
    {
        // GET: Medicine
        public ActionResult Index()
        {
            if(Session["adminname"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            return View();
        }

        public ActionResult GetData()
        {
            using (MediPrimeDBEntities db = new MediPrimeDBEntities())
            {
                
                List<Medicine> medicines = db.Medicines.ToList<Medicine>();
                return Json(new { data = medicines }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddorEdit(int id = 0)
        {
            if (id == 0)
                return View(new Medicine());
            else
            {
                using (MediPrimeDBEntities db = new MediPrimeDBEntities())
                {
                    return View(db.Medicines.Where(x => x.medicine_id == id).FirstOrDefault<Medicine>());
                }
            }

        }

        [HttpPost]
        public ActionResult AddorEdit(Medicine med)
        {
            using (MediPrimeDBEntities db = new MediPrimeDBEntities())
            {

                if (med.medicine_id == 0)
                {
                    db.Medicines.Add(med);
                    db.SaveChanges();
                    return Json(new { success = "true", message = "Saved Successfully" });
                }
                else
                {
                    db.Entry(med).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = "true", message = "Updated Successfully" });

                }
            }



        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (MediPrimeDBEntities db = new MediPrimeDBEntities())
            {
                Medicine med = db.Medicines.Where(x => x.medicine_id == id).FirstOrDefault<Medicine>();
                db.Medicines.Remove(med);
                db.SaveChanges();
                return Json(new { success = "true", message = "Deleted Successfully" });

            }
        }
    }
}