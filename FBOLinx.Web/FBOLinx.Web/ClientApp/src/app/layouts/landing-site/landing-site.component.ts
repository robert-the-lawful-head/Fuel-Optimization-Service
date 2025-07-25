import { Component, HostListener } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacySnackBar as MatSnackBar } from '@angular/material/legacy-snack-bar';

import { UserService } from '../../services/user.service';
// Components
import { ForgotPasswordDialogComponent } from '../../shared/components/forgot-password/forgot-password-dialog/forgot-password-dialog.component';
import { LoginModalComponent } from '../../shared/components/login-modal/login-modal.component';
// Services
import { SharedService } from '../shared-service';
import { urls } from 'src/app/constants/externalUrlsConstants';

export interface ContactUsMessage {
    name: string;
    email: string;
    phoneNumber: string;
    message: string;
}

@Component({
    providers: [SharedService],
    selector: 'landing-site-layout',
    styleUrls: ['./landing-site.component.scss'],
    templateUrl: './landing-site.component.html',
})
export class LandingSiteLayoutComponent {
    // Images
    slideHalfImage1URL = '../../../assets/content/slide-half-1.png';
    slideHalfImage2URL = '../../../assets/content/slide-half-2.png';
    slideHalfImage3URL = '../../../assets/content/slide-half-3.png';
    featureLayoutImage = '../../../assets/content/feature-layout.png';
    featureHtml5Image = '../../../assets/content/feature-html5.png';
    featureWebfontImage = '../../../assets/content/feature-webfonts.png';
    featureDesignImage = '../../../assets/content/feature-design.png';
    featureCustomizeImage = '../../../assets/content/feature-customize.png';
    featureSupportImage = '../../../assets/content/feature-support.png';
    paragonSmallLogoImage = '../../../assets/content/paragon-2c-logo_small.png';
    fuelerlinxLogo = '../../../assets/img/FuelerLinxLogo.png';
    x1fboLogo = '../../../assets/img/X1.png';
    flightAwareLogo = '../../../assets/img/FlightAware.png';
    titanLogo = '../../../assets/img/titan.png';
    amstatLogo = '../../../assets/img/Amstat.png';
    fbodirectorLogo = '../../../assets/img/fbodirector.png';
    fbopartnersLogo = '../../../assets/img/fbopartners.png';
    planeFrontImage = '../../../assets/img/landing-page/DSC03168.jpg';

    carouselImages: Array<any> = [
        this.slideHalfImage1URL,
        this.slideHalfImage2URL,
        this.slideHalfImage3URL,
    ];

    integrationPartners: Array<any> = [
        [this.fuelerlinxLogo, this.x1fboLogo],
        [this.flightAwareLogo, this.titanLogo],
        [this.amstatLogo, this.fbodirectorLogo, this.fbopartnersLogo],
    ];

    integrationPartnerView = 0;
    contactUsMessage: ContactUsMessage;
    isLoggingIn = false;
    error = '';
    isSticky = false;

    constructor(
        private snackBar: MatSnackBar,
        private forgotPasswordDialog: MatDialog,
        private userService: UserService,
        private loginDialog: MatDialog,
    ) {}

    @HostListener('window:scroll', ['$event'])
    checkScroll(): void {
        this.isSticky = window.pageYOffset >= 171;
    }

    onLogin() {
        const data = {};
        const dialogRef = this.loginDialog.open(LoginModalComponent, {
            data,
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            if (result.mode === 'request-demo') {
                this.openRequestDemo();
            }
            if (result.mode === 'forgot-password') {
                this.forgotPassword();
            }
        });
    }

    openRequestDemo() {
        window.open(urls.demoRequestUrl, '_blank').focus();
        //const data = {
        //    succeed: false,
        //};
        //const dialogRef = this.requestDemoDialog.open(
        //    RequestDemoModalComponent,
        //    {
        //        data,
        //        height: '650px',
        //        panelClass: 'request-demo-container',
        //        width: '600px',
        //    }
        //);

        //dialogRef.afterClosed().subscribe((result) => {
        //    if (!result) {
        //        return;
        //    }

        //    this.requestDemoSuccessDialog.open(RequestDemoSuccessComponent);
        //});
    }

    forgotPassword() {
        const data = {
            email: '',
        };
        const dialogRef = this.forgotPasswordDialog.open(
            ForgotPasswordDialogComponent,
            {
                data,
                width: '450px',
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.userService.requestResetPassword(result).subscribe(
                () => {
                    this.snackBar.open(
                        'An email has been sent with access instructions',
                        '',
                        {
                            duration: 5000,
                        }
                    );
                },
                (error) => {
                    this.error = error;
                    // Simple message.
                    this.snackBar.open(this.error, '', {
                        duration: 5000,
                    });
                }
            );
        });
    }

    changeIntegrationPartnerView(event: MatButtonToggleChange) {
        switch (Number(event.value)) {
            case 0:
                this.integrationPartners = [
                    [this.fuelerlinxLogo, this.x1fboLogo],
                    [this.flightAwareLogo, this.titanLogo],
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
                    [this.fbopartnersLogo],
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
                    [this.titanLogo],
                ];
                break;
            default:
                break;
        }
    }
}
