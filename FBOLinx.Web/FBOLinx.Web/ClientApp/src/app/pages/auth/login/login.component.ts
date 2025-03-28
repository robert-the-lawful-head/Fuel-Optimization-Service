import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

// Services
import { OAuthService } from '../../../services/oauth.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-login',
    styleUrls: ['./login.component.scss'],
    templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
    partner: string;
    redirectTo: string;
    loginForm: UntypedFormGroup;
    error: string;
    submit: boolean;

    routeSubscription: Subscription;
    
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private oauthService: OAuthService,
        private formBuilder: UntypedFormBuilder
    ) {
        this.loginForm = this.formBuilder.group({
            password: new UntypedFormControl(''),
            username: new UntypedFormControl(''),
        });
    }

    ngOnInit() {
        this.routeSubscription =this.route.queryParams.subscribe((params) => {
            if (!params.partner || !params.redirectTo) {
                this.router.navigate(['/']);
            }
            this.partner = params.partner;
            this.redirectTo = params.redirectTo;
        });
    }
    ngOnDestroy() {
        this.routeSubscription?.unsubscribe();
    }
    public onSubmit() {
        if (this.submit) {
            return;
        }

        this.error = '';
        if (this.loginForm.valid) {
            this.submit = true;
            this.oauthService
                .login(
                    this.loginForm.value.username,
                    this.loginForm.value.password,
                    this.partner
                )
                .subscribe(
                    (token: any) => {
                        let tokenQueryString =
                            'accessToken=' +
                            encodeURIComponent(token.accessToken);
                        if (this.redirectTo.indexOf('?') === -1) {
                            tokenQueryString = '?' + tokenQueryString;
                        } else {
                            tokenQueryString = '&' + tokenQueryString;
                        }
                        window.location.href =
                            this.redirectTo + tokenQueryString;
                    },
                    (err: any) => {
                        console.log(err);
                        this.error = err.error.message;
                        this.submit = false;
                    }
                );
        }
    }
}
