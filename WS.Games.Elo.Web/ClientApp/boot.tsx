import './css/site.css';
import 'bootstrap';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { BrowserRouter, Route } from 'react-router-dom';
import { library } from '@fortawesome/fontawesome-svg-core'
import {
    faTrophy, faCertificate, faArrowUp, faArrowDown, faMinus, faUser, faUserCheck, faTachometerAlt,
    faUsers, faSignOutAlt, faCog, faCogs, faUserCog
} from '@fortawesome/free-solid-svg-icons'
import { Layout } from './components/Layout';

library.add(faTrophy);
library.add(faCertificate);
library.add(faArrowUp);
library.add(faArrowDown);
library.add(faMinus);
library.add(faUser);
library.add(faUserCheck);
library.add(faTachometerAlt);
library.add(faUsers);
library.add(faSignOutAlt);
library.add(faCog);
library.add(faCogs);
library.add(faUserCog);

function renderApp() {
    // This code starts up the React app when it runs in a browser. It sets up the routing
    // configuration and injects the app into a DOM element.
    const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
    ReactDOM.render(
        <AppContainer>
            <BrowserRouter children={<Route path="/" component={Layout}/>} basename={baseUrl} />
        </AppContainer>,
        document.getElementById('react-app')
    );
}
renderApp();