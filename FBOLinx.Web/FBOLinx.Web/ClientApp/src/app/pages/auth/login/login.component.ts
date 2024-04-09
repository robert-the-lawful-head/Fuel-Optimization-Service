import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

// Services
import { OAuthService } from '../../../services/oauth.service';

@Component({
    selector: 'app-login',
    styleUrls: ['./login.component.scss'],
    templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
    partner: string;
    redirectTo: string;
    loginForm: FormGroup;
    error: string;
    submit: boolean;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private oauthService: OAuthService,
        private formBuilder: FormBuilder
    ) {
        this.loginForm = this.formBuilder.group({
            password: new FormControl(''),
            username: new FormControl(''),
        });
    }

    ngOnInit() {
        this.route.queryParams.subscribe((params) => {
            if (!params.partner || !params.redirectTo) {
                this.router.navigate(['/']);
            }
            this.partner = params.partner;
            this.redirectTo = params.redirectTo;
        });
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
                        this.error = err;
                        this.submit = false;
                    }
                );
        }
    }
}
