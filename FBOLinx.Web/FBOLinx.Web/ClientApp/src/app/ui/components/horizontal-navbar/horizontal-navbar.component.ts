import {
    Component,
    OnInit,
    AfterViewInit,
    Input,
    Output,
    EventEmitter,
    OnDestroy,
} from "@angular/core";
import { Router } from "@angular/router";
import { MatDialog } from "@angular/material/dialog";

import * as _ from "lodash";

// Services
import { UserService } from "../../../services/user.service";
import { AuthenticationService } from "../../../services/authentication.service";
import { SharedService } from "../../../layouts/shared-service";
import { CustomerinfobygroupService } from "../../../services/customerinfobygroup.service";
import { FbopricesService } from "../../../services/fboprices.service";
import { FboairportsService } from "../../../services/fboairports.service";
import { FbosService } from "../../../services/fbos.service";

import * as SharedEvents from "../../../models/sharedEvents";

// Components
import { AccountProfileComponent } from "../../../shared/components/account-profile/account-profile.component";
import { WindowRef } from "../../../shared/components/zoho-chat/WindowRef";
import { fboChangedEvent } from "../../../models/sharedEvents";

@Component({
    moduleId: module.id,
    selector: "horizontal-navbar",
    templateUrl: "horizontal-navbar.component.html",
    styleUrls: ["horizontal-navbar.component.scss"],
    host: {
        "[class.app-navbar]": "true",
        "[class.show-overlay]": "showOverlay",
    },
    providers: [WindowRef],
})
export class HorizontalNavbarComponent implements OnInit, AfterViewInit, OnDestroy {
    @Input()
    title: string;
    @Input()
    openedSidebar: boolean;
    @Output()
    sidebarState = new EventEmitter();
    showOverlay: boolean;
    isOpened: boolean;
    isLocationsLoaded: boolean;

    window: any;

    public userFullName: string;
    public accountProfileMenu: any = { isOpened: false };
    public needsAttentionMenu: any = { isOpened: false };
    public currrentJetACostPricing: any;
    public currrentJetARetailPricing: any;
    public hasLoadedJetACost = false;
    public hasLoadedJetARetail = false;
    public viewAllNotifications = false;
    public locations: any[];
    public fboAirport: any;
    public fbo: any;
    public currentUser: any;
    public needsAttentionCustomersData: any[];
    public subscription: any;

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
            this.currentUser.firstName + " " + this.currentUser.lastName;
        if (this.userFullName.length < 2) {
            this.userFullName = this.currentUser.username;
        }
    }

    ngOnInit() {
        this.loadCurrentPrices();
        this.loadLocations();
        this.loadFboInfo();
        this.loadNeedsAttentionCustomers();
        this.checkImpersonatedRole();
    }

    ngAfterViewInit() {
        this.subscription = this.sharedService.changeEmitted$.subscribe((message) => {
            if (message === fboChangedEvent) {
                this.loadLocations();
                this.loadFboInfo();
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
            this.close(event);
        } else {
            this.open(event);
        }
    }

    open(event) {
        const clickedComponent = event.target.closest(".nav-item");
        const items = clickedComponent.parentElement.children;

        event.preventDefault();

        for (const item of items) {
            item.classList.remove("opened");
        }
        clickedComponent.classList.add("opened");

        this.isOpened = true;
    }

    openUpdate() {
        this.needsAttentionMenu.isOpened = !this.needsAttentionMenu.isOpened;
    }

    close(event) {
        event.preventDefault();
        this.accountProfileMenu.isOpened = false;
        this.needsAttentionMenu.isOpened = false;
        this.isOpened = false;
    }

    openSidebar() {
        this.openedSidebar = !this.openedSidebar;
        this.sidebarState.emit();
    }

    public logout() {
        sessionStorage.removeItem("impersonatedrole");
        sessionStorage.removeItem("fboId");
        this.authenticationService.logout();
        this.router.navigate(["/landing-site-layout"]);
    }

    public accountProfileClicked(event) {
        this.userService.getCurrentUser().subscribe((response: any) => {
            const dialogRef = this.accountProfileDialog.open(
                AccountProfileComponent,
                {
                    height: "550px",
                    width: "850px",
                    data: response,
                }
            );
            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }
                this.userService.update(result).subscribe((data: any) => {
                    if (result.newPassword && result.newPassword !== "") {
                        this.userService
                            .updatePassword({
                                user: data,
                                newPassword: result.newPassword,
                            })
                            .subscribe((newPass: any) => {
                                result.password = newPass;
                            });
                    }
                });
            });
        });
        this.close(event);
    }

    public stopManagingClicked(event) {
        this.sharedService.currentUser.impersonatedRole = null;
        sessionStorage.removeItem("impersonatedrole");
        sessionStorage.removeItem("fboId");
        this.sharedService.currentUser.fboId = 0;
        this.locations = [];
        this.fboAirport = null;
        this.fbo = null;
        this.close(event);
        this.router.navigate(["/default-layout/fbos/"]);
    }

    public updatePricingClicked(event) {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(["/default-layout/dashboard-fbo"]);
        this.close(event);
    }

    public gotoCustomer(customer: any, event: any) {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(["/default-layout/customers/" + customer.oid]);
        this.close(event);
    }

    public viewAllNotificationsClicked(event) {
        this.needsAttentionMenu.isOpened = false;
        this.router.navigate(["/default-layout/customers"]);
        this.close(event);
    }

    public showLessClicked() {
        this.viewAllNotifications = false;
    }

    public loadCurrentPrices() {
        this.fboPricesService
            .getFbopricesByFboIdAndProductCurrent(
                this.currentUser.fboId,
                "JetA Cost"
            )
            .subscribe((data: any) => {
                this.currrentJetACostPricing = data;
                this.hasLoadedJetACost = true;
            });

        this.fboPricesService
            .getFbopricesByFboIdAndProductCurrent(
                this.currentUser.fboId,
                "JetA Retail"
            )
            .subscribe((data: any) => {
                this.currrentJetARetailPricing = data;
                this.hasLoadedJetARetail = true;
            });
    }

    public loadLocations() {
        if (!this.currentUser.groupId) {
            return;
        }
        this.fbosService.getForGroup(this.currentUser.groupId).subscribe(
            (data: any) => {
                this.isLocationsLoaded = true;
                if (data && data.length) {
                    this.locations = _.cloneDeep(data);
                }
            },
            (error: any) => {
                this.isLocationsLoaded = true;
                console.log(error);
            }
        );
    }

    public loadFboInfo() {
        if (!this.currentUser.fboId && !sessionStorage.getItem("fboId")) {
            return;
        }

        if (!this.currentUser.fboId) {
            this.currentUser.fboId = sessionStorage.getItem("fboId");
        }

        this.fboAirportsService
            .getForFbo({ oid: this.currentUser.fboId })
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
        this.fbosService.get({ oid: this.currentUser.fboId }).subscribe(
            (data: any) => {
                this.fbo = _.assign({}, data);
            },
            (error: any) => {
                console.log(error);
            }
        );
    }

    public checkImpersonatedRole() {
        if (!this.currentUser.fboId && !sessionStorage.getItem("fboId")) {
            return;
        }

        if (!this.currentUser.impersonatedRole) {
            if (sessionStorage.getItem("impersonatedrole")) {
                this.currentUser.impersonatedRole = 1;
            }
        }

    }

    public changeLocation(location: any) {
        this.isOpened = false;
        this.fboAirport.iata = location.iata;
        this.fboAirport.icao = location.icao;
        this.fboAirport.fboid = location.oid;
        this.accountProfileMenu.isOpened = false;
        this.needsAttentionMenu.isOpened = false;
        this.sharedService.currentUser.fboId = this.fboAirport.fboid;
        sessionStorage.setItem("fboId", this.sharedService.currentUser.fboId.toString());
        this.sharedService.emitChange(SharedEvents.locationChangedEvent);
    }

    public toggleProfileMenu() {
        if (!this.isLocationsLoaded) {
            return;
        }
        this.accountProfileMenu.isOpened = !this.accountProfileMenu.isOpened;
    }

    public loadNeedsAttentionCustomers() {
        this.customerInfoByGroupService
            .getNeedsAttentionByGroupAndFbo(this.currentUser.groupId, this.currentUser.fboId)
            .subscribe((data: any) => {
                this.needsAttentionCustomersData = data;
            });
    }

    showWidget() {
        if (this.window) {
            this.window.$zoho.salesiq.floatwindow.visible("show");
        }
    }

    hideWidget() {
        if (this.window) {
            this.window.$zoho.salesiq.floatwindow.visible("hide");
        }
    }
}
