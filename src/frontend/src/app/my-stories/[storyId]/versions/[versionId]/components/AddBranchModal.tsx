'use client';

import { Dispatch, SetStateAction, useMemo } from 'react';
import { Modal, Textarea, Button, Group, Select, Space, Center } from '@mantine/core';
import addBranch from '../addBranch';
import { ICreateBranchRequest, IFragment } from '@/app/types';
import { useForm } from '@mantine/form';
import ModalTitle from '@/app/components/ModalTitle';

interface IProps {
    opened: boolean;
    close: () => void;
    storyInfoVersionId: string;
    onStoryChanged: () => void;
    fragments: IFragment[];
};

const AddBranchModal = ({ opened, close, storyInfoVersionId, onStoryChanged, fragments }: IProps) => {
    const form = useForm<ICreateBranchRequest>({
        initialValues: {
            inscription: '',
            fromFragmentId: '',
            toFragmentId: '',
            storyInfoVersionId
        },
    });

    const fragmentOptions = useMemo(() => fragments.map(f => ({
        label: f.name,
        value: f.id
    })), [fragments]);

    const handleAddBranch = async () => {
        await addBranch(form.values);
        onStoryChanged();
        close();
    }
    
        return (
            <Modal
                opened={opened}
                onClose={close}
                title={<ModalTitle>Добавление новой ветки</ModalTitle>}
            >
                <form>
                    <Select
                        label="Начальный фрагмент"
                        placeholder="Выберите фрагмент"
                        data={fragmentOptions}
                        searchable
                        {...form.getInputProps('fromFragmentId')}
                    />
                    
                    <Select
                        label="Конечный фрагмент"
                        placeholder="Выберите фрагмент"
                        data={fragmentOptions}
                        searchable
                        {...form.getInputProps('toFragmentId')}
                    />

                    <Textarea
                        label="Текст ветки"
                        placeholder="Введите текст ветки"
                        autosize
                        minRows={3}
                        mt="sm"
                        {...form.getInputProps('inscription')}
                    />
                    <Space h="md" />
                    
                    <Center>
                        <Button onClick={handleAddBranch}>
                            Добавить
                        </Button>
                    </Center>
                </form>
            </Modal>
        );
};
export default AddBranchModal;