'use client';

import React, {useEffect, useState } from 'react';
import { useQuery, gql, ApolloClient, InMemoryCache } from '@apollo/client';
import { Container, TextInput, Title, List, Badge, Button, Group, MultiSelect, Center, Loader, Stack, Card, Text, Space } from '@mantine/core';
import GET_STORY_INFOS from '../graphql/queries/getStoryInfosQuery';
import { ITag } from '../types';
import getTags from '../api/getTags';
import apolloClient from '@/app/clients/apollo-client';
import Link from 'next/link';

const ITEMS_PER_PAGE = 10;

export default function StoriesPage() {
    const [descriptionFilter, setDescriptionFilter] = useState('');
    const [selectedTags, setSelectedTags] = useState<ITag[]>([]);
    
    const [tags, setTags] = useState<ITag[] | null>(null);

    const { data, loading, error, fetchMore, refetch } = useQuery(GET_STORY_INFOS, {
        variables: {
            after: null,
            first: ITEMS_PER_PAGE,
            description: '',
            tagIds: []
        },
        client: apolloClient
    });

    useEffect(() => {
        const fetchTags = async () => {
            const data = await getTags();
            setTags(data);
        }
        
        fetchTags();
    }, []);
    
    if (!tags || loading) {
        return (
            <Center h='300px'>
                <Loader />
            </Center>
        );
    };
    
    const handleNextPage = () => {
        if (data?.storyInfos.pageInfo.hasNextPage) {
            fetchMore({
                variables: {
                    tagNames: selectedTags.map(t => t.name),
                    after: data.storyInfos.pageInfo.endCursor,
                    first: ITEMS_PER_PAGE,
                    description: descriptionFilter
                },
            });
        }
    };

    const handleTagsChange = (tags1: string[]) => {
        setSelectedTags(tags.filter(t => tags1.find(tt => tt == t.id)));
        refetch({
            tagIds: tags1,
            after: null,
            first: ITEMS_PER_PAGE,
            description: descriptionFilter
        });
    };

    const handleChangeDescription = (event: React.ChangeEvent<HTMLInputElement>) => {
        setDescriptionFilter(event.target.value);
        refetch({
            tagIds: selectedTags.map(t => t.name),
            after: null,
            first: ITEMS_PER_PAGE,
            description: event.target.value
        });
    };
    
    return (
        <>
        <Container>
            <Title order={1}>Story list with filters and pagination</Title>
            <TextInput
                placeholder="Filter by description"
                value={descriptionFilter}
                onChange={handleChangeDescription}
                mt="md"
            />

            <MultiSelect
                data={tags.map((tag) => ({ value: tag.id, label: tag.name })) || []}
                value={selectedTags.map(st => st.name)}
                onChange={handleTagsChange}
                placeholder="Выберите теги"
                label="Фильтр по тегам"
                mt="md"
            />

            
                <List spacing="sm" mt="md">
                    {data?.storyInfos.nodes.map((storyInfo: any) => (
<>
    <Stack>
        <Link href={`my-stories/${storyInfo.id.value}`} style={{ textDecoration: 'none' }}>
            <Card key={storyInfo.id} shadow="sm" padding="lg">
                <Text>{storyInfo.description}</Text>
                <Space h="md" />
                <Group>
                    {storyInfo.tags.map((tag: any) => (
                        <Badge key={tag.id.value} color="blue" variant="outline">
                            {tag.name}
                        </Badge>
                    ))}
                </Group>
            </Card>
        </Link>
    </Stack>
    <Space h="md" />
</>
                    ))}
                </List>
            
        </Container>
            <Button
                onClick={handleNextPage}
                disabled={!data?.storyInfos.pageInfo.hasNextPage}
                style={{
                    position: 'fixed',
                    bottom: 20,
                    left: '50%',
                    transform: 'translateX(-50%)',
                    zIndex: 999,
                }}
            >
                Загрузить еще
            </Button>
        </>
    );
}