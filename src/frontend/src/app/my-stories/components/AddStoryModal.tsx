import {useMemo, useState } from 'react';
import { Modal, Textarea, Button, Group, Space, Text, Center, MultiSelect } from '@mantine/core';

import { useRouter } from 'next/navigation';
import ModalTitle from '@/app/components/ModalTitle';
import addStory from '@/app/api/addStory';
import { useForm } from '@mantine/form';
import { ICreateStoryRequest, ITag } from '@/app/types';

interface IProps {
    opened: boolean;
    close: () => void;
    tags: ITag[];
};

const AddStoryModal = ({ opened, close, tags }: IProps) => {
    const [description, setDescription] = useState('');
    const router = useRouter();

    const tagOptions = useMemo(() => {
        return tags.map(tag => ({ value: tag.id.toString(), label: tag.name }));
    }, [tags]);

    const form = useForm<ICreateStoryRequest>({
        initialValues: {
            description: '',
            tagIds: []
        }
    });
    
    const handleAddStory = async () => {
        const storyId = await addStory(form.values);
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
                autosize
                minRows={3}
                mt="sm"
                {...form.getInputProps('description')}
            />
            <MultiSelect
                data={tagOptions}
                label="Теги"
                placeholder="Выберите один или несколько тегов"
                searchable
                {...form.getInputProps('tagIds')}
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