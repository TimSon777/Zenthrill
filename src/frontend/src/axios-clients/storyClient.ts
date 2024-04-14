'use client'

import axios from 'axios';

const storyClient = axios.create({
    baseURL: 'http://localhost:8081'
});

storyClient.interceptors.request.use((config) => {
    if (typeof window !== 'undefined') {
        // Выполнение кода только на стороне клиента
        const token = localStorage.getItem('access_token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
    }
    return config;
});

storyClient.interceptors.response.use(
    response => {
        const { code } = response.data;
        
        if (code === 'success') {
            return response.data.value;
        }
        
        throw new Error(code);
    },
    error => {
        console.error(error);
    }
);
export default storyClient;