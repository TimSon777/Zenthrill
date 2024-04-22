'use client';

import {useEffect, useState } from 'react';
import { Container, Group, Burger, Anchor, Button, Space } from '@mantine/core';
import { MantineLogo } from '@mantinex/mantine-logo';
import Link from 'next/link';

const Header = () => {
    const defaultAccessToken = typeof window !== 'undefined'
        ? localStorage.getItem('access_token')
        : null;

    const [accessToken, setAccessToken] = useState<string | null>(null);

    useEffect(() => {
        const token = localStorage.getItem('access_token');
        setAccessToken(token);
    }, []);

    const handleLogout = () => {
        localStorage.removeItem('access_token');
        setAccessToken(null);
    };

    const alwaysVisibleItems = (
        <Button component={Link} href="/stories">
            Все истории
        </Button>
    );

    const authItems = accessToken ? (
        <>
            <Button component={Link} href="/my-stories">
                Мои истории
            </Button>
            <Button onClick={handleLogout}>
                Выход
            </Button>
        </>
    ) : (
        <>
            <Button component={Link} href="/register">
                Регистрация
            </Button>
            <Button component={Link} href="/login">
                Авторизация
            </Button>
        </>
    );

    return (
        <header>
            <Space h="md" />
            <Container size="md">
                <Group justify='space-between'>
                    <Anchor href='/'>
                        <MantineLogo size={28} /> {/* Используйте свой логотип */}
                    </Anchor>
                    <Group justify='flex-end'>
                        {alwaysVisibleItems}
                        {authItems}
                    </Group>
                </Group>
            </Container>
            <Space h="md" />
        </header>
    );
};

export default Header;