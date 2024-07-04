import React, { useState } from 'react';
import axios from 'axios';
import { Button, TextField,Box } from '@mui/material';

const ReconcileButton = ({ onReconcile }) => {
  const [tolerance, setTolerance] = useState(0.05); // État local pour la tolérance

  const handleReconcile = async () => {
    try {
      const response = await axios.post('/api/reconciliation', null, {
        params: { tolerance: tolerance }, // Utilisation de la tolérance actuelle
      });
      onReconcile(response.data);
    } catch (error) {
      console.error('Error reconciling transactions:', error);
    }
  };

  const handleToleranceChange = (event) => {
    setTolerance(event.target.value); // Mettre à jour la tolérance à partir du champ de saisie
  };

  return (
    <Box
    component="form"
    sx={{
      '& > :not(style)': { m: 1, width: '25ch' },
    }}
    noValidate
    autoComplete="off"
  >
      <TextField
        id="tolerance-input"
        label="Tolerance"
        variant="outlined"
        type="number"
        step="0.01"
        value={tolerance}
        onChange={handleToleranceChange}
        className="mr-2"
      />
      <Button
        variant="contained"
        color="secondary"
        onClick={handleReconcile}
      >
        Reconcile Transactions
      </Button>
    </Box>
  );
};

export default ReconcileButton;
