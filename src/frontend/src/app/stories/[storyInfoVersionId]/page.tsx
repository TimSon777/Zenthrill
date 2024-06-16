'use client';

import executeStep from "@/app/api/executeStep";
import getFragment from "@/app/api/getFragment";
import startStory from "@/app/api/startStory";
import { IFragmentForUser } from "@/app/types";
import { Button, Card, Center, Divider, Group, Loader, Space, Stack, Text, Title, UnstyledButton } from "@mantine/core";
import Link from "next/link";
import { useState, useEffect } from "react";

const StoryPage = (params: { params: { storyInfoVersionId: string } }) => {
    const [fragment, setFragment] = useState<IFragmentForUser | null>(null);
    
    useEffect(() => {
        const fetchFragment = async () => {
          const fragment = await getFragment(params.params.storyInfoVersionId);
          setFragment(fragment);
        };
        fetchFragment();
    }, []);
    
    if (!fragment) {
        return (
          <Center h={'200px'}>
              <Loader />
          </Center>  
        );
    }
    
    const handleStart = async () => {
        const fragment = await startStory(params.params.storyInfoVersionId);
        setFragment(fragment);
    }
    
    const handleExecuteStep = async (branchId: string) => {
        const fragment = await executeStep(params.params.storyInfoVersionId, branchId);
        setFragment(fragment);
    };
    
    if (!fragment.fragment || !fragment.outputBranches) {
        return (
            <Center>
                <Button onClick={handleStart}>
                    Начать историю
                </Button>
            </Center>
        )
    }
    
    return (
        <Stack>
            {fragment.fragment && (
                <Card shadow="sm" padding="lg">
                    <Title order={3}>{fragment.fragment.name}</Title>
                    <Divider />
                    <Space h='md' />
                    <div dangerouslySetInnerHTML={{__html: fragment.fragment.body}}/>
                </Card>
            )}

            {fragment.outputBranches && fragment.outputBranches.map((branch, index) => (
                <UnstyledButton onClick={() => handleExecuteStep(branch.id)}>
                    <Card key={index} shadow="sm" padding="lg">
                        <Text>{branch.inscription}</Text>
                    </Card>
                </UnstyledButton>
            ))}

            <Space h='md' />
            
            {!fragment.outputBranches.length && (
                <>
                    <Center>
                        <Title>Вы закончили прохождение истории!</Title>
                        <Group>
                            <Link href={'/'}>Вернуться на главную</Link>
                            <Button onClick={handleStart}>Начать историю заново</Button>
                        </Group>
                    </Center>
                </>
            )}
        </Stack>
    )
};

export default StoryPage;