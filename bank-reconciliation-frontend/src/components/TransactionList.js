import React from 'react';
import { DataGrid } from '@mui/x-data-grid';

const TransactionList = ({ transactions, title }) => {
  // Ensure transactions is defined and has data before mapping
  const addIdsToTransactions = (transactions) => {
    if (!transactions) return []; // Handle null or undefined transactions
    return transactions.map((transaction, index) => ({
      id: index + 1,
      ...transaction
    }));
  };


  const transactionsWithIds = addIdsToTransactions(transactions);

  const columns = [
    { field: 'id', headerName: 'ID', width: 90 },
    { field: 'date', headerName: 'Date', flex: 1 },
    { field: 'description', headerName: 'Description', flex: 2 },
    { field: 'amount', headerName: 'Amount', flex: 1 },
  ];

  return (
    <div className="my-4" style={{ height: '100%', minHeight: '400px' }}>
      <h2 className="text-xl font-bold mb-2">{title}</h2>
      <div style={{ height: '100%', width: '100%', marginBottom: '1rem' }}>
        <DataGrid
          rows={transactionsWithIds}
          columns={columns}
          pageSize={5}
          rowsPerPageOptions={[5, 10, 20]}
          pagination
          autoHeight
        />
      </div>
    </div>
  );
};

export default TransactionList;
