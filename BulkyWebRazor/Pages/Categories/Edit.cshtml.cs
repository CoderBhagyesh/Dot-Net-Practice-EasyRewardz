using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category category { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            category = _db.Categories.Find(id);
        }
    }
}
