import { ICreateBranchRequest } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function addBranch(request: ICreateBranchRequest) : Promise<string> {
    const response = await storyClient.post<IResponse>("branches", request);
    return response.data.id;
}

interface IResponse {
    id: string;
}