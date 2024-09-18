import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
//Services
import { AuthenticationService } from '../../../services/authentication.service';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';
import { GroupsService } from 'src/app/services/groups.service';
import { UserRole } from 'src/app/enums/user-role';

@Component({
    selector: 'app-login-modal',
    styleUrls: ['./login-modal.component.scss'],
    templateUrl: './login-modal.component.html',
})
export class LoginModalComponent {
    loginForm: FormGroup;
    error: '';
    public groupsFbosData: any;

    constructor(
        public dialogRef: MatDialogRef<LoginModalComponent>,
        private formBuilder: FormBuilder,
        private router: Router,
        private authenticationService: AuthenticationService,
        private sharedService: SharedService,
        private groupsService: GroupsService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.loginForm = this.formBuilder.group({
            password: new FormControl(''),
            remember: new FormControl(false),
            username: new FormControl(''),
        });
        authenticationService.logout();
    }
    onCancelClick(): void {
        this.dialogRef.close();
    }

    public onSubmit() {
        if (this.loginForm.valid) {
            this.authenticationService
                .login(
                    this.loginForm.value.username,
                    this.loginForm.value.password,
                    this.loginForm.value.remember
                )
                .subscribe(
                    (data: any) => {
                        this.authenticationService.postAuth().subscribe(async () => {
                            this.dialogRef.close();
                            if (data.role === UserRole.Conductor) {
                                this.router.navigate([
                                    '/default-layout/groups/',
                                ]);
                            } else if (data.role === UserRole.GroupAdmin) {
                                this.router.navigate(['/default-layout/fbos/']);
                            } else {
                                this.setFboSessionVariables(data);

                                if (data.role === UserRole.CSR)
                                    this.router.navigate([
                                        '/default-layout/dashboard-csr/',
                                    ]);
                                else
                                    this.router.navigate([
                                        '/default-layout/dashboard-fbo-updated/',
                                    ]);
                            }
                        });
                    },
                    (error) => {
                        this.error = error;
                    }
                );
        }
    }
    private async setFboSessionVariables(fboObj: any) {
        let icao: string = fboObj.icao;
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.accountType,fboObj.fbo?.accountType);
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.fbo,fboObj.fbo.fbo);
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.fboId,fboObj.fboId);
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.icao,icao);
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.isNetworkFbo,(fboObj.group.fbos.length > 1));
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.accountType,fboObj.fbo?.accountType);
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.groupId,fboObj.groupId);

        var isSingleSourceFbo = await this.groupsService.isGroupFboSingleSource(icao).toPromise();
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.isSingleSourceFbo,isSingleSourceFbo.toString());
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
