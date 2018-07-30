import * as React from "react";
import { IPlayer } from "./iPlayer";

export interface IPlayerGridRowProps {
    player : IPlayer;
}

export class PlayerGridRow extends React.Component<IPlayerGridRowProps, {}> {
    public render() {
        return (
            <tr><td>{this.props.player.name}</td><td>{this.props.player.rating}</td></tr>
        );
    }
}