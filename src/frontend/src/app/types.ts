export interface IStoryInfo {
    id: string;
    description: string;
}

export interface IStory {
    storyInfo: {
        id: string;
        description: string;
    }
    versions: [
        {
        id: string;
        name: string;
        }
    ]
}
