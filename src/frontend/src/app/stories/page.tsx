import getStories from "./getStories";
import StoriesListComponent from "./components/StoriesListComponent";

const StoriesPage = async () => {
    const stories = await getStories();

    return (
        <StoriesListComponent
            stories={stories}
        />
    );
}

export default StoriesPage;