import { ApolloClient, InMemoryCache } from "@apollo/client";

const apolloClient = new ApolloClient({
    uri: 'http://localhost:8081/graphql',
    cache: new InMemoryCache({
        typePolicies: {
            Query: {
                fields: {
                    storyInfos: {
                        keyArgs: false,
                        merge(existing = { nodes: [], pageInfo: {} }, incoming, { readField }) {
                            if (!incoming.nodes) {
                                return existing;
                            }
                            const mergedNodes = existing.nodes ? existing.nodes.slice(0) : [];
                            const endCursor = incoming.pageInfo.endCursor;

                            incoming.nodes.forEach((node: any) => {
                                const idValue = readField('id', node);
                                if (mergedNodes.some((mergedNode: any) => readField('id', mergedNode) === idValue)) {
                                    return;
                                }
                                mergedNodes.push(node);
                            });

                            return {
                                ...incoming, // Узлы от последнего вызова fetchMore
                                nodes: mergedNodes,
                                pageInfo: {
                                    ...existing.pageInfo,
                                    ...incoming.pageInfo,
                                    endCursor,
                                }
                            };
                        }
                    }
                }
            }
        }
    })
});

export default apolloClient;