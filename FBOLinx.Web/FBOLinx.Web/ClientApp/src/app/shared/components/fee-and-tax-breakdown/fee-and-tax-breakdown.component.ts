import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';


export enum FeeAndTaxBreakdownDisplayModes {
  PriceTaxBreakdown = 0,
  CustomerOmitting = 1
}

@Component({
  selector: 'fee-and-tax-breakdown',
  templateUrl: './fee-and-tax-breakdown.component.html',
  styleUrls: ['./fee-and-tax-breakdown.component.scss']
})
export class FeeAndTaxBreakdownComponent implements OnInit {
  @Input() feesAndTaxes: Array<any>;
  @Input() marginType: number;
  @Input() customerMargin: number;
  @Input() displayMode: FeeAndTaxBreakdownDisplayModes = FeeAndTaxBreakdownDisplayModes.PriceTaxBreakdown;
  @Input() fboPrice: number;
  @Input() validDepartureTypes: Array<number> = [0, 1, 2, 3];
  @Input() validFlightTypes: Array<number> = [0, 1, 2, 3];
  @Output() omitCheckChanged: EventEmitter<any> = new EventEmitter<any>();

  public aboveTheLineTaxes: Array<any> = [];
  public belowTheLineTaxes: Array<any> = [];
  public preMarginSubTotal: number = 0;
  public subTotalWithMargin: number = 0;
  public total: number = 0;
  
  constructor() {

  }

  ngOnInit(): void {
    this.prepareTaxes();
    this.calculatePrices();
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
    if (!this.feesAndTaxes)
      return;
    this.aboveTheLineTaxes = this.feesAndTaxes.filter((tax) => tax.whenToApply == 0);
    this.belowTheLineTaxes = this.feesAndTaxes.filter((tax) => tax.whenToApply == 1);
  }

  private calculatePrices(): void {
    this.calculatePreMarginSubTotal();
    this.calculateSubTotalWithMargin();
    this.calculateTotal();
  }

  private calculatePreMarginSubTotal(): void {
    let result = this.fboPrice;
    let basePrice = this.fboPrice;

    if (this.marginType == 1) {
      this.preMarginSubTotal = result;
      return;
    }

    this.aboveTheLineTaxes.forEach((fee) => {
      if (fee.calculationType == 0) {
        fee.amount = fee.value;
      } else if (fee.calculationType == 1) {
        fee.amount = (fee.value / 100.0) * basePrice;
      }
      if (this.isFeeValidForTotal(fee)) {
        result += fee.amount;
      }
    });

    this.preMarginSubTotal = result;
  }

  private calculateSubTotalWithMargin() {
    this.subTotalWithMargin = 0;
    if (this.marginType == 0) {
      this.subTotalWithMargin = this.preMarginSubTotal + this.customerMargin;
    } else if (this.marginType == 1) {
      this.subTotalWithMargin = this.preMarginSubTotal - this.customerMargin;
    }
  }

  private calculateTotal() {
    let result = this.subTotalWithMargin;

    this.belowTheLineTaxes.forEach((fee) => {
      if (fee.calculationType == 0) {
        fee.amount = fee.value;
      } else if (fee.calculationType == 1) {
        fee.amount = (fee.value / 100.0) * this.subTotalWithMargin;
      }
      if (this.isFeeValidForTotal(fee)) {
        result += fee.amount;
      }
    });

    this.total = result;
  }

  private isFeeValidForTotal(fee: any): boolean {
    if (fee.isOmitted) {
      return false;
    }
    if (this.validDepartureTypes.indexOf(fee.departureType) == -1) {
      return false;
    }
    if (this.validFlightTypes.indexOf(fee.flightTypeClassification) == -1) {
      return false;
    }
    return true;
  }
}
