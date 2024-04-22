'use client';

import getStories from "./getStories";
import { useDisclosure } from "@mantine/hooks";
import {Button, Card, Loader, Stack, Text } from "@mantine/core";
import { useEffect, useState } from "react";
import { IStoryInfo } from '@/app/types';
import AddStoryModal from "./components/AddStoryModal";
import Link from "next/link";

const StoriesPage = () => {
    const [stories, setStories] = useState<IStoryInfo[] | null>(null);
    const [opened, { open, close }] = useDisclosure();

    useEffect(() => {
        const fetchStories = async () => {
            const data = await getStories();
            setStories(data);
        };

        fetchStories();
    }, []);

    if (!stories) {
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

            <AddStoryModal opened={opened} close={close} />
        </>
    );
}

export default StoriesPage;