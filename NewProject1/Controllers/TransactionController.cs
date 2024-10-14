using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Banktransactions.Data;
using Banktransactions.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Banktransactions.Controllers
{
    public class TransactionController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Branches = new SelectList(_context.Branches, "Id", "name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction model)
        {
            if (!ModelState.IsValid)
            {

                var branch = await _context.Branches.FindAsync(model.BranchId);

                if (branch == null)
                {
                    ModelState.AddModelError(string.Empty, "Branch not found.");
                    return View(model);
                }


                if (model.IsCredit && model.Amount > branch.creditlimit)
                {
                    TempData["message"] = "Credit amount exceeds the branch's credit limit.";
                    //ModelState.AddModelError("Amount", "Credit amount exceeds the branch's credit limit.");
                    return View(model);
                }

                if (!model.IsCredit && model.Amount > branch.debitlimit)
                {
                    TempData["message"] = "debit amount exceeds the branch's debit limit.";
                    //ModelState.AddModelError("Amount", "Debit amount exceeds the branch's debit limit.");
                    return View(model);
                }


                model.TransactionDate = DateTime.Now;
                _context.Transactions.Add(model);
                await _context.SaveChangesAsync();
                TempData["message"] = "Transaction added successfully";
                return RedirectToAction("Index");

            }

            ViewBag.Branches = new SelectList(_context.Branches, "Id", "name");
            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var query = _context.Transactions.Include(t => t.Branch).AsQueryable();
            var transactions = await _context.Transactions.Include(t => t.Branch).ToListAsync();
            var Transactions = await query.ToListAsync();
            ViewBag.TotalAmount = Transactions.Sum(t => t.Amount);
            return View(Transactions);
                   
        }
        public async Task<IActionResult> Filter(DateTime? fromDate, DateTime? toDate, bool? isCredit)
        {
            var query = _context.Transactions.Include(t => t.Branch).AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= toDate.Value);
            }

            if (isCredit.HasValue)
            {
                query = query.Where(t => t.IsCredit == isCredit.Value);
            }


           var filteredTransactions = await query.ToListAsync();
            return View(filteredTransactions);
            
        }
    }
}