import { useState } from 'react';
import { Modal, Textarea, Button, Group, Space, Text, Center } from '@mantine/core';
import addStory from '../addStory';
import { useRouter } from 'next/navigation';
import ModalTitle from '@/app/components/ModalTitle';

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
        router.push(`/my-stories/${storyId}`);
    }
    
        return (
            <Modal
                opened={opened}
                onClose={close}
                title={<ModalTitle>Добавление новой истории</ModalTitle>}
            >
                <Textarea
                    label="Описание истории"
                    placeholder="Введите описание новой истории"
                    onChange={(event) => setDescription(event.currentTarget.value)}
                    value={description}
                    autosize
                    minRows={3}
                    mt="sm"
                />
                <Space h="md" />
                <Center>
                    <Button onClick={handleAddStory}>
                        Добавить
                    </Button>
                </Center>

            </Modal>
        );
    };
export default AddStoryModal;