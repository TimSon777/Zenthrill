import storyClient from "@/axios-clients/storyClient";
import { ITag } from "../types"

export default async function getTags() {
    const response = await storyClient.get<IResponse>("tags");
    return response.data.tags;
}

interface IResponse {
    tags: ITag[];
}