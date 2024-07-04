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
        /// <summary>
        /// Action pour rapprocher les transactions bancaires et comptables selon une tolérance spécifiée.
        /// </summary>
        /// <param name="tolerance">La tolérance pour le rapprochement, exprimée en pourcentage.</param>
        /// <returns>ActionResult avec le taux de rapprochement et les paires de transactions rapprochées.</returns>
        [HttpPost]
        public IActionResult ReconcileTransactions([FromQuery] double tolerance)
        {
            var bankTransactions = BankTransactionsController._bankTransactions; // Récupération des transactions bancaires depuis BankTransactionsController
            var accountingTransactions = AccountingTransactionsController._accountingTransactions; // Récupération des transactions comptables depuis AccountingTransactionsController

            var reconciledPairs = new List<(BankTransaction, AccountingTransaction)>(); // Liste pour stocker les paires rapprochées

            foreach (var bankTransaction in bankTransactions)
            {
                foreach (var accountingTransaction in accountingTransactions)
                {
                    // Conversion des montants en double pour la comparaison avec la tolérance
                    if (Math.Abs((double)bankTransaction.Amount - (double)accountingTransaction.Amount) <= tolerance * Math.Max((double)bankTransaction.Amount, (double)accountingTransaction.Amount))
                    {
                        reconciledPairs.Add((bankTransaction, accountingTransaction)); // Ajout de la paire rapprochée à la liste
                        break; // Sortie de la boucle interne après avoir trouvé une correspondance
                    }
                }
            }

            double reconciliationRate = (double)reconciledPairs.Count / Math.Max(bankTransactions.Count, accountingTransactions.Count); // Calcul du taux de rapprochement

            return Ok(new { ReconciliationRate = reconciliationRate, ReconciledPairs = reconciledPairs }); // Retourne un OK avec le taux de rapprochement et les paires rapprochées
        }
    }
}
