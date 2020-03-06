import {
    Component,
    OnInit,
    Input,
    Output,
    EventEmitter,
    ViewChild,
    OnDestroy,
    AfterViewInit
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs';
import { EventService as OverlayEventService } from '@ivylab/overlay-angular';

import { TemporaryAddOnMarginComponent } from '../../shared/components/temporary-add-on-margin/temporary-add-on-margin.component';
import { FbopricesService } from '../../services/fboprices.service';
import { UserService } from '../../services/user.service';
import { SharedService } from '../../layouts/shared-service';
import { Popover, PopoverProperties } from '../../shared/components/popover';
import { TooltipModalComponent } from '../../shared/components/tooltip-modal/tooltip-modal.component';


@Component({
    selector: 'ni-card',
    templateUrl: './ni-card.component.html',
    styleUrls: ['./ni-card.component.scss'],
    host: {
        '[class.ni-card]': 'true'
    }
})
export class NiCardComponent implements OnInit, OnDestroy, AfterViewInit {
    vId: any;
    vEffectiveFrom: any;
    vEffectiveTo: any;
    vJet: any;
    user: any;

    @Output() jetChanged: EventEmitter<any> = new EventEmitter();
    @Output() priceDeleted: EventEmitter<any> = new EventEmitter();

    @Input() title: string = '';
    @Input() tempId: string = '';
    @Input() visible: string = '';
    @Input() visibleSuspend: string = '';
    @Input() bgColor: string = '';
    @Input() customBgColor: string = '';
    @Input() color: string = '';
    @Input() customColor: string = '';
    @Input() bgImage: string = '';
    @Input() outline: boolean = false;
    @Input() indents: any = '';
    @Input() align: string = 'left';
    @Input() headerBgColor: string = '';
    @Input() headerColor: string = '';
    @Input() theme: string = '';
    @Input() fboPrices: Array<any>;
    @Input() addOnMargin: boolean;

    @ViewChild('tooltip') tooltip: any;
    @ViewChild('cancelTooltip') cancelTooltip: any;

    message: string;
    subscription: any;

    constructor(
        public tempAddOnMargin: MatDialog,
        public deleteFBODialog: MatDialog,
        private fboPricesService: FbopricesService,
        private userService: UserService,
        private sharedService: SharedService,
        private popover: Popover
    ) {}

    ngOnInit() {
        if (this.addOnMargin) {
            this.popover.changeEmitted$.subscribe(message => {
                if (message == 'proceed') {
                    this.openDialog();
                    this.user.addOnMarginTries = !this.user.addOnMarginTries
                        ? 1
                        : this.user.addOnMarginTries + 1;
                    this.userService.update(this.user).subscribe(() => {});
                }
            });
        }
        this.checkPrices();

        this.sharedService.priceMessage.subscribe(message => {
            if (message) {
                this.visibleSuspend = 'true';
            }
        });
    }

    ngAfterViewInit() {
        this.subscription = this.sharedService.loadedEmitted$.subscribe(message => {
            if (message == 'price-tooltips-showed') {
                setTimeout(() => {
                    if (this.cancelTooltip && this.cancelTooltip.nativeElement) {
                        this.cancelTooltip.nativeElement.click();
                        this.subscription.unsubscribe();
                    }
                }, 300);
            }
        });
    }

    ngOnDestroy(): void {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    openDialog(): Observable<any> {
        const dialogRef = this.tempAddOnMargin.open(
            TemporaryAddOnMarginComponent,
            {
                data: {
                    EffectiveFrom: this.vEffectiveFrom,
                    EffectiveTo: this.vEffectiveTo,
                    MarginJet: this.vJet,
                    Id: this.vId,
                    update: false
                },
                autoFocus: false,
                panelClass: 'my-panel'
            }
        );
        dialogRef.componentInstance.idChanged1.subscribe(result => {
            this.jetChanged.emit(result);
        });
        return dialogRef.afterClosed();
    }

    private openAddOnMargin(prop: PopoverProperties) {
        this.getLoggedInUser().subscribe((user: any) => {
            if (user && (!user.addOnMarginTries || user.addOnMarginTries < 3)) {
                prop.component = TooltipModalComponent;
                this.popover.load(prop);
            } else {
                this.openDialog();
            }
        });
    }

    public checkPricing() {
        this.checkPrices();
    }

    private checkPrices() {
        var jetACost = this.getCurrentPriceByProduct('JetA Cost');
        var jetAprice = this.getCurrentPriceByProduct('JetA Retail');

        if (jetACost.oid != 0 || jetAprice.oid != 0) {
            this.visibleSuspend = 'true';
        } else {
            this.visibleSuspend = 'false';
        }
    }

    private suspendPricing() {
        this.fboPricesService
            .suspendAllPricing(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.checkPrices();
                this.priceDeleted.emit('ok');
            });
    }

    private getCurrentPriceByProduct(product) {
        var result = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            oid: 0
        };
        if (this.fboPrices && this.fboPrices.length > 0) {
            for (let fboPrice of this.fboPrices) {
                if (fboPrice.product == product) result = fboPrice;
            }
        }
        return result;
    }

    private getLoggedInUser() {
        return new Observable(observer => {
            if (this.user) {
                observer.next(this.user);
            } else {
                this.userService.getCurrentUser().subscribe(user => {
                    this.user = user;
                    observer.next(user);
                });
            }
        });
    }

    private showPopover(prop: PopoverProperties) {
        prop.component = TooltipModalComponent;
        this.popover.load(prop);
    }
}
