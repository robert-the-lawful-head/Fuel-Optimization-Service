import { ActivatedRoute } from '@angular/router';
import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { forkJoin, Observable } from 'rxjs';

import { ApplicableTaxFlights } from '../../../enums/applicable-tax-flights';
import { FlightTypeClassifications } from '../../../enums/flight-type-classifications';
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { FbopricesService } from '../../../services/fboprices.service';
import {
    FeeAndTaxBreakdownComponent,
    FeeAndTaxBreakdownDisplayModes,
} from '../fee-and-tax-breakdown/fee-and-tax-breakdown.component';

export enum PriceBreakdownDisplayTypes {
    SingleColumnAllFlights = 0,
    TwoColumnsDomesticInternationalOnly = 1,
    TwoColumnsApplicableFlightTypesOnly = 2,
    FourColumnsAllRules = 3,
}

@Component({
    selector: 'price-breakdown',
    styleUrls: ['./price-breakdown.component.scss'],
    templateUrl: './price-breakdown.component.html',
})
export class PriceBreakdownComponent implements OnInit {
    @ViewChild('feeAndTaxBreakdown')
    private feeAndTaxBreakdown: FeeAndTaxBreakdownComponent;
    @ViewChild('dynamicFeeAndTaxBreakdown')
    private dynamicFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;

    @Input() priceTemplateId;
    @Input() tailNumber = '';
    @Input() customerInfoByGroupId = 0;
    @Input() feesAndTaxes: Array<any>;
    @Input() hideFeeAndTaxGeneralBreakdown = false;
    @Input() hidePriceTierBreakdown = false;
    @Input() feeAndTaxDisplayMode: FeeAndTaxBreakdownDisplayModes =
        FeeAndTaxBreakdownDisplayModes.PriceTaxBreakdown;
    @Input() showFeeAndTaxLineSeparator = false;
    @Input() tooltipPlacement = 'top-left';
    @Input() priceBreakdownLoader = 'price-breakdown-loader';
    @Input() hideTooltips = false;
    @Output() omitCheckChanged: EventEmitter<any> = new EventEmitter<any>();
    @Output() calculationsComplated: EventEmitter<any> =
        new EventEmitter<any>();
    @Output() customerActiveCheckCompleted: EventEmitter<any> =
        new EventEmitter<any>();

    public internationalCommercialPricing: any;
    public internationalPrivatePricing: any;
    public domesticCommercialPricing: any;
    public domesticPrivatePricing: any;
    public internationalCommercialFeesAndTaxes: Array<any>;
    public internationalPrivateFeesAndTaxes: Array<any>;
    public domesticCommercialFeesAndTaxes: Array<any>;
    public domesticPrivateFeesAndTaxes: Array<any>;
    public feeAndTaxCloneForPopOver: Array<any>;
    public priceBreakdownDisplayType: PriceBreakdownDisplayTypes =
        PriceBreakdownDisplayTypes.SingleColumnAllFlights;
    public activeHoverPriceItem: any = {};
    public activeHoverDeparturetypes: Array<number> = [];
    public activeHoverFlightTypes: Array<number> = [];
    public defaultValidDepartureTypes: Array<number> = [0, 1, 2, 3];
    public defaultValidFlightTypes: Array<number> = [0, 1, 2, 3];
    public isCustomerActive = true;

    constructor(
        private feesAndTaxesService: FbofeesandtaxesService,
        private sharedService: SharedService,
        private fboPricesService: FbopricesService,
        private NgxUiLoader: NgxUiLoaderService,
        private customerInfoByGroupService: CustomerinfobygroupService ,
        private route :ActivatedRoute
    ) {

         this.priceTemplateId = this.route.snapshot.paramMap.get('id');
      }

    ngOnInit(): void {

        this.prepareDefaultSettings();
        this.performCalculations();
    }

    public omitChanged(fee: any): void {
        if (fee.isOmitted) {
            fee.omittedFor = (this.feeAndTaxDisplayMode == FeeAndTaxBreakdownDisplayModes.CustomerOmitting && this.customerInfoByGroupId > 0 ? 'C' : 'P');
        } else {
            fee.omittedFor = '';
        }
        this.omitCheckChanged.emit(fee);
    }

