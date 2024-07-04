import React, { useState } from 'react';
import axios from 'axios';

function AccountingTransactions() {
  const [transactions, setTransactions] = useState([]);

  const handleFileUpload = async (event) => {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = async (e) => {
      const text = e.target.result;
      const lines = text.split('\n').slice(1);
      const data = lines.map(line => {
        const [date, description, amount] = line.split(',');
        return { date, description, amount: parseFloat(amount) };
      });

      const response = await axios.post('http://localhost:5097/api/accountingtransactions/import', data);
      setTransactions(response.data);
    };

    reader.readAsText(file);
  };

  return (
    <div className="p-4 bg-white rounded shadow">
      <h2 className="text-xl mb-4">Accounting Transactions</h2>
      <input type="file" accept=".csv" onChange={handleFileUpload} />
      <table className="mt-4 w-full">
        <thead>
          <tr>
            <th>Date</th>
            <th>Description</th>
            <th>Amount</th>
          </tr>
        </thead>
        <tbody>
          {transactions.map((transaction, index) => (
            <tr key={index}>
              <td>{transaction.date}</td>
              <td>{transaction.description}</td>
              <td>{transaction.amount}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default AccountingTransactions;
