'use client';

import { useState } from 'react';
import { Modal, Textarea, Button, Group } from '@mantine/core';
import addStory from '../addStory';

interface IProps {
    opened: boolean;
    close: () => void;
};

const AddStoryModal = ({ opened, close }: IProps) => {
    const [newStoryDescription, setNewStoryDescription] = useState('');

    const handleAddStory = () => {
        addStory(newStoryDescription);
        setNewStoryDescription('');
        close();
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
                value={newStoryDescription}
                onChange={(event) => setNewStoryDescription(event.currentTarget.value)}
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