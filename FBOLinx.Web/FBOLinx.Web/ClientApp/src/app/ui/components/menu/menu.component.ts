import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { Observable } from 'rxjs';
import { IMenuItem } from './menu-item';
import { MenuService } from './menu.service';
import { SharedService } from '../../../layouts/shared-service';
import { UserService } from '../../../services/user.service';
import { Popover, PopoverProperties } from '../../../shared/components/popover';
import { TooltipModalComponent } from '../../../shared/components/tooltip-modal/tooltip-modal.component';
import { EventService as OverlayEventService } from '@ivylab/overlay-angular';

@Component({
    moduleId: module.id,
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss'],
    providers: [MenuService],
    host: {
        class: 'app-menu'
    }
})
export class MenuComponent implements OnInit {
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;

    menuItems: IMenuItem[];
    user: any;
    tooltipIndex = 0;

    constructor(
        private menuService: MenuService,
        private sharedService: SharedService,
        private userService: UserService,
        public popover: Popover,
        private overlayEventService: OverlayEventService
    ) {
        overlayEventService.emitter.subscribe(response => {
            if (response.type == '[Overlay] Hide') {
                const tooltipsArr = this.priceTooltips.toArray();
                if (tooltipsArr.length > this.tooltipIndex) {
                    setTimeout(() => {
                        tooltipsArr[this.tooltipIndex].nativeElement.click();
                        this.tooltipIndex++;
                    }, 1000);
                }
            }
        });
        sharedService.loadedEmitted$.subscribe(message => {
            if (message == 'fbo-prices-loaded') {
                this.showTooltipsIfFirstLogin();
            }
        });
    }

    getMenuItems(): void {
        const OBSERVER = {
            next: x => (this.menuItems = x),
            error: err => this.menuService.handleError(err)
        };
        this.menuService.getData().subscribe(OBSERVER);
    }

    getLiClasses(item: any, isActive: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole)
            role = this.sharedService.currentUser.impersonatedRole;
        let hidden = item.roles && item.roles.indexOf(role) == -1;
        return {
            'has-sub': item.sub,
            active: item.active || isActive,
            'menu-item-group': item.groupTitle,
            disabled: item.disabled,
            hidden: hidden
        };
    }

    isHidden(item: any) {
        let role = this.sharedService.currentUser.role;
        if (this.sharedService.currentUser.impersonatedRole)
            role = this.sharedService.currentUser.impersonatedRole;
        return item.roles && item.roles.indexOf(role) == -1;
    }

    getStyles(item: any) {
        return {
            background: item.bg,
            color: item.color
        };
    }

    ngOnInit(): void {
        this.getMenuItems();
    }

    toggle(event: Event, item: any, el: any) {
        event.preventDefault();

        let items: any[] = el.menuItems;

        if (item.active) {
            item.active = false;
        } else {
            for (let i = 0; i < items.length; i++) {
                items[i].active = false;
            }
            item.active = true;
        }
    }

    getLoggedInUser() {
        return new Observable(observer => {
            if (this.user) {
                observer.next(this.user);
            }
            this.userService.getCurrentUser().subscribe(user => {
                this.user = user;
                observer.next(user);
            });
        });
    }

    showTooltipsIfFirstLogin() {
        this.getLoggedInUser().subscribe((user: any) => {
            if (user && (!user.loginCount || user.loginCount <= 1)) {
                setTimeout(() => {
                    const tooltipsArr = this.priceTooltips.toArray();
                    tooltipsArr[this.tooltipIndex].nativeElement.click();
                    this.tooltipIndex++;
                }, 1000);
                user.loginCount = !this.user.loginCount
                    ? 2
                    : this.user.loginCount + 1;
                this.userService.update(user).subscribe(() => {});
            }
        });
    }

    showPopover(prop: PopoverProperties) {
        prop.component = TooltipModalComponent;
        this.popover.load(prop);
    }

    closePopover() {
        this.popover.close();
    }
}
