import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
//Services
import { AuthenticationService } from '../../../services/authentication.service';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';
import { ManageFboGroupsService } from 'src/app/services/managefbo.service';
import { GroupsService } from 'src/app/services/groups.service';
import { GroupFboViewModel } from 'src/app/models/groups';

@Component({
    selector: 'app-login-modal',
    styleUrls: ['./login-modal.component.scss'],
    templateUrl: './login-modal.component.html',
})
export class LoginModalComponent {
    loginForm: FormGroup;
    error: '';
    public groupsFbosData: GroupFboViewModel;

    constructor(
        public dialogRef: MatDialogRef<LoginModalComponent>,
        private formBuilder: FormBuilder,
        private router: Router,
        private authenticationService: AuthenticationService,
        private sharedService: SharedService,
        private manageFboGroupsService: ManageFboGroupsService,
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
                    (data) => {
                        console.log("🚀 ~ LoginModalComponent ~ onSubmit ~ data:", data)
                        localStorage.removeItem('impersonatedrole');
                        localStorage.removeItem('managerGroupId');
                        localStorage.removeItem('conductorFbo');
                        this.authenticationService.postAuth().subscribe(() => {
                            this.dialogRef.close();
                            if (data.role === 3) {
                                this.router.navigate([
                                    '/default-layout/groups/',
                                ]);
                            } else if (data.role === 2) {
                                this.router.navigate(['/default-layout/fbos/']);
                            } else {
                                this.setFboSessionVariables(data.fbo.groupId, data.fbo.fboAirport.icao);

                                if (data.role === 5)
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
    private async setFboSessionVariables(groupId: number, icao: string) {
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.groupId,groupId.toString());
        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.icao,icao);

        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.isNetworkFbo,this.manageFboGroupsService.isNetworkFbo(this.groupsFbosData,groupId).toString());

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
