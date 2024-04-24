import userStoryAxiosClient from "../clients/userStoryAxiosClient";
import { IFragmentForUser } from "@/app/types";

export default async function startStory(storyInfoVersionId: string) {
    console.log("AAA", storyInfoVersionId)
    const response = await userStoryAxiosClient.post<IFragmentForUser>(`story-runtime`, {
        storyInfoVersionId: storyInfoVersionId,
        branchId: null
    });
    
    return response.data;
}