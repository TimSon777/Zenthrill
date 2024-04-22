import { ICreateVersionRequest } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function addVersion(request: ICreateVersionRequest) : Promise<string> {
    const response = await storyClient.post<IResponse>("story-versions", request);
    
    return response.data.id;
}

interface IResponse {
    id: string;
}