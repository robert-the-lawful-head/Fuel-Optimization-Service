import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import 'rxjs/add/operator/debounceTime';

// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';

// Enums
import { EnumOptions } from '../../../models/enum-options';

export interface TailLookupResponse {
  template?: string;
  company?: string;
  makeModel?: string;
  pricingList: Array<any>;
  rampFee: any;
}

export interface PriceLookupRequest {
  pricingTemplateId: number;
  tailNumber: string;
  customerInfoByGroupId: number;
}

export enum PriceCheckerLookupTypes {
  ByPricingTemplate = 0,
  ByCustomer = 1,
  ByTail = 2,
}

@Component({
  selector: 'price-checker',
  templateUrl: './price-checker.component.html',
  styleUrls: ['./price-checker.component.scss']
})
export class PriceCheckerComponent implements OnInit, OnDestroy {
  
  public customerForTailLookup: any;
  public tailNumberForTailLookup: string = '';
  public customersForTail: Array<any>;
  public tailNumberForLookupControl: FormControl = new FormControl();
  public tailNumberFormControlSubscription: any;

  public customerForCustomerLookup: any;
  public tailNumberForCustomerLookup: string = '';
  public allCustomers: Array<any>;
  public aircraftForCustomer: Array<any>;
  
  public pricingTemplateId: number = 0;
  public pricingTemplates: Array<any>;
  
  public priceCheckerLookupType: PriceCheckerLookupTypes = PriceCheckerLookupTypes.ByPricingTemplate;
  public priceLookupInfo: TailLookupResponse;
  public tailLookupError: boolean;
  public strictApplicableTaxFlightOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictApplicableTaxFlightOptions;
  public strictFlightTypeClassificationOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictFlightTypeClassificationOptions;

  public sampleCalculation: PriceLookupRequest;

  public tailLoader: string = 'tail-loader';

  @ViewChild('priceBreakdownPreview') private priceBreakdownPreview: PriceBreakdownComponent;

  constructor(private sharedService: SharedService,
    private aircraftsService: AircraftsService,
    private NgxUiLoader: NgxUiLoaderService,
    private pricingTemplateService: PricingtemplatesService,
    private customerInfoByGroupService: CustomerinfobygroupService,
    private customerAircraftsService: CustomeraircraftsService
  ) {
    // Register change subscription for tail number entry
    this.tailNumberFormControlSubscription = this.tailNumberForLookupControl.valueChanges.debounceTime(1000).subscribe(tailValue => {
      this.NgxUiLoader.startLoader(this.tailLoader);
      this.customerForTailLookup = null;
      this.tailNumberForTailLookup = tailValue;
      this.aircraftsService.getCustomersByTail(this.sharedService.currentUser.groupId, this.tailNumberForTailLookup).subscribe((response: any) => {
        this.NgxUiLoader.stopLoader(this.tailLoader);
        if (!response) {
          this.customersForTail = [];
          this.customerForTailLookup = null;
          return;
        }
        this.customersForTail = response;
        if (this.customersForTail.length > 0) {
          this.customerForTailLookup = this.customersForTail[0];
        }
      });
    });
  }

  ngOnInit(): void {
    this.loadPricingTemplates();
    this.loadAllCustomers();
  }

  ngOnDestroy(): void {
    if (this.tailNumberFormControlSubscription) {
      this.tailNumberFormControlSubscription.unsubscribe();
    }
  }

  public customerForLookupChanged() {
    this.NgxUiLoader.startLoader(this.tailLoader);
    this.tailNumberForCustomerLookup = '';
    this.customerAircraftsService.getCustomerAircraftsByGroupAndCustomerId(this.sharedService.currentUser.groupId,
      this.sharedService.currentUser.fboId,
      this.customerForCustomerLookup.customerId).subscribe((response:
        any) => {
      this.NgxUiLoader.stopLoader(this.tailLoader);
        if (!response) {
          alert('There was an issue loading aircraft for the specified company.');
        }
        this.aircraftForCustomer = response;
        if (this.aircraftForCustomer.length > 0) {
          this.tailNumberForCustomerLookup = this.aircraftForCustomer[0].tailNumber;
        }
    });
  }

  public lookupPricing() {
    this.sampleCalculation = {
      pricingTemplateId: this.getPricingTemplateId(),
      tailNumber: this.getTailNumber(),
      customerInfoByGroupId: this.getCustomerInfoByGroupId()
    };

    var self = this;
    setTimeout(function () {
      if (self.priceBreakdownPreview) {
        self.priceBreakdownPreview.performRecalculation();
      }
    });
  }

  public onTabChanged(event: any): void {
    this.priceCheckerLookupType = event.index;
    this.priceLookupInfo = null;
  }

  public priceBreakdownCalculationsCompleted(calculationResults: any[]): void {
    if (!calculationResults || calculationResults.length == 0) {
      this.priceLookupInfo = null;
      return;
    }
    this.priceLookupInfo = calculationResults[0];
  }

  // Private Methods
  private loadPricingTemplates(): void {
    this.NgxUiLoader.startLoader(this.tailLoader);
    this.pricingTemplateService.getByFbo(
      this.sharedService.currentUser.fboId,
      this.sharedService.currentUser.groupId
    ).subscribe((response: any) => {
      this.NgxUiLoader.stopLoader(this.tailLoader);
      this.pricingTemplates = response;
      for (const pricingTemplate of this.pricingTemplates) {
        if (pricingTemplate.default) {
          this.pricingTemplateId = pricingTemplate.oid;
        }
      }
      if (this.pricingTemplateId === 0 && this.pricingTemplates.length > 0) {
        this.pricingTemplateId = this.pricingTemplates[0].oid;
      }
      this.lookupPricing();
    });
  }

  private loadAllCustomers(): void {
    this.customerInfoByGroupService
      .getByGroupAndFbo(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId).subscribe(
        (response:
          any) => {
          if (!response) {
            alert('There was an error pulling customer information for price checking.');
          }
          this.allCustomers = response;
        });
  }

  private getTailNumber(): string {
    if (this.priceCheckerLookupType == PriceCheckerLookupTypes.ByCustomer) {
      return this.tailNumberForCustomerLookup;
    } else if (this.priceCheckerLookupType == PriceCheckerLookupTypes.ByTail) {
      return this.tailNumberForTailLookup;
    } else {
      return '';
    }
  }

  private getCustomerInfoByGroupId(): number {
    if (this.priceCheckerLookupType == PriceCheckerLookupTypes.ByCustomer) {
      return (!this.customerForCustomerLookup ? 0 : this.customerForCustomerLookup.customerInfoByGroupId);
    } else if (this.priceCheckerLookupType == PriceCheckerLookupTypes.ByTail) {
      return (!this.customerForTailLookup ? 0 : this.customerForTailLookup.id);
    } else {
      return 0;
    }
  }

  private getPricingTemplateId(): number {
    if (this.priceCheckerLookupType == PriceCheckerLookupTypes.ByCustomer) {
      return 0;
    } else if (this.priceCheckerLookupType == PriceCheckerLookupTypes.ByTail) {
      return 0;
    } else {
      return this.pricingTemplateId;
    }
  }
}
