import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

// Services
import { AuthenticationService } from '../../../services/authentication.service';

@Component({
    selector: 'app-authtoken',
    styleUrls: ['./authtoken.component.scss'],
    templateUrl: './authtoken.component.html',
})
export class AuthtokenComponent {
    tokenParam: string;
    idParam: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        // Check for passed in id
        //const token = this.route.snapshot.paramMap.get('token');
        this.route.queryParamMap.subscribe((params) => {
            this.tokenParam = params.get('token');
            this.idParam = params.get('id');
        });

        if (!this.tokenParam || this.tokenParam === '') {
            this.router.navigate(['/']);
        } else {
            const decodedToken = decodeURIComponent(this.tokenParam);

            this.authenticationService
                .getAuthToken(decodedToken)
                // .pipe(first())
                .subscribe(
                    (authTokenData) => {
                        if (authTokenData.success) {
                            this.authenticationService
                                .preAuth(authTokenData.authToken)
                                .subscribe((data) => {
                                    if (data.role === 3 || data.role === 2) {
                                        this.router.navigate([
                                            '/default-layout/fbos/',
                                        ]);
                                    }
                                    else if (this.idParam != "" && (data.role == 6 || data.role == 1))
                                    {
                                        this.router.navigate(['/default-layout/fuelreqs'], { queryParams: { id: this.idParam } });
                                    }
                                    else if (data.role == 6) {
                                        this.router.navigate([
                                            '/default-layout/fuelreqs',
                                        ]);
                                    }
                                    else {
                                        this.router.navigate([
                                            '/default-layout/dashboard-fbo-updated/',
                                        ]);
                                    }
                                });
                        } else {
                            this.router.navigate(['/landing-site']);
                        }
                    },
                    () => {
                        this.router.navigate(['/landing-site']);
                    }
                );
        }
    }
}
