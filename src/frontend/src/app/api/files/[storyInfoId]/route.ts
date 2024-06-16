import { S3 } from 'aws-sdk';
import type { NextApiRequest } from 'next';
import { NextResponse } from 'next/server';

export const config = {
    api: {
        bodyParser: false, // Отключить автоматический парсер Next.js
    }
};

export async function GET(req: NextApiRequest) {
    const pathname = req.url!;

    // Разделяем путь на части
    const pathParts = pathname.split('/');

    console.log(pathname, "AAAA")
    // Параметр storyInfoId должен быть последним в массиве pathParts
    const storyInfoId = pathParts[pathParts.length - 1];

    const s3 = new S3({
        endpoint: 'http://localhost:4566', // Укажите ваш публичный S3 endpoint
        s3ForcePathStyle: true, // Необходимо для локальных S3
        credentials: { // Эти данные будут использоваться для локального тестирования 
            accessKeyId: 'test',    // Замените на ваши реальные credentials при деплое 
            secretAccessKey: 'test', // Замените на ваши реальные credentials при деплое
        },
    });

    const params = {
        Bucket: 'zenthrill-files',
        Prefix: `${storyInfoId}/`,
    };

        const s3Response = await s3.listObjectsV2(params).promise();

        // Отфильтровываем только ключи объектов
        const files = s3Response.Contents?.map((content) => content.Key) || [];

        // Отправляем ответ клиенту
        return new NextResponse(JSON.stringify({ files: files.map(x => `http://localhost:4566/zenthrill-files/${x}`) }), { status: 200 });
}
