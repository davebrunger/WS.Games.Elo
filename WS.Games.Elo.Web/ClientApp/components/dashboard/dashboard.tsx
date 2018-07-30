import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { HttpService } from '../../services/httpService';
import { IPlayer } from '../players/iPlayer';
import { PlayerGrid } from '../players/playerGrid';
import { Arrays } from '../../services/arrays';
import { Strings } from '../../services/strings';

export interface IDashboardState {
    players?: IPlayer[];
}

export class Dashboard extends React.Component<RouteComponentProps<{}>, IDashboardState> {

    private readonly topPlayersMinimumNumberOfGames = 1;
    private readonly topPlayersMinimumNumberOfPlayers = 5;

    constructor(props: any) {
        super(props);
        this.state = {};
    }

    private getPlayers() {
        HttpService.get<IPlayer[]>(`/api/players?minimumNumberOfGames=${this.topPlayersMinimumNumberOfGames}`, data => {
            var topPlayers = Arrays.orderByDescending(data, d => d.rating).splice(0, this.topPlayersMinimumNumberOfPlayers);
            if (topPlayers.length == this.topPlayersMinimumNumberOfPlayers) {
                var lastPlayerRating = topPlayers[4].rating;
                topPlayers = Arrays.orderByDescending(data.filter(p => p.rating >= lastPlayerRating), p => p.rating);
            }
            this.setState({ players: topPlayers });
        })
    }

    public componentDidMount() {
        this.getPlayers();
    }

    public render() {
        var numberOfTopPlayers = (this.state.players || []).length || this.topPlayersMinimumNumberOfPlayers;
        var numberOfTopPlayersHeading = `Top ${numberOfTopPlayers}  ${Strings.pluralize(numberOfTopPlayers, "Player")}`;
        var numberOfTopPlayersSubheading = `That have played at least ${this.topPlayersMinimumNumberOfGames} ${Strings.pluralize(this.topPlayersMinimumNumberOfGames, "game")}`;
        var noTopPlayersMessage = `No players have played at least ${this.topPlayersMinimumNumberOfGames} ${Strings.pluralize(this.topPlayersMinimumNumberOfGames, "game")}`;
        return (
            <div>
                <h1>Welcome to the Dashboard</h1>
                <div className="row">
                    <div className="col-sm-3">
                        <h4>{numberOfTopPlayersHeading}<br /><small>{numberOfTopPlayersSubheading}</small></h4>
                        <PlayerGrid players={this.state.players} noPlayersMessage={noTopPlayersMessage} />
                    </div>
                </div>
            </div>
        );
    }
}
