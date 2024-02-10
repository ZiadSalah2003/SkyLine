using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using skyline.Data;
using skyline.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.IO;

namespace skyline.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DepartmentController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        //[Route("[controller]/List")]
        [HttpGet]
        public IActionResult GetIndexView(string? search, string sortType, string sortOrder, int pageSize = 20, int pageNumber =1)
        {
            ViewBag.CurrentSearch = search;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            IQueryable<Department> departments = _context.Departments.AsQueryable();
            if (string.IsNullOrEmpty(search) == false)
            {
                departments = departments.Where(d => d.Name.Contains(search));
            }
            if (sortType == "Name" && sortOrder == "asc")
            {
                departments = departments.OrderBy(d => d.Name);
            }
            else if (sortType == "Name" && sortOrder == "desc")
            {
                departments = departments.OrderByDescending(d => d.Name);
            }
            else if (sortType == "Description" && sortOrder == "asc")
            {
                departments = departments.OrderBy(d => d.Description);
            }
            else if (sortType == "Description" && sortOrder == "desc")
            {
                departments = departments.OrderByDescending(d => d.Description);
            }

            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 1;
            if (pageNumber < 1) pageNumber = 1;
            departments = departments.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            return View("Index", departments);
        }
        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Department dep = _context.Departments.Include(e => e.Employees).FirstOrDefault(e => e.Id == id);
            ViewBag.CurrentDepartment = dep;
            if (dep == null)
            {
                return NotFound();
            }
            else
            {
                return View("Details", dep);
            }
        }
        [HttpGet]
        public IActionResult GetCreateView()
        {
            return View("Create");
        }
        [HttpPost]
        public IActionResult AddNew(Department dep, IFormFile? imageFormFile)
        {
            if (ModelState.IsValid == true)
            {
                if (imageFormFile == null)
                {
                    dep.ImagePath = "\\images\\No.jpg";
                }
                else
                {
                    Guid imgGuid = Guid.NewGuid();
                    string imgExtenstion = Path.GetExtension(imageFormFile.FileName);
                    string imgName = imgGuid + imgExtenstion;
                    dep.ImagePath = "\\images\\department\\" + imgName;
                    string imgFullPath = _webHostEnvironment.WebRootPath + dep.ImagePath;
                    FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                _context.Departments.Add(dep);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Create", dep);
            }
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Department dep = _context.Departments.Find(id);
            if (dep == null)
            {
                return NotFound();
            }
            else
            {
                return View("Edit", dep);
            }
        }

        [HttpPost]
        public IActionResult EditCurrent(Department dep, IFormFile? imageFormFile)
        {
            if (ModelState.IsValid == true)
            {
                if (imageFormFile != null)
                {
                    if (dep.ImagePath != "\\images\\No.jpg")
                    {

                        string imgpath = _webHostEnvironment.WebRootPath + dep.ImagePath;
                        if (System.IO.File.Exists(imgpath))
                        {
                            System.IO.File.Delete(imgpath);
                        }
                    }
                    Guid imgGuid = Guid.NewGuid();
                    string imgExtenstion = Path.GetExtension(imageFormFile.FileName);
                    string imgName = imgGuid + imgExtenstion;
                    dep.ImagePath = "\\images\\department\\" + imgName;
                    string imgFullPath = _webHostEnvironment.WebRootPath + dep.ImagePath;
                    FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                _context.Departments.Update(dep);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Edit", dep);
            }
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Department dep = _context.Departments.Include(e => e.Employees).FirstOrDefault(e => e.Id == id);
            ViewBag.CurrentDepartment = dep;
            if (dep == null)
            {
                return NotFound();
            }
            else
            {
                return View("Delete", dep);
            }
        }
        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Department dep = _context.Departments.Find(id);
            if (dep.ImagePath != "\\images\\No.jpg")
            {
                string imgpath = _webHostEnvironment.WebRootPath + dep.ImagePath;
                if (System.IO.File.Exists(imgpath))
                {
                    System.IO.File.Delete(imgpath);
                }
            }
            _context.Departments.Remove(dep);
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
