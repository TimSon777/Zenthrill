import userStoryAxiosClient from "../clients/userStoryAxiosClient";
import { IFragmentForUser } from "@/app/types";

export default async function executeStep(storyInfoVersionId: string, branchId: string) {
    const response = await userStoryAxiosClient.post<IFragmentForUser>(`story-runtime`, {
        storyInfoVersionId,
        branchId
    });
    
    return response.data;
}