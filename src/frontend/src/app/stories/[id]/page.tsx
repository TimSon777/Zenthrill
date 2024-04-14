'use client';

import { useEffect, useState } from "react";
import getStory from "./getStory";
import { IStory } from "@/app/types";

const StoryPage = (params: { params: { id: string } }) => {
    const [story, setStory] = useState<IStory | null>(null);
    const id = params.params.id;
    
    useEffect(() => {
        const fetchStory = async () => {
            const storyData = await getStory(id);
            setStory(storyData);
        };

        fetchStory();
    }, [id]);

    if (!story) {
        return <div>Loading...</div>;
    }

    return (
        <div>
            <h1>Story details</h1>
            <div>
                ID: {story.storyInfo.id}
                <p>Description: {story.storyInfo.description}</p>
                <div>
                    Versions:
                    {story.versions.map(v => (
                        <div key={v.id}>{v.id}</div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default StoryPage;