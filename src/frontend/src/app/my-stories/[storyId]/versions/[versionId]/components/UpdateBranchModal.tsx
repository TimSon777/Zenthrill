'use client';

import { Dispatch, SetStateAction, useMemo } from 'react';
import { Modal, Textarea, Button, Group, Select, Space, Center } from '@mantine/core';
import updateBranch from '../updateBranch';
import { IUpdateBranchRequest, IBranch } from '@/app/types';
import { useForm } from '@mantine/form';
import ModalTitle from '@/app/components/ModalTitle';

interface IProps {
    opened: boolean;
    close: () => void;
    storyInfoVersionId: string;
    onStoryChanged: () => void;
    branch: IBranch;
};

const UpdateBranchModal = ({ opened, close, storyInfoVersionId, onStoryChanged, branch }: IProps) => {
    const form = useForm<IUpdateBranchRequest>({
        initialValues: {
            inscription: branch.inscription,
            branchId: branch.id,
            storyInfoVersionId
        },
    });

    const handleAddBranch = async () => {
        await updateBranch(form.values);
        onStoryChanged();
        close();
    }
    
        return (
            <Modal
                opened={opened}
                onClose={close}
                title={<ModalTitle>Обновление ветки</ModalTitle>}
            >
                <form>

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
export default UpdateBranchModal;