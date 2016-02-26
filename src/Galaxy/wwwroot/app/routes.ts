import { Route, Router } from 'angular2/router'
import { Account } from './components/account/account';

export var Routes = {
    account: new Route({ path: '/account/...', name: 'Account', component: Account })
};

export const APP_ROUTES = Object.keys(Routes).map(r => Routes[r]);