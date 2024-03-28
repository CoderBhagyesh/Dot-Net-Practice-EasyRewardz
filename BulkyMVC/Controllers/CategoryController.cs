using Bulky.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Controllers
{
    public class CategoryController : Controller
    {
        // To get the list of rows of tables, first we will need to get the db obj
        // we will get that through dbcontext

        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) { 
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            /*if(obj.Name == obj.DisplayOrder)
            {
                ModelState.AddModelError("Name", "Name should not be same as Display Value");
            }*/

            if(ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created succesfully";
                return RedirectToAction("Index"); // the 1st parameter will be action name and 2nd param will be controller name and it's optional
            }

            return View();
           
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id==0)
            {
                return NotFound();
            }

            Category? categroyFromDb = _db.Categories.Find(id);
            Category? categroyFromDb2 = _db.Categories.FirstOrDefault(x => x.Id == id);
            Category? categroyFromDb3 = _db.Categories.Where(x=>x.Id == id).FirstOrDefault();

            if (categroyFromDb2 == null)
            {
                return NotFound();
            }

            return View(categroyFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated succesfully";
                return RedirectToAction("Index"); 
            }

            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categroyFromDb = _db.Categories.Find(id);
       
            if (categroyFromDb == null)
            {
                return NotFound();
            }

            return View(categroyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id==0)
            {
                return NotFound();
            }   

            Category category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
