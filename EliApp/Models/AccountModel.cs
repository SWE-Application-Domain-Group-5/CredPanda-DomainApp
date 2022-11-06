using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Security.Cryptography;

namespace EliApp.Models
{
    public enum Statement
    {
        BalanceSheet,
        IncomeStatement, 
        RetainedEarnings
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
                accnum += rand.Next(0, 9).ToString();
            }
            AccountNumber = accnum;
            AccountCurrentBalance = AccountInitialBalance;
            AccountUserID = rand.Next(0,100).ToString();
        }
        public int Id { get; set; }
        [DisplayName("Name")]
        public string AccountName { get; set; }
        [DisplayName("Account #")]
        public string AccountNumber { get; set; }
        [DisplayName("Description")]
        public string AccountDescription { get; set; }
        [DisplayName("Type")]
        public string AccountType { get; set; } //debit or credit
        [DisplayName("Category")]
        public string AccountCategory { get; set; }
        [DisplayName("Subcategory")]
        public string AccountSubcategory { get; set; }
        [DisplayName("Initial Balance")]
        public float AccountInitialBalance { get; set; }
        [DisplayName("Current Balance")]
        public float AccountCurrentBalance { get; set; }
        [DisplayName("Creation Time")]
        public DateTime AccountCreationTime { get; set; } = DateTime.Now;
        [DisplayName("UserID")]
        public string AccountUserID { get; set; }
        [DisplayName("Order")]
        public string AccountOrder { get; set; } //"essentially the order the entries are made i.e. Cash (01), Credit (02). "
        [DisplayName("Statement")]
        public string AccountStatement { get; set; }
        [DisplayName("Comment")]
        public string AccountComment { get; set; }
    }
}
