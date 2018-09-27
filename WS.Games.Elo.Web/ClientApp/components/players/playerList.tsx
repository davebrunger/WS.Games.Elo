import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { IPlayer } from './iPlayer';
import { HttpService } from '../../services/httpService';
import { PlayerGrid } from './playerGrid';

export interface IPlayerListState {
    players?: IPlayer[];
}

export interface IPlayerListProps extends RouteComponentProps<{}> {
    httpService : HttpService;
}

export class PlayerList extends React.Component<IPlayerListProps, IPlayerListState> {

    constructor(props: any) {
        super(props);
        this.state = {};
    }

    private getPlayers() {
        this.props.httpService.get<IPlayer[]>("/api/players", data => {
            this.setState({ players: data });
        })
    }

    public componentDidMount() {
        this.getPlayers();
    }

    public render() {
        return (
            <div>
                <h1>All Players</h1>
                <PlayerGrid players={this.state.players} noPlayersMessage={"No players have been registered"} />
            </div>
        );
    }
}
