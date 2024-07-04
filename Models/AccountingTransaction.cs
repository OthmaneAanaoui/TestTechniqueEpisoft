using System;

namespace BankReconciliationAPI.Models
{
    public class AccountingTransaction
    {

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
