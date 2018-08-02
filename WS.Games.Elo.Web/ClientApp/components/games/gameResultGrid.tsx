import * as React from "react";
import { IGameResult } from "./iGameResult";
import { GameResultGridRow } from "./gameResultGridRow";

export interface IGameResultGridProps {
    gameResults?: IGameResult[];
    noGameResultsMessage: string;
}

export class GameResultGrid extends React.Component<IGameResultGridProps, {}> {
    public render() {
        var gameResults: JSX.Element[];
        if (!this.props.gameResults) {
            gameResults = [<div key={0}>Please wait...</div>];
        } else if (this.props.gameResults.length === 0) {
            gameResults = [<div key={0}>{this.props.noGameResultsMessage}</div>];
        } else {
            gameResults = this.props.gameResults.map(g => (<GameResultGridRow key={g.startTime + g.game} gameResult={g} />));
        }

        return (
            <div>
                {gameResults}
            </div>
        );
    }
}