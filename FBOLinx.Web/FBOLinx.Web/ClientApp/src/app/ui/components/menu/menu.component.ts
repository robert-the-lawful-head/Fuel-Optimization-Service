import {
    AfterViewInit,
    Component,
    OnInit,
    QueryList,
    ViewChildren,
} from '@angular/core';
import { Observable, Subscription } from 'rxjs';

import { SharedService } from '../../../layouts/shared-service';
import { ServiceOrderService } from 'src/app/services/serviceorder.service';
import { accountTypeChangedEvent, fboChangedEvent, fboPricesLoadedEvent, menuTooltipShowedEvent, serviceOrdersChangedEvent } from '../../../constants/sharedEvents';
import { UserService } from '../../../services/user.service';
import { MenuService } from './menu.service';
import { IMenuItem } from './menu-item';
import { ServiceOrder } from 'src/app/models/service-order';
import { EntityResponseMessage } from 'src/app/models/entity-response-message';

import * as moment from 'moment';

@Component({
    host: { class: 'app-menu' },
    providers: [MenuService],
    selector: 'app-menu',
    styleUrls: ['./menu.component.scss'],
    templateUrl: './menu.component.html',
})
export class MenuComponent implements OnInit, AfterViewInit {
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;

    menuItems: IMenuItem[];
    user: any;
    tooltipIndex = 0;
    hasShownTutorial = false;

    changeEmittedSubscription: Subscription;
    integrationStatus: boolean = false;

    constructor(
        private menuService: MenuService,
        private sharedService: SharedService,
        private userService: UserService,
        private serviceOrderService: ServiceOrderService,
    ) {}

    ngOnInit(): void {
        this.getMenuItems();

    }
    ngOnDestroy() {
        this.changeEmittedSubscription?.unsubscribe();
    }
    ngAfterViewInit(): void {
        this.changeEmittedSubscription = this.sharedService.changeEmitted$.subscribe((message) => {
            if (message === fboChangedEvent || message === accountTypeChangedEvent) {
                this.menuService.setMenuProps(this.menuItems);
            }
            if (message === fboPricesLoadedEvent) {
                this.showTooltipsIfFirstLogin();
                this.showPendingServiceOrders();
            }
            if (message === serviceOrdersChangedEvent) {
                this.showPendingServiceOrders();
            }
        });
    }

    getMenuItems(): void {
        const OBSERVER = {
            error: (err) => this.menuService.handleError(err),
            next: (x) => {
                this.menuItems = x;
                this.menuService.setMenuProps(this.menuItems);            
            },
        };
        this.menuService.getData().subscribe(OBSERVER);
    }

    getItemNgClass(item: any, isActive: boolean) {
        return {
          ...item.class,
          active: isActive
        };
      }

    isHidden(item: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole) {
            role = this.sharedService.currentUser.impersonatedRole;
        }
        return (item.title != 'Dashboard' && this.sharedService.currentUser.integrationStatus) || (item.roles && item.roles.indexOf(role) === -1);
    }

    getStyles(item: any) {
        return {
            background: item.bg,
            color: item.color,
        };
    }

    toggle(event: Event, item: any, el: any) {
        event.preventDefault();

        const items: any[] = el.menuItems;

        if (item.active) {
            item.active = false;
        } else {
            for (const i of items) {
                i.active = false;
            }
            item.active = true;
        }
    }

    getLoggedInUser() {
        return new Observable((observer) => {
            if (this.user) {
                observer.next(this.user);
            } else {
                this.userService.getCurrentUser().subscribe((user) => {
                    this.user = user;
                    observer.next(user);
                });
            }
        });
    }

    showTooltipsIfFirstLogin() {
        this.getLoggedInUser().subscribe((user: any) => {
            if (user && !user.goOverTutorial && !this.hasShownTutorial) {
                this.hasShownTutorial = true;
                setTimeout(() => {
                    const tooltipsArr = this.priceTooltips.toArray();
                    tooltipsArr[this.tooltipIndex].open();
                    this.tooltipIndex++;
                }, 400);
            }
        });
    }

    tooltipHidden() {
        const tooltipsArr = this.priceTooltips.toArray();
        if (this.tooltipIndex === 1) {
            this.user.goOverTutorial = true;
            this.userService.update(this.user).subscribe(() => {});
        }
        if (tooltipsArr.length > this.tooltipIndex) {
            setTimeout(() => {
                tooltipsArr[this.tooltipIndex].open();
                this.tooltipIndex++;
            }, 400);
        } else {
            this.sharedService.emitChange(menuTooltipShowedEvent);
        }
    }

    showPendingServiceOrders() {
        var serviceOrderMenuItemMatches = this.menuItems?.filter(x => x.title == 'Service Orders');
        if (!serviceOrderMenuItemMatches || serviceOrderMenuItemMatches.length == 0)
            return;
        var serviceOrderMenuItem = serviceOrderMenuItemMatches[0];
        this.serviceOrderService.getServiceOrdersForFbo(this.sharedService.currentUser.fboId, moment().add(-30, 'days').toDate(), moment().add(30, 'days').toDate()).subscribe((response: EntityResponseMessage<Array<ServiceOrder>>) => {
            if (response.success && response.result != null) {
                var inCompleteItems = response.result.filter(x => !x.isCompleted).length;
                if (inCompleteItems > 0) {
                    setTimeout(() => {
                        serviceOrderMenuItem.badge = {
                            text: inCompleteItems.toString(),
                            color: '#FFFFFF'
                        };
                    });
                }
                else {
                    setTimeout(() => {
                        serviceOrderMenuItem.badge = {};
                    });
                }
            }
        });
    }
}
