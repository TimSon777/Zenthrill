import { IStoryVersion } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function getVersion(versionId: string) : Promise<IStoryVersion> {
    const response = await storyClient.get<IStoryVersion>(`story-versions/${versionId}`);
    return response.data;
}