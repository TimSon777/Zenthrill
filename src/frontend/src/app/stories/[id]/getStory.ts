import { IStory } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function getStory(id: string) : Promise<IStory> {
    console.log(id, "aaa", storyClient);
    const response = await storyClient.get<IStory>(`/stories/${id}`);
    return response.data;
}