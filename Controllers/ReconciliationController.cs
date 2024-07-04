using Microsoft.AspNetCore.Mvc;
using BankReconciliationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankReconciliationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReconciliationController : ControllerBase
    {
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
                    // Convert decimal to double before multiplication
                    if (Math.Abs((double)bankTransaction.Amount - (double)accountingTransaction.Amount) <= tolerance * Math.Max((double)bankTransaction.Amount, (double)accountingTransaction.Amount))
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
