using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EliApp.Data;
using EliApp.Models;
using Microsoft.AspNetCore.Identity;
using EliApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace EliApp.Controllers
{
    public class EntryController : Controller
    {
        private readonly EliAppContext _context;
        private readonly UserManager<EliAppUser> _userManager;
        private IWebHostEnvironment _environment; // added

        public EntryController(EliAppContext context, UserManager<EliAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Entry
        public async Task<IActionResult> Index(string searchString)
        {
            var entries = from e in _context.EntryModel
                           select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                entries = entries.Where(e => e.accountInvolved.Contains(searchString)
                                       || e.amount.ToString().Contains(searchString));
            }
            return View(await entries.ToListAsync());
        }

        // GET: Entry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EntryModel == null)
            {
                return NotFound();
            }

            var entryModel = await _context.EntryModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entryModel == null)
            {
                return NotFound();
            }

            return View(entryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,DateTime,accountInvolved,supportingFile,accountType,state,amount, comment")] EntryModel entryModel)
        {
            if (id != entryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryModelExists(entryModel.Id))
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
            return View(entryModel);
        }

        // GET: Entry/Create
        // Returns the Create VIEW, not to actually create an object
        public IActionResult Create()
        {
            EntryModel model = new EntryModel();
            model.userId = User.Identity.Name;
            return View(model);
        }

        // POST: Entry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateTime,userId,accountInvolved,accountType,Upload,amount,state,")] EntryModel entryModel)
        {
            //AccountModel account;
            if (ModelState.IsValid)
            {
                entryModel.DateTime = DateTime.Today;
                if (entryModel.EntryUpload != null)
                {
                    var file = Path.Combine(_environment.WebRootPath, "uploads", entryModel.EntryUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await entryModel.EntryUpload.CopyToAsync(fileStream);
                    }
                    entryModel.supportingFile = entryModel.EntryUpload.FileName;
                }
                else
                { entryModel.supportingFile = "None"; }
                _context.Add(entryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entryModel);
        }

        // GET: Entry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EntryModel == null)
            {
                return NotFound();
            }

            var entryModel = await _context.EntryModel.FindAsync(id);
            if (entryModel == null)
            {
                return NotFound();
            }
            return View(entryModel);
        }

        // POST: Entry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,userId,DateTime,accountInvolved,Upload,accountType,state,amount")] EntryModel entryModel)
        {
            if (id != entryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryModelExists(entryModel.Id))
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
            return View(entryModel);
        }

        // GET: Entry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EntryModel == null)
            {
                return NotFound();
            }

            var entryModel = await _context.EntryModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entryModel == null)
            {
                return NotFound();
            }

            return View(entryModel);
        }

        // POST: Entry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EntryModel == null)
            {
                return Problem("Entity set 'EliAppContext.EntryModel'  is null.");
            }
            var entryModel = await _context.EntryModel.FindAsync(id);
            if (entryModel != null)
            {
                _context.EntryModel.Remove(entryModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntryModelExists(int id)
        {
          return _context.EntryModel.Any(e => e.Id == id);
        }

        [Authorize (Roles = "Manager")]
        public async Task<IActionResult> Approved(int id)
        {
            var entry = await _context.EntryModel.FindAsync(id);

            if (entry != null)
            {
                entry.state = EntryState.APPROVED;
                _context.Update(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = $"Entry with Id = {id} cannot be found";
                return View("NotFound");
            }

        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Declined([Bind("comment")] EntryModel entryModel, int id)
        {
            var entry = await _context.EntryModel.FindAsync(id);

            if (entry != null)
            {
                entry.state = EntryState.DECLINED;
                entry.comment = entryModel.comment;
                _context.Update(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = $"Entry with Id = {id} cannot be found";
                return View("NotFound");
            }
            
        }

        public AccountModel MakeAccount(int id)
        {
            var account = new AccountModel();
            return account;
        }

        /*
         * 
         * var file = Path.Combine(_environment.WebRootPath, "uploads", Upload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
                user.ProfilePicture = Upload.FileName;
        public string GetUsername(string userId)
        {
            //var user = userManager.FindByIdAsync(userId);
            string username = "";
            foreach (var user in userManager.Users)
            {
                if (userId == user.GeneratedUserName)
                {
                    username = user.GeneratedUserName;
                    break;
                }
            }
            return username;
        }
        */
    }
}
