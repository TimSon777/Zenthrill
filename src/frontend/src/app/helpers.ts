import { IVersion } from "./types";

export function versionToString(version: IVersion) {
    return `${version.major}.${version.minor}.${version.suffix}`;
}