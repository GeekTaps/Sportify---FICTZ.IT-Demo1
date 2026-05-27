import axios from 'axios';

// src/api/api-client.js
export const apiClient = axios.create({
  baseURL: 'http://localhost:5266/api', // El puerto que les asigne Visual Studio
});