    public mouseEnterPriceItem(
        priceItem: any,
        departureTypes: Array<number>,
        flightTypes: Array<number>
    ): void {

        this.activeHoverPriceItem = priceItem;
        console.log(this.activeHoverPriceItem);
        this.activeHoverDeparturetypes = departureTypes;
        this.activeHoverFlightTypes = flightTypes;
        if (priceItem.feesAndTaxes != null) {
            this.feeAndTaxCloneForPopOver = [];
            priceItem.feesAndTaxes.forEach((val) =>
                this.feeAndTaxCloneForPopOver.push(Object.assign({}, val))
            );
        } else {
            this.feeAndTaxCloneForPopOver = [];
        }
        const self = this;
        setTimeout(() => {
            if (self.dynamicFeeAndTaxBreakdown) {
                self.dynamicFeeAndTaxBreakdown.performRecalculation();
            }
        });
    }

    public performRecalculation(): void {
        this.performCalculations();
    }

    // Private Methods
    private prepareDefaultSettings(): void {
        if (
            this.feeAndTaxDisplayMode ===
            FeeAndTaxBreakdownDisplayModes.CustomerOmitting
        ) {
            this.defaultValidDepartureTypes = [0, 1, 2, 3];
            this.defaultValidFlightTypes = [0, 1, 2, 3];
        }
    }

    private performCalculations(): void {
        this.NgxUiLoader.startLoader(this.priceBreakdownLoader);
        if (!this.feesAndTaxes) {
            this.loadFeesAndTaxes();
        } else {
            this.groupFeesAndTaxes();
            this.determineDisplayType();
            this.loadCalculations();
        }
    }

    private loadFeesAndTaxes() {
        this.feesAndTaxesService
            .getByFbo(this.sharedService.currentUser.fboId)
            .subscribe((response: any) => {

                this.feesAndTaxes = response;
                this.groupFeesAndTaxes();
                this.determineDisplayType();
                this.loadCalculations();

            });
    }

    private groupFeesAndTaxes() {
        if (!this.feesAndTaxes) {
            return;
        }
        this.domesticCommercialFeesAndTaxes = this.feesAndTaxes.filter(
            (fee) =>
                (fee.departureType === 1 || fee.departureType === 3) &&
                (fee.flightTypeClassification === 2 ||
                    fee.flightTypeClassification === 3)
        );
        this.domesticPrivateFeesAndTaxes = this.feesAndTaxes.filter(
            (fee) =>
                (fee.departureType === 1 || fee.departureType === 3) &&
                (fee.flightTypeClassification === 1 ||
                    fee.flightTypeClassification === 3)
        );
        this.internationalCommercialFeesAndTaxes = this.feesAndTaxes.filter(
            (fee) =>
                (fee.departureType === 2 || fee.departureType === 3) &&
                (fee.flightTypeClassification === 2 ||
                    fee.flightTypeClassification === 3)
        );
        this.internationalPrivateFeesAndTaxes = this.feesAndTaxes.filter(
            (fee) =>
                (fee.departureType === 2 || fee.departureType === 3) &&
                (fee.flightTypeClassification === 1 ||
                    fee.flightTypeClassification === 3)
        );
    }

    private determineDisplayType(): void {
        if (!this.feesAndTaxes) {
            return;
        }

        let hasDepartureTypeRule = false;
        let hasFlightTypeRule = false;

        this.feesAndTaxes.forEach((fee) => {
            if (fee.departureType === 1 || fee.departureType === 2) {
                hasDepartureTypeRule = true;
            }
            if (
                fee.flightTypeClassification === 1 ||
                fee.flightTypeClassification === 2
            ) {
                hasFlightTypeRule = true;
            }
        });

        if (!hasDepartureTypeRule && !hasFlightTypeRule) {
            this.priceBreakdownDisplayType =
                PriceBreakdownDisplayTypes.SingleColumnAllFlights;
        } else if (!hasDepartureTypeRule && hasFlightTypeRule) {
            this.priceBreakdownDisplayType =
                PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly;
        } else if (hasDepartureTypeRule && !hasFlightTypeRule) {
            this.priceBreakdownDisplayType =
                PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly;
        } else if (hasDepartureTypeRule && hasFlightTypeRule) {
            this.priceBreakdownDisplayType =
                PriceBreakdownDisplayTypes.FourColumnsAllRules;
        }
    }

