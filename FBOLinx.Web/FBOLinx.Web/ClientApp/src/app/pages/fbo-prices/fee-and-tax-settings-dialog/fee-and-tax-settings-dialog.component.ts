import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';

import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ApplicableTaxFlights } from '../../../enums/applicable-tax-flights';
import { FlightTypeClassifications } from '../../../enums/flight-type-classifications';
import { FeeCalculationTypes } from '../../../enums/fee-calculation-types';
import { FeeCalculationApplyingTypes } from '../../../enums/fee-calculation-applying-types';
import { MatTableDataSource } from '@angular/material/table';
import { EnumOptions } from '../../../models/enum-options';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { SharedService } from '../../../layouts/shared-service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';


export interface FeeAndTaxDialogData {
  oid: number;
  fboid: number;
  name: string;
  calculationType: FeeCalculationTypes;
  value: number;
  flightTypeClassification: FlightTypeClassifications;
  departureType: ApplicableTaxFlights;
  requiresUpdate: boolean;
  whenToApply: FeeCalculationApplyingTypes;
}

export interface SampleCalculation {
  pricingTemplateId: number;
  flightTypeClassification: FlightTypeClassifications;
  departureType: ApplicableTaxFlights;
  isCalculating: boolean;
  exclusivePrice: number;
  inclusivePrice: number;
}

@Component({
  selector: 'app-fee-and-tax-settings-dialog',
  templateUrl: './fee-and-tax-settings-dialog.component.html',
  styleUrls: ['./fee-and-tax-settings-dialog.component.scss'],
  providers: [SharedService, PricingtemplatesService, FbopricesService, NgxUiLoaderService]
})
export class FeeAndTaxSettingsDialogComponent implements OnInit {

  public calculationLoader = 'calculation-loader';

  public feeAndTaxDatasource: MatTableDataSource<any> = null;
  public displayedColumns: string[] = ['name', 'calculationType', 'whenToApply', 'value', 'flightTypeClassification', 'departureType', 'delete'];
  public feeCalculationTypes: Array<EnumOptions.EnumOption> = EnumOptions.feeCalculationTypeOptions;
  public flightTypeClassifications: Array<EnumOptions.EnumOption> = EnumOptions.flightTypeClassificationOptions;
  public applicableTaxFlightOptions: Array<EnumOptions.EnumOption> = EnumOptions.applicableTaxFlightOptions;
  public strictApplicableTaxFlightOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictApplicableTaxFlightOptions;
  public strictFlightTypeClassificationOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictFlightTypeClassificationOptions;
  public feeCalculationApplyingTypes: Array<EnumOptions.EnumOption> = EnumOptions.feeCalculationApplyingTypeOptions;
  public deletedFeesAndTaxes: Array<FeeAndTaxDialogData> = [];
  public pricingTemplates: Array<any>;
  public sampleCalculation: SampleCalculation = {
    pricingTemplateId: 0,
    flightTypeClassification: FlightTypeClassifications.Private,
    departureType: ApplicableTaxFlights.DomesticOnly,
    isCalculating: false,
    exclusivePrice: 0,
    inclusivePrice: 0
  };
  public requiresSaving = false;

  @ViewChild('priceBreakdownPreview') private priceBreakdownPreview: PriceBreakdownComponent;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Array<FeeAndTaxDialogData>,
    public dialogRef: MatDialogRef<FeeAndTaxSettingsDialogComponent>,
    private feesAndTaxesService: FbofeesandtaxesService,
    private sharedService: SharedService,
    private pricingTemplateService: PricingtemplatesService
  ) {}

  public ngOnInit(): void {
    this.loadPricingTemplates();
    if (!this.data) {
      this.loadFeesAndTaxes();
    } else {
      this.prepareDataSource();
    }
  }

  public saveChanges(): void {
    for (const feeAndTax of this.data) {
      if (feeAndTax.requiresUpdate) {
        this.saveFeeAndTax(feeAndTax);
      }
    }

    for (const deletedFeeAndTax of this.deletedFeesAndTaxes) {
      this.deleteFeeAndTax(deletedFeeAndTax);
    }

    this.dialogRef.close(this.data);
  }

  public feeAndTaxChanged(feeAndTax, avoidRecalculation = false): void {
    feeAndTax.requiresUpdate = true;
    this.requiresSaving = true;
    if (!avoidRecalculation) {
      this.sampleCalculationChanged();
    }
  }

  public feeAndTaxDeleted(feeAndTax): void {
    this.data.splice(this.data.indexOf(feeAndTax), 1);
    this.deletedFeesAndTaxes.push(feeAndTax);
    this.requiresSaving = true;
    this.prepareDataSource();
    if (this.priceBreakdownPreview) {
      this.priceBreakdownPreview.feesAndTaxes = this.data;
      this.priceBreakdownPreview.performRecalculation();
    }
  }

  public feeAndTaxAdded(): void {
    this.data.push({
      oid: 0,
      fboid: this.sharedService.currentUser.fboId,
      name: '',
      requiresUpdate: false,
      calculationType: 0,
      value: 0,
      flightTypeClassification: 3,
      departureType: 3,
      whenToApply: 0
    });
    this.prepareDataSource();
    this.requiresSaving = true;
  }

  public sampleCalculationChanged(): void {
      if (this.priceBreakdownPreview) {
          // Use a timeout here as the child component won't know of the template change until after the cycle
          const self = this;
          setTimeout(() => {
              self.priceBreakdownPreview.performRecalculation();
          });
      }
  }

  public feeValueChanged(feeAndTax, value) {
    try {
        feeAndTax.value = parseFloat(value);
    } catch (e) {
        feeAndTax.value = 0;
    }

    this.feeAndTaxChanged(feeAndTax);
  }

  public onCancelClick(): void {
    this.dialogRef.close();
  }

  // Private Methods
  private loadFeesAndTaxes(): void {
    this.feesAndTaxesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((response: any) => {
      this.data = response;
      this.prepareDataSource();
    });
  }

  private saveFeeAndTax(feeAndTax): void {
    if (feeAndTax.oid > 0) {
      this.feesAndTaxesService.update(feeAndTax).subscribe((response: any) => {

      });
    } else {
      this.feesAndTaxesService.add(feeAndTax).subscribe((response: any) => {
        feeAndTax.oid = response.oid;
      });
    }
  }

  private deleteFeeAndTax(feeAndTax): void {
    this.feesAndTaxesService.remove(feeAndTax).subscribe((response: any) => {
      this.data.splice(this.data.indexOf(feeAndTax), 1);
    });
  }

  private prepareDataSource(): void {
    this.feeAndTaxDatasource = new MatTableDataSource(this.data);
  }

  private loadPricingTemplates(): void {
    this.pricingTemplateService.getByFbo(
      this.sharedService.currentUser.fboId,
      this.sharedService.currentUser.groupId
    ).subscribe((response: any) => {
      this.pricingTemplates = response;
      for (const pricingTemplate of this.pricingTemplates) {
        if (pricingTemplate.default) {
          this.sampleCalculation.pricingTemplateId = pricingTemplate.oid;
        }
      }
      if (this.sampleCalculation.pricingTemplateId === 0 && this.pricingTemplates.length > 0) {
        this.sampleCalculation.pricingTemplateId = this.pricingTemplates[0].oid;
      }
    });
  }
}
