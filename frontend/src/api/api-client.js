import axios from 'axios';

// src/api/api-client.js
export const apiClient = axios.create({
  baseURL: 'https://localhost:/api', // El puerto que les asigne Visual Studio
});
