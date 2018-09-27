import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Profile extends React.Component<RouteComponentProps<{}>, {}> {
    public render () {
        return (
            <h1>Profile</h1>
        );
    }
}