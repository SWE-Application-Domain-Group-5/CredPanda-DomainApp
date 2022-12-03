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
using Microsoft.EntityFrameworkCore.Update;

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
                entries = entries.Where(e => e.account1.Contains(searchString)
                                       || e.account2.Contains(searchString)
                                       || e.amount.ToString().Contains(searchString));
            }
            return View(await entries.ToListAsync());
        }

        // GET: Entry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EntryModel == null) //if the id given is null, or there are no entries in the database
            {
                //return a Notfound error page
                return NotFound();
            }
            //function to find an entry model based on the id given
            var entryModel = await _context.EntryModel
                .FirstOrDefaultAsync(m => m.Id == id);
            //if the entry model returned is null
            if (entryModel == null)
            {
                //return a Notfound error page
                return NotFound();
            }
            //return a viewing page with the identified entry model passed
            return View(entryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("comment")] EntryModel entryModel)
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
            List<AccountModel> accountList = new List<AccountModel>();
            accountList = (from a in _context.AccountModel select a).ToList();
            accountList.Insert(0, new AccountModel
            {
                AccountName= "Select an Account",
            });
            ViewBag.message = accountList;
            return View(model);
        }

        // POST: Entry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateTime,userId,account1,account2,accountType,EntryUpload,amount,state,")] EntryModel entryModel)
        {
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
                var account = await _context.AccountModel.FindAsync(Convert.ToInt32(entryModel.account2));
                entryModel.account2 = account.AccountName;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,userId,DateTime,account1,account2,Upload,accountType,state,amount")] EntryModel entryModel)
        {
            if (id != entryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    entryModel.state = EntryState.PENDING;
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
                //make two null accounts that will hold the information for the involved accounts
                AccountModel account1 = null;
                AccountModel account2 = null;
                //make two new ledgers for both sides of the interaction
                LedgerModel ledger1 = new LedgerModel()
                {
                    description = "",
                    date = entry.DateTime,
                    accountID = 0,
                    associatedAccountName = "",
                    journalEntryID = entry.Id,
                    balance = 0,
                    debitAmount = 0,
                    creditAmount = 0,
                };
                LedgerModel ledger2 = new LedgerModel()
                {
                    description = "",
                    date = entry.DateTime,
                    accountID = 0,
                    associatedAccountName = "",
                    journalEntryID = entry.Id,
                    balance = 0,
                    debitAmount = 0,
                    creditAmount = 0,
                };
                foreach (var acc in _context.AccountModel) //try and match the frst account to an existing one
                {
                    if (entry.account1 == acc.AccountName)
                    {
                        account1 = acc;
                        ledger1.accountID = account1.Id;
                        break;
                    }
                }
                foreach (var acc in _context.AccountModel) //account 2 always exists in the system, so just match it
                {
                    if (entry.account2 == acc.AccountName)
                    {
                        account2 = acc;
                        ledger2.accountID = account2.Id;
                        break;
                    }
                }
                if (account1 == null) //if account 1 is still null...
                {
                    //...then it is a new account
                    account1 = new AccountModel
                    {
                        AccountName = entry.account1,
                        AccountInitialBalance = entry.amount,
                        AccountCategory = AccountCategory.None,
                        AccountCreationTime= DateTime.Now,
                        AccountCurrentBalance = entry.amount,
                        AccountDescription = "",
                        AccountNumber = "",
                        AccountOrder = _context.AccountModel.Count() + 1,
                        AccountStatement = Statement.BalanceSheet,
                        AccountSubcategory = "",
                        AccountType = AccountType.Debit,
                        AccountUserID = entry.userId,
                        entryId = entry.Id,
                    };
                    DateTime now = new DateTime();
                    now = DateTime.Now;
                    Random rand = new Random();
                    string accnum;
                    accnum = now.Month.ToString() + now.Day.ToString();
                    for (int ctr = 0; ctr <= 5; ctr++)
                    {
                        accnum += rand.Next(0, 9);
                    }
                    account1.AccountNumber = accnum;
                    //Attach account ids to the ledgers
                    ledger1.accountID = account1.Id;
                    ledger2.accountID = account2.Id;
                    //Associated account names are for the accounts that are receiving/giving money. They are NOT the accounts the ledger will go under
                    ledger1.associatedAccountName = account2.AccountName;
                    ledger2.associatedAccountName = account1.AccountName;

                    if (entry.accountType == AccountType.Debit) //if the entry is a debit... 
                    {
                        //then account 1 is a debit account and money goes from account 2 to account 1
                        account1.AccountType = AccountType.Debit;
                        ledger1.creditAmount = entry.amount;
                        ledger1.balance = account1.AccountCurrentBalance;
                        account2.AccountCurrentBalance -= entry.amount;
                        ledger2.debitAmount = entry.amount;
                        ledger2.balance = account2.AccountCurrentBalance;
                    }
                    else //otherwise... 
                    {
                        //the account is a credit and money goes from account 1 to account 2
                        account1.AccountType = AccountType.Credit;
                        ledger1.debitAmount = entry.amount;
                        ledger1.balance = account1.AccountCurrentBalance;
                        account2.AccountCurrentBalance += entry.amount;
                        ledger2.creditAmount = entry.amount;
                        ledger2.balance = account2.AccountCurrentBalance;
                    }
                    //Add the new account to the database
                    _context.Add(account1);
                }
                else //otherwise account 1 is not a new account
                {
                    //attach accountids to their corresponding ledgers
                    ledger1.accountID = account1.Id;
                    ledger2.accountID = account2.Id;
                    //Associated account names are for the accounts that are receiving/giving money. They are NOT the accounts the ledger will go under
                    ledger1.associatedAccountName = account2.AccountName;
                    ledger2.associatedAccountName = account1.AccountName;
                    if (entry.accountType == AccountType.Debit) //if the entry is a debit... 
                    {
                        //then account 1 is a debit account and money goes from account 2 to account 1
                        account1.AccountCurrentBalance += entry.amount;
                        ledger1.creditAmount = entry.amount;
                        ledger1.balance = account1.AccountCurrentBalance;
                        account2.AccountCurrentBalance -= entry.amount;
                        ledger2.debitAmount = entry.amount;
                        ledger2.balance = account2.AccountCurrentBalance;
                    }
                    else //otherwise... 
                    {
                        //the account is a credit and money goes from account 1 to account 2
                        account1.AccountCurrentBalance -= entry.amount;
                        ledger1.debitAmount = entry.amount;
                        ledger1.balance = account1.AccountCurrentBalance;
                        account2.AccountCurrentBalance += entry.amount;
                        ledger2.creditAmount = entry.amount;
                        ledger2.balance = account2.AccountCurrentBalance;
                    }
                    //Update account 1 in the database
                    _context.Update(account1);
                }
                //Add the ledgers to the database and update account 2 and the approved journal entry
                _context.Add(ledger1);
                _context.Add(ledger2);
                _context.Update(account2);
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
        public async Task<IActionResult> Declined(int id)
        {
            var entry = await _context.EntryModel.FindAsync(id);

            if (entry != null)
            {
                entry.state = EntryState.DECLINED;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Declined([Bind("comment")] EntryModel entryModel, int id)
        {
            var entry = await _context.EntryModel.FindAsync(id);

            if (entry != null)
            {
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

        /* Uploading functionality and username getting
        var file = Path.Combine(_environment.WebRootPath, "uploads", Upload.FileName);
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
