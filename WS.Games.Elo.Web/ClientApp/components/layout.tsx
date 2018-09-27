import * as React from 'react';
import { NavMenu } from './navMenu';
import { Route, RouteComponentProps, Redirect, Switch } from 'react-router-dom';
import { Dashboard } from './dashboard/Dashboard';
import { PlayerList } from './players/PlayerList';
import { HttpService } from '../services/httpService';
import { SecurityService } from '../services/securityService';
import { Login } from './security/login';
import { Players } from './admin/players';
import { Games } from './admin/games';
import { RegisterGameResult } from './admin/registerGameResult';
import { Profile } from './security/profile';

export interface ILayoutState {
    currentUser?: string;
}

export class Layout extends React.Component<RouteComponentProps<{}>, ILayoutState> {

    private readonly securityService?: SecurityService;
    private readonly httpService?: HttpService;

    constructor(props: any) {
        super(props);
        this.state = {};
        this.securityService = new SecurityService(
            currentUser => {
                this.setState({ currentUser: currentUser });
            },
            () => {
                this.setState({ currentUser: undefined }, () => {
                    //this.props.history.push("/");
                })
            }
        );
        this.httpService = new HttpService(this.securityService);
        this.securityService.getCurrentUser(currentUser => {
            if (currentUser) {
                this.setState({ currentUser: currentUser });
            }
        });
        this.logout = this.logout.bind(this);
    }

    private logout() {
        if (this.securityService) {
            this.securityService.logout();
        }
    }

    public render() {

        var httpServiceProps = {
            httpService: this.httpService as HttpService
        };

        var securityServiceProps = {
            securityService: this.securityService as SecurityService
        };

        var currentUserProps = {
            currentUser: this.state.currentUser
        };

        var adminRoutes = this.state.currentUser
            ? [
                <Route path='/admin' key='adminRoutes'>
                    <div>
                        <Route path='/admin/games' render={(routeProps) => <Games {...routeProps}  {...currentUserProps} />} />
                        <Route path='/admin/players' render={(routeProps) => <Players {...routeProps} {...currentUserProps} />} />
                        <Route path='/admin/registerGameResult' render={(routeProps) => <RegisterGameResult {...routeProps} {...currentUserProps} />} />
                    </div>
                </Route>,
                <Route path='/profile' key='profileRoute' render={(routeProps) => <Profile {...routeProps} {...currentUserProps} />} />
            ]
            : null;

        return (
            <div>
                <NavMenu currentUser={this.state.currentUser} logout={this.logout} />
                <div className='container-fluid'>
                    <Switch>
                        <Route exact path='/' render={(routeProps) => <Dashboard {...routeProps} {...httpServiceProps} />} />
                        <Route path='/players' render={(routeProps) => <PlayerList {...routeProps} {...httpServiceProps} />} />
                        <Route path='/login' render={(routeProps) => <Login {...routeProps} {...securityServiceProps} />} />
                        {adminRoutes}
                        <Route>
                            <Redirect to='/' />
                        </Route>
                    </Switch>
                </div>
            </div>
        );
    }
}
