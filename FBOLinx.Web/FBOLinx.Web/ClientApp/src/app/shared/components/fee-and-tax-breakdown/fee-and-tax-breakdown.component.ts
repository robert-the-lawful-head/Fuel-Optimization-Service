import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FeeAndTaxBreakdownDisplayModes } from 'src/app/enums/price.enum';
import { DiscountType, FeeCalculationTypes } from 'src/app/enums/fee-calculation-types';
import { MarginType } from 'src/app/enums/margin-type.enum';
import { FlightTypeClassifications } from 'src/app/enums/flight-type-classifications';
import { ApplicableTaxFlights } from 'src/app/enums/applicable-tax-flights';

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
    validDepartureTypes: Array<ApplicableTaxFlights> = [ApplicableTaxFlights.Never, ApplicableTaxFlights.DomesticOnly, ApplicableTaxFlights.InternationalOnly, ApplicableTaxFlights.All];
    @Input()
    validFlightTypes: Array<FlightTypeClassifications> = [FlightTypeClassifications.NotSet, FlightTypeClassifications.Private, FlightTypeClassifications.Commercial,FlightTypeClassifications.All];
    @Input()
    discountType: DiscountType;
    @Input()
    isMember: boolean;
    @Output()
    omitCheckChanged: EventEmitter<any> = new EventEmitter<any>();

    public aboveTheLineTaxes: Array<any> = [];
    public belowTheLineTaxes: Array<any> = [];
    public preMarginSubTotal = 0;
    public subTotalWithMargin = 0;
    public total = 0;
    public marginITP: number = 0;
    public subMargin: number = 0;

    constructor() {}

    ngOnInit(): void {
        this.performRecalculation();
        this.calcItpMargin();
    }

    public omitChanged(fee: any): void {
        this.omitCheckChanged.emit(fee);
    }

    public performRecalculation(): void {
        this.prepareTaxes();
        this.calculatePrices();
    }
    get isPriceTaxBreakdown(): boolean {
        return this.displayMode === FeeAndTaxBreakdownDisplayModes.PriceTaxBreakdown;
    }
    get isCustomerOmitting(): boolean {
        return this.displayMode === FeeAndTaxBreakdownDisplayModes.CustomerOmitting;
    }
    get isPricingPanel(): boolean {
        return this.displayMode === FeeAndTaxBreakdownDisplayModes.PricingPanel;
    }
    get isRetailMinus(): boolean {
        return this.marginType == MarginType.RetailMinus;
    }
    get isCostPlus(): boolean {
        return this.marginType == MarginType.CostPlus;
    }
    isCalculationTypePercentage(lineTax: any): boolean {
        return lineTax.calculationType === FeeCalculationTypes.Percentage;
    }
    isCalculationTypeFlatPerGallon(lineTax: any): boolean {
        return lineTax.calculationType === FeeCalculationTypes.FlatPerGallon;
    }
    isTextStrikeThrough(lineTax: any): boolean {
        return (
            lineTax.isOmitted ||
            this.validDepartureTypes.indexOf(lineTax.departureType) == -1 ||
            this.validFlightTypes.indexOf(lineTax.flightTypeClassification) ==
                -1
        );
    }
    isOmmited(lineTax: any): boolean {
        return lineTax.omittedFor && lineTax.omittedFor.length > 0;
    }
    // Private Methods
    private prepareTaxes() {
        if (!this.feesAndTaxes) {
            return;
        }
        this.aboveTheLineTaxes = this.feesAndTaxes.filter(
            (tax) => tax.whenToApply === FeeCalculationTypes.FlatPerGallon
        );
        this.belowTheLineTaxes = this.feesAndTaxes.filter(
            (tax) => tax.whenToApply === FeeCalculationTypes.Percentage
        );
    }

    private calculatePrices(): void {
        this.calculatePreMarginSubTotal();
        this.calcItpMargin();
        this.calculateSubTotalWithMargin();
        this.calculateTotal();
    }

    private calcItpMargin() {
        //Cost+ Mode
        if (this.marginType == MarginType.CostPlus) {
            if (this.discountType == DiscountType.Percentage) {
                this.marginITP = (this.fboPrice * this.customerMargin) / 100;
            } else {
                this.marginITP = this.customerMargin;
            }

            this.subTotalWithMargin = this.marginITP + this.preMarginSubTotal;
        }

        // Retail- Mode
        else {
            if (this.discountType == DiscountType.Percentage) {
                this.marginITP = (this.fboPrice * this.customerMargin) / 100;
            } else {
                this.marginITP = this.customerMargin;
            }

            this.subTotalWithMargin = this.preMarginSubTotal - this.marginITP;
        }
    }

    private calculatePreMarginSubTotal(): void {
        let result = this.fboPrice;
        const basePrice = this.fboPrice;

        this.aboveTheLineTaxes.forEach((fee) => {
            if (fee.calculationType === FeeCalculationTypes.FlatPerGallon) {
                fee.amount = fee.value;
            } else if (fee.calculationType === FeeCalculationTypes.Percentage) { FeeCalculationTypes.Percentage
                fee.amount = (fee.value / 100.0) * basePrice;
            }
            if (this.isFeeValidForTotal(fee)) {
                result += fee.amount;
            }
        });

        if (this.marginType == MarginType.RetailMinus) {
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
        if (this.marginType == MarginType.CostPlus) {
            if (this.discountType == DiscountType.Percentage) {
                this.marginITP = (this.fboPrice * this.customerMargin) / 100;
            } else {
                this.marginITP = this.customerMargin;
            }

            this.subTotalWithMargin = this.marginITP + this.preMarginSubTotal;
        }

        // Retail- Mode
        else {
            if (this.discountType == DiscountType.Percentage) {
                this.marginITP = (this.fboPrice * this.customerMargin) / 100;
            } else {
                this.marginITP = this.customerMargin;
            }

            this.subTotalWithMargin = this.preMarginSubTotal - this.marginITP;
        }
    }

    private calculateTotal() {
        let result = this.subTotalWithMargin;

        this.belowTheLineTaxes.forEach((fee) => {
            if (fee.calculationType === FeeCalculationTypes.FlatPerGallon) {
                fee.amount = fee.value;
            } else if (fee.calculationType === FeeCalculationTypes.Percentage) {
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
