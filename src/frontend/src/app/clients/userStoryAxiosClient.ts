'use client'

import axios from 'axios';

const userStoryAxiosClient = axios.create({
    baseURL: 'http://localhost:8082'
});

userStoryAxiosClient.interceptors.response.use(
    // @ts-ignore
    response => {
        const { code } = response.data;

        if (code === 'success') {
            return {
                data: response.data.value
            }
        }

        throw new Error(code);
    },
    error => {
        console.error(error);
    }
);

export default userStoryAxiosClient;