import { ICreateBranchRequest } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function publishVersion(versionId: string) {
    await storyClient.post<IResponse>(`story-versions/publish/${versionId}`);
}

interface IResponse {
    id: string;
}