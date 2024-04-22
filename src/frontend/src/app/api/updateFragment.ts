import { IUpdateFragmentRequest } from "@/app/types";
import storyClient from "@/axios-clients/storyClient";

export default async function updateFragment(request: IUpdateFragmentRequest) {
    await storyClient.put('fragments', request);
}