'use client';

import getStories from "./getStories";
import { useDisclosure } from "@mantine/hooks";
import {Button, Card, Loader, Stack, Text } from "@mantine/core";
import { useEffect, useState } from "react";
import { IStoryInfo, ITag } from '@/app/types';
import AddStoryModal from "./components/AddStoryModal";
import Link from "next/link";
import getTags from "@/app/api/getTags";

const StoriesPage = () => {
    const [stories, setStories] = useState<IStoryInfo[] | null>(null);
    const [tags, setTags] = useState<ITag[] | null>(null);
    
    const [opened, { open, close }] = useDisclosure();

    useEffect(() => {
        const fetchStories = async () => {
            const data = await getStories();
            setStories(data);
        };

        const fetchTags = async () => {
            const data = await getTags();
            setTags(data);
        }
        
        fetchStories();
        fetchTags();
    }, []);

    if (!stories || !tags) {
        return <Loader />
    }

    return (
        <>
            <Button
                onClick={open}
                style={{
                    position: 'fixed',
                    bottom: 20,
                    left: '50%',
                    transform: 'translateX(-50%)',
                    zIndex: 999,
                }}
            >
                Добавить историю
            </Button>

            <Stack>
                {stories.map((story) => (
                    <Link href={`my-stories/${story.id}`} style={{ textDecoration: 'none' }}>
                        <Card key={story.id} shadow="sm" padding="lg">
                            <Text>{story.description}</Text>
                        </Card>
                    </Link>

                ))}
            </Stack>

            <AddStoryModal
                opened={opened}
                close={close}
                tags={tags} />
        </>
    );
}

export default StoriesPage;