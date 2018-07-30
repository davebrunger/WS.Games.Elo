import * as React from "react";
import { IPlayer } from "./iPlayer";
import { PlayerGridRow } from "./playerGridRow";

export interface IPlayerGridProps {
    players?: IPlayer[];
    noPlayersMessage: string;
}

export class PlayerGrid extends React.Component<IPlayerGridProps, {}> {
    public render() {
        var players: JSX.Element[];
        if (!this.props.players) {
            players = [<tr key={0}><td colSpan={2}>Please wait...</td></tr>];
        } else if (this.props.players.length === 0) {
            players = [<tr key={0}><td colSpan={2}>{this.props.noPlayersMessage}</td></tr>];
        } else {
            players = this.props.players.map(p => (<PlayerGridRow key={p.name} player={p} />));
        }

        return (
            <table className="table table-condensed">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Rating</th>
                    </tr>
                </thead>
                <tbody>
                    {players}
                </tbody>
            </table>
        );
    }
}