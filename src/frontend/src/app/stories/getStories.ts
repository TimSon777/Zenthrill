import { IStoryInfo } from '@/app/types';

export default function getStories() : Promise<IStoryInfo[]> {
    const stories : IStoryInfo[] = [
        {
            id: "test",
            description: "test"
        }
    ];

    return Promise.resolve(stories);
}