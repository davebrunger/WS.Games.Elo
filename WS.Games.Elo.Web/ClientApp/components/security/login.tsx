import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { SecurityService } from '../../services/securityService';

export interface ILoginProps extends RouteComponentProps<{}> {
    securityService: SecurityService;
}

export interface ILoginState {
    username? : string;
    password? : string;
}

export class Login extends React.Component<ILoginProps, ILoginState> {

    constructor(props : any)
    {
        super(props);
        this.state = {};
        this.handleUsernameChanged = this.handleUsernameChanged.bind(this);
        this.handlePasswordChanged = this.handlePasswordChanged.bind(this);
        this.handleButtonClick = this.handleButtonClick.bind(this);
    }

    private handleUsernameChanged(event : React.FormEvent<HTMLInputElement>) {
        this.setState({username : event.currentTarget.value});
    }

    private handlePasswordChanged(event : React.FormEvent<HTMLInputElement>) {
        this.setState({password : event.currentTarget.value});
    }

    private handleButtonClick() {
        this.props.securityService.login(this.state.username || "", this.state.password || "", loggedIn => {
            if (loggedIn) {
                this.props.history.push("/");
            }
            else {
                alert("Login Failed");
            }
        });
    }

    public render() {
        return (
            <div>
                <h1>Login</h1>
                <form className="form-horizontal">
                    <div className="form-group">
                        <label className="col-sm-2 control-label">Username</label>
                        <div className="col-sm-10">
                            <input type="text" className="form-control" placeholder="Username" onChange={this.handleUsernameChanged} />
                        </div>
                    </div>
                    <div className="form-group">
                        <label className="col-sm-2 control-label">Password</label>
                        <div className="col-sm-10">
                            <input type="password" className="form-control" placeholder="Password" onChange={this.handlePasswordChanged}/>
                        </div>
                    </div>
                    <div className="form-group">
                        <div className="col-sm-offset-2 col-sm-10">
                            <button type="button" className="btn btn-primary" onClick={this.handleButtonClick}>Login</button>
                        </div>
                    </div>
                </form>
            </div>
        );
    };
}
