import {
    Component,
    OnInit,
    Input,
    Output,
    EventEmitter,
    ViewChild,
    OnDestroy,
    AfterViewInit,
} from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Observable } from "rxjs";
import { NgbPopover } from "@ng-bootstrap/ng-bootstrap";

import { TemporaryAddOnMarginComponent } from "../../shared/components/temporary-add-on-margin/temporary-add-on-margin.component";
import { FbopricesService } from "../../services/fboprices.service";
import { UserService } from "../../services/user.service";
import { SharedService } from "../../layouts/shared-service";

@Component({
    selector: "ni-card",
    templateUrl: "./ni-card.component.html",
    styleUrls: ["./ni-card.component.scss"],
    host: {
        "[class.ni-card]": "true",
    },
})
export class NiCardComponent implements OnInit, OnDestroy, AfterViewInit {
    vId: any;
    vEffectiveFrom: any;
    vEffectiveTo: any;
    vJet: any;
    user: any;

    @Output() jetChanged: EventEmitter<any> = new EventEmitter();
    @Output() priceDeleted: EventEmitter<any> = new EventEmitter();

    @Input() title = "";
    @Input() tempId = "";
    @Input() visible = "";
    @Input() visibleSuspend = "";
    @Input() bgColor = "";
    @Input() customBgColor = "";
    @Input() color = "";
    @Input() customColor = "";
    @Input() bgImage = "";
    @Input() outline = false;
    @Input() indents: any = "";
    @Input() align = "left";
    @Input() headerBgColor = "";
    @Input() headerColor = "";
    @Input() theme = "";
    @Input() fboPrices: Array<any>;
    @Input() addOnMargin: boolean;
    @Input() noActivePricing = false;

    @ViewChild("cancelTooltip") cancelTooltip: any;
    @ViewChild("addOnMargin") addOnMarginPopover: NgbPopover;
    @ViewChild("cancelPricing") cancelPricingPopover: NgbPopover;

    message: string;
    subscription: any;

    constructor(
        public tempAddOnMargin: MatDialog,
        public deleteFBODialog: MatDialog,
        private fboPricesService: FbopricesService,
        private userService: UserService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.checkPrices();

        this.sharedService.priceMessage.subscribe((message) => {
            if (message) {
                this.visibleSuspend = "true";
            }
        });
    }

    ngAfterViewInit() {
        this.subscription = this.sharedService.loadedEmitted$.subscribe(
            (message) => {
                if (message === "price-tooltips-showed" && this.cancelPricingPopover) {
                    setTimeout(() => {
                        this.cancelPricingPopover.open();
                    }, 400);
                }
            }
        );
    }

    ngOnDestroy(): void {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    openDialog(): Observable<any> {
        this.updateUserAddOnMarginTries();

        this.addOnMarginPopover.close();

        const dialogRef = this.tempAddOnMargin.open(
            TemporaryAddOnMarginComponent,
            {
                data: {
                    EffectiveFrom: this.vEffectiveFrom,
                    EffectiveTo: this.vEffectiveTo,
                    MarginJet: this.vJet,
                    Id: this.vId,
                    update: false,
                },
                autoFocus: false,
                panelClass: "my-panel",
            }
        );
        dialogRef.componentInstance.idChanged1.subscribe((result) => {
            this.jetChanged.emit(result);
        });
        return dialogRef.afterClosed();
    }

    public updateUserAddOnMarginTries() {
        if (this.user.addOnMarginTries < 3) {
            this.user.addOnMarginTries = !this.user.addOnMarginTries
                            ? 1
                            : this.user.addOnMarginTries + 1;
            this.userService.update(this.user).subscribe(() => {});
        }
    }

    public openAddOnMargin() {
        this.getLoggedInUser().subscribe((user: any) => {
            if (user && (!user.addOnMarginTries || user.addOnMarginTries < 3)) {
                this.addOnMarginPopover.open();
            } else {
                this.openDialog();
            }
        });
    }

    public checkPricing() {
        this.checkPrices();
    }

    public checkPrices() {
        const jetACost = this.getCurrentPriceByProduct("JetA Cost");
        const jetAprice = this.getCurrentPriceByProduct("JetA Retail");

        if (jetACost.oid !== 0 || jetAprice.oid !== 0) {
            this.visibleSuspend = "true";
        } else {
            this.visibleSuspend = "false";
        }
    }

    public suspendPricing() {
        this.fboPricesService
            .suspendAllPricing(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.checkPrices();
                this.priceDeleted.emit("ok");
            });
    }

    public getCurrentPriceByProduct(product) {
        let result = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            oid: 0,
        };
        if (this.fboPrices && this.fboPrices.length > 0) {
            for (const fboPrice of this.fboPrices) {
                if (fboPrice.product === product) {
                    result = fboPrice;
                }
            }
        }
        return result;
    }

    public getLoggedInUser() {
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
}
