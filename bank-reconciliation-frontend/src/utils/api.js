import axios from 'axios';

const instance = axios.create({
  baseURL: 'http://localhost:5097/api/', // Replace with your backend API URL
  headers: {
    'Content-Type': 'application/json',
  },
});

export default instance;
