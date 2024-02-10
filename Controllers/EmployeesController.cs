using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using skyline.Data;
using skyline.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace skyline.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        // [Route("[controller]/List")]
        [HttpGet]
        public IActionResult GetIndexView(int depId ,string? search, string sortType, string sortOrder, int pageSize=20,int pageNumber=1)
        {
            ViewBag.AllDepartments = _context.Departments.ToList();
            ViewBag.SelectedDeptId = depId;
            ViewBag.CurrentSearch = search;
            ViewBag.PageSize = pageSize;
            @ViewBag.PageNumber = pageNumber;
            IQueryable<Employee> employees = _context.Employees.AsQueryable();
            if (depId > 0)
            {
                employees = employees.Where(e => e.DepartmentId == depId);
            }
            if (string.IsNullOrEmpty(search) == false)
            {
                employees = employees.Where(e => e.FullName.Contains(search));
            }
            if (sortType == "FullName" && sortOrder == "asc")
            {
                employees = employees.OrderBy(e => e.FullName);
            }
            else if (sortType == "FullName" && sortOrder == "desc")
            {
                employees = employees.OrderByDescending(e => e.FullName);
            }
            else if (sortType == "Position" && sortOrder == "asc")
            {
                employees = employees.OrderBy(e => e.Position);
            }
            else if (sortType == "Position" && sortOrder == "desc")
            {
                employees = employees.OrderByDescending(e => e.Position);
            }

            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 1;
            if (pageNumber < 1) pageNumber = 1;
            employees = employees.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            return View("Index", employees);
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Employee emp = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            ViewBag.CurrentEmployee = emp;
            if (emp == null)
            {
                return NotFound();
            }
            else
            {
                return View("Details", emp);
            }
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Create");
        }

        [HttpPost]
        public IActionResult AddNew(Employee emp, IFormFile? imageFormFile)
        {
            if (((emp.JoinDateTime - emp.BirthDate).Days / 365) < 18)
            {
                ModelState.AddModelError("", "illegal Hiring/Joining Age(Under 18 Years Old).");
            }
            if (ModelState.IsValid == true)
            {
                if (imageFormFile == null)
                {
                    emp.ImagePath = "\\images\\No.jpg";
                }
                else
                {
                    Guid imgGuid = Guid.NewGuid();
                    string imgExtenstion = Path.GetExtension(imageFormFile.FileName);
                    string imgName = imgGuid + imgExtenstion;
                    emp.ImagePath = "\\images\\employees\\" + imgName;
                    string imgFullPath = _webHostEnvironment.WebRootPath + emp.ImagePath;
                    FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(fileStream);
                    fileStream.Dispose();
                }

                _context.Employees.Add(emp);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Create", emp);

            }
        }
        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Employee emp = _context.Employees.Find(id);
            if (emp == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit", emp);
            }
        }

        [HttpPost]
        public IActionResult EditCurrent(Employee emp, IFormFile? imageFormFile)
        {
            if (((emp.JoinDateTime - emp.BirthDate).Days / 365) < 18)
            {
                ModelState.AddModelError("", "illegal Hiring/Joining Age(Under 18 Years Old).");
            }
            if (ModelState.IsValid == true)
            {
                if (imageFormFile != null)
                {
                    if (emp.ImagePath != "\\images\\No.jpg")
                    {

                        string imgpath = _webHostEnvironment.WebRootPath + emp.ImagePath;
                        if (System.IO.File.Exists(imgpath))
                        {
                            System.IO.File.Delete(imgpath);
                        }
                    }
                    Guid imgGuid = Guid.NewGuid();
                    string imgExtenstion = Path.GetExtension(imageFormFile.FileName);
                    string imgName = imgGuid + imgExtenstion;
                    emp.ImagePath = "\\images\\employees\\" + imgName;
                    string imgFullPath = _webHostEnvironment.WebRootPath + emp.ImagePath;
                    FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                _context.Employees.Update(emp);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit", emp);
            }
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Employee emp = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            ViewBag.CurrentEmployee = emp;
            if (emp == null)
            {
                return NotFound();
            }
            else
            {
                return View("Delete", emp);
            }
        }
        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Employee emp = _context.Employees.Find(id);

            if (emp.ImagePath != "\\images\\No.jpg")
            {
                string imgpath = _webHostEnvironment.WebRootPath + emp.ImagePath;
                System.IO.File.Delete(imgpath);
            }

            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction("GetIndexView");
        }
        public string GreeVisitor()
        {
            return "welcome to Skyline";
        }
        public string GreeUser(string name)
        {
            return "Hi " + name;
        }
        public string GetAge(string name, int birthyear)
        {
            int ageyear = DateTime.Now.Year - birthyear;
            return $" Hi {name}. You are {ageyear} years old.";
        }
    }

}
