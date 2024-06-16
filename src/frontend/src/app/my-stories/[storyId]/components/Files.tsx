'use client';

import { useState, useEffect } from 'react';
import { Group, Text, FileInput, Stack, Card, Center, Loader, Space, Divider } from '@mantine/core';
import axios from 'axios';
import uploadFile from '@/app/api/uploadFile';
import FilePreview from './FilePreview';

interface IProps {
    storyInfoId: string;
}

const Files = ({ storyInfoId }: IProps) => {
    const [files, setFiles] = useState<string[] | null>();


    const fetchFiles = async () => {
        const response = await axios.get(`/api/files/${storyInfoId}`);
        setFiles(response.data.files);
    };

    useEffect(() => {
        if (storyInfoId) {
            fetchFiles();
        }
    }, [storyInfoId]);

    if (!files) {
        return (
            <Center h='300px'>
                <Loader />
            </Center>
        )
    }
    
    async function handleFileUpload(file: File) {
        await uploadFile(storyInfoId, file);
        fetchFiles();
    }

    return (
        <Stack>
            <Card withBorder>
                <Text fw={700}>Мультимедиа</Text>
                <Divider />
                
                <Space h={'md'}/>
                {files.map((file) => (
                    <FilePreview uri={file} name={file.split('/').pop()!.split('.')[0]}/>
                ))}

                <Space h={'md'}/>
                <Group grow>
                    <FileInput
                        placeholder="Загрузить файл"
                        onChange={(file) => {
                            if (file) {
                                handleFileUpload(file);
                            }
                        }}
                    />
                </Group>
            </Card>
            
        </Stack>
    );
};

export default Files;