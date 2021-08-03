import {
    AfterViewInit,
    Component,
    OnInit,
    QueryList,
    ViewChildren,
} from '@angular/core';
import { Observable } from 'rxjs';

import { SharedService } from '../../../layouts/shared-service';
import { menuTooltipShowedEvent } from '../../../models/sharedEvents';
import { UserService } from '../../../services/user.service';
import { MenuService } from './menu.service';
import { IMenuItem } from './menu-item';

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

    constructor(
        private menuService: MenuService,
        private sharedService: SharedService,
        private userService: UserService
    ) {}

    ngOnInit(): void {
        this.getMenuItems();
    }

    ngAfterViewInit(): void {
        this.sharedService.changeEmitted$.subscribe((message) => {
            if (message === 'fbo-prices-loaded') {
                this.showTooltipsIfFirstLogin();
            }
        });
    }

    getMenuItems(): void {
        const OBSERVER = {
            error: (err) => this.menuService.handleError(err),
            next: (x) => (this.menuItems = x),
        };
        this.menuService.getData().subscribe(OBSERVER);
    }

    getLiClasses(item: any, isActive: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole) {
            role = this.sharedService.currentUser.impersonatedRole;
        }

        const hidden = item.roles && item.roles.indexOf(role) === -1;
        return {
            active: item.active || isActive,
            disabled: item.disabled,
            'has-sub': item.sub,
            hidden,
            'menu-item-group': item.groupTitle,
        };
    }

    isHidden(item: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole) {
            role = this.sharedService.currentUser.impersonatedRole;
        }
        return item.roles && item.roles.indexOf(role) === -1;
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
}
