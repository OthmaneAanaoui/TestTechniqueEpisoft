using Microsoft.AspNetCore.Mvc;
using BankReconciliationAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using System.Globalization;

namespace BankReconciliationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankTransactionsController : ControllerBase
    {
        public static List<BankTransaction> _bankTransactions = new List<BankTransaction>();

        /// <summary>
        /// Importe les transactions bancaires à partir d'un fichier CSV.
        /// </summary>
        /// <returns>La liste des transactions bancaires importées.</returns>
        [HttpPost("import")]
        public IActionResult ImportBankTransactions()
        {
            var file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                _bankTransactions = csv.GetRecords<BankTransaction>().ToList();
            }

            return Ok(_bankTransactions);
        }

        /// <summary>
        /// Récupère la liste des transactions bancaires importées.
        /// </summary>
        /// <returns>La liste des transactions bancaires.</returns>
        [HttpGet]
        public IActionResult GetBankTransactions()
        {
            return Ok(_bankTransactions);
        }
    }
}
