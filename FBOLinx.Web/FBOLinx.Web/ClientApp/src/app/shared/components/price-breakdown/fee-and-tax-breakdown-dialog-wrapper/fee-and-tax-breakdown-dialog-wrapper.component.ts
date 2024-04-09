import { Component, Inject, OnInit, SimpleChanges } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface FeeAndTaxBreakDown {
    omitCheckChanged: any;
    customerMargin: any;
    displayMode: any;
    fboPrice: any;
    feesAndTaxes: any;
    marginType: any;
    discountType: any;
    validDepartureTypes: any;
    validFlightTypes: any;
}

@Component({
  selector: 'app-fee-and-tax-breakdown-dialog-wrapper',
  templateUrl: './fee-and-tax-breakdown-dialog-wrapper.component.html',
  styleUrls: ['./fee-and-tax-breakdown-dialog-wrapper.component.scss']
})
export class FeeAndTaxBreakdownDialogWrapperComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: FeeAndTaxBreakDown) {
  }

  ngOnInit() {
  }
}
