import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { NavDropdown, MenuItem } from 'react-bootstrap';

export interface NavMenuProps {
    currentUser?: string;
    logout: () => void;
}

export class NavMenu extends React.Component<NavMenuProps, {}> {

    constructor(props: any) {
        super(props);
        this.logoutClicked = this.logoutClicked.bind(this);
    }

    private logoutClicked() {
        if (confirm("Are you sure you want to logout?")) {
            this.props.logout();
        }
    }

    public render() {

        var userLink = this.props.currentUser
            ? (
            <NavDropdown title={<span><FontAwesomeIcon icon="user" /> {this.props.currentUser}</span>} id="user-dropdown">
                    <MenuItem onClick={this.logoutClicked}><FontAwesomeIcon icon="sign-out-alt" /> Logout</MenuItem>
                    <li>
                        <NavLink to={'/profile'} activeClassName='active'>
                            <FontAwesomeIcon icon="user-cog" /> Profile
                        </NavLink>
                    </li>
                </NavDropdown>
            )
            : (
                <li>
                    <NavLink to={'/login'} activeClassName='active'>
                        <FontAwesomeIcon icon="user" /> Login
                    </NavLink>
                </li>
            );

        var adminLink = this.props.currentUser
            ? (
                <NavDropdown title={<span><FontAwesomeIcon icon="cogs" /> Admin</span>} id="admin-dropdown">
                    <li>
                        <NavLink to={'/admin/players'} activeClassName='active'>
                            <FontAwesomeIcon icon="cog" /> Players
                        </NavLink>
                        <NavLink to={'/admin/games'} activeClassName='active'>
                            <FontAwesomeIcon icon="cog" /> Games
                        </NavLink>
                        <NavLink to={'/admin/registerGameResult'} activeClassName='active'>
                            <FontAwesomeIcon icon="cog" /> Register Game Result
                        </NavLink>
                    </li>
                </NavDropdown>
            )
            : null;

        return (
            <div className='navbar navbar-inverse navbar-static-top'>
                <div className='container-fluid'>
                    <div className='navbar-header'>
                        <button type='button' className='navbar-toggle' data-toggle='collapse' data-target='.navbar-collapse'>
                            <span className='sr-only'>Toggle navigation</span>
                            <span className='icon-bar'></span>
                            <span className='icon-bar'></span>
                            <span className='icon-bar'></span>
                        </button>
                        <Link className='navbar-brand' to={'/'}>ELO Ratings</Link>
                    </div>
                    <div className='navbar-collapse collapse clearfix'>
                        <ul className='nav navbar-nav'>
                            <li>
                                <NavLink to={'/'} activeClassName='active'>
                                    <FontAwesomeIcon icon="tachometer-alt" /> Dashboard
                                </NavLink>
                            </li>
                            <li>
                                <NavLink to={'/players'} activeClassName='active'>
                                    <FontAwesomeIcon icon="users" /> Players
                                </NavLink>
                            </li>
                            {adminLink}
                        </ul>
                        <ul className='nav navbar-nav pull-right hidden-xs'>
                            {userLink}
                        </ul>
                    </div>
                </div>
            </div>
        );
    }
}
