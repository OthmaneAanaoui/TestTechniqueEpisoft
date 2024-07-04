using System;

namespace BankReconciliationAPI.Models
{
    /// <summary>
    /// Représente une transaction comptable.
    /// </summary>
    public class AccountingTransaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
