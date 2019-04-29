import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { User } from '../models/User';

import { AuthenticationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    private currentUser: User;

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        //const currentUser = this.authenticationService.currentUserValue;
        
        if (this.currentUser) {
            var expectedRoles = [];
            if (route.data && route.data.expectedRoles)
                expectedRoles = route.data.expectedRoles;

            if (expectedRoles.length == 0 || expectedRoles.indexOf(this.currentUser.role) > -1 || expectedRoles.indexOf(this.currentUser.impersonatedRole) > -1)
                return true;
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/landing-site'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
