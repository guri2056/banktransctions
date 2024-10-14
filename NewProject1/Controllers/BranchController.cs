using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Banktransactions.Data;
using Banktransactions.Models;

namespace Banktransactions.Controllers
{
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BranchController(ApplicationDbContext context)
        {
         _context = context;
        }
        [HttpGet]
        public IActionResult Create()
        {
           

            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Create(Branch model)
        {
            if (!ModelState.IsValid)
            {

                var existingBranch = await _context.Branches
           .FirstOrDefaultAsync(b => b.name.ToLower() == model.name.ToLower());

                if (existingBranch != null)
                {
                    TempData["message"] = "A branch with this name already exists.";
                    return View(model);
                }

                _context.Branches.Add(model);
                await _context.SaveChangesAsync();
                TempData["message"] = "Branch added succesfully";
                return RedirectToAction("Index");
            }

            return View(model);
        }


        public async Task<IActionResult> Index()
        {
            var branches = await _context.Branches.ToListAsync();
            return View(branches);
        }
    }
    
    
}
