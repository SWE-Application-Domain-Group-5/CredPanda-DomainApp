using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EliApp.Data;
using EliApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EliApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly EliAppContext _context;
        private AccountModel oldAccount; //This will hold the 'before' snapshot of any changes to an account - Rasul

        public AccountController(EliAppContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountModel.ToListAsync());
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AccountModel == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            AccountModel model = new AccountModel();
            model.AccountUserID = User.Identity.Name;
            return View(model);
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountName,AccountNumber,AccountDescription,AccountType,AccountCategory,AccountSubcategory,AccountInitialBalance,AccountCurrentBalance,DisplayInitialBalance,DisplayCurrentBalance,AccountCreationTime,AccountOrder,AccountStatement")] AccountModel accountModel)
        {
            accountModel.AccountUserID = User.Identity.Name;
            //Later - Add a quick Find algorithm to change the email into the generated username - Rasul
            accountModel.AccountCurrentBalance = accountModel.AccountInitialBalance;
            if (ModelState.IsValid)
            {
                _context.Add(accountModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountModel);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AccountModel == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModel.FindAsync(id);
            if (accountModel == null)
            {
                return NotFound();
            }
            else
            {
                oldAccount = accountModel;
            }
            return View(accountModel);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountName,AccountNumber,AccountDescription,AccountType,AccountCategory,AccountSubcategory,AccountInitialBalance,AccountCurrentBalance,AccountCreationTime,AccountUserID,AccountOrder,AccountStatement")] AccountModel accountModel)
        {
            if (id != accountModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountModelExists(accountModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(accountModel);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AccountModel == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AccountModel == null)
            {
                return Problem("Entity set 'EliAppContext.AccountModel' is null.");
            }
            var accountModel = await _context.AccountModel.FindAsync(id);
            if (accountModel != null)
            {
                _context.AccountModel.Remove(accountModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountModelExists(int id)
        {
          return _context.AccountModel.Any(e => e.Id == id);
        }

        public async Task<string> CreateNewAccount(AccountModel model)
        {
            var newAccount = new AccountModel()
            {
                Id = model.Id,
                AccountName = model.AccountName,
                AccountNumber = model.AccountNumber,
                AccountDescription = model.AccountDescription,
                AccountType = model.AccountType,
                AccountCategory = model.AccountCategory,
                AccountSubcategory = model.AccountSubcategory,
                AccountInitialBalance = model.AccountInitialBalance,
                AccountCurrentBalance = model.AccountCurrentBalance,
                AccountOrder = model.AccountOrder,
                AccountStatement = model.AccountStatement,
                AccountCreationTime = DateTime.Today
            };
            _context.Add(newAccount);
            await _context.SaveChangesAsync();
            return model.AccountNumber;
        }
        public async Task<IActionResult> ViewJournalEntry(string id)
        {
            //Find the associated Journal Entry and return its "Details" View
            return View(_context.EntryModel.Find());
        }
    }
}
