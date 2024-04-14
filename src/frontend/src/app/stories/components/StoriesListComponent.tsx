'use client';

import { IStoryInfo } from '@/app/types';
import { List, ListItem, Button } from '@mantine/core';
import AddStoryModal from "../components/AddStoryModal";
import { useDisclosure } from '@mantine/hooks';

interface IProps {
    stories: IStoryInfo[],
}

const StoriesListComponent = ({ stories }: IProps) => {
    const [opened, { open, close }] = useDisclosure();

    return (
        <>
            <List>
                {stories.map((story) => (
                        <ListItem key={story.id}>{story.description}</ListItem>
                    ))}
            </List>

            <Button
                onClick={open}
            >
                Добавить историю
            </Button>

            <AddStoryModal 
               opened={opened}
               close={close}
            />
        </>
    );
}

export default StoriesListComponent;