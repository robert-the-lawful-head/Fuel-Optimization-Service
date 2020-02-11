import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { TemporaryAddOnMarginComponent } from '../../shared/components/temporary-add-on-margin/temporary-add-on-margin.component';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs';
import { FbopricesService } from '../../services/fboprices.service';
import { SharedService } from '../../layouts/shared-service';


@Component({
    selector: 'ni-card',
    templateUrl: './ni-card.component.html',
    styleUrls: ['./ni-card.component.scss'],
    host: {
        '[class.ni-card]': 'true'
    }
})


export class NiCardComponent implements OnInit {
    vId: any;
    vEffectiveFrom: any;
    vEffectiveTo: any;
    vJet: any;
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


    public currentPrices: any[];
    //public isPricingSuspended: boolean = true;
    //@Input() isPricingSuspended: boolean = false;
    constructor(public tempAddOnMargin: MatDialog, public deleteFBODialog: MatDialog, private fboPricesService: FbopricesService, private sharedService: SharedService) { }

    ngOnInit() {
        this.checkPrices();
    }

    openDialog(): Observable<any> {
        const dialogRef = this.tempAddOnMargin.open(TemporaryAddOnMarginComponent, {
            data: { EffectiveFrom: this.vEffectiveFrom, EffectiveTo: this.vEffectiveTo, MarginJet: this.vJet, Id: this.vId, update: false },
            autoFocus: false,
            panelClass: 'my-panel'
        });
        dialogRef.componentInstance.idChanged1.subscribe((result) => {
            console.log(result);
            this.jetChanged.emit(result);

        });
        return dialogRef.afterClosed();
    }

    private openAddOnMargin() {
        this.openDialog();
    }

    public checkPricing() {
        this.checkPrices();
    }

    private checkPrices() {
        this.fboPricesService.getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.currentPrices = data;
                var jetACost = this.getCurrentPriceByProduct('JetA Cost');
                var jetAprice = this.getCurrentPriceByProduct('JetA Retail');

                if (jetACost.oid != 0 || jetAprice.oid !=0) {
                    // this.isPricingSuspended = false;
                    this.visibleSuspend = 'true';
                }
                else {
                  //  this.isPricingSuspended = true;
                    this.visibleSuspend = 'false';
                }
            });
    }

    private suspendPricing() {
        this.fboPricesService.suspendAllPricing(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.checkPrices();
            this.priceDeleted.emit('ok');
        });
    }

    private getCurrentPriceByProduct(product) {
        var result = { fboId: this.sharedService.currentUser.fboId, groupId: this.sharedService.currentUser.groupId, oid: 0 };
        for (let fboPrice of this.currentPrices) {
            if (fboPrice.product == product)
                result = fboPrice;
        }
        return result;
    }
}
