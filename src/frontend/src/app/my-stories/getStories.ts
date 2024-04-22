import { IStoryInfo } from '@/app/types';
import storyClient from '@/axios-clients/storyClient';

export default async function getStories() : Promise<IStoryInfo[]> {
    const response = await storyClient.get<IResponse>("stories");
    return response.data.storyInfos;
}

interface IResponse {
    storyInfos: IStoryInfo[];
}