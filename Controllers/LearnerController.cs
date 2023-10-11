using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebAppl.Data;
using WebAppl.Models;

namespace WebAppl.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;
        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        public IActionResult Index(int? mid)
        {
            if (mid == null)
            {
                var learner = db.Learners.Include(m => m.Major).ToList();
                return View(learner);
            }
            else
            {
                var learner = db.Learners.Where(l=>l.MajorID==mid).Include(m=>m.Major).ToList();
                return View(learner);
            }
        }
        public IActionResult Create()
        {
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName"); //cách 2
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            //lại dùng 1 trong 2 cách tạo SelectList gửi về View để hiển thị danh sách Majors
            else
            {
                ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
                return View();
            }
        }
        public IActionResult Delete(int learnerID)
        {
            var temp=db.Learners.Find(learnerID);
            if (temp == null)
            {
                return NotFound();
            }
            if (temp.Enrollments.Count() <= 0)
            {
                db.Learners.Remove(temp);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Edit(int learnerID)
        {
            var temp = db.Learners.Find(learnerID);
            if (temp == null)
            {
                return NotFound();
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learners)
        {
            if (ModelState.IsValid)
            {
                db.Update(learners);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View(learners);
        }
    }
}
