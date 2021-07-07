import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import * as _ from 'lodash';

// Services
import { UserService } from '../../../services/user.service';
import { AuthenticationService } from '../../../services/authentication.service';
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { FbosService } from '../../../services/fbos.service';

import * as SharedEvents from '../../../models/sharedEvents';
import { customerUpdatedEvent, fboChangedEvent } from '../../../models/sharedEvents';

// Components
import { AccountProfileComponent } from '../../../shared/components/account-profile/account-profile.component';
import { WindowRef } from '../../../shared/components/zoho-chat/WindowRef';

@Component({
    selector: 'app-horizontal-navbar',
    templateUrl: 'horizontal-navbar.component.html',
    styleUrls: [ 'horizontal-navbar.component.scss' ],
    host: {
        '[class.app-navbar]': 'true',
        '[class.show-overlay]': 'showOverlay',
    },
    providers: [ WindowRef ],
})
export class HorizontalNavbarComponent implements OnInit, OnDestroy {
    @Input() title: string;
    @Input() openedSidebar: boolean;
    @Output() sidebarState = new EventEmitter();
    showOverlay: boolean;
    isOpened: boolean;
    isLocationsLoading: boolean;

    window: any;

    userFullName: string;
    accountProfileMenu: any = {
        isOpened: false
    };
    needsAttentionMenu: any = {
        isOpened: false
    };
    currrentJetACostPricing: any;
    currrentJetARetailPricing: any;
    hasLoadedJetACost = false;
    hasLoadedJetARetail = false;
    viewAllNotifications = false;
    locations: any[];
    fboAirport: any;
    fbo: any;
    currentUser: any;
    needsAttentionCustomersData: any[];
    subscription: any;

    constructor(
        private authenticationService: AuthenticationService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
        private router: Router,
        private route: ActivatedRoute,
        private accountProfileDialog: MatDialog,
        private userService: UserService,
        private fboPricesService: FbopricesService,
        private fboAirportsService: FboairportsService,
        private fbosService: FbosService,
        private winRef: WindowRef
    ) {
        this.openedSidebar = false;
        this.showOverlay = false;
        this.isOpened = false;
        this.currentUser = this.sharedService.currentUser;

        // getting the native window obj
        this.window = this.winRef.nativeWindow;

        if (!this.currentUser) {
            return;
        }
        this.userFullName =
            this.currentUser.firstName + ' ' + this.currentUser.lastName;
        if (this.userFullName.length < 2) {
            this.userFullName = this.currentUser.username;
        }
    }

    get notificationVisible() {
        return this.sharedService.currentUser.fboId > 0 && this.sharedService.currentUser.role !== 5;
    }

    ngOnInit() {
        if (this.canUserSeePricing()) {
            this.loadCurrentPrices();
            this.loadLocations();
            this.loadFboInfo();
            this.loadNeedsAttentionCustomers();
        }

        this.subscription = this.sharedService.changeEmitted$.subscribe((message) => {
            if (!this.canUserSeePricing())
                return;
            if (message === fboChangedEvent) {
                this.loadLocations();
                this.loadFboInfo();
                this.loadNeedsAttentionCustomers();
            }
            if (message === customerUpdatedEvent) {
                this.loadNeedsAttentionCustomers();
            }
        });
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    toggle(event) {
        if (this.isOpened) {
            this.close();
        } else {
            this.open(event);
        }
    }

    open(event) {
        const clickedComponent = event.target.closest('.nav-item');
        const items = clickedComponent.parentElement.children;

        event.preventDefault();

        for (const item of items) {
            item.classList.remove('opened');
        }
        clickedComponent.classList.add('opened');

        this.isOpened = true;
    }

    openUpdate() {
        this.needsAttentionMenu.isOpened = !this.needsAttentionMenu.isOpened;
    }

    close() {
        this.accountProfileMenu.isOpened = false;
        this.needsAttentionMenu.isOpened = false;
        this.isOpened = false;
    }

    openSidebar() {
        this.openedSidebar = !this.openedSidebar;
        this.sidebarState.emit();
    }

    logout() {
        localStorage.removeItem('impersonatedrole');
        localStorage.removeItem('fboId');
        localStorage.removeItem('managerGroupId');
        localStorage.removeItem('groupId');
        localStorage.removeItem('conductorFbo');
        this.authenticationService.logout();
        this.router.navigate([ '/landing-site-layout' ]);
    }

    accountProfileClicked() {
        this.userService.getCurrentUser().subscribe((response: any) => {
            const dialogRef = this.accountProfileDialog.open(
                AccountProfileComponent, {
                    height: '550px',
                    width: '1000px',
                    data: response,
                }
            );
            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }
                this.userService.update(result).subscribe(() => {
                    this.userService
                        .updatePassword({
                            user: result,
                            newPassword: result.newPassword,
                        })
                        .subscribe((newPass: any) => {
                            result.password = newPass;
                        });
                });
            });
        });
        this.close();
    }

    stopManagingFBOClicked() {
        localStorage.removeItem('fboId');
        localStorage.removeItem('managerGroupId');
        localStorage.removeItem('impersonatedrole');
        this.sharedService.currentUser.fboId = 0;
        this.sharedService.currentUser.impersonatedRole = null;
        if (this.sharedService.currentUser.managerGroupId && this.sharedService.currentUser.managerGroupId > 0) {
            localStorage.setItem('groupId', this.sharedService.currentUser.managerGroupId.toString());
            this.sharedService.currentUser.groupId = this.sharedService.currentUser.managerGroupId;
        } else {
            localStorage.removeItem('groupId');
        }
        this.locations = [];
        this.fboAirport = null;
        this.fbo = null;
        this.close();
        if (this.sharedService.currentUser.conductorFbo) {
            localStorage.removeItem('conductorFbo');
            this.sharedService.currentUser.conductorFbo = false;
            this.router.navigate([ '/default-layout/groups/' ]);
        } else {
            if (this.sharedService.currentUser.role === 3) {
                this.sharedService.currentUser.impersonatedRole = 2;
                localStorage.setItem('impersonatedrole', '2');
            }
            this.router.navigate([ '/default-layout/fbos/' ]);
        }
    }

    stopManagingGroupClicked() {
        this.sharedService.currentUser.impersonatedRole = null;
        localStorage.removeItem('impersonatedrole');
        localStorage.removeItem('fboId');
        localStorage.removeItem('managerGroupId');
        localStorage.removeItem('groupId');
        this.sharedService.currentUser.fboId = 0;
        if (this.sharedService.currentUser.managerGroupId && this.sharedService.currentUser.managerGroupId > 0) {
            this.sharedService.currentUser.groupId = this.sharedService.currentUser.managerGroupId;
        }
        this.locations = [];
        this.fboAirport = null;
        this.fbo = null;
        this.close();

        this.router.navigate([ '/default-layout/groups/' ]);
    }

    updatePricingClicked() {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate([ '/default-layout/dashboard-fbo' ]);
        this.close();
    }

    gotoCustomer(customer: any) {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate([ '/default-layout/customers/' + customer.oid ]);
        this.close();
    }

    viewAllNotificationsClicked() {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate([ '/default-layout/customers' ]);
        this.close();
    }

    showLessClicked() {
        this.viewAllNotifications = false;
    }

    loadCurrentPrices() {
        this.fboPricesService
            .getFbopricesByFboIdAndProductCurrent(
                this.currentUser.fboId,
                'JetA Cost'
            )
            .subscribe((data: any) => {
                this.currrentJetACostPricing = data;
                this.hasLoadedJetACost = true;
            });

        this.fboPricesService
            .getFbopricesByFboIdAndProductCurrent(
                this.currentUser.fboId,
                'JetA Retail'
            )
            .subscribe((data: any) => {
                this.currrentJetARetailPricing = data;
                this.hasLoadedJetARetail = true;
            });
    }

    loadLocations() {
        if (!this.currentUser.groupId) {
            return;
        }
        this.isLocationsLoading = true;
        this.fbosService.getForGroup(this.currentUser.groupId).subscribe(
            (data: any) => {
                if (data && data.length) {
                    this.locations = _.cloneDeep(data);
                }
                this.isLocationsLoading = false;
            },
            (error: any) => {
                this.isLocationsLoading = false;
                console.log(error);
            }
        );
    }

    loadFboInfo() {
        if (!this.currentUser.fboId && !localStorage.getItem('fboId')) {
            return;
        }

        if (!this.currentUser.fboId) {
            this.currentUser.fboId = localStorage.getItem('fboId');
        }

        this.fboAirportsService
            .getForFbo({
                oid: this.currentUser.fboId
            })
            .subscribe(
                (data: any) => {
                    this.fboAirport = _.assign({}, data);
                    this.sharedService.currentUser.icao = this.fboAirport.icao;
                    this.sharedService.emitChange(SharedEvents.icaoChangedEvent);
                },
                (error: any) => {
                    console.log(error);
                }
            );
        this.fbosService.get({
            oid: this.currentUser.fboId
        }).subscribe(
            (data: any) => {
                this.fbo = _.assign({}, data);
            },
            (error: any) => {
                console.log(error);
            }
        );
    }

    changeLocation(location: any) {
        this.isOpened = false;
        this.fboAirport.iata = location.iata;
        this.fboAirport.icao = location.icao;
        this.fboAirport.fboid = location.oid;
        this.accountProfileMenu.isOpened = false;
        this.needsAttentionMenu.isOpened = false;
        this.sharedService.currentUser.fboId = this.fboAirport.fboid;
        this.loadFboInfo();
        localStorage.setItem('fboId', this.sharedService.currentUser.fboId.toString());
        if (this.isOnDashboard()) {
            this.sharedService.emitChange(SharedEvents.locationChangedEvent);
        } else {
            this.router.navigate([ '/default-layout/dashboard/' ]).then();
        }
    }

    toggleProfileMenu() {
        if (this.isLocationsLoading) {
            return;
        }
        this.accountProfileMenu.isOpened = !this.accountProfileMenu.isOpened;
    }

    loadNeedsAttentionCustomers() {
        if (this.currentUser.fboId) {
            this.customerInfoByGroupService
                .getNeedsAttentionByGroupAndFbo(this.currentUser.groupId, this.currentUser.fboId)
                .subscribe((data: any) => {
                    this.needsAttentionCustomersData = data;
                });
        }
    }

    showWidget() {
        if (this.window) {
            this.window.$zoho.salesiq.floatwindow.visible('show');
        }
    }

    hideWidget() {
        if (this.window) {
            this.window.$zoho.salesiq.floatwindow.visible('hide');
        }
    }

    // Private Methods
    private isOnDashboard(): boolean {
        if (!this.route || !this.route.url) {
            return false;
        }
        const urlInfo: any = !this.route.url;
        if (!urlInfo.value) {
            return false;
        }
        const dashboardResults = urlInfo.value.filter(value => value && value.toLowerCase().indexOf('dashboard') > -1);
        if (!dashboardResults || dashboardResults.length === 0) {
            return false;
        }
        return true;
    }

    private canUserSeePricing(): boolean {
        return ([1, 4].includes(this.sharedService.currentUser.role) ||
            [1, 4].includes(this.sharedService.currentUser.impersonatedRole));
    }
}
