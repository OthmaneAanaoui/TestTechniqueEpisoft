import React, { useState } from 'react';
import axios from 'axios';
import Button from '@mui/material/Button';
import { TextField, Box, CircularProgress, Snackbar } from '@mui/material';
import MuiAlert from '@mui/material/Alert';

const ImportTransactions = ({ type, onImport }) => {
  const [file, setFile] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleImport = async () => {
    if (!file) return;

    setLoading(true);
    setError(null);

    const formData = new FormData();
    formData.append('file', file);

    try {
      const response = await axios.post(`/api/${type}/import`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      onImport(response.data);
      showSnackbar(`${type} transactions imported successfully`);
    } catch (error) {
      console.error('Error importing file:', error);
      setError('Error importing file. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const showSnackbar = (message) => {
    setSnackbarMessage(message);
    setSnackbarOpen(true);
  };

  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  return (
    <Box className="my-4">
      <TextField type="file" onChange={handleFileChange} variant="outlined" />
      <Button
        variant="contained"
        color="primary"
        onClick={handleImport}
        className="ml-2"
        disabled={!file || loading}
      >
        {loading ? <CircularProgress size={24} /> : `Import ${type}`}
      </Button>

      <Snackbar open={snackbarOpen} autoHideDuration={6000} onClose={handleCloseSnackbar}>
        <MuiAlert elevation={6} variant="filled" onClose={handleCloseSnackbar} severity="success">
          {snackbarMessage}
        </MuiAlert>
      </Snackbar>

      {error && (
        <MuiAlert severity="error" className="mt-2" onClose={() => setError(null)}>
          {error}
        </MuiAlert>
      )}
    </Box>
  );
};

export default ImportTransactions;
