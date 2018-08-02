import { IPlayerResult } from "./iPlayerResult";

export interface IGameResult {
    game: string;
    location: string;
    startTime: string;
    playerResultsByPosition: IPlayerResult[][];
}
