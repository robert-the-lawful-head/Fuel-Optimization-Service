import 'rxjs/add/operator/debounceTime';

import {
    AfterViewInit,
    Component,
    Input,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';

import { SharedService } from '../../../layouts/shared-service';
// Enums
import { EnumOptions } from '../../../models/enum-options';
// Model
import * as SharedEvents from '../../../models/sharedEvents';
// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';

interface TailLookupResponse {
    template?: string;
    company?: string;
    makeModel?: string;
    pricingList: Array<any>;
    rampFee: any;
}

interface PriceLookupRequest {
    pricingTemplateId: number;
    tailNumber: string;
    customerInfoByGroupId: number;
}

enum PriceCheckerLookupTypes {
    ByCustomer = 0,
    ByTail = 1,
    ByPricingTemplate = 2,
}

@Component({
    selector: 'price-checker',
    styleUrls: ['./price-checker.component.scss'],
    templateUrl: './price-checker.component.html',
})
export class PriceCheckerComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChild('priceBreakdownPreview')
    private priceBreakdownPreview: PriceBreakdownComponent;
    @Input() hideFeeAndTaxGeneralBreakdown = false;
    @Input() hidePriceTierBreakdown = false;
    @Input() hideTooltips = false;

    public customerForTailLookup: any;
    public tailNumberForTailLookup = '';
    public customersForTail: Array<any>;
    public tailNumberFormControlSubscription: any;
    public locationChangedSubscription: any;
    public customerForCustomerLookup: any;
    public tailNumberForCustomerLookup = '';
    public allCustomers: Array<any>;
    public allTailNumbers: Array<string>;
    public aircraftForCustomer: Array<any>;
    public pricingTemplateId = 0;
    public pricingTemplates: Array<any>;
    public priceCheckerLookupType: PriceCheckerLookupTypes =
        PriceCheckerLookupTypes.ByCustomer;
    public priceLookupInfo: TailLookupResponse;
    public tailLookupError: boolean;
    public strictApplicableTaxFlightOptions: Array<EnumOptions.EnumOption> =
        EnumOptions.strictApplicableTaxFlightOptions;
    public strictFlightTypeClassificationOptions: Array<EnumOptions.EnumOption> =
        EnumOptions.strictFlightTypeClassificationOptions;
    public sampleCalculation: PriceLookupRequest;
    public tailLoader = 'tail-loader';
    public isPriceBreakdownCustomerActive = true;

    public feesAndTaxes: Array<any>;

    constructor(
        private sharedService: SharedService,
        private aircraftsService: AircraftsService,
        private NgxUiLoader: NgxUiLoaderService,
        private pricingTemplateService: PricingtemplatesService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerAircraftsService: CustomeraircraftsService,
        private fboFeesAndTaxesService: FbofeesandtaxesService
    ) {}

    ngOnInit(): void {
        this.resetAll();
    }

    ngAfterViewInit(): void {
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.pricingTemplateId = 0;
                    this.resetAll();
                }
            });
    }

    ngOnDestroy(): void {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
        if (this.tailNumberFormControlSubscription) {
            this.tailNumberFormControlSubscription.unsubscribe();
        }
    }

    public resetAll(): void {
        this.loadPricingTemplates();
        this.loadAllCustomers();
        this.loadAllTailNumbers();
    }

    public displayCustomerName(customer: any) {
        return customer ? customer.company : customer;
    }

    public customerForLookupChanged(changedValue: any) {
        this.customerForCustomerLookup = changedValue;
        this.NgxUiLoader.startLoader(this.tailLoader);
        this.tailNumberForCustomerLookup = '';

        this.customerAircraftsService
            .getCustomerAircraftsByGroupAndCustomerId(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.customerForCustomerLookup.customerId
            )
            .subscribe((response: any) => {
                this.NgxUiLoader.stopLoader(this.tailLoader);
                if (!response) {
                    alert(
                        'There was an issue loading aircraft for the specified company.'
                    );
                }
                this.aircraftForCustomer = response;
                if (this.aircraftForCustomer.length > 0) {
                    this.tailNumberForCustomerLookup =
                        this.aircraftForCustomer[0].tailNumber;
                } else {
                    this.tailNumberForCustomerLookup = '';
                }
                this.lookupPricing();
            });
    }

    public tailNumberLookupChanged(changedValue: any) {
        this.NgxUiLoader.startLoader(this.tailLoader);
        this.customerForTailLookup = null;
        this.tailNumberForTailLookup = changedValue;
        this.aircraftsService
            .getCustomersByTail(
                this.sharedService.currentUser.groupId,
                this.tailNumberForTailLookup
            )
            .subscribe((response: any) => {
                this.NgxUiLoader.stopLoader(this.tailLoader);
                if (!response) {
                    this.customersForTail = [];
                    this.customerForTailLookup = null;
                    return;
                }
                this.customersForTail = response;
                if (this.customersForTail.length > 0) {
                    this.customerForTailLookup = this.customersForTail[0];
                    this.lookupPricing();
                }
            });
    }

    public lookupPricing() {
        const pricingTemplateId = this.getPricingTemplateId();
        const tailNumber = this.getTailNumber();
        const customerInfoByGroupId = this.getCustomerInfoByGroupId();

        if (pricingTemplateId > 0 || customerInfoByGroupId > 0) {
            this.sampleCalculation = {
                customerInfoByGroupId,
                pricingTemplateId,
                tailNumber,
            };
        }

        if (
            this.priceCheckerLookupType ===
            PriceCheckerLookupTypes.ByPricingTemplate
        ) {
            this.loadPricingTemplateFeesAndTaxes();
        } else {
            this.loadCustomerFeesAndTaxes();
        }
    }

    public onTabChanged(event: any): void {
        this.priceCheckerLookupType = event.index;
        this.priceLookupInfo = null;
        this.sampleCalculation = null;
        this.lookupPricing();
    }

    public priceBreakdownCalculationsCompleted(
        calculationResults: any[]
    ): void {
        if (!calculationResults || !calculationResults.length) {
            this.priceLookupInfo = null;
            return;
        }
        this.priceLookupInfo = calculationResults[0];
        try {
            this.feesAndTaxes = this.priceLookupInfo.pricingList[0].feesAndTaxes;
        } catch (e) {

        }
    }

    public priceTemplateChanged(): void {
        this.lookupPricing();
    }

    public customerForLookupTailChanged(): void {
        this.lookupPricing();
    }

    public tailNumberLookupCustomerChanged(): void {
        this.lookupPricing();
    }

    public refresh(): void {
        this.loadPricingTemplates();
    }

    public priceBreakdownCustomerActiveCheckCompleted(event: any): void {
        this.isPriceBreakdownCustomerActive = event;
    }

    // Private Methods
    private loadPricingTemplates(): void {
        this.NgxUiLoader.startLoader(this.tailLoader);
        this.pricingTemplateService
            .getByFbo(
                this.sharedService.currentUser.fboId,
                this.sharedService.currentUser.groupId
            )
            .subscribe((response: any) => {
                this.NgxUiLoader.stopLoader(this.tailLoader);
                this.pricingTemplates = response;
                if (!this.pricingTemplateId || this.pricingTemplateId === 0) {
                    for (const pricingTemplate of this.pricingTemplates) {
                        if (pricingTemplate.default) {
                            this.pricingTemplateId = pricingTemplate.oid;
                        }
                    }
                    if (
                        this.pricingTemplateId === 0 &&
                        this.pricingTemplates.length > 0
                    ) {
                        this.pricingTemplateId = this.pricingTemplates[0].oid;
                    }
                }

                this.lookupPricing();
            });
    }

    private loadAllCustomers(): void {
        this.customerInfoByGroupService
            .getByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((response: any) => {
                if (!response) {
                    alert(
                        'There was an error pulling customer information for price checking.'
                    );
                }
                this.allCustomers = response;
            });
    }

    private loadAllTailNumbers(): void {
        this.customerAircraftsService
            .getAircraftsListByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((data: Array<any>) => {
                this.allTailNumbers = data.map((t) => t.tailNumber);
            });
    }

    private getTailNumber(): string {
        if (
            this.priceCheckerLookupType === PriceCheckerLookupTypes.ByCustomer
        ) {
            return this.tailNumberForCustomerLookup;
        } else if (
            this.priceCheckerLookupType === PriceCheckerLookupTypes.ByTail
        ) {
            return this.tailNumberForTailLookup;
        } else {
            return '';
        }
    }

    private getCustomerInfoByGroupId(): number {
        if (
            this.priceCheckerLookupType === PriceCheckerLookupTypes.ByCustomer
        ) {
            return !this.customerForCustomerLookup
                ? 0
                : this.customerForCustomerLookup.customerInfoByGroupId;
        } else if (
            this.priceCheckerLookupType === PriceCheckerLookupTypes.ByTail
        ) {
            return !this.customerForTailLookup
                ? 0
                : this.customerForTailLookup.oid;
        } else {
            return 0;
        }
    }

    private getPricingTemplateId(): number {
        if (
            this.priceCheckerLookupType === PriceCheckerLookupTypes.ByCustomer
        ) {
            return 0;
        } else if (
            this.priceCheckerLookupType === PriceCheckerLookupTypes.ByTail
        ) {
            return 0;
        } else {
            return this.pricingTemplateId;
        }
    }

    private loadPricingTemplateFeesAndTaxes(): void {
        this.fboFeesAndTaxesService
            .getByFboAndPricingTemplate(
                this.sharedService.currentUser.fboId,
                this.pricingTemplateId
            )
            .subscribe((response: any[]) => {
                this.feesAndTaxes = response;
                const self = this;
                setTimeout(() => {
                    self.priceBreakdownPreview?.performRecalculation();
                });
            });
    }

    private loadCustomerFeesAndTaxes(): void {
        const customerId =
            this.priceCheckerLookupType === PriceCheckerLookupTypes.ByCustomer
                ? this.customerForCustomerLookup?.customerId
                : this.customerForTailLookup?.customerId;

        if (!customerId) {
            return;
        }

        this.fboFeesAndTaxesService
            .getByFboAndCustomer(
                this.sharedService.currentUser.fboId,
                customerId
            )
            .subscribe((response: any[]) => {
                this.feesAndTaxes = response;
                const self = this;
                setTimeout(() => {
                    self.priceBreakdownPreview?.performRecalculation();
                });
            });
    }
}
