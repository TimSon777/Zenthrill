import { ICreateFragmentRequest } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function addFragment(request: ICreateFragmentRequest) : Promise<string> {
    const response = await storyClient.post<IResponse>("fragments", request);
    console.log(response)
    return response.data.id;
}

interface IResponse {
    id: string;
}