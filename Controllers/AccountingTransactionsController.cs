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
    public class AccountingTransactionsController : ControllerBase
    {
        public static List<AccountingTransaction> _accountingTransactions = new List<AccountingTransaction>();

        [HttpPost("import")]
        public IActionResult ImportAccountingTransactions()
        {
            var file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                _accountingTransactions = csv.GetRecords<AccountingTransaction>().ToList();
            }

            return Ok(_accountingTransactions);
        }

        [HttpGet]
        public IActionResult GetAccountingTransactions()
        {
            return Ok(_accountingTransactions);
        }
    }
}
