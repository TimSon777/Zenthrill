'use client';

import React from 'react';
import { Modal, Button, TextInput, NumberInput, Select, Space, Center } from '@mantine/core';
import { useForm } from '@mantine/form';
import addVersion from '../addVersion';
import { IStoryVersionInfo, ICreateVersionRequest } from '@/app/types';
import { versionToString } from '@/app/helpers';
import ModalTitle from '@/app/components/ModalTitle';

interface IProps {
    versions: IStoryVersionInfo[];
    storyId: string;
    opened: boolean;
    close: () => void;
    onVersionAdded: () => void;
}

const RequestModal = ({ versions, storyId, opened, close, onVersionAdded }: IProps) => {

    const versionOptions = versions.map((version) => ({
        label: `${versionToString(version.version)} ${version.name}`,
        value: version.id,
    }));

    const form = useForm<ICreateVersionRequest>({
        initialValues: {
            storyInfoId: storyId,
            baseStoryInfoVersionId: null,
            name: '',
            version: {
                major: 1,
                minor: 0,
                suffix: '0',
            },
        },
    });

    const handleAddVersion = async () => {
        await addVersion(form.values);
        onVersionAdded();
        close();
    };

    const handleSelectVersion = (value: string | null) => {
        if (value === null) {
            form.setFieldValue('baseStoryInfoVersionId', '');
            form.setFieldValue('version.major', 0);
            form.setFieldValue('version.minor', 0);
            form.setFieldValue('version.suffix', '');
        } else {
            form.setFieldValue('baseStoryInfoVersionId', value);
            const selectedVersion = versions.find((version) => version.id === value);
            if (selectedVersion) {
                form.setFieldValue('version.major', selectedVersion.version.major);
                form.setFieldValue('version.minor', selectedVersion.version.minor);
                form.setFieldValue('version.suffix', selectedVersion.version.suffix);
            }
        }
    };

    return (
        <Modal opened={opened} onClose={close} title={<ModalTitle>Добавление новой версии</ModalTitle>}>
            <form>
                <Select
                    label="Базовая версия"
                    placeholder="Выберите версию либо оставьте поле пустым"
                    data={versionOptions}
                    searchable
                    value={form.values.baseStoryInfoVersionId}
                    onChange={handleSelectVersion}
                />

                <TextInput
                    required
                    label="Название"
                    placeholder="Введите название версии"
                    {...form.getInputProps('name')}
                />

                <NumberInput
                    required
                    label="Мажорная версия"
                    placeholder="Введите мажорную версию"
                    {...form.getInputProps('version.major')}
                />

                <NumberInput
                    required
                    label="Минорная версия"
                    placeholder="Введите минорную версию"
                    {...form.getInputProps('version.minor')}
                />

                <TextInput
                    required
                    label="Суффикс версии"
                    placeholder="Введите суффикс версии"
                    {...form.getInputProps('version.suffix')}
                />

                <Space h="md" />
                <Center>
                    <Button onClick={handleAddVersion}>
                        Добавить
                    </Button>
                </Center>
            </form>
        </Modal>
    );
};

export default RequestModal;