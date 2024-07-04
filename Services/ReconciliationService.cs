// Services/ReconciliationService.cs
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BankReconciliationAPI.Models; 
public class ReconciliationService
{
    public List<BankTransaction> ParseBankTransactions(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<BankTransaction>().ToList();
    }

    public List<AccountingTransaction> ParseAccountingTransactions(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<AccountingTransaction>().ToList();
    }

   public (List<(BankTransaction, AccountingTransaction)>, double) Reconcile(
    List<BankTransaction> bankTransactions,
    List<AccountingTransaction> accountingTransactions,
    double tolerancePercentage)
{
    var matches = new List<(BankTransaction, AccountingTransaction)>();
    decimal tolerance = (decimal)(tolerancePercentage / 100); // Conversion explicite en decimal

    foreach (var bankTx in bankTransactions)
    {
        var match = accountingTransactions.FirstOrDefault(accTx =>
            Math.Abs(bankTx.Amount - accTx.Amount) <= Math.Abs(bankTx.Amount * tolerance));

        if (match != null)
        {
            matches.Add((bankTx, match));
            accountingTransactions.Remove(match);
        }
    }

    double successRate = (double)matches.Count / bankTransactions.Count * 100;
    return (matches, successRate);
}
}
