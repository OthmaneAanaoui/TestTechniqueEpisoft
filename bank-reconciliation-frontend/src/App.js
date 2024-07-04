import React, { useState } from 'react';
import axios from 'axios';
import Container from '@mui/material/Container';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import ImportTransactions from './components/ImportTransactions';
import TransactionList from './components/TransactionList';
import ReconcileButton from './components/ReconcileButton';
import TextField from '@mui/material/TextField';

import './App.css';

const App = () => {
  const [bankTransactions, setBankTransactions] = useState([]);
  const [accountingTransactions, setAccountingTransactions] = useState([]);
  const [reconciliationData, setReconciliationData] = useState(null);
  const [highlightedRow, setHighlightedRow] = useState(null);

  const handleBankTransactionsImport = (data) => {
    setBankTransactions(data);
  };

  const handleAccountingTransactionsImport = (data) => {
    setAccountingTransactions(data);
  };

  const handleReconcile = async () => {
    try {
      const response = await axios.post('/api/reconciliation', null, {
        params: { tolerance: 0.05 }, // Modifier la tolérance selon les besoins
      });
      setReconciliationData(response.data);
    } catch (error) {
      console.error('Error reconciling transactions:', error);
    }
  };

  const handleRowHover = (params) => {
    setHighlightedRow(params.row.id);
  };

  return (
    <Container>
      <Box className="py-4">
        <Typography variant="h4" component="h1" className="text-center mb-4">
          Bank Reconciliation
        </Typography>
        <Box className="flex flex-col md:flex-row md:justify-between">
          <ImportTransactions type="banktransactions" onImport={handleBankTransactionsImport} />
          <ImportTransactions type="accountingtransactions" onImport={handleAccountingTransactionsImport} />
        </Box>
        <TransactionList transactions={bankTransactions} title="Bank Transactions" onRowHover={handleRowHover} />
        <TransactionList transactions={accountingTransactions} title="Accounting Transactions" onRowHover={handleRowHover} />

        <Box
          component="form"
          sx={{
            '& > :not(style)': { m: 1, width: '25ch' },
          }}
          noValidate
          autoComplete="off"
        >
   
          <ReconcileButton onReconcile={handleReconcile} />
          {reconciliationData && (
            <Box>
              
              <Typography variant="body1" className="text-center">
                Reconciliation Rate: {reconciliationData.ReconciliationRate !== undefined
                  ? (reconciliationData.ReconciliationRate * 100).toFixed(2) + '%'
                  : 'N/A'}
              </Typography>
              
            </Box>
          )}
        </Box>
       
        
      </Box>
    </Container>
  );
};

export default App;
