import { Component, Input, Output, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { SharedService } from '../../../layouts/shared-service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FlightTypeClassifications } from '../../../enums/flight-type-classifications';
import { ApplicableTaxFlights } from '../../../enums/applicable-tax-flights';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { FeeAndTaxBreakdownComponent } from '../fee-and-tax-breakdown/fee-and-tax-breakdown.component';

import { forkJoin } from 'rxjs';
import { Observable } from 'rxjs';
import { FeeAndTaxBreakdownDisplayModes } from '../fee-and-tax-breakdown/fee-and-tax-breakdown.component';


export enum PriceBreakdownDisplayTypes {
  SingleColumnAllFlights = 0,
  TwoColumnsDomesticInternationalOnly = 1,
  TwoColumnsApplicableFlightTypesOnly = 2,
  FourColumnsAllRules = 3
}

@Component({
    selector: 'price-breakdown',
    templateUrl: './price-breakdown.component.html',
    styleUrls: ['./price-breakdown.component.scss']
})
export class PriceBreakdownComponent implements OnInit {

    @Input()
    priceTemplateId: number = 0;
    @Input()
    tailNumber: string = '';
    @Input()
    customerInfoByGroupId: number = 0;
    @Input()
    feesAndTaxes: Array<any>;
    @Input()
    hideFeeAndTaxGeneralBreakdown: boolean = false;
    @Input()
    hidePriceTierBreakdown: boolean = false;
    @Input()
    feeAndTaxDisplayMode: FeeAndTaxBreakdownDisplayModes = FeeAndTaxBreakdownDisplayModes.PriceTaxBreakdown;
    @Input()
    showFeeAndTaxLineSeparator: boolean = false;
    @Output()
    omitCheckChanged: EventEmitter<any> = new EventEmitter<any>();
    @Output()
    calculationsComplated: EventEmitter<any> = new EventEmitter<any>();

    public internationalCommercialPricing: any;
    public internationalPrivatePricing: any;
    public domesticCommercialPricing: any;
    public domesticPrivatePricing: any;
    public internationalCommercialFeesAndTaxes: Array<any>;
    public internationalPrivateFeesAndTaxes: Array<any>;
    public domesticCommercialFeesAndTaxes: Array<any>;
    public domesticPrivateFeesAndTaxes: Array<any>;
    public feeAndTaxCloneForPopOver: Array<any>;
    public priceBreakdownDisplayType: PriceBreakdownDisplayTypes = PriceBreakdownDisplayTypes.SingleColumnAllFlights;
    public priceBreakdownLoader = 'price-breakdown-loader';
    public activeHoverPriceItem: any = {};
    public activeHoverDeparturetypes: Array<number> = [];
    public activeHoverFlightTypes: Array<number> = [];
    public defaultValidDepartureTypes: Array<number> = [0,1,3];
    public defaultValidFlightTypes: Array<number> = [0,1,3];

    @ViewChild('feeAndTaxBreakdown')
    private feeAndTaxBreakdown: FeeAndTaxBreakdownComponent;
    @ViewChild('dynamicFeeAndTaxBreakdown')
    private dynamicFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;


    constructor(private feesAndTaxesService: FbofeesandtaxesService,
        private sharedService: SharedService,
        private fboPricesService: FbopricesService,
        private NgxUiLoader: NgxUiLoaderService) {

    }

    ngOnInit(): void {
        this.prepareDefaultSettings();
        this.performCalculations();
    }

    public omitChanged(fee: any): void {
        this.omitCheckChanged.emit(fee);
    }

    public mouseEnterPriceItem(priceItem: any, departureTypes: Array<number>, flightTypes: Array<number>): void {
        this.activeHoverPriceItem = priceItem;
        this.activeHoverDeparturetypes = departureTypes;
        this.activeHoverFlightTypes = flightTypes;
        let self = this;
        setTimeout(function() {
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
        if (this.feeAndTaxDisplayMode == FeeAndTaxBreakdownDisplayModes.CustomerOmitting) {
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
        this.feesAndTaxesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((response: any) => {
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
            (fee) => (fee.departureType == 1 || fee.departureType == 3) &&
            (fee.flightTypeClassification == 2 || fee.flightTypeClassification == 3));
        this.domesticPrivateFeesAndTaxes = this.feesAndTaxes.filter(
            (fee) => (fee.departureType == 1 || fee.departureType == 3) &&
            (fee.flightTypeClassification == 1 || fee.flightTypeClassification == 3));
        this.internationalCommercialFeesAndTaxes = this.feesAndTaxes.filter(
            (fee) => (fee.departureType == 2 || fee.departureType == 3) &&
            (fee.flightTypeClassification == 2 || fee.flightTypeClassification == 3));
        this.internationalPrivateFeesAndTaxes = this.feesAndTaxes.filter(
            (fee) => (fee.departureType == 2 || fee.departureType == 3) &&
            (fee.flightTypeClassification == 1 || fee.flightTypeClassification == 3));
    }

    private determineDisplayType(): void {
        if (!this.feesAndTaxes) {
            return;
        }

        let hasDepartureTypeRule: boolean = false;
        let hasFlightTypeRule: boolean = false;

        this.feesAndTaxes.forEach((fee) => {
            if (fee.departureType == 1 || fee.departureType == 2) {
                hasDepartureTypeRule = true;
            }
            if (fee.flightTypeClassification == 1 || fee.flightTypeClassification == 2) {
                hasFlightTypeRule = true;
            }
        });

        if (!hasDepartureTypeRule && !hasFlightTypeRule) {
            this.priceBreakdownDisplayType = PriceBreakdownDisplayTypes.SingleColumnAllFlights;
        } else if (!hasDepartureTypeRule && hasFlightTypeRule) {
            this.priceBreakdownDisplayType = PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly;
        } else if (hasDepartureTypeRule && !hasFlightTypeRule) {
            this.priceBreakdownDisplayType = PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly;
        } else if (hasDepartureTypeRule && hasFlightTypeRule) {
            this.priceBreakdownDisplayType = PriceBreakdownDisplayTypes.FourColumnsAllRules;
        }
    }

    private loadCalculations() {
        forkJoin(this.loadInternationalCommercialPricing(),
            this.loadInternationalPrivatePricing(),
            this.loadDomesticCommercialPricing(),
            this.loadDomesticPrivatePricing()).subscribe((responseList:
            any[]) => {
            this.NgxUiLoader.stopLoader(this.priceBreakdownLoader);
            if (!responseList) {
                alert('There was a problem fetching prices.');
            }
            this.internationalCommercialPricing = responseList[0];
            this.internationalPrivatePricing = responseList[1];
            this.domesticCommercialPricing = responseList[2];
                this.domesticPrivatePricing = responseList[3];
                this.feeAndTaxCloneForPopOver = [];
                this.feesAndTaxes.forEach(val => this.feeAndTaxCloneForPopOver.push(Object.assign({}, val)));
            if (this.feeAndTaxBreakdown) {
                this.feeAndTaxBreakdown.performRecalculation();
            }
            this.calculationsComplated.emit([
                this.internationalCommercialPricing, this.internationalPrivatePricing, this.domesticCommercialPricing,
                this.domesticPrivatePricing
            ]);
        });
    }

    private loadInternationalCommercialPricing(): Observable<Object> {
        return this.fboPricesService.getFuelPricesForCompany({
            flightTypeClassification: FlightTypeClassifications.Commercial,
            departureType: ApplicableTaxFlights.InternationalOnly,
            icao: this.sharedService.currentUser.icao,
            fboid: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            pricingTemplateId: this.priceTemplateId,
            customerInfoByGroupId: this.customerInfoByGroupId,
            tailNumber: this.tailNumber
        });
    }

    private loadInternationalPrivatePricing(): Observable<Object> {
        return this.fboPricesService.getFuelPricesForCompany({
            flightTypeClassification: FlightTypeClassifications.Private,
            departureType: ApplicableTaxFlights.InternationalOnly,
            icao: this.sharedService.currentUser.icao,
            fboid: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            pricingTemplateId: this.priceTemplateId,
            customerInfoByGroupId: this.customerInfoByGroupId,
            tailNumber: this.tailNumber
        });
    }

    private loadDomesticCommercialPricing(): Observable<Object> {
        return this.fboPricesService.getFuelPricesForCompany({
            flightTypeClassification: FlightTypeClassifications.Commercial,
            departureType: ApplicableTaxFlights.DomesticOnly,
            icao: this.sharedService.currentUser.icao,
            fboid: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            pricingTemplateId: this.priceTemplateId,
            customerInfoByGroupId: this.customerInfoByGroupId,
            tailNumber: this.tailNumber
        });
    }

    private loadDomesticPrivatePricing(): Observable<Object> {
        return this.fboPricesService.getFuelPricesForCompany({
            flightTypeClassification: FlightTypeClassifications.Private,
            departureType: ApplicableTaxFlights.DomesticOnly,
            icao: this.sharedService.currentUser.icao,
            fboid: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            replacementFeesAndTaxes: this.feesAndTaxes,
            pricingTemplateId: this.priceTemplateId,
            customerInfoByGroupId: this.customerInfoByGroupId,
            tailNumber: this.tailNumber
        });
    }
}
