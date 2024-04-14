import { IStoryInfo } from '@/app/types';
import storyClient from "@/axios-clients/storyClient";

export default async function addStory(description: string) : Promise<IStoryInfo> {
    const response = await storyClient.post<string>("/stories", {
        description
    });
    
    return {
        id: response.data,
        description
    };
}