import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Games extends React.Component<RouteComponentProps<{}>, {}> {
    public render () {
        return (
            <h1>Games</h1>
        );
    }
}