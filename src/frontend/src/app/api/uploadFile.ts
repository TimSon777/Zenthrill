import storyClient from "@/axios-clients/storyClient";
import axios from 'axios';

export default async function uploadFile(storyInfoId: string, file: File) {
    console.log(file.type)
    const config = {
        headers: {
            'Content-Type': file.type // Убедитесь, что устанавливаете правильный тип содержимого
        },
        onUploadProgress: (progressEvent) => {
            console.log(`Upload Progress: ${Math.round((progressEvent.loaded / progressEvent.total) * 100)}%`);
        }
    };
    
    const response = await storyClient.get<IResponse>(`/files/upload-link?storyInfoId=${storyInfoId}&fileName=${file.name}`);
    const uploadUri = response.data.uri;
    const formData = new FormData();
    formData.append('file', file);
    await axios.put(uploadUri.replace("localstack", "localhost"), file, config);
}

interface IResponse {
    uri: string;
}