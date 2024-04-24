import { gql } from '@apollo/client';

const GET_STORY_INFOS = gql`
    query GetStoryInfos($after: String, $description: String, $first: Int, $tagIds: [UUID!]!) {
        storyInfos(
            tagIds: $tagIds
            after: $after,
            first: $first,
            where: {
                description: {
                    startsWith: $description
                }
            }) {
            nodes {
                id {
                    value
                }
                description
                tags {
                    id {
                        value
                    }
                    name
                }
                versions {
                    id {
                        value
                    }
                    name
                }
            }
            pageInfo {
                endCursor
                hasNextPage
            }
        }
    }
`;

export default GET_STORY_INFOS;