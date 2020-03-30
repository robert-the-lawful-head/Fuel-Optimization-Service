import {
    Component,
    OnInit,
    ViewChildren,
    QueryList,
    AfterViewInit,
    OnDestroy,
    HostBinding,
} from "@angular/core";
import { Observable } from "rxjs";
import { IMenuItem } from "./menu-item";
import { MenuService } from "./menu.service";
import { SharedService } from "../../../layouts/shared-service";
import { UserService } from "../../../services/user.service";
import { Popover, PopoverProperties } from "../../../shared/components/popover";
import { TooltipModalComponent } from "../../../shared/components/tooltip-modal/tooltip-modal.component";
import { EventService as OverlayEventService } from "@ivylab/overlay-angular";

@Component({
    moduleId: module.id,
    selector: "app-menu",
    templateUrl: "./menu.component.html",
    styleUrls: ["./menu.component.scss"],
    providers: [MenuService],
    host: {class: "app-menu"}
})
export class MenuComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChildren("tooltip") priceTooltips: QueryList<any>;

    menuItems: IMenuItem[];
    user: any;
    tooltipIndex = 0;
    neverShowTooltip = false;
    subscription: any;

    constructor(
        private menuService: MenuService,
        private sharedService: SharedService,
        private userService: UserService,
        public popover: Popover,
        private overlayEventService: OverlayEventService
    ) {}

    ngOnInit(): void {
        this.getMenuItems();
    }

    ngAfterViewInit(): void {
        this.sharedService.loadedEmitted$.subscribe((message) => {
            if (message === "fbo-prices-loaded") {
                this.showTooltipsIfFirstLogin();
            }
        });
        this.subscription = this.overlayEventService.emitter.subscribe(
            (response) => {
                if (
                    !this.neverShowTooltip &&
                    response.type === "[Overlay] Hide"
                ) {
                    const tooltipsArr = this.priceTooltips.toArray();
                    if (tooltipsArr.length > this.tooltipIndex) {
                        setTimeout(() => {
                            tooltipsArr[
                                this.tooltipIndex
                            ].nativeElement.click();
                            this.tooltipIndex++;
                        }, 300);
                    } else {
                        this.neverShowTooltip = true;
                        this.sharedService.loadedChange("menu-tooltips-showed");
                        this.subscription.unsubscribe();
                    }
                }
            }
        );
    }

    ngOnDestroy(): void {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
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
        if (this.sharedService.currentUser.impersonatedRole) {
            role = this.sharedService.currentUser.impersonatedRole;
        }
        const hidden = item.roles && item.roles.indexOf(role) === -1;
        return {
            "has-sub": item.sub,
            active: item.active || isActive,
            "menu-item-group": item.groupTitle,
            disabled: item.disabled,
            hidden,
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
            if (user && !user.goOverTutorial) {
                setTimeout(() => {
                    const tooltipsArr = this.priceTooltips.toArray();
                    tooltipsArr[this.tooltipIndex].nativeElement.click();
                    this.tooltipIndex++;
                }, 300);
                this.user.goOverTutorial = true;
                this.userService.update(this.user).subscribe(() => {});
            } else {
                this.neverShowTooltip = true;
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
