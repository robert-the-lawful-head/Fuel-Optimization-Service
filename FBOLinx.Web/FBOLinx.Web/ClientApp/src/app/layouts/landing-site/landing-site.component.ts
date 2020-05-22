import { Component, OnInit, HostListener } from "@angular/core";
import { NgbCarouselConfig } from "@ng-bootstrap/ng-bootstrap";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

// Services
import { LandingsiteService } from "../../services/landingsite.service";
import { SharedService } from "../shared-service";
import { AuthenticationService } from "../../services/authentication.service";
import { UserService } from "../../services/user.service";

// Components
import { ForgotPasswordDialogComponent } from "../../shared/components/forgot-password/forgot-password-dialog/forgot-password-dialog.component";
import { MatButtonToggleChange } from "@angular/material/button-toggle";
import { LoginModalComponent } from "../../shared/components/login-modal/login-modal.component";
import { RequestDemoModalComponent } from "../../shared/components/request-demo-modal/request-demo-modal.component";
import { RequestDemoSuccessComponent } from "../../shared/components/request-demo-success/request-demo-success.component";

// import * as jqueryFancyBox from "jquery.fancybox";

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
    selector: "landing-site-layout",
    templateUrl: "./landing-site.component.html",
    styleUrls: ["./landing-site.component.scss"],
    providers: [SharedService],
})
export class LandingSiteLayoutComponent {
    public rememberMeUsernameKey = "rememberMeUsername";
    public rememberMePasswordKey = "rememberMePassword";

    // Images
    public slideHalfImage1URL = "../../../assets/content/slide-half-1.png";
    public slideHalfImage2URL = "../../../assets/content/slide-half-2.png";
    public slideHalfImage3URL = "../../../assets/content/slide-half-3.png";
    public featureLayoutImage = "../../../assets/content/feature-layout.png";
    public featureHtml5Image = "../../../assets/content/feature-html5.png";
    public featureWebfontImage = "../../../assets/content/feature-webfonts.png";
    public featureDesignImage = "../../../assets/content/feature-design.png";
    public featureCustomizeImage = "../../../assets/content/feature-customize.png";
    public featureSupportImage = "../../../assets/content/feature-support.png";
    public paragonSmallLogoImage = "../../../assets/content/paragon-2c-logo_small.png";
    public fuelerlinxLogo = "../../../assets/img/FuelerLinxLogo.png";
    public x1fboLogo = "../../../assets/img/X1.png";
    public millionAirLogo = "../../../assets/img/million-air.png";
    public flightAwareLogo = "../../../assets/img/FlightAware.png";
    public titanLogo = "../../../assets/img/titan.png";
    public jetAviation = "../../../assets/img/JetAviation.png";
    public amstatLogo = "../../../assets/img/Amstat.png";
    public fbodirectorLogo = "../../../assets/img/fbodirector.png";
    public fbopartnersLogo = "../../../assets/img/fbopartners.png";
    public planeFrontImage = "../../../assets/img/landing-page/DSC03168.jpg";

    public carouselImages: Array<any> = [
        this.slideHalfImage1URL,
        this.slideHalfImage2URL,
        this.slideHalfImage3URL,
    ];

    public integrationPartners: Array<any> = [
        [
            this.fuelerlinxLogo,
            this.x1fboLogo,
            this.millionAirLogo,
        ],
        [
            this.flightAwareLogo,
            this.titanLogo,
            this.jetAviation,
        ],
        [
            this.amstatLogo,
            this.fbodirectorLogo,
            this.fbopartnersLogo,
        ],
    ];

    public integrationPartnerView = 0;
    public contactUsMessage: ContactUsMessage;
    public loginRequest: LoginRequest;
    public rememberMe: any;
    public isLoggingIn = false;
    public error = "";
    public isSticky = false;    

    constructor(
        private snackBar: MatSnackBar,
        private forgotPasswordDialog: MatDialog,
        private userService: UserService,
        private loginDialog: MatDialog,
        private requestDemoDialog: MatDialog,
        private requestDemoSuccessDialog: MatDialog
    ) {}

    public onLogin() {
        const data = {};
        const dialogRef = this.loginDialog.open(
            LoginModalComponent,
            {
                data,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            if (result.mode === "request-demo") {
                this.openRequestDemo();
            }
            if (result.mode === "forgot-password") {
                this.forgotPassword();
            }
        });
    }

    public openRequestDemo() {
        const data ={
            succeed: false,
        };
        const dialogRef = this.requestDemoDialog.open(
            RequestDemoModalComponent,
            {
                width: "600px",
                height: "650px",
                panelClass: "request-demo-container",
                data,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) return;

            this.requestDemoSuccessDialog.open(RequestDemoSuccessComponent);
        });
    }

    public forgotPassword() {
        const data = { username: "" };
        const dialogRef = this.forgotPasswordDialog.open(
            ForgotPasswordDialogComponent,
            {
                width: "450px",
                data,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            console.log("Dialog data: ", result);
            this.userService.resetPassword(result).subscribe(
                () => {
                    this.snackBar.open(
                        "An email has been sent with access instructions",
                        "",
                        {
                            duration: 5000,
                        }
                    );
                },
                (error) => {
                    this.error = error;
                    // Simple message.
                    this.snackBar.open(this.error, "", {
                        duration: 5000,
                    });
                }
            );
        });
    }

    public setRememberMeVariables() {
        if (!localStorage) {
            return;
        }
        if (!this.rememberMe) {
            localStorage.setItem(this.rememberMeUsernameKey, "");
            localStorage.setItem(this.rememberMePasswordKey, "");
        } else {
            localStorage.setItem(
                this.rememberMeUsernameKey,
                this.loginRequest.username
            );
            localStorage.setItem(
                this.rememberMePasswordKey,
                this.loginRequest.password
            );
        }
    }

    public changeIntegrationPartnerView(event: MatButtonToggleChange) {
        switch (Number(event.value)) {
            case 0:
                this.integrationPartners = [
                    [
                        this.fuelerlinxLogo,
                        this.x1fboLogo,
                        this.millionAirLogo,
                    ],
                    [
                        this.flightAwareLogo,
                        this.titanLogo,
                        this.jetAviation,
                    ],
                    [
                        this.amstatLogo,
                        this.fbodirectorLogo,
                        this.fbopartnersLogo,
                    ],
                ];
                break;
            case 1:
                this.integrationPartners = [
                    [
                        this.fuelerlinxLogo,
                        this.flightAwareLogo,
                        this.amstatLogo,
                    ],
                    [
                        this.fbopartnersLogo,
                    ],
                ];
                break;
            case 2:
                this.integrationPartners = [
                    [
                        this.fbopartnersLogo,
                        this.titanLogo,
                        this.fbodirectorLogo,
                    ],
                ];
                break;
            case 3:
                this.integrationPartners = [
                    [
                        this.titanLogo,
                        this.millionAirLogo,
                        this.jetAviation,
                    ],
                ];
                break;
            default:
                break;
        }
    }
    
    @HostListener("window:scroll", ["$event"])
    public checkScroll(): void {
        this.isSticky = window.pageYOffset >= 171;
    }
}
