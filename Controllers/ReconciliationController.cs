using Microsoft.AspNetCore.Mvc;
using BankReconciliationAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankReconciliationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReconciliationController : ControllerBase
    {
        /// <summary>
        /// Réconcilie les transactions bancaires et comptables en fonction d'une tolérance.
        /// </summary>
        /// <param name="tolerance">Tolérance en pourcentage.</param>
        /// <returns>Le taux de réconciliation et les paires de transactions rapprochées.</returns>
        [HttpPost]
        public IActionResult ReconcileTransactions([FromQuery] double tolerance)
        {
            var bankTransactions = BankTransactionsController._bankTransactions;
            var accountingTransactions = AccountingTransactionsController._accountingTransactions;

            var reconciledPairs = new List<(BankTransaction, AccountingTransaction)>();

            foreach (var bankTransaction in bankTransactions)
            {
                foreach (var accountingTransaction in accountingTransactions)
                {
                    if (Math.Abs(bankTransaction.Amount - accountingTransaction.Amount) <= tolerance * Math.Max(bankTransaction.Amount, accountingTransaction.Amount))
                    {
                        reconciledPairs.Add((bankTransaction, accountingTransaction));
                        break;
                    }
                }
            }

            double reconciliationRate = (double)reconciledPairs.Count / Math.Max(bankTransactions.Count, accountingTransactions.Count);

            return Ok(new { ReconciliationRate = reconciliationRate, ReconciledPairs = reconciledPairs });
        }
    }
}
