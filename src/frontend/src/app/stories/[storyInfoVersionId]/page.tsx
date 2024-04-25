'use client';

import executeStep from "@/app/api/executeStep";
import getFragment from "@/app/api/getFragment";
import startStory from "@/app/api/startStory";
import { IFragmentForUser } from "@/app/types";
import { Button, Card, Center, Loader, Stack, Text, Title } from "@mantine/core";
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
                    <Text size="sm">{fragment.fragment.body}</Text>
                </Card>
            )}

            {fragment.outputBranches && fragment.outputBranches.map((branch, index) => (
                <Button onClick={() => handleExecuteStep(branch.id)}>
                    <Card key={index} shadow="sm" padding="lg">
                        <Text>{branch.inscription}</Text>
                    </Card>
                </Button>
            ))}
        </Stack>
    )
};

export default StoryPage;