import { Component, Input, OnInit, HostListener } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { NavigationStart, Router, RouterEvent } from '@angular/router';
import { Store } from '@ngrx/store';
import * as moment from 'moment';
import { filter } from 'rxjs/operators';
import { Observable, Subscription } from 'rxjs';

import * as SharedEvents from '../../constants/sharedEvents';
// Services
import { FboairportsService } from '../../services/fboairports.service';
import { FbopricesService } from '../../services/fboprices.service';
import { FbopreferencesService } from '../../services/fbopreferences.service';
import { PricingtemplatesService } from '../../services/pricingtemplates.service';
import { FbosService } from '../../services/fbos.service';
// Components
import { PricingExpiredNotificationComponent } from '../../shared/components/pricing-expired-notification/pricing-expired-notification.component';
import { customerGridClear } from '../../store/actions';
import { State } from '../../store/reducers';
import { SharedService } from '../shared-service';
import { ProceedConfirmationComponent } from '../../shared/components/proceed-confirmation/proceed-confirmation.component';
import { AgreementsAndDocumentsModalComponent } from 'src/app/shared/components/Agreements-and-documents-modal/Agreements-and-documents-modal.component';
import { DocumentService } from 'src/app/services/documents.service';
import { AccountType } from 'src/app/enums/user-role';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';

@Component({
    providers: [SharedService],
    selector: 'default-layout',
    styleUrls: ['../layouts.scss'],
    templateUrl: 'default.component.html',
})
export class DefaultLayoutComponent implements OnInit {
    @Input() openedSidebar: boolean;

    pageTitle: any;
    boxed: boolean;
    compress: boolean;
    menuStyle: string;
    layoutClasses: any;
    pricingTemplatesData: any[];
    retailSaf: number;
    costSaf: number;
    retailJetA: number;
    costJetA: number;
    effectiveToSaf: any;
    effectiveToJetA: any;
    timezone: string = "";
    enableJetA: boolean;
    enableSaf: boolean;

    hidePricesPanel: boolean;
    subscriptions: Subscription[] = [];

    public getScreenWidth: any;
    public getScreenHeight: any;

    isExpiredPricingDialogBlocked: boolean = true;

    constructor(
        private fboairportsService: FboairportsService,
        private sharedService: SharedService,
        private fboPricesService: FbopricesService,
        private fboPreferencesService: FbopreferencesService,
        private pricingTemplatesService: PricingtemplatesService,
        private expiredPricingDialog: MatDialog,
        private router: Router,
        private store: Store<State>,
        private templateDialog: MatDialog,
        private fbosService: FbosService,
        private documentService: DocumentService
    ) {
        this.openedSidebar = false;
        this.boxed = false;
        this.compress = false;
        this.menuStyle = 'style-3';

        sharedService.titleChanged$.subscribe((title: string) => {
            this.sharedService.title = title;
            setTimeout(() => (this.pageTitle = title), 100);
        });

        this.router.events
            .pipe(filter((event) => event instanceof NavigationStart))
            .subscribe((event: RouterEvent) => {
                if (!event.url.startsWith('/default-layout/customers')) {
                    this.store.dispatch(customerGridClear());
                }
            });
    }

