'use client';

import { useEffect, useState } from "react";
import getStory from "./getStory";
import { IStory } from "@/app/types";
import AddVersionModal from "./components/AddVersionModal";
import { useDisclosure } from "@mantine/hooks";
import {Badge, Button, Card, Center, Divider, Group, Space, Stack, Text } from "@mantine/core";
import Link from "next/link";
import { versionToString } from "@/app/helpers";

const StoryPage = (params: { params: { storyId: string } }) => {
    const [opened, { open, close }] = useDisclosure();
    const [story, setStory] = useState<IStory | null>(null);
    const id = params.params.storyId;
    
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

    function getVersionsBody() {
        if (!story?.versions.length) {
            return (
                <Center>
                    Еще нет созданных версий
                </Center>
            );
        }
        
        return (
            <Card withBorder>
                <Text fw={700}>Версии</Text>
                <Divider mb={'10px'}/>
                {story.versions.map((version) => (
                    <Link href={`/my-stories/${story.storyInfo.id}/versions/${version.id}`} style={{ textDecoration: 'none' }}>
                        <Card key={version.id} shadow="sm" padding="lg">
                            <Text>{versionToString(version.version)}: {version.name}</Text>
                        </Card>
                    </Link>
                ))}
            </Card>
        )
    };
    
    function getTagsBody() {
        return (
            <Group>
                {story!.tags.map((tag) => (
                    <Badge key={tag.id} variant="outline">
                        {tag.name}
                    </Badge>
                ))}
            </Group>
        )
    }
    
    return (
        <>
            <div>
                <Card withBorder>
                    <Text fw={700}>Описание истории</Text>
                    <Divider mb={'10px'}/>
                    <Text>{story.storyInfo.description}</Text>
                    <Space h={'md'} />
                    <Text fw={700}>Теги</Text>
                    <Divider mb={'10px'}/>
                    {getTagsBody()}
                </Card>
                <Space h={'md'} />
                <Stack>
                    {getVersionsBody()}
                </Stack>
                
                <AddVersionModal opened={opened} close={close} storyId={id} versions={story.versions} />
            </div>

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
                Добавить версию
            </Button>
        </>
    );
}

export default StoryPage;