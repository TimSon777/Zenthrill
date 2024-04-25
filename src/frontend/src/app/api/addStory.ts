import { ICreateStoryRequest } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function addStory(request: ICreateStoryRequest) : Promise<string> {
    const response = await storyClient.post<{id: string}>("/stories", request);
    return response.data.id;
}