import { Component, OnInit } from '@angular/core';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from '@angular/material';

//Services
import { LandingsiteService } from '../../services/landingsite.service';
import { SharedService } from '../shared-service';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';

//Components
import { ForgotPasswordDialogComponent } from '../../shared/components/forgot-password/forgot-password-dialog/forgot-password-dialog.component';

//import * as jqueryFancyBox from "jquery.fancybox";

export interface ContactUsMessage {
    name: string;
    email: string;
    phoneNumber: string;
    message: string;
}

export interface LoginRequest {
    username: string;
    password: string;
}

@Component({
    moduleId: module.id,
    selector: 'landing-site-layout',
    templateUrl: './landing-site.component.html',
    styleUrls: ['./landing-site.component.scss'],
    providers: [SharedService]
})
/** landing-site component*/
export class LandingSiteLayoutComponent implements OnInit {

    private rememberMeUsernameKey: string = 'rememberMeUsername';
    private rememberMePasswordKey: string = 'rememberMePassword';

    //Images
    public slideHalfImage1URL = require('../../../assets/content/slide-half-1.png');
    public slideHalfImage2URL = require('../../../assets/content/slide-half-2.png');
    public slideHalfImage3URL = require('../../../assets/content/slide-half-3.png');
    public featureLayoutImage = require('../../../assets/content/feature-layout.png');
    public featureHtml5Image = require('../../../assets/content/feature-html5.png');
    public featureWebfontImage = require('../../../assets/content/feature-webfonts.png');
    public featureDesignImage = require('../../../assets/content/feature-design.png');
    public featureCustomizeImage = require('../../../assets/content/feature-customize.png');
    public featureSupportImage = require('../../../assets/content/feature-support.png');
    public paragonSmallLogoImage = require('../../../assets/content/paragon-2c-logo_small.png');

    public carouselImages: Array<any>;
    public contactUsMessage: ContactUsMessage;
    public loginRequest: LoginRequest;
    public rememberMe: any;
    public isLoggingIn: boolean = false;
    public error: string = '';

    /** landing-site ctor */
    constructor(private config: NgbCarouselConfig,
        private landingsiteService: LandingsiteService,
        private router: Router,
        private sharedService: SharedService,
        private authenticationService: AuthenticationService,
        private snackBar: MatSnackBar,
        public forgotPasswordDialog: MatDialog,
        private userService: UserService    ) {
        this.carouselImages = [this.slideHalfImage1URL, this.slideHalfImage2URL, this.slideHalfImage3URL];
    }

    ngOnInit() {
        this.resetContactUsMessage();
        this.resetLoginRequest();
    }

    public sendContactUsEmailClicked() {
        this.landingsiteService.postContactUsMessage(this.contactUsMessage).subscribe((data: any) => {
            this.resetContactUsMessage();
            //Show successful message sent here
        });
    }

    public attemptLogin() {
        this.isLoggingIn = true;

        this.authenticationService.login(this.loginRequest.username, this.loginRequest.password)
            //.pipe(first())
            .subscribe(
            data => {
                this.setRememberMeVariables();
                this.authenticationService.postAuth().subscribe(postLoginCheckResult => {
                    if (data.role == 3 || data.role == 2)
                    this.router.navigate(['/default-layout/fbos/']);
                else
                    this.router.navigate(['/default-layout/dashboard-fbo/']);
                });
            },
                error => {
                    this.error = error;
                    this.isLoggingIn = false;
                    // Simple message.
                    let snackBarRef = this.snackBar.open(this.error, '', {
                        duration: 5000
                    });
                });

        //this.landingsiteService.login(this.loginRequest).subscribe((data: any) => {
        //    if (data && data.fboId > 0) {
        //        this.sharedService.currentUser = data;
        //        this.setRememberMeVariables();
        //        this.router.navigate(['/default-layout/dashboard-fbo/']);
        //    } else {
        //        //Show failure popup here
        //    }
        //    this.isLoggingIn = false;
        //});
    }

    public scrollToElement($element): void {
        console.log($element);
        $element.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
    }

    public onEnterKeyUp(event) {
        if (this.loginRequest.password == '' || this.loginRequest.username == '')
            return;
        this.attemptLogin();
    }

    public forgotPasswordClicked() {
        var data = { username: '' };
        const dialogRef = this.forgotPasswordDialog.open(ForgotPasswordDialogComponent, {
            width: '450px',
            data: data
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            console.log('Dialog data: ', result);
            this.userService.resetPassword(result).subscribe((data: any) => {
                    let snackBarRef = this.snackBar.open('An email has been sent with access instructions',
                        '',
                        {
                            duration: 5000
                        });
                },
                error => {
                    this.error = error;
                    // Simple message.
                    let snackBarRef = this.snackBar.open(this.error, '', {
                        duration: 5000
                    });
                });
        });
    }

    //Private Methods
    private resetContactUsMessage() {
        this.contactUsMessage = {
            name: '',
            email: '',
            phoneNumber: '',
            message: ''
        }
    }

    private resetLoginRequest() {
        this.loginRequest = {
            username: '',
            password: ''
        }
    }

    private setRememberMeVariables() {
        if (!localStorage)
            return;
        if (!this.rememberMe) {
            localStorage.setItem(this.rememberMeUsernameKey, '');
            localStorage.setItem(this.rememberMePasswordKey, '');
        } else {
            localStorage.setItem(this.rememberMeUsernameKey, this.loginRequest.username);
            localStorage.setItem(this.rememberMePasswordKey, this.loginRequest.password);
        }
    }
}
