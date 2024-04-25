import userStoryAxiosClient from '@/app/clients/userStoryAxiosClient';
import { IFragmentForUser } from '@/app/types';

export default async function getFragment(storyInfoVersionId: string) {
    const response = await userStoryAxiosClient.get<IFragmentForUser>(`stories/${storyInfoVersionId}`);
    return response.data;
}