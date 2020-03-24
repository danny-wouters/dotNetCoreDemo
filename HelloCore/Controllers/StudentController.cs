using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelloCore.Data;
using HelloCore.Models;
using HelloCore.ViewModels;

namespace HelloCore.Controllers
{
    public class StudentController : Controller
    {
        private readonly HelloCoreContext _context;

        public StudentController(HelloCoreContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students.Include(s => s.Courses).SingleOrDefault(x => x.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            var viewmodel = new StudentViewModel
            {
                Student = student,
                CourseList = new SelectList(_context.Courses, "Id", "Name"),
                SelectedCourses = student.Courses.Select(sc => sc.CourseId).ToList()
            };
            return View(viewmodel);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentViewModel viewModel)
        {
            if (id != viewModel.Student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Student student = _context.Students.Include(c => c.Courses).SingleOrDefault(x => x.Id == id);
                var newCourses = new List<StudentCourse>();
                foreach (int courseId in viewModel.SelectedCourses)
                {
                    newCourses.Add(new StudentCourse
                    {
                        StudentId = student.Id,
                        CourseId = courseId
                    });
                }

                student.Courses.RemoveAll(sc => !newCourses.Contains(sc));
                student.Courses.AddRange(
                    newCourses.Where(nc => !student.Courses.Contains(nc)));
                _context.Update(student);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
