import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { IPlayer } from './iPlayer';
import { HttpService } from '../../services/httpService';
import { PlayerGrid } from './playerGrid';

export interface IPlayerListState {
    players?: IPlayer[];
}

export class PlayerList extends React.Component<RouteComponentProps<{}>, IPlayerListState> {

    constructor(props: any) {
        super(props);
        this.state = {};
    }

    private getPlayers() {
        HttpService.get<IPlayer[]>("/api/players", data => {
            this.setState({ players: data });
        })
    }

    public componentDidMount() {
        this.getPlayers();
    }

    public render() {
        return (
            <div>
                <h1>Welcome to the List of All Players</h1>
                <PlayerGrid players={this.state.players} noPlayersMessage={"No players have been registered"} />
            </div>
        );
    }
}