    get isCsr() {
        return this.sharedService.currentUser.role === 5;
    }
    get isConductor() {
        return this.sharedService.currentUser.role === 3;
    }
    get isNotGroupAdmin() {
        return this.sharedService.currentUser.role !== 2 || (this.sharedService.currentUser.role == 2 && this.sharedService.currentUser.fboId > 0);
    }
    async ngOnInit() {
        if(this.isConductor) {
            this.isExpiredPricingDialogBlocked = false;
        }
        else{
            await this.openAgreementsAndDocumentsModal();
        }

        if (!this.isExpiredPricingDialogBlocked){
            this.triggerPrices();
        }

        this.sharedService.valueChanged$.subscribe((value: any) => {
            if (!this.canUserSeePricing()) {
                return;
            }
            if (value.message === SharedEvents.fboPricesUpdatedEvent) {
                this.costJetA = value.JetACost;
                this.retailJetA = value.JetARetail;
                this.costSaf = value.SafCost;
                this.retailSaf = value.SafRetail;
                this.effectiveToSaf = value.PriceExpirationSaf;
                this.effectiveToJetA = value.PriceExpirationJetA;
            }

            if (value.message === SharedEvents.fboProductPreferenceChangeEvent) {
                this.enableJetA = value.EnableJetA;
                this.enableSaf = value.EnableSaf;
            }
        });

        this.layoutClasses = this.getClasses();

        this.LogUserForAnalytics();

        this.getScreenWidth = window.innerWidth;
        this.getScreenHeight = window.innerHeight;

        if (!this.isSidebarInvisible() && this.getScreenWidth >= 768) this.sidebarState();
    }
    private triggerPrices(){
        var isConductorRefresh = true;
        this.sharedService.changeEmitted$.subscribe((message) => {
            if (!this.canUserSeePricing()) {
                return;
            }
            if (
                (message === SharedEvents.fboChangedEvent ||
                    message === SharedEvents.locationChangedEvent) &&
                this.sharedService.currentUser.fboId
            ) {
                isConductorRefresh = false;
                this.pricingTemplatesService
                    .getByFbo(
                        this.sharedService.currentUser.fboId,
                        this.sharedService.currentUser.groupId
                    )
                    .subscribe(
                        (data: any) => {
                            this.pricingTemplatesData = data;
                            if (this.canUserSeePricing()) {
                                this.loadPrices();
                            }
                        }
                    );
            }
        });

        if (this.canUserSeePricing() || isConductorRefresh) {
            this.pricingTemplatesService
                .getByFbo(
                    this.sharedService.currentUser.fboId,
                    this.sharedService.currentUser.groupId
                )
                .subscribe(
                    (data: any) => {
                        this.pricingTemplatesData = data;
                        this.loadPrices();
                    }
                );
        }
    }
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.getScreenWidth = window.innerWidth;
        this.getScreenHeight = window.innerHeight;
    }
    async openAgreementsAndDocumentsModal(){
        let data = await this.documentService
        .getDocumentsToAccept(
            this.sharedService.currentUser.oid,
            this.sharedService.currentUser.groupId
        ).toPromise();
        if(!data.hasPendingDocumentsToAccept){
            this.isExpiredPricingDialogBlocked = false;
            return;
        }

        const config: MatDialogConfig = {
            disableClose: true,
            data: {
                userId: data.userId,
                eulaDocument: data.documentToAccept
             }
          };
        const dialogRef = this.templateDialog.open(
            AgreementsAndDocumentsModalComponent,
            config
        );

        dialogRef.afterClosed().subscribe(result => {
            if(result)
            this.triggerPrices();
        });
    }
    isPricePanelVisible() {
        const blacklist = [
            '/default-layout/groups',
            '/default-layout/fbos',
            '/default-layout/group-analytics',
            '/default-layout/fbo-geofencing',
            '/default-layout/group-customers',
            '/default-layout/about-fbolinx'
        ];
        if (
            blacklist.findIndex((v) =>
                window.location.pathname.startsWith(v)
            ) >= 0
        ) {
            return false;
        }
        return !this.isCsr;
    }
    isAdditionNavBarVisible() {
        const blacklist = [
            '/default-layout/about-fbolinx'
        ];
        if (
            blacklist.findIndex((v) =>
                window.location.pathname.startsWith(v)
            ) >= 0
        ) {
            return false;
        }
        return true;
    }
    getClasses() {
        const menu: string = this.menuStyle;

        return {
            ['menu-' + menu]: menu,
            boxed: this.boxed,
            'compress-vertical-navbar': this.compress,
            'open-sidebar': this.openedSidebar,
            rtl: false,
        };
    }

    sidebarState() {
        this.openedSidebar = !this.openedSidebar;
    }

    checkCurrentPrices() {
        const currentRoute = this.router.url;

        if(this.sharedService.currentUser.accountType == AccountType.Freemium) return;

        if (currentRoute == '/default-layout/groups') return;

        if (!this.sharedService.currentUser.fboId) return;

        const remindMeLaterFlag = localStorage.getItem(
            'pricingExpiredNotification'
        );
        const noThanksFlag = sessionStorage.getItem(
            'pricingExpiredNotification'
        );
        if (noThanksFlag) {
            return;
        }

        if (
            remindMeLaterFlag &&
            moment(new Date(moment().format('L'))) <=
            moment(new Date(remindMeLaterFlag))
        ) return;

        return new Observable((observer) => {
            this.subscriptions.push(
                this.fboPricesService
                    .checkFboExpiredPricing(this.sharedService.currentUser.fboId)
                    .subscribe((data: any) => {
                        if (this.sharedService.currentUser.role != 6 && this.sharedService.currentUser.fboId > 0 && !data) {
                            const dialogRef = this.expiredPricingDialog.open(
                                PricingExpiredNotificationComponent,
                                {
                                    autoFocus: false,
                                    data: {},
                                }
                            );
                            dialogRef.afterClosed().subscribe();
                        }
                    },
                        (error: any) => {
                            observer.error(error);
                        }
                    ))
        });
    }
    isSidebarInvisible() {
        return (
            !this.sharedService.currentUser.role &&
            !this.sharedService.currentUser.impersonatedRole
        );
    }

    public onClearFboPrice(event): void {
        const dialogRef = this.templateDialog.open(
            ProceedConfirmationComponent,
            {
                autoFocus: false,
                data: {
                    buttonText: 'Yes',
                    description:
                        "This will unpublish all associated pricing for this product",
                    title: 'Are you sure you want to clear your pricing?',
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.fboPricesService.removePricing(this.sharedService.currentUser.fboId, event)
                .subscribe((data: any) => {
                    if (event === 'SAF') {
                        this.costSaf = 0;
                        this.retailSaf = 0;
                    }
                    else if (event === 'JetA') {
                        this.costJetA = 0;
                        this.retailJetA = 0;
                    }

                    this.sharedService.emitChange('fbo prices cleared');
                    this.sharedService.valueChange({
                        message: SharedEvents.fboPricesClearedEvent,
                    });
                });
        });
    }

    private loadFboPreferences() {
        return new Observable((observer) => {
            this.subscriptions.push(
                this.fboPreferencesService.getForFbo(this.sharedService.currentUser.fboId).subscribe((preferences: any) => {
                    if (preferences.enableJetA)
                        this.enableJetA = true;
                    if (preferences.enableSaf)
                        this.enableSaf = true;

                    observer.next();
                },
                    (error: any) => {
                        observer.error(error);
                    }
                ));
        });
    }

    private loadFboPrices() {
        return new Observable((observer) => {
            this.subscriptions.push(
                this.fboairportsService.getLocalTimeZone(this.sharedService.currentUser.fboId).subscribe((timezone: any) => {
                    this.timezone = timezone;

                    var _this = this;
                    this.fboPricesService
                        .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
                        .subscribe((data: any) => {
                            for (const price of data) {
                                if (price.product === 'SAF Cost') {
                                    _this.costSaf = price.price;
                                }
                                if (price.product === 'SAF Retail') {
                                    _this.retailSaf = price.price;
                                    if (moment(price.effectiveTo).format("M/D/YY") == "12/31/99" || price.source == "1")
                                        _this.effectiveToSaf = "Updated via " + (price.integrationPartner == "" ? "PoS" : price.integrationPartner) + " Integration";
                                    else
                                        _this.effectiveToSaf = "Expires " + moment(price.effectiveTo).format("M/D/YY @ HH:mm") + " " + this.timezone;
                                }
                                if (price.product === 'JetA Cost') {
                                    _this.costJetA = price.price;
                                }
                                if (price.product === 'JetA Retail') {
                                    _this.retailJetA = price.price;
                                    if (moment(price.effectiveTo).format("M/D/YY") == "12/31/99" || price.source == "1")
                                        _this.effectiveToJetA = "Updated via " + (price.integrationPartner == "" ? "PoS" : price.integrationPartner) + " Integration";
                                    else
                                        _this.effectiveToJetA = "Expires " + moment(price.effectiveTo).format("M/D/YY @ HH:mm") + " " + this.timezone;
                                }
                            }

                            observer.next();
                        },
                            (error: any) => {
                                observer.error(error);
                            }
                        )
                }))
        });
    }

    private loadPrices() {
        this.subscriptions.push(
            this.loadFboPreferences().subscribe(() => {
                this.subscriptions.push(
                    this.loadFboPrices().subscribe(() => {
                        var checkPricesResult = this.checkCurrentPrices();
                        if (!checkPricesResult)
                            return;
                        this.subscriptions.push(
                            this.checkCurrentPrices().subscribe(() => {
                            }))
                    }))
            })
        );
    }

    private canUserSeePricing(): boolean {
        return this.sharedService.currentUser == null ? false : (
            [1, 4].includes(this.sharedService.currentUser.role) ||
            [1, 4].includes(this.sharedService.currentUser.impersonatedRole)
        );
    }

    private LogUserForAnalytics() {
        //Log data about the user for Lucky Orange analytics
        if (!this.sharedService.currentUser || !this.sharedService.currentUser.username || this.sharedService.currentUser.username == '')
            return;

        if (this.sharedService.currentUser.fboId > 0) {
            this.fbosService
                .getByFboId(
                    this.sharedService.currentUser.fboId
                )
                .subscribe(
                    (data: any) => {
                        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.accountType, data.accountType);
                        this.sharedService.emitChange(SharedEvents.accountTypeChangedEvent);

                        var fbo = data;
                        this.fboairportsService
                            .getForFbo(
                                fbo
                            )
                            .subscribe(
                                (result: any) => {
                                    this.LogAnalytics(fbo.fbo, result.icao);
                                }
                        );
                    }
                );
        }
        else
            this.LogAnalytics("", "");
    }

    private LogAnalytics(fbo, icao) {
        var userData = {
            "Name": (!this.sharedService.currentUser.firstName ? '' : this.sharedService.currentUser.firstName) + ' ' + (!this.sharedService.currentUser.lastName ? '' : this.sharedService.currentUser.lastName),
            "UserName": this.sharedService.currentUser.username,
            "FBO": fbo,
            "ICAO": icao
        };

        window._loq = window._loq || []
        window._loq.push(['custom', userData]);
    }
}
