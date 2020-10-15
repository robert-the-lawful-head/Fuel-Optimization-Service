import {
    Component,
    OnInit,
    ViewChildren,
    QueryList,
    AfterViewInit,
} from '@angular/core';
import { Observable } from 'rxjs';
import { IMenuItem } from './menu-item';
import { MenuService } from './menu.service';
import { SharedService } from '../../../layouts/shared-service';
import { UserService } from '../../../services/user.service';
import { menuTooltipShowedEvent } from '../../../models/sharedEvents';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss'],
    providers: [MenuService],
    host: {class: 'app-menu'},
})
export class MenuComponent implements OnInit, AfterViewInit {
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;

    menuItems: IMenuItem[];
    user: any;
    tooltipIndex = 0;

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
            next: (x) => (this.menuItems = x),
            error: (err) => this.menuService.handleError(err),
        };
        this.menuService.getData().subscribe(OBSERVER);
    }

    getLiClasses(item: any, isActive: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole || sessionStorage.getItem('impersonatedrole')) {
            role = 1;
        }

        const hidden = item.roles && item.roles.indexOf(role) === -1;
        return {
            'has-sub': item.sub,
            active: item.active || isActive,
            'menu-item-group': item.groupTitle,
            disabled: item.disabled,
            hidden,
        };
    }

    isHidden(item: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole || sessionStorage.getItem('impersonatedrole')) {
            role = 1;
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
            if (user && !user.goOverTutorial) {
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
