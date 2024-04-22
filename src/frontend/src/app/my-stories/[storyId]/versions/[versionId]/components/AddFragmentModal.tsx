'use client';

import {Dispatch, SetStateAction, useState } from 'react';
import { Modal, Textarea, Button, Group, Input, TextInput, Space, Center } from '@mantine/core';
import addFragment from '../addFragment';
import { useRouter } from 'next/navigation';
import { ICreateFragmentRequest, IFragment } from '@/app/types';
import ModalTitle from '@/app/components/ModalTitle';

interface IProps {
    opened: boolean;
    close: () => void;
    fromFragmentId: string | null;
    storyInfoVersionId: string;
    onStoryChanged: () => void;
};

const AddFragmentModal = ({ opened, close, fromFragmentId, storyInfoVersionId, onStoryChanged }: IProps) => {
    const [body, setBody] = useState('');
    const [name, setName] = useState('');
    
    const handleAddFragment = async () => {
        const request : ICreateFragmentRequest = {
            name,
            body,
            fromFragmentId,
            storyInfoVersionId
        };

        const fragmentId = await addFragment(request);

        console.log(fragmentId)
        onStoryChanged();
        setBody('');
        setName('');
        close();
    }
    
        return (
            <Modal
                opened={opened}
                onClose={close}
                title={<ModalTitle>Добавление нового фрагмента</ModalTitle>}
            >
                <Textarea
                    label="Описание истории"
                    placeholder="Введите описание новой истории"
                    onChange={(event) => setBody(event.currentTarget.value)}
                    value={body}
                    autosize
                    minRows={3}
                    mt="sm"
                />
                <TextInput
                    label="Название истории"
                    placeholder="Введите описание новой истории"
                    onChange={(event) => setName(event.currentTarget.value)}
                    value={name}
                    mt="sm"
                />
                <Space h="md" />
                <Center>
                    <Button onClick={handleAddFragment}>
                        Добавить
                    </Button>
                </Center>
            </Modal>
        );
    };
export default AddFragmentModal;