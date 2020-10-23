import {
  Component,
  OnInit,
  AfterViewInit,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
} from '@angular/core';
import { Router } from '@angular/router';
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

// Components
import { AccountProfileComponent } from '../../../shared/components/account-profile/account-profile.component';
import { WindowRef } from '../../../shared/components/zoho-chat/WindowRef';
import { fboChangedEvent, customerUpdatedEvent } from '../../../models/sharedEvents';

@Component({
  selector: 'horizontal-navbar',
  templateUrl: 'horizontal-navbar.component.html',
  styleUrls: ['horizontal-navbar.component.scss'],
  host: {
    '[class.app-navbar]': 'true',
    '[class.show-overlay]': 'showOverlay',
  },
  providers: [WindowRef],
})
export class HorizontalNavbarComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() title: string;
  @Input() openedSidebar: boolean;
  @Output() sidebarState = new EventEmitter();
  showOverlay: boolean;
  isOpened: boolean;
  isLocationsLoaded: boolean;

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

  ngOnInit() {
    this.loadCurrentPrices();
    this.loadLocations();
    this.loadFboInfo();
    this.loadNeedsAttentionCustomers();
  }

  ngAfterViewInit() {
    this.subscription = this.sharedService.changeEmitted$.subscribe((message) => {
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
    sessionStorage.removeItem('impersonatedrole');
    sessionStorage.removeItem('fboId');
    sessionStorage.removeItem('managerGroupId');
    sessionStorage.removeItem('groupId');
    sessionStorage.removeItem('conductorFbo');
    this.authenticationService.logout();
    this.router.navigate(['/landing-site-layout']);
  }

  accountProfileClicked() {
    this.userService.getCurrentUser().subscribe((response: any) => {
      const dialogRef = this.accountProfileDialog.open(
        AccountProfileComponent, {
          height: '550px',
          width: '850px',
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
    sessionStorage.removeItem('fboId');
    sessionStorage.removeItem('managerGroupId');
    sessionStorage.removeItem('impersonatedrole');
    this.sharedService.currentUser.fboId = 0;
    this.sharedService.currentUser.impersonatedRole = null;
    if (this.sharedService.currentUser.managerGroupId && this.sharedService.currentUser.managerGroupId > 0) {
      this.sharedService.currentUser.groupId = this.sharedService.currentUser.managerGroupId;
    } else {
      sessionStorage.removeItem('groupId');
    }
    this.locations = [];
    this.fboAirport = null;
    this.fbo = null;
    this.close();
    if (this.sharedService.currentUser.conductorFbo) {
      sessionStorage.removeItem('conductorFbo');
      this.sharedService.currentUser.conductorFbo = false;
      this.router.navigate(['/default-layout/groups/']);
    } else {
      if (this.sharedService.currentUser.role === 3) {
        this.sharedService.currentUser.impersonatedRole = 2;
        sessionStorage.setItem('impersonatedrole', '2');
      }
      this.router.navigate(['/default-layout/fbos/']);
    }
  }

  stopManagingGroupClicked() {
    this.sharedService.currentUser.impersonatedRole = null;
    sessionStorage.removeItem('impersonatedrole');
    sessionStorage.removeItem('fboId');
    sessionStorage.removeItem('managerGroupId');
    sessionStorage.removeItem('groupId');
    this.sharedService.currentUser.fboId = 0;
    if (this.sharedService.currentUser.managerGroupId && this.sharedService.currentUser.managerGroupId > 0) {
      this.sharedService.currentUser.groupId = this.sharedService.currentUser.managerGroupId;
    }
    this.locations = [];
    this.fboAirport = null;
    this.fbo = null;
    this.close();

    this.router.navigate(['/default-layout/groups/']);
  }

  updatePricingClicked() {
    this.needsAttentionMenu.isOpened = false;
    this.router.navigate(['/default-layout/dashboard-fbo']);
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

  loadFboInfo() {
    if (!this.currentUser.fboId && !sessionStorage.getItem('fboId')) {
      return;
    }

    if (!this.currentUser.fboId) {
      this.currentUser.fboId = sessionStorage.getItem('fboId');
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
    sessionStorage.setItem('fboId', this.sharedService.currentUser.fboId.toString());
    this.sharedService.emitChange(SharedEvents.locationChangedEvent);
  }

  toggleProfileMenu() {
    if (!this.isLocationsLoaded) {
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

  get notificationVisible() {
    return this.sharedService.currentUser.fboId > 0 && this.sharedService.currentUser.role !== 5;
  }
}
