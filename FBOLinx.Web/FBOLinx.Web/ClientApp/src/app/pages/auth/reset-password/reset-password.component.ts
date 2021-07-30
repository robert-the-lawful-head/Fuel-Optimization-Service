import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { User } from '../../../models/user';
// Services
import { UserService } from '../../../services/user.service';

@Component({
    selector: 'app-reset-password',
    styleUrls: [ './reset-password.component.scss' ],
    templateUrl: './reset-password.component.html',
})
export class ResetPasswordComponent implements OnInit {
    token: string;
    form: FormGroup;
    submit: boolean;
    validated: boolean;
    validationError: boolean;
    error: string;
    user: User;
    reset: boolean;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService,
        private formBuilder: FormBuilder
    ) {
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            if (!params.token) {
                this.router.navigate([ '/' ]);
            }

            this.token = params.token;
            this.validated = false;
            this.validationError = false;

            this.userService.validateResetPasswordToken(this.token).subscribe((user: User) => {
                this.form = this.formBuilder.group({
                    confirmPassword: new FormControl(''),
                    email: new FormControl(user.username),
                    newPassword: new FormControl(''),
                }, {
                    validators: this.passwordConfirming
                });
                this.validated = true;
                this.user = user;
            }, () => {
                this.validated = true;
                this.validationError = true;
            });
        });
    }

    passwordConfirming(c: AbstractControl) {
        if (c.get('newPassword').value !== c.get('confirmPassword').value) {
            return {
                passwordNotMatch: true
            };
        }
    }

    onSubmit() {
        if (this.submit) {
            return;
        }

        this.error = '';
        if (this.form.valid) {
            this.submit = true;
            this.userService.resetPassword({
                password: this.form.value.newPassword,
                resetPasswordToken: this.token,
                username: this.user.username
            }).subscribe(() => {
                this.reset = true;
                setTimeout(() => {
                    this.router.navigate([ '/' ]);
                }, 2000);
            }, () => {
                this.error = 'Failed to reset the password. Please contact the FBOLinx team!';
                this.submit = false;
            });
        }
    }
}
