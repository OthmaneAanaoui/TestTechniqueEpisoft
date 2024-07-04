using Microsoft.AspNetCore.Mvc;
using BankReconciliationAPI.Models; // Importation du modèle de données des transactions comptables
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper; // Importation de CsvHelper pour la manipulation des fichiers CSV
using System.Globalization;

namespace BankReconciliationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountingTransactionsController : ControllerBase
    {
        // Liste statique pour stocker les transactions comptables importées
        public static List<AccountingTransaction> _accountingTransactions = new List<AccountingTransaction>();

        /// <summary>
        /// Action pour importer les transactions comptables à partir d'un fichier CSV.
        /// </summary>
        /// <returns>ActionResult avec les transactions comptables importées ou BadRequest si aucun fichier n'est téléchargé.</returns>
        [HttpPost("import")]
        public IActionResult ImportAccountingTransactions()
        {
            var file = Request.Form.Files[0]; // Récupération du fichier CSV envoyé via la requête

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded."); // Retourne un BadRequest si aucun fichier n'est téléchargé

            using (var reader = new StreamReader(file.OpenReadStream())) // Utilisation de StreamReader pour lire le contenu du fichier
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // Utilisation de CsvHelper avec le bon format culturel
            {
                _accountingTransactions = csv.GetRecords<AccountingTransaction>().ToList(); // Lecture des enregistrements CSV et conversion en liste de transactions comptables
            }

            return Ok(_accountingTransactions); // Retourne un OK avec les transactions comptables importées
        }

        /// <summary>
        /// Action pour récupérer la liste des transactions comptables importées.
        /// </summary>
        /// <returns>ActionResult avec la liste des transactions comptables.</returns>
        [HttpGet]
        public IActionResult GetAccountingTransactions()
        {
            return Ok(_accountingTransactions); // Retourne un OK avec la liste actuelle des transactions comptables
        }
    }
}
