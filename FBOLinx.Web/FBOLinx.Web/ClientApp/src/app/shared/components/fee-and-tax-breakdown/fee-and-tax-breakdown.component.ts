import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

export enum FeeAndTaxBreakdownDisplayModes {
    PriceTaxBreakdown = 0,
    CustomerOmitting = 1,
    PricingPanel = 2,
}

@Component({
    selector: 'fee-and-tax-breakdown',
    styleUrls: ['./fee-and-tax-breakdown.component.scss'],
    templateUrl: './fee-and-tax-breakdown.component.html',
})
export class FeeAndTaxBreakdownComponent implements OnInit {
    @Input()
      feesAndTaxes: Array<any>;
    @Input()
      marginType: number;
    @Input()
      customerMargin: number;
    @Input()
    showLineSeparator = false;
    @Input()
    displayMode: FeeAndTaxBreakdownDisplayModes =
        FeeAndTaxBreakdownDisplayModes.PriceTaxBreakdown;
    @Input()
       fboPrice: number;
    @Input()
       validDepartureTypes: Array<number> = [0, 1, 2, 3];
    @Input()
       validFlightTypes: Array<number> = [0, 1, 2, 3];
    @Input()
       discountType : number;
    @Output()
       omitCheckChanged: EventEmitter<any> = new EventEmitter<any>();

    public aboveTheLineTaxes: Array<any> = [];
    public belowTheLineTaxes: Array<any> = [];
    public preMarginSubTotal = 0;
    public subTotalWithMargin = 0;
    public total = 0;
    public marginITP : number = 0;
    public subMargin : number = 0;

    constructor() { }

    ngOnInit(): void {
        this.performRecalculation();
        this.calcItpMargin()

    }

    public omitChanged(fee: any): void {
        this.omitCheckChanged.emit(fee);

    }


    public performRecalculation(): void {


        this.prepareTaxes();
        this.calculatePrices();
    }

    // Private Methods
    private prepareTaxes() {
        if (!this.feesAndTaxes) {
            return;
        }
        this.aboveTheLineTaxes = this.feesAndTaxes.filter(
            (tax) => tax.whenToApply === 0
        );
        this.belowTheLineTaxes = this.feesAndTaxes.filter(
            (tax) => tax.whenToApply === 1
        );
    }

    private calculatePrices(): void {

        this.calculatePreMarginSubTotal();
        this.calcItpMargin();
        this.calculateSubTotalWithMargin();
        this.calculateTotal();
    }

    private calcItpMargin ()
    {
        //Cost+ Mode
       if(this.marginType == 0)
       {
        if(this.discountType == 1)
        {
            this.marginITP = this.fboPrice *this.customerMargin /100;

        }

        else
        {
             this.marginITP = this.customerMargin ;


        }

        this.subTotalWithMargin = this.marginITP + this.preMarginSubTotal
       }

       // Retail- Mode
       else
       {
        if(this.discountType == 1)
        {
            this.marginITP = this.fboPrice *this.customerMargin /100;

        }

        else
        {
             this.marginITP = this.customerMargin ;


        }

             this.subTotalWithMargin =  this.preMarginSubTotal - this.marginITP

       }
   }

    private calculatePreMarginSubTotal(): void {
        let result = this.fboPrice;
        const basePrice = this.fboPrice;

        this.aboveTheLineTaxes.forEach((fee) => {
            if (fee.calculationType === 0) {
                fee.amount = fee.value;
            } else if (fee.calculationType === 1) {
                fee.amount = (fee.value / 100.0) * basePrice;
            }
            if (this.isFeeValidForTotal(fee)) {
                result += fee.amount;
            }
        });

        if (this.marginType === 1) {
            this.preMarginSubTotal = this.fboPrice;
        } else {
            this.preMarginSubTotal = result;
        }
    }

    private calculateSubTotalWithMargin() {
        if (!this.customerMargin && this.customerMargin !== 0) {
            return this.preMarginSubTotal;
        }

        this.subTotalWithMargin = 0;
        if(this.marginType == 0)
        {
         if(this.discountType == 1)
         {
             this.marginITP = this.fboPrice *this.customerMargin /100;

         }

         else
         {
              this.marginITP = this.customerMargin ;


         }

         this.subTotalWithMargin = this.marginITP + this.preMarginSubTotal
        }

        // Retail- Mode
        else
        {
         if(this.discountType == 1)
         {
             this.marginITP = this.fboPrice *this.customerMargin /100;

         }

         else
         {
              this.marginITP = this.customerMargin ;


         }

              this.subTotalWithMargin =  this.preMarginSubTotal - this.marginITP

        }
    }

    private calculateTotal() {
        let result = this.subTotalWithMargin;

        this.belowTheLineTaxes.forEach((fee) => {
            if (fee.calculationType === 0) {
                fee.amount = fee.value;
            } else if (fee.calculationType === 1) {
                fee.amount = (fee.value / 100.0) * this.subTotalWithMargin;
            }
            if (this.isFeeValidForTotal(fee)) {
                result += fee.amount;
            }
        });

        this.total = result;
    }

    private isFeeValidForTotal(fee: any): boolean {
        if (fee.isOmitted || (fee.omittedFor && fee.omittedFor.length > 0)) {
            return false;
        }
        if (this.validDepartureTypes.indexOf(fee.departureType) === -1) {
            return false;
        }
        if (
            this.validFlightTypes.indexOf(fee.flightTypeClassification) === -1
        ) {
            return false;
        }
        return true;
    }
}
