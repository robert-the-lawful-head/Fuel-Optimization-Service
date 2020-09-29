import { Component, Inject, OnInit } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';

import { ApplicableTaxFlights } from '../../../enums/applicable-tax-flights';
import { FlightTypeClassifications } from '../../../enums/flight-type-classifications';
import { FeeCalculationTypes } from '../../../enums/fee-calculation-types';
import { MatTableDataSource } from '@angular/material/table';
import { EnumOptions } from '../../../models/enum-options';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { SharedService } from '../../../layouts/shared-service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { FbopricesService } from '../../../services/fboprices.service';


export interface FeeAndTaxDialogData {
  oid: number;
  fboid: number;
  name: string;
  calculationType: FeeCalculationTypes;
  value: number;
  flightTypeClassification: FlightTypeClassifications;
  departureType: ApplicableTaxFlights;
  requiresUpdate: boolean;
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
  providers: [SharedService, PricingtemplatesService, FbopricesService]
})
export class FeeAndTaxSettingsDialogComponent implements OnInit {    

  public feeAndTaxDatasource: MatTableDataSource<any> = null;
  public displayedColumns: string[] = ['name', 'calculationType', 'value', 'flightTypeClassification', 'departureType', 'delete'];
  public feeCalculationTypes: Array<EnumOptions.EnumOption> = EnumOptions.feeCalculationTypeOptions;
  public flightTypeClassifications: Array<EnumOptions.EnumOption> = EnumOptions.flightTypeClassificationOptions;
  public applicableTaxFlightOptions: Array<EnumOptions.EnumOption> = EnumOptions.applicableTaxFlightOptions;
  public strictApplicableTaxFlightOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictApplicableTaxFlightOptions;
  public strictFlightTypeClassificationOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictFlightTypeClassificationOptions;
  public deletedFeesAndTaxes: Array<FeeAndTaxDialogData> = [];
  public pricingTemplates: Array<any>;
  public sampleCalculation: SampleCalculation = {pricingTemplate: null};

  constructor(
    public dialogRef: MatDialogRef<FeeAndTaxSettingsDialogComponent>,
    public closeConfirmationDialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: Array<FeeAndTaxDialogData>,
    private feesAndTaxesService: FbofeesandtaxesService,
    private sharedService: SharedService,
    private pricingTemplateService: PricingtemplatesService,
    private FbopricesService: FbopricesService
  ) {

  }

  public ngOnInit(): void {
    this.loadPricingTemplates();
    if (!this.data) {
      this.loadFeesAndTaxes();
    } else {
      this.prepareDataSource();
    }
  }

  public saveChanges(): void {
    for (let feeAndTax of this.data) {
      if (feeAndTax.requiresUpdate) {
        this.saveFeeAndTax(feeAndTax);
      }
    }

    for (let deletedFeeAndTax of this.deletedFeesAndTaxes) {
      this.deleteFeeAndTax(deletedFeeAndTax);
    }

    this.dialogRef.close(this.data);
  }

  public feeAndTaxChanged(feeAndTax): void {
    feeAndTax.requiresUpdate = true;
  }

  public feeAndTaxDeleted(feeAndTax): void {
    this.data.splice(this.data.indexOf(feeAndTax), 1);
    this.deletedFeesAndTaxes.push(feeAndTax);
  }

  public feeAndTaxAdded(): void {
    this.data.push({ oid: 0, fboid: this.sharedService.currentUser.fboId, name: '', requiresUpdate: false, calculationType: 0, value: 0, flightTypeClassification: 0, departureType: 3 });
    this.prepareDataSource();
  }

  public sampleCalculationChanged(): void {
    this.sampleCalculation.isCalculating = true;
    this.FbopricesService.getFuelPricesForCompany({
      flightTypeClassification: this.sampleCalculation.flightTypeClassification,
      DepartureType: this.sampleCalculation.departureType,
      fboid: this.sharedService.currentUser.fboId,
      groupId: this.sharedService.currentUser.groupId
    }).subscribe((response: any) => {
      this.sampleCalculation.inclusivePrice = response.pricePerGallon;
      this.sampleCalculation.exclusivePrice = response.basePricePerGallon;
      this.sampleCalculation.isCalculating = false;
    });
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
    this.pricingTemplateService.getByFbo(this.sharedService.currentUser.fboId, this.sharedService.currentUser.groupId).subscribe((response: any) => {
      this.pricingTemplates = response;
      for (let pricingTemplate of this.pricingTemplates) {
        if (pricingTemplate.default) {
          this.sampleCalculation.pricingTemplate = pricingTemplate;
        }
      }
    });
  }
}
