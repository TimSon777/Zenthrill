import { Image, Group, AspectRatio, Card, Text, Stack } from '@mantine/core';
import Link from 'next/link';
import { showNotification } from '@mantine/notifications';

interface IProps {
    uri: string;
    name: string;
}

const fileExtensions : Record<string, string> = {
    // Изображения
    'jpg': 'image',
    'jpeg': 'image',
    'png': 'image',
    'gif': 'image',
    'bmp': 'image',
    'webp': 'image',
    'svg': 'image',

    // Видео
    'mp4': 'video',
    'mov': 'video',
    'wmv': 'video',
    'avi': 'video',
    'avchd': 'video',
    'flv': 'video',
    'mkv': 'video',
    'webm': 'video',

    // Аудио
    'mp3': 'audio',
    'wav': 'audio',
    'm4a': 'audio',
    'flac': 'audio',
    'ogg': 'audio',
    'aac': 'audio',
};

function FilePreview ({ uri, name }: IProps) {
  
    const extension = uri.split('.').pop()!;
    console.log(extension)
    const type = Object.keys(fileExtensions).some(s => s === extension) ? fileExtensions[extension] : null;

    const handleCopyLink = (event: React.MouseEvent<HTMLDivElement>, uri: string) => {
        event.preventDefault();
        navigator.clipboard.writeText(uri)
            .then(() => {
                showNotification({
                    title: 'Успех',
                    message: 'Ссылка скопирована в буфер обмена!',
                    color: 'green',
                });
            })
            .catch((err) => {
                showNotification({
                    title: 'Ошибка',
                    message: `Ошибка при копировании ссылки: ${err}`,
                    color: 'red',
                });
            });
    };
    
    const renderPreview = () => {
        switch (type) {
            case 'image':
                return (
                    <>
                        <Group>
                            <Card withBorder>
                                <Image src={uri} alt="Preview" h={'300px'} w={'300px'}/>
                            </Card>
                            <Stack>
                                <Text>
                                    Тип мультимедиа: изображение
                                </Text>
                                <Text onClick={(event) => handleCopyLink(event, uri)}>
                                    {name}
                                </Text>
                            </Stack>
                        </Group>
                    </>
                );
            case 'video':
                return (
                    <>
                        <Group>
                            <Card withBorder>
                                <video height={'300px'} width={'300px'} controls src={uri}/>
                            </Card>

                            <Stack>
                                <Text>
                                Тип мультимедиа: видео
                                </Text>
                                <Text onClick={(event) => handleCopyLink(event, uri)}>
                                    {name}
                                </Text>
                            </Stack>
                        </Group>
                    </>

                );
            case 'audio':
                return <audio controls src={uri} />;
            default:
                return <p>Превью файла не доступно</p>;
        }
    };

    return (
            <Card withBorder mb={'10px'}>
                {renderPreview()}
            </Card>
    );
}

export default FilePreview;