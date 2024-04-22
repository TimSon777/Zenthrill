import storyClient from "@/axios-clients/storyClient";

export default async function addStory(description: string) : Promise<string> {
    const response = await storyClient.post<{id: string}>("/stories", {
        description
    });
    
    console.log(response)
    return response.data.id;
}