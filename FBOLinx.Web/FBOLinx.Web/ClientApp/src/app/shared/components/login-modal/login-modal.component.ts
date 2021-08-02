import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { AuthenticationService } from '../../../services/authentication.service';
import { Router } from '@angular/router';

//Services
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-login-modal',
    templateUrl: './login-modal.component.html',
    styleUrls: [ './login-modal.component.scss' ],
})
export class LoginModalComponent {
    loginForm: FormGroup;
    error: '';

    constructor(
        public dialogRef: MatDialogRef<LoginModalComponent>,
        private formBuilder: FormBuilder,
        private router: Router,
        private authenticationService: AuthenticationService,
        private sharedService: SharedService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.loginForm = this.formBuilder.group({
            username: new FormControl(''),
            password: new FormControl(''),
            remember: new FormControl(false),
        });
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    public onSubmit() {
        if (this.loginForm.valid) {
            this.authenticationService.login(
                this.loginForm.value.username,
                this.loginForm.value.password,
                this.loginForm.value.remember
            ).subscribe((data) => {
                localStorage.removeItem('impersonatedrole');
                localStorage.removeItem('managerGroupId');
                localStorage.removeItem('conductorFbo');
                this.authenticationService
                    .postAuth()
                    .subscribe(() => {
                        this.dialogRef.close();
                        if (data.role === 3) {
                            this.router.navigate([ '/default-layout/groups/' ]);
                        } else if (data.role === 2) {
                            this.router.navigate([ '/default-layout/fbos/' ]);
                        } else if (data.role === 5) {
                            this.sharedService.currentUser.icao = data.fbo.fboAirport.icao;
                            this.router.navigate([
                                '/default-layout/dashboard-csr/',
                            ]);
                        } else {
                            this.router.navigate([
                                '/default-layout/dashboard-fbo/',
                            ]);
                        }
                    });
            }, (error) => {
                this.error = error;
            });
        }
    }

    public openRequestDemo() {
        this.dialogRef.close({
            mode: 'request-demo',
        });
    }

    public openForgotPassword() {
        this.dialogRef.close({
            mode: 'forgot-password',
        });
    }
}
