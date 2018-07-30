import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Dashboard } from './components/dashboard/Dashboard';
import { PlayerList } from './components/players/PlayerList';

export const routes = <Layout>
    <Route exact path='/' component={ Dashboard } />
    <Route exact path='/players' component={ PlayerList } />
</Layout>;