    private loadCalculations() {
        forkJoin([
            this.loadInternationalCommercialPricing(),
            this.loadInternationalPrivatePricing(),
            this.loadDomesticCommercialPricing(),
            this.loadDomesticPrivatePricing(),
            this.loadCustomerInfoByGroup(),
        ]).subscribe((responseList: any[]) => {
            this.NgxUiLoader.stopLoader(this.priceBreakdownLoader);
            if (!responseList) {
                alert('There was a problem fetching prices.');
            }

            this.internationalCommercialPricing = responseList[0];
            this.internationalPrivatePricing = responseList[1];
            this.domesticCommercialPricing = responseList[2];
            this.domesticPrivatePricing = responseList[3];
            const customerInfoByGroup = responseList[4];

            console.log()

            //If no result was returned for the customer then this wasn't a customer-level price check so mark it as active.
            //Otherwise check the actual active flag of the customer.
            this.isCustomerActive =
                !customerInfoByGroup || customerInfoByGroup.active;
            this.customerActiveCheckCompleted.emit(this.isCustomerActive);

            this.feeAndTaxCloneForPopOver = [];
            this.feesAndTaxes.forEach((val) =>
                this.feeAndTaxCloneForPopOver.push(Object.assign({}, val))
            );

            const self = this;
            setTimeout(() => {
                if (self.feeAndTaxBreakdown) {
                    self.feeAndTaxBreakdown.performRecalculation();
                }
            });
            this.calculationsComplated.emit([
                this.internationalCommercialPricing,
                this.internationalPrivatePricing,
                this.domesticCommercialPricing,
                this.domesticPrivatePricing,
            ]);
        });
    }

    private loadInternationalCommercialPricing(): Observable<any> {
        return this.fboPricesService.getFuelPricesForCompany({
            customerInfoByGroupId: this.customerInfoByGroupId,
            departureType: ApplicableTaxFlights.InternationalOnly,
            fboid: this.sharedService.currentUser.fboId,
            flightTypeClassification: FlightTypeClassifications.Commercial,
            groupId: this.sharedService.currentUser.groupId,
            icao: this.sharedService.currentUser.icao,
            pricingTemplateId: this.priceTemplateId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            tailNumber: this.tailNumber,
        });
    }

    private loadInternationalPrivatePricing(): Observable<any> {
        return this.fboPricesService.getFuelPricesForCompany({
            customerInfoByGroupId: this.customerInfoByGroupId,
            departureType: ApplicableTaxFlights.InternationalOnly,
            fboid: this.sharedService.currentUser.fboId,
            flightTypeClassification: FlightTypeClassifications.Private,
            groupId: this.sharedService.currentUser.groupId,
            icao: this.sharedService.currentUser.icao,
            pricingTemplateId:this.priceTemplateId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            tailNumber: this.tailNumber,
        });
    }

    private loadDomesticCommercialPricing(): Observable<any> {
        return this.fboPricesService.getFuelPricesForCompany({
            customerInfoByGroupId: this.customerInfoByGroupId,
            departureType: ApplicableTaxFlights.DomesticOnly,
            fboid: this.sharedService.currentUser.fboId,
            flightTypeClassification: FlightTypeClassifications.Commercial,
            groupId: this.sharedService.currentUser.groupId,
            icao: this.sharedService.currentUser.icao,
            pricingTemplateId: this.priceTemplateId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            tailNumber: this.tailNumber,
        });
    }

    private loadDomesticPrivatePricing(): Observable<any> {
        return this.fboPricesService.getFuelPricesForCompany({
            customerInfoByGroupId: this.customerInfoByGroupId,
            departureType: ApplicableTaxFlights.DomesticOnly,
            fboid: this.sharedService.currentUser.fboId,
            flightTypeClassification: FlightTypeClassifications.Private,
            groupId: this.sharedService.currentUser.groupId,
            icao: this.sharedService.currentUser.icao,
            pricingTemplateId: this.priceTemplateId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            tailNumber: this.tailNumber,
        });
    }

    private loadCustomerInfoByGroup(): Observable<any> {
        return this.customerInfoByGroupService.get({
            oid: this.customerInfoByGroupId,
        });
    }
}
