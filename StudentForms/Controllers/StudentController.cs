using StudentForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentForms.Controllers
{
    public class StudentController : Controller
    {
        studentvEntities1 db=new studentvEntities1();
        // GET: Student
        public ActionResult Index()
        {
            var students = db.StudentLists
                             .Join(db.Countries, s => s.CountryId, c => c.Id, (s, c) => new { s, c })
                             .Join(db.States, sc => sc.s.StateId, st => st.Id, (sc, st) => new { sc, st })
                             .Join(db.Cities, sct => sct.sc.s.CityId, ci => ci.Id, (sct, ci) => new StudentViewModel
                             {
                                 Id = sct.sc.s.Id,
                                 FirstName = sct.sc.s.FirstName,
                                 LastName = sct.sc.s.LastName,
                                 Gender = sct.sc.s.Gender,
                                 Mobile = sct.sc.s.Mobile,
                                 CountryName = sct.sc.c.Name,
                                 StateName = sct.st.Name,
                                 CityName = ci.Name
                             }).ToList();

            return View(students);
        }

       
        public ActionResult Create()
        {
            ViewBag.Countries = new SelectList(db.Countries, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentList student)
        {
            if (ModelState.IsValid)
            {
                db.StudentLists.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Countries = new SelectList(db.Countries, "Id", "Name");
            return View(student);
        }

       
        public JsonResult GetStates(int countryId)
        {
            var states = db.States.Where(s => s.CountryId == countryId).Select(s => new { s.Id, s.Name }).ToList();
            return Json(states, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCities(int stateId)
        {
            var cities = db.Cities.Where(c => c.StateId == stateId).Select(c => new { c.Id, c.Name }).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = db.StudentLists.Find(id);
            if (student == null)
                return HttpNotFound();

        
            var studentViewModel = new StudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = student.Gender,
                Mobile = student.Mobile,
                CountryId = (int)student.CountryId,
                StateId = (int)student.StateId,
                CityId = (int)student.CityId,
                CountryName = db.Countries.Where(c => c.Id == student.CountryId).Select(c => c.Name).FirstOrDefault(),
                StateName = db.States.Where(s => s.Id == student.StateId).Select(s => s.Name).FirstOrDefault(),
                CityName = db.Cities.Where(c => c.Id == student.CityId).Select(c => c.Name).FirstOrDefault()
            };

            PopulateDropdowns(studentViewModel);
            return View(studentViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentViewModel studentViewModel)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(studentViewModel);
                return View(studentViewModel);
            }

            var existingStudent = db.StudentLists.Find(studentViewModel.Id);
            if (existingStudent == null)
            {
                ModelState.AddModelError("", "Student not found.");
                PopulateDropdowns(studentViewModel);
                return View(studentViewModel);
            }

            
            if (!db.Countries.Any(c => c.Id == studentViewModel.CountryId))
            {
                ModelState.AddModelError("CountryId", "Invalid Country selected.");
                PopulateDropdowns(studentViewModel);
                return View(studentViewModel);
            }

            if (!db.States.Any(s => s.Id == studentViewModel.StateId))
            {
                ModelState.AddModelError("StateId", "Invalid State selected.");
                PopulateDropdowns(studentViewModel);
                return View(studentViewModel);
            }

            if (!db.Cities.Any(c => c.Id == studentViewModel.CityId))
            {
                ModelState.AddModelError("CityId", "Invalid City selected.");
                PopulateDropdowns(studentViewModel);
                return View(studentViewModel);
            }

    
            existingStudent.FirstName = studentViewModel.FirstName;
            existingStudent.LastName = studentViewModel.LastName;
            existingStudent.Gender = studentViewModel.Gender;
            existingStudent.Mobile = studentViewModel.Mobile;
            existingStudent.CountryId = studentViewModel.CountryId;
            existingStudent.StateId = studentViewModel.StateId;
            existingStudent.CityId = studentViewModel.CityId;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void PopulateDropdowns(StudentViewModel student)
        {
            ViewBag.Countries = new SelectList(db.Countries, "Id", "Name", student.CountryId);
            ViewBag.States = new SelectList(db.States.Where(s => s.CountryId == student.CountryId), "Id", "Name", student.StateId);
            ViewBag.Cities = new SelectList(db.Cities.Where(c => c.StateId == student.StateId), "Id", "Name", student.CityId);
        }


        public ActionResult Delete(int id)
        {
            var student = db.StudentLists.Find(id);
            if (student == null) return HttpNotFound();

            db.StudentLists.Remove(student);
            db.SaveChanges();

            return RedirectToAction("Index"); 
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var student = db.StudentLists.Find(id);
            db.StudentLists.Remove(student);
            db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}