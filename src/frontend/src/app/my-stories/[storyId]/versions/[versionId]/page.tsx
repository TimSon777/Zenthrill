'use client';

import { useEffect, useState } from "react";
import getVersion from "./getVersion";
import { IStoryVersion } from "@/app/types";
import VersionGraph from "./components/VersionGraph";
import { useDisclosure } from "@mantine/hooks";
import AddFragmentModal from "./components/AddFragmentModal";
import { Button, Card, Center, Text, Group, Space } from "@mantine/core";
import AddBranchModal from "./components/AddBranchModal";
import { versionToString } from "@/app/helpers";
import { v4 as uuidv4 } from 'uuid';

const VersionsPage = (params: { params: { versionId: string } }) => {
    const [storyVersion, setStoryVersion] = useState<IStoryVersion | null>(null);
    const id = params.params.versionId;
    const [opened, { open, close }] = useDisclosure();
    const [branchModalOpened, { open: openBranchModal, close: closeBranchModal }] = useDisclosure();
    
    const [storyChanged, setStoryChanged] = useState('');
    
    const onStoryChanged = () => {
        setStoryChanged(uuidv4());
    };
    
    useEffect(() => {
        const fetchStory = async () => {
            const storyData = await getVersion(id);
            setStoryVersion(storyData);
        };

        fetchStory();
    }, [id, storyChanged]);

    if (!storyVersion) {
        return <div>Loading...</div>;
    }

    return (
        <>
            <Card withBorder>
                <Text>{versionToString(storyVersion.version)} {storyVersion.name}</Text>
            </Card>
            <Space h={'md'} />
            
            <Center>
                <Button onClick={open} mr={'10px'}>Добавь фрагмент</Button>
                <Button onClick={openBranchModal}>Добавь ветку</Button>
            </Center>
            <Space h="md" />
            <Card withBorder>
                <VersionGraph key={uuidv4()} storyVersion={storyVersion} onStoryChanged={onStoryChanged}/>
            </Card>

            <AddFragmentModal close={close} opened={opened} onStoryChanged={onStoryChanged} fromFragmentId={null} storyInfoVersionId={id}></AddFragmentModal>
            <AddBranchModal close={closeBranchModal} opened={branchModalOpened} storyInfoVersionId={id} fragments={storyVersion.components.flatMap(c => c.fragments)} setBranchAdded={setStoryChanged}></AddBranchModal>
        </>
    );
}

export default VersionsPage;