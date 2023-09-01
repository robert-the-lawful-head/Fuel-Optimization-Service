import {
    Component,
    EventEmitter,
    Input,
    OnDestroy,
    OnInit,
    Output,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription, timer } from 'rxjs';
import * as _ from 'lodash';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import {
    customerUpdatedEvent,
    fboChangedEvent,
} from '../../../models/sharedEvents';
import { AuthenticationService } from '../../../services/authentication.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FbosService } from '../../../services/fbos.service';
import { FuelreqsService } from '../../../services/fuelreqs.service';
// Services
import { UserService } from '../../../services/user.service';
// Components
import { AccountProfileComponent } from '../../../shared/components/account-profile/account-profile.component';
import { WindowRef } from '../../../shared/components/zoho-chat/WindowRef';

import * as moment from 'moment';
import { UserRole } from 'src/app/enums/user-role';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';
import { ApiResponseWraper } from 'src/app/models/apiResponseWraper';
import { FlightWatchService } from 'src/app/services/flightwatch.service';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { FlightLegStatus } from 'src/app/enums/flight-watch.enum';

@Component({
    host: {
        '[class.app-navbar]': 'true',
        '[class.show-overlay]': 'showOverlay',
    },
    providers: [WindowRef],
    selector: 'app-horizontal-navbar',
    styleUrls: ['horizontal-navbar.component.scss'],
    templateUrl: 'horizontal-navbar.component.html',
})
export class HorizontalNavbarComponent implements OnInit, OnDestroy {
    @Input() title: string;
    @Input() openedSidebar: boolean;
    @Input() isPublicView: boolean = false;
    @Output() sidebarState = new EventEmitter();

    showOverlay: boolean;
    isOpened: boolean;
    isLocationsLoading: boolean;

    window: any;

    fuelOrdersSubscription: Subscription;
    userFullName: string;
    accountProfileMenu: any = {
        isOpened: false,
    };
    needsAttentionMenu: any = {
        isOpened: false,
    };
    fuelOrderNotificationsMenu: any = {
        isOpened: false
    };
    favoriteAircrafts: any = {
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
    fuelOrders: any[] = [];

    mapLoadSubscription: Subscription;
    selectedICAO: string = "";
    airportWatchFetchSubscription: Subscription;
    favoriteAircraftsData : FlightWatchModelResponse[];
    dismissedFavoriteAircrafts : FlightWatchModelResponse[] = [];

    constructor(
        private authenticationService: AuthenticationService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
        private router: Router,
        private accountProfileDialog: MatDialog,
        private userService: UserService,
        private fboPricesService: FbopricesService,
        private fboAirportsService: FboairportsService,
        private fbosService: FbosService,
        private fuelReqsService: FuelreqsService,
        private winRef: WindowRef,
        private Location: Location,
        private flightWatchService: FlightWatchService,
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
        return (
            this.sharedService.currentUser.fboId > 0 &&
            this.sharedService.currentUser.role !== 5
        );
    }

    ngOnInit() {
        if (this.canUserSeePricing()) {
            this.loadCurrentPrices();
            this.loadLocations();
            this.loadFboInfo();
            this.loadNeedsAttentionCustomers();
        }

        this.subscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (!this.canUserSeePricing()) {
                    this.fuelOrders.length = 0;
                    return;
                }
                if (message === fboChangedEvent) {
                    this.loadLocations();
                    this.loadFboInfo();
                    this.loadNeedsAttentionCustomers();
                    this.loadUpcomingOrders();
                }
                if (message === customerUpdatedEvent) {
                    this.loadNeedsAttentionCustomers();
                }
                if (message === SharedEvents.locationChangedEvent) {
                    this.loadAirportWatchData();
                }else if(message == SharedEvents.icaoChangedEvent){
                    this.selectedICAO = this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao);
                }
            }
        );

        this.fuelOrdersSubscription = timer(0, 120000).subscribe(() =>
            this.loadUpcomingOrders()
        );

        this.mapLoadSubscription = timer(0, 15000).subscribe(() =>{
            if(this.selectedICAO)
                this.loadAirportWatchData();
        });
        this.selectedICAO = this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao);

        this.dismissedFavoriteAircrafts = JSON.parse(localStorage.getItem(localStorageAccessConstant.dismissedFavoriteAircrafts)) ?? [];
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
        if (this.fuelOrdersSubscription) {
            this.fuelOrdersSubscription.unsubscribe();
        }
        if (this.mapLoadSubscription) this.mapLoadSubscription.unsubscribe();
        if (this.airportWatchFetchSubscription) this.airportWatchFetchSubscription

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
        if(this.sharedService.currentUser.role == 6) return;
        this.needsAttentionMenu.isOpened = !this.needsAttentionMenu.isOpened;
    }

    openfavoriteAircrafts() {
        this.favoriteAircrafts.isOpened = !this.favoriteAircrafts.isOpened;
    }

    openFuelOrdersUpdate() {
        this.fuelOrderNotificationsMenu.isOpened = !this.fuelOrderNotificationsMenu.isOpened;
    }

    close() {
        this.accountProfileMenu.isOpened = false;
        this.needsAttentionMenu.isOpened = false;
        this.fuelOrderNotificationsMenu.isOpened = false;
        this.favoriteAircrafts.isOpened = false;
        this.isOpened = false;
    }

    openSidebar() {
        this.openedSidebar = !this.openedSidebar;
        this.sidebarState.emit();
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/landing-site-layout']);
    }

    accountProfileClicked() {
        this.userService.getCurrentUser().subscribe((response: any) => {
            const dialogRef = this.accountProfileDialog.open(
                AccountProfileComponent,
                {
                    data: response,
                    height: '550px',
                    width: '1000px',
                }
            );

            dialogRef.componentInstance.productChanged.subscribe(result => {
                this.sharedService.emitChange(SharedEvents.fboProductPreferenceChangeEvent);
                this.sharedService.valueChange({
                    EnableJetA: result.enableJetA,
                    EnableSaf: result.enableSaf,
                    message: SharedEvents.fboProductPreferenceChangeEvent,
                });
            });

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }
                this.userService.update(result).subscribe(() => {
                    this.userService
                        .updatePassword({
                            newPassword: result.newPassword,
                            user: result,
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
        localStorage.removeItem(localStorageAccessConstant.fboId);
        localStorage.removeItem(localStorageAccessConstant.managerGroupId);
        localStorage.removeItem(localStorageAccessConstant.impersonatedrole);
        this.sharedService.currentUser.fboId = 0;
        this.sharedService.currentUser.impersonatedRole = null;
        if (
            this.sharedService.currentUser.managerGroupId &&
            this.sharedService.currentUser.managerGroupId > 0
        ) {
            localStorage.setItem(
                'groupId',
                this.sharedService.currentUser.managerGroupId.toString()
            );
            this.sharedService.currentUser.groupId =
                this.sharedService.currentUser.managerGroupId;
        } else {
            localStorage.removeItem(localStorageAccessConstant.groupId);
        }
        this.locations = [];
        this.fboAirport = null;
        this.fbo = null;
        this.sharedService.emitChange(fboChangedEvent);
        this.close();
        if (this.sharedService.currentUser.conductorFbo) {
            localStorage.removeItem(localStorageAccessConstant.conductorFbo);
            this.sharedService.currentUser.conductorFbo = false;
            this.router.navigate(['/default-layout/groups/']);
        } else {
            if (this.sharedService.currentUser.role === 3) {
                this.sharedService.currentUser.impersonatedRole = 2;
                localStorage.setItem(localStorageAccessConstant.impersonatedrole, '2');
            }
            this.router.navigate(['/default-layout/fbos/']);
        }
    }

    stopManagingGroupClicked() {
        this.sharedService.currentUser.impersonatedRole = null;
        localStorage.removeItem(localStorageAccessConstant.impersonatedrole);
        localStorage.removeItem(localStorageAccessConstant.fboId);
        localStorage.removeItem(localStorageAccessConstant.managerGroupId);
        localStorage.removeItem(localStorageAccessConstant.groupId);
        this.sharedService.currentUser.fboId = 0;
        if (
            this.sharedService.currentUser.managerGroupId &&
            this.sharedService.currentUser.managerGroupId > 0
        ) {
            this.sharedService.currentUser.groupId =
                this.sharedService.currentUser.managerGroupId;
        }
        this.locations = [];
        this.fboAirport = null;
        this.fbo = null;
        this.close();

        this.router.navigate(['/default-layout/groups/']);
    }

    updatePricingClicked() {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(['/default-layout/dashboard-fbo-updated']);
        this.close();
    }

    gotoCustomer(customer: any) {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(['/default-layout/customers/' + customer.oid]);
        this.close();
    }

    viewAllNotificationsClicked() {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(['/default-layout/customers']);
        this.close();
    }

    viewAllFuelOrdersClicked() {
        this.fuelOrderNotificationsMenu.isOpened = false;
        this.router.navigate(['/default-layout/fuelreqs']);
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
        if (!this.currentUser.fboId && !localStorage.getItem(localStorageAccessConstant.fboId)) {
            return;
        }

        if (!this.currentUser.fboId) {
            this.currentUser.fboId = localStorage.getItem(localStorageAccessConstant.fboId);
        }

        this.fboAirportsService
            .getForFbo({
                oid: this.currentUser.fboId,
            })
            .subscribe(
                (data: any) => {
                    this.fboAirport = _.assign({}, data);
                    this.sharedService.setLocationStorageValues(this.fboAirport.icao);
                    this.sharedService.emitChange(
                        SharedEvents.icaoChangedEvent
                    );
                },
                (error: any) => {
                    console.log(error);
                }
            );
        this.fbosService
            .get({
                oid: this.currentUser.fboId,
            })
            .subscribe(
                (data: any) => {
                    this.fbo = _.assign({}, data);
                    localStorage.setItem(localStorageAccessConstant.fbo, this.fbo.fbo);

                    if (this.sharedService.currentUser.role != 3) {
                        this.fbosService.updateLastLogin(this.currentUser.fboId).subscribe((data: any) => {
                        });
                    }
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
        localStorage.setItem(localStorageAccessConstant.fboId,this.sharedService.currentUser.fboId.toString());

        this.sharedService.setLocationStorageValues(location.icao);

        this.fbosService.manageFbo(this.sharedService.currentUser.fboId).subscribe(() => {
            if (this.isOnDashboard())
                this.sharedService.emitChange(SharedEvents.locationChangedEvent);
            else
                this.router.navigate(['/default-layout/dashboard-fbo-updated/']).then();
        });
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
                .getNeedsAttentionByGroupAndFbo(
                    this.currentUser.groupId,
                    this.currentUser.fboId
                )
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

    loadUpcomingOrders() {

        this.fuelOrders.length = 0;
        var startDate = moment().add(-1, 'hour').local().toDate();
        var endDate = moment().add(2, 'd').local().toDate();

        this.fuelReqsService
            .getForGroupFboAndDateRange(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                startDate,
                endDate
            )
            .subscribe((data: any) => {
                if (data && data != null) {
                    data.forEach(x => {
                        if (!x || x.cancelled)
                            return;
                        if (x.timeStandard != null && x.timeStandard.toLowerCase() == 'z' ||
                            x.timeStandard == '0')
                            x.minutesUntilArrival =
                                moment.duration(moment(x.eta).diff(moment().utc())).asMinutes();
                        else
                            x.minutesUntilArrival =
                                moment.duration(moment(x.eta).diff(moment())).asMinutes();
                        x.minutesUntilArrival = Math.round(x.minutesUntilArrival);
                        x.hoursUntilArrival = Math.round(x.minutesUntilArrival / 60);
                        this.fuelOrders.push(x);
                    });
                }

                this.fuelOrders = this.fuelOrders.sort((x1, x2) => x1.minutesUntilArrival - x2.minutesUntilArrival);
            }, (error: any) => {

            });
    }
    public isLobbyViewVisible():boolean {
        return this.currentUser &&
        (this.currentUser.role ===  UserRole.Primary ||
            this.currentUser.role ===  UserRole.CSR ||
            this.currentUser.role ===  UserRole.Member ||
            this.currentUser.role ===  UserRole.GroupAdmin ||
            this.currentUser.role ===  UserRole.Conductor
        );
    }
    public get userRole(){
        return UserRole;
    }

    loadAirportWatchData() {
        return this.airportWatchFetchSubscription = this.flightWatchService
        .getAirportLiveData(
            this.sharedService.currentUser.fboId,
            this.selectedICAO
        )
        .subscribe((data: ApiResponseWraper<FlightWatchModelResponse[]>) => {
            if (data.success) {
                var filteredFavoriteAircrafts = data.result.filter(item => item.isCustomerManagerAircraft == true && item.favoriteAircraft != null && item.status == FlightLegStatus.EnRoute && item.etaLocal != null);
                this.favoriteAircraftsData =
                filteredFavoriteAircrafts?.filter(item =>
                    !this.dismissedFavoriteAircrafts.some(obj => obj.tailNumber == item.tailNumber)
                );
                this.dismissedFavoriteAircrafts = this.dismissedFavoriteAircrafts.filter(item =>
                    filteredFavoriteAircrafts.some(obj => obj.tailNumber == item.tailNumber)
                );
                localStorage.setItem(localStorageAccessConstant.dismissedFavoriteAircrafts, JSON.stringify(this.dismissedFavoriteAircrafts));

            }else{
                console.log("flight watch data: message", data.message);
            }
            this.sharedService.valueChange(
            {
                event: SharedEvents.flightWatchDataEvent,
                data: data.result ?? null,
            });
        }, (error: any) => {
            console.log("flight watch error:", error)
        });
    }
    removeFavoriteAircraft(flightwatch: FlightWatchModelResponse):void{
        this.dismissedFavoriteAircrafts.push(flightwatch);
        this.favoriteAircraftsData = this.favoriteAircraftsData.filter(item => item.tailNumber != flightwatch.tailNumber);
        localStorage.setItem(localStorageAccessConstant.dismissedFavoriteAircrafts, JSON.stringify(this.dismissedFavoriteAircrafts));
    }
    goToFlightWatch(flightwatch: FlightWatchModelResponse):void{
        this.sharedService.valueChange(
        {
            event: SharedEvents.flyToOnMapEvent,
            data: flightwatch,
        });
    }
    // Private Methods
    private isOnDashboard(): boolean {
        if (!this.Location) {
            return false;
        }

        const urlInfo: any = this.Location.path();
        const dashboardResults = urlInfo.toLowerCase().indexOf('dashboard') > -1 ? true : false;
        if (!dashboardResults) {
            return false;
        }
        return true;
    }

    private canUserSeePricing(): boolean {
        return (
            [1, 4].includes(this.sharedService.currentUser.role) ||
            [1, 4].includes(this.sharedService.currentUser.impersonatedRole)
        );
    }
}
