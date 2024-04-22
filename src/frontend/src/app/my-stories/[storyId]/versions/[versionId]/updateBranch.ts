import storyClient from "@/axios-clients/storyClient";

export default async function updateBranch(request: IUpdateBranchRequest) {
    await storyClient.put("branches", request);
}