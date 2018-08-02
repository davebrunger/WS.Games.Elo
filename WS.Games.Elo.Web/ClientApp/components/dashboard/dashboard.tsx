import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { HttpService } from '../../services/httpService';
import { IPlayer } from '../players/iPlayer';
import { PlayerGrid } from '../players/playerGrid';
import { Arrays } from '../../services/arrays';
import { Strings } from '../../services/strings';
import { IGameResult } from '../games/iGameResult';
import { GameResultGrid } from '../games/gameResultGrid';

export interface IDashboardState {
    players?: IPlayer[];
    gameresults?: IGameResult[];
}

export class Dashboard extends React.Component<RouteComponentProps<{}>, IDashboardState> {

    private readonly topPlayersMinimumNumberOfGames = 1;
    private readonly topPlayersMinimumNumberOfPlayers = 5;
    private readonly maximumNumberOfRecentgames = 5;

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

    private getRecentGames() {
        HttpService.get<IGameResult[]>(`/api/games/recent/${this.maximumNumberOfRecentgames}`, data => {
            this.setState({ gameresults: data });
        })
    }

    public componentDidMount() {
        this.getPlayers();
        this.getRecentGames();
    }

    public render() {
        var numberOfTopPlayers = (this.state.players || []).length || this.topPlayersMinimumNumberOfPlayers;
        var numberOfTopPlayersHeading = `Top ${numberOfTopPlayers}  ${Strings.pluralize(numberOfTopPlayers, "Player")}`;
        var numberOfTopPlayersSubheading = `That have played at least ${this.topPlayersMinimumNumberOfGames} ${Strings.pluralize(this.topPlayersMinimumNumberOfGames, "game")}`;
        var noTopPlayersMessage = `No players have played at least ${this.topPlayersMinimumNumberOfGames} ${Strings.pluralize(this.topPlayersMinimumNumberOfGames, "game")}`;
        var numberOfRecentGames = (this.state.gameresults || []).length || this.maximumNumberOfRecentgames;
        var recentGamesHeading = `Most Recent ${numberOfRecentGames} ${Strings.pluralize(numberOfRecentGames, "Game")}`;
        var noRecentGamesMessage = "No recent games have been played";
        return (
            <div>
                <h1>Welcome to the Dashboard</h1>
                <div className="row">
                    <div className="col-sm-3">
                        <h4>{numberOfTopPlayersHeading}<br /><small>{numberOfTopPlayersSubheading}</small></h4>
                        <PlayerGrid players={this.state.players} noPlayersMessage={noTopPlayersMessage} />
                    </div>
                    <div className="col-sm-9">
                        <h4>{recentGamesHeading}<br /><small>&nbsp;</small></h4>
                        <GameResultGrid gameResults={this.state.gameresults} noGameResultsMessage={noTopPlayersMessage} />
                    </div>
                </div>
            </div>
        );
    }
}
