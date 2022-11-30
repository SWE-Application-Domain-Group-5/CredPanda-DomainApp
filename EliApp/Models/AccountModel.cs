using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EliApp.Models
{
    public enum Statement
    {
        [Display(Name = "Select a Statement")]
        None = 0,
        [Display(Name = "Balance Sheet")]
        BalanceSheet = 1,
        [Display(Name = "Income Statement")]
        IncomeStatement = 2,
        [Display(Name = "Retained Earnings")]
        RetainedEarnings = 3,
    }

    public enum AccountCategory
    {
        [Display(Name = "Select a Category")]
        None = 0,
        [Display(Name = "Assets")]
        Assets = 1,
        [Display(Name = "Revenue")]
        Revenue = 2,
        [Display(Name = "Liabilities")]
        Liabilities = 3,
        [Display(Name = "Expenses")]
        Expenses = 4,
        [Display(Name = "Equity")]
        Equity = 5,

    }

    public class AccountModel
    {
        public AccountModel()
        {
            DateTime now = new DateTime();
            now = DateTime.Now;
            Random rand = new Random();
            string accnum;
            accnum = now.Month.ToString() + now.Day.ToString();
            for (int ctr = 0; ctr <= 5; ctr++)
            {
                accnum += rand.Next(0, 9);
            }
            AccountNumber = accnum;
            this.AccountCurrentBalance = this.AccountInitialBalance;
        }
        public int Id { get; set; }
        [DisplayName("Name")]
        public string AccountName { get; set; }
        [DisplayName("Account #")]
        public string AccountNumber { get; set; }
        [DisplayName("Description")]
        [AllowNull]
        [ValidateNever]
        public string AccountDescription { get; set; }
        [DisplayName("Type")]
        public AccountType AccountType { get; set; } //debit or credit
        [DisplayName("Category")]
        public AccountCategory AccountCategory { get; set; }
        [DisplayName("Subcategory")]
        [AllowNull]
        [ValidateNever]
        public string AccountSubcategory { get; set; }
        [DisplayName("Initial Balance")]
        public float AccountInitialBalance { get; set; }
        [DisplayName("Current Balance")]
        public float AccountCurrentBalance { get; set; }
        [DisplayName("Creation Time")]
        public DateTime AccountCreationTime { get; set; } = DateTime.Now;
        [DisplayName("UserID")]
        [ValidateNever]
        public string AccountUserID { get; set; }
        [DisplayName("Order")]
        public int AccountOrder { get; set; } //"essentially the order the entries are made i.e. Cash (01), Credit (02). "
        [DisplayName("Statement")]
        public Statement AccountStatement { get; set; }
        [AllowNull]
        [ValidateNever]
        public int entryId { get; set; }
        /* List of LedgerModels. Don't know if it'll work rn.
        [AllowNull]
        [ValidateNever]
        public List<LedgerModel> LedgerList { get; set;}
        */
    }
}
