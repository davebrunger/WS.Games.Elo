import './css/site.css';
import 'bootstrap';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { BrowserRouter } from 'react-router-dom';
import * as RoutesModule from './routes';
import { library } from '@fortawesome/fontawesome-svg-core'
import { faTrophy, faCertificate, faArrowUp, faArrowDown, faMinus } from '@fortawesome/free-solid-svg-icons'

library.add(faTrophy);
library.add(faCertificate);
library.add(faArrowUp);
library.add(faArrowDown);
library.add(faMinus);

let routes = RoutesModule.routes;

function renderApp() {
    // This code starts up the React app when it runs in a browser. It sets up the routing
    // configuration and injects the app into a DOM element.
    const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
    ReactDOM.render(
        <AppContainer>
            <BrowserRouter children={ routes } basename={ baseUrl } />
        </AppContainer>,
        document.getElementById('react-app')
    );
}
renderApp();