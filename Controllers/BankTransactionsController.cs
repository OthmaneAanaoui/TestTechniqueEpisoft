using Microsoft.AspNetCore.Mvc;
using BankReconciliationAPI.Models; // Importation du modèle de données des transactions bancaires
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper; // Importation de CsvHelper pour la manipulation des fichiers CSV
using System.Globalization;

namespace BankReconciliationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankTransactionsController : ControllerBase
    {
        // Liste statique pour stocker les transactions bancaires importées
        public static List<BankTransaction> _bankTransactions = new List<BankTransaction>();

        /// <summary>
        /// Action pour importer les transactions bancaires à partir d'un fichier CSV.
        /// </summary>
        /// <returns>ActionResult avec les transactions bancaires importées ou BadRequest si aucun fichier n'est téléchargé.</returns>
        [HttpPost("import")]
        public IActionResult ImportBankTransactions()
        {
            var file = Request.Form.Files[0]; // Récupération du fichier CSV envoyé via la requête

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded."); // Retourne un BadRequest si aucun fichier n'est téléchargé

            using (var reader = new StreamReader(file.OpenReadStream())) // Utilisation de StreamReader pour lire le contenu du fichier
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // Utilisation de CsvHelper avec le bon format culturel
            {
                _bankTransactions = csv.GetRecords<BankTransaction>().ToList(); // Lecture des enregistrements CSV et conversion en liste de transactions bancaires
            }

            return Ok(_bankTransactions); // Retourne un OK avec les transactions bancaires importées
        }

        /// <summary>
        /// Action pour récupérer la liste des transactions bancaires importées.
        /// </summary>
        /// <returns>ActionResult avec la liste des transactions bancaires.</returns>
        [HttpGet]
        public IActionResult GetBankTransactions()
        {
            return Ok(_bankTransactions); // Retourne un OK avec la liste actuelle des transactions bancaires
        }
    }
}
