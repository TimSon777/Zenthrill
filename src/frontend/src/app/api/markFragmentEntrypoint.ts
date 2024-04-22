import storyClient from "@/axios-clients/storyClient";

export default async function markFragmentEntrypoint(fragmentId: string, storyInfoVersionId: string) {
    await storyClient.put('fragments/entrypoint', {
        id: fragmentId,
        storyInfoVersionId
    });
}