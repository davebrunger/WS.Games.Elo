import * as React from "react";
import { IGameResult } from "./iGameResult";
import { IPlayerResult } from "./iPlayerResult";
import { PositionIcon } from "./positionIcon";

export interface IGameResultGridRowProps {
    gameResult: IGameResult;
}

interface IPositionedPlayerResult {
    position: number;
    playerResult: IPlayerResult;
}

export class GameResultGridRow extends React.Component<IGameResultGridRowProps, {}> {

    private comparePlayerResults(pr1: IPositionedPlayerResult, pr2: IPositionedPlayerResult): any {
        // Sort by position ascending
        var positionDiff = pr1.position - pr2.position;
        if (positionDiff != 0) {
            return positionDiff;
        }
        // Then by rating after descending
        var ratingDiff = pr2.playerResult.ratingAfter - pr1.playerResult.ratingAfter;
        if (ratingDiff != 0) {
            return ratingDiff;
        }
        // Then by name ascending
        return pr1.playerResult.playerName < pr2.playerResult.playerName ? -1 : 1;
    }

    public render() {

        var playerResultRows = this.props.gameResult.playerResultsByPosition
            .map((prs, index) => prs.map(pr => { return { position: index + 1, playerResult: pr }; }))
            .reduce((a, prs) => a.concat(prs))
            .sort((p1, p2) => this.comparePlayerResults(p1, p2))
            .map(p => {
                return (
                    <tr key={p.playerResult.playerName}>
                        <td><PositionIcon position={p.position} /></td>
                        <td>{p.playerResult.playerName}</td>
                        <td>{p.playerResult.ratingAfter}</td>
                        <td>{p.playerResult.ratingAfter - p.playerResult.ratingBefore}</td>
                    </tr>
                );
            });

        return (
            <div>
                <h5>{this.props.gameResult.game}<br /><small>{this.props.gameResult.startTime} - {this.props.gameResult.location}</small></h5>
                <div className="row">
                    <div className="col-sm-3">
                        <img src={`/api/games/${encodeURIComponent(this.props.gameResult.game)}/thumbnail`} />
                    </div>
                    <div className="col-sm-9">
                        <table className="table table-condensed">
                            <tbody>
                                {playerResultRows}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        );
    }
}