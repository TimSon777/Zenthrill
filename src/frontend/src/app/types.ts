export interface IStoryInfo {
    id: string;
    description: string;
}

export interface IStoryVersionInfo {
    id: string;
    name: string;
    version: IVersion;
}
export interface IStory {
    tags: ITag[];
    storyInfo: IStoryInfo;
    versions: IStoryVersionInfo[];
}

export interface IFragment {
    id: string;
    isEntrypoint: boolean;
    body: string;
    name: string;
}

export interface IBranch {
    id: string;
    inscription: string;
    fromFragmentId: string;
    toFragmentId: string;
}

export interface IComponent {
    fragments: IFragment[];
    branches: IBranch[];
}

export interface IStoryVersion {
    storyInfo: IStoryInfo;
    components: IComponent[];
    name: string;
    version: IVersion;
    id: string;
}

export interface IVersion {
    major: number;
    minor: number;
    suffix: string;
}

export interface ICreateVersionRequest {
    storyInfoId: string;
    baseStoryInfoVersionId: string | null;
    name: string;
    version: IVersion;
}

export interface ICreateFragmentRequest {
    name: string;
    body: string;
    storyInfoVersionId: string;
    fromFragmentId: string | null;
}

export interface ICreateBranchRequest {
    inscription: string;
    fromFragmentId: string;
    toFragmentId: string;
    storyInfoVersionId: string;
}

export interface IUpdateBranchRequest {
    inscription: string;
    storyInfoVersionId: string;
    branchId: string;
}

export interface IUpdateFragmentRequest {
    storyInfoVersionId: string;
    body: string;
    name: string;
    fragmentId: string;
}

export interface ICreateStoryRequest {
    description: string;
    tagIds: string[];
}

export interface ITag {
    id: string;
    name: string;
}

export interface IFragmentForUser {
    fragment?: {
        name: string;
        body: string;
    }
    outputBranches?: [
        {
            id: string;
            inscription: string;
            fromFragmentId: string;
            toFragmentId: string;
        }
    ]
}