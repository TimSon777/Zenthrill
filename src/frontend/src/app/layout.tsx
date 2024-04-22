// Import styles of packages that you've installed.
// All packages except `@mantine/hooks` require styles imports
import '@mantine/core/styles.css';
import Header from './components/Header';

import { ColorSchemeScript, Container, MantineProvider, Space } from '@mantine/core';

export const metadata = {
  title: 'My Mantine app',
  description: 'I have followed setup instructions carefully',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
      <html lang="en">
        <head>
            <ColorSchemeScript />
        </head>
        <body>
            <MantineProvider>
                <Header />
                <Space h="md" />
                <Container size="md">
                    {children}
                </Container>
                <Space h="md" />
            </MantineProvider>
        </body>
      </html>
  );
}