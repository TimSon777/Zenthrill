'use client';

import {Box, Button, Center, Container, SimpleGrid, Stack, Text, Textarea } from "@mantine/core";
import { useRouter } from 'next/navigation';

export default function Home() {
    const router = useRouter();
    
  return (
      <Container size='md' h={'500px'}>
          <Center h={'100%'}>
              <Stack>
                  <Text style={{ fontSize: '4vw', fontWeight: 700 }}>
                      Интерактивный контент
                  </Text>
                  <Text style={{ fontSize: '2vw' }}>
                      это контент, основу которого составляет активное взаимодействие с пользователем
                  </Text>

                  <SimpleGrid cols={2} spacing="lg"> {/* Две колонки */}
                      <Box>
                          <Text>
                              Если ты читатель
                          </Text>
                          <Button variant="outline" onClick={() => router.push('/stories')}>
                              начни отсюда
                          </Button>
                      </Box>
   
                      <Box>
                          <Text>
                              Если ты автор
                          </Text>
                          <Button variant="outline" onClick={() => router.push('/my-stories')}>
                              создай свою первую историю
                          </Button>  
                      </Box>

                  </SimpleGrid>
              </Stack>
          </Center>
      </Container>
  );
}
