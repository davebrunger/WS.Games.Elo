import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Players extends React.Component<RouteComponentProps<{}>, {}> {
    public render () {
        return (
            <h1>Players</h1>
        );
    }
}