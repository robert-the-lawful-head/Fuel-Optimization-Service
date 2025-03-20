import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

// Services
import { AuthenticationService } from '../../../services/security/authentication.service';
import { Subscription } from 'rxjs';
import { UserService } from '../../../services/user.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-authtoken',
    styleUrls: ['./authtoken.component.scss'],
    templateUrl: './authtoken.component.html',
})
export class AuthtokenComponent {
    tokenParam: string;
    idParam: string;
    groupIdParam: string;
    fboIdParam: string;
    accountTypeParam: string;
    icaoParam: string;
    isSingleSourceFboParam: string;
    isNetWorkFboParam: string;

    routeSubscription: Subscription;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private sharedService: SharedService
    ) {
        // Check for passed in id
        //const token = this.route.snapshot.paramMap.get('token');
        this.routeSubscription = this.route.queryParamMap.subscribe((params) => {
            this.tokenParam = params.get('token');
            this.idParam = params.get('id');
            this.groupIdParam = params.get('groupId');
            this.fboIdParam = params.get('fboId');
            this.accountTypeParam = params.get('accountType');
            this.icaoParam = params.get('icao');
            this.isSingleSourceFboParam = params.get('isSingleSourceFbo');
            this.isNetWorkFboParam = params.get('isNetworkFbo');
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
                                .subscribe(() => {
                                    this.userService
                                        .getCurrentUser().subscribe((data) => {
                                            this.authenticationService.setCurrentUser(data);

                                            if (this.groupIdParam != undefined && this.groupIdParam != "") {
                                                this.sharedService.setLocalStorageForImpersonation(Number(this.groupIdParam), Number(this.fboIdParam), Number(this.accountTypeParam), this.icaoParam, this.isSingleSourceFboParam.toLowerCase(), this.isNetWorkFboParam.toLowerCase());
                                            }
                                            else {
                                                if (data.getCurrentUser === 3 || data.role === 2) {
                                                    this.router.navigate([
                                                        '/default-layout/fbos/',
                                                    ]);
                                                }
                                                else if (this.idParam != "" && (data.role == 6 || data.role == 1)) {
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
                                            }
                                        });
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
    ngOnDestroy() {
        this.routeSubscription?.unsubscribe();
    }
}
