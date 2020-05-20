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
export class LandingSiteLayoutComponent implements OnInit {
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
    public planeFrontImage = "../../../assets/img/landing-page/DSC03168.jpg";

    public carouselImages: Array<any>;
    public contactUsMessage: ContactUsMessage;
    public loginRequest: LoginRequest;
    public rememberMe: any;
    public isLoggingIn = false;
    public error = "";
    public isSticky: boolean = false;    

    constructor(
        private config: NgbCarouselConfig,
        private landingsiteService: LandingsiteService,
        private router: Router,
        private sharedService: SharedService,
        private authenticationService: AuthenticationService,
        private snackBar: MatSnackBar,
        public forgotPasswordDialog: MatDialog,
        private userService: UserService
    ) {
        this.carouselImages = [
            this.slideHalfImage1URL,
            this.slideHalfImage2URL,
            this.slideHalfImage3URL,
        ];
    }

    ngOnInit() {
        this.resetContactUsMessage();
        this.resetLoginRequest();
    }

    public sendContactUsEmailClicked() {
        this.landingsiteService
            .postContactUsMessage(this.contactUsMessage)
            .subscribe((data: any) => {
                this.resetContactUsMessage();
                // Show successful message sent here
            });
    }

    public attemptLogin() {
        this.isLoggingIn = true;

        this.authenticationService
            .login(this.loginRequest.username, this.loginRequest.password)
            // .pipe(first())
            .subscribe(
                (data) => {
                    this.setRememberMeVariables();
                    this.authenticationService
                        .postAuth()
                        .subscribe((postLoginCheckResult) => {
                            if (data.role === 3 || data.role === 2) {
                                this.router.navigate(["/default-layout/fbos/"]);
                            } else {
                                this.router.navigate([
                                    "/default-layout/dashboard-fbo/",
                                ]);
                            }
                        });
                },
                (error) => {
                    this.error = error;
                    this.isLoggingIn = false;
                    // Simple message.
                    const snackBarRef = this.snackBar.open(this.error, "", {
                        duration: 5000,
                    });
                }
            );
    }

    public scrollToElement($element): void {
        console.log($element);
        $element.scrollIntoView({
            behavior: "smooth",
            block: "start",
            inline: "nearest",
        });
    }

    public onEnterKeyUp(event) {
        if (
            this.loginRequest.password === "" ||
            this.loginRequest.username === ""
        ) {
            return;
        }
        this.attemptLogin();
    }

    public forgotPasswordClicked() {
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

    public resetContactUsMessage() {
        this.contactUsMessage = {
            name: "",
            email: "",
            phoneNumber: "",
            message: "",
        };
    }

    public resetLoginRequest() {
        this.loginRequest = {
            username: "",
            password: "",
        };
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

    @HostListener('window:scroll', ['$event'])
    public checkScroll(): void {
        this.isSticky = window.pageYOffset >= 171;
    }
}
