using System;

namespace BankReconciliationAPI.Models
{
    /// <summary>
    /// Représente une transaction bancaire.
    /// </summary>
    public class BankTransaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
