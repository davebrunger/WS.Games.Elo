import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';

export class NavMenu extends React.Component<{}, {}> {
    public render() {
        return (
            <div className='navbar navbar-inverse'>
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
                    <div className='navbar-collapse collapse'>
                        <ul className='nav navbar-nav'>
                            <li>
                                <NavLink to={'/'} exact activeClassName='active'>
                                    <span className='glyphicon glyphicon-home'></span> Dashboard
                            </NavLink>
                            </li>
                            <li>
                                <NavLink to={'/players'} activeClassName='active'>
                                    <span className='glyphicon glyphicon-education'></span> Players
                            </NavLink>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        );
    }
}
