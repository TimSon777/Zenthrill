'use client';

import { useState } from 'react';
import { Modal, Textarea, Button, Group } from '@mantine/core';
import addStory from '../addStory';
import { useRouter } from 'next/navigation';

interface IProps {
    opened: boolean;
    close: () => void;
};

const AddStoryModal = ({ opened, close }: IProps) => {
    const [description, setDescription] = useState('');
    const router = useRouter();

    const handleAddStory = async () => {
        const storyId = await addStory(description);
        setDescription('');
        close();
        router.push(`/stories/${storyId}`);
    };

    return (
        <Modal
            opened={opened}
            onClose={close}
            title="Добавление новой истории"
        >
            <Textarea
                label="Описание истории"
                placeholder="Введите описание новой истории"
                value={description}
                onChange={(event) => setDescription(event.currentTarget.value)}
                autosize
                minRows={3}
                mt="sm"
            />
            <Button onClick={handleAddStory}>
                Добавить
            </Button>
        </Modal>
    );
};

export default AddStoryModal;