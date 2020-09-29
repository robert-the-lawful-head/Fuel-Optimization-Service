import {
    Component,
    OnInit,
    OnDestroy,
    AfterViewInit,
    ViewChildren,
    QueryList,
    HostListener,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Observable } from 'rxjs';
import * as moment from 'moment';

// Services
import { FbopricesService } from '../../../services/fboprices.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { TemporaryAddOnMarginService } from '../../../services/temporaryaddonmargin.service';
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
import { SharedService } from '../../../layouts/shared-service';

// Enums
import { ApplicableTaxFlights } from '../../../enums/applicable-tax-flights';
import { FlightTypeClassifications } from '../../../enums/flight-type-classifications';
import { EnumOptions } from '../../../models/enum-options';

// Components
import { FboPricesSelectDefaultTemplateComponent } from '../fbo-prices-select-default-template/fbo-prices-select-default-template.component';
import { FeeAndTaxSettingsDialogComponent } from '../fee-and-tax-settings-dialog/fee-and-tax-settings-dialog.component';

import * as SharedEvents from '../../../models/sharedEvents';

export interface DefaultTemplateUpdate {
    currenttemplate: number;
    newtemplate: number;
    fboid: number;
}

export interface TemporaryAddOnMargin {
    id?: any;
    fboId?: any;
    EffectiveFrom?: any;
    EffectiveTo?: any;
    MarginJet?: any;
}

export interface TailLookupResponse {
    template?: string;
    company?: string;
    makeModel?: string;
    pricingList: Array<any>;
}

@Component({
    selector: 'app-fbo-prices-home',
    templateUrl: './fbo-prices-home.component.html',
  styleUrls: ['./fbo-prices-home.component.scss']
    
})
export class FboPricesHomeComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;
    // Public Members
    public pricingLoader = 'pricing-loader';
    public tailLoader = 'tail-loader';

    public currentPrices: any[];
    public currentPricingEffectiveFrom = new Date();
    public currentPricingEffectiveTo: any;
    public pricingTemplates: any[];
    public jtCost: any;
    public jtRetail: any;
    public isLoadingRetail = false;
    public isLoadingCost = false;
    public tailNumber: string;
    public flightTypeClassification: FlightTypeClassifications = FlightTypeClassifications.Private;
    public departureType: ApplicableTaxFlights = ApplicableTaxFlights.DomesticOnly;
    public strictApplicableTaxFlightOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictApplicableTaxFlightOptions;
    public strictFlightTypeClassificationOptions: Array<EnumOptions.EnumOption> = EnumOptions.strictFlightTypeClassificationOptions;

    public tailLookupInfo: TailLookupResponse;
    public tailLookupError: boolean;

    public TempValueJet: number;
    public TempValueId = 0;
    public TempDateFrom: Date;
    public TempDateTo: Date;
    public isUpdatingMargin = false;
    public isClearingMargin = false;

    public priceEntryError = '';

    public tooltipIndex = 0;

    public updateModel: DefaultTemplateUpdate = {
        currenttemplate: 0,
        fboid: 0,
        newtemplate: 0,
    };

    // Additional Public Members for direct reference (date filtering/restrictions)
    public currentFboPriceJetARetail: any;
    public currentFboPriceJetACost: any;
    public currentFboPrice100LLRetail: any;
    public currentFboPrice100LLCost: any;
    public stagedFboPriceJetA: any[];
    public stagedFboPrice100LL: any;

    public locationChangedSubscription: any;
    public tooltipSubscription: any;

    public layoutChanged: boolean;

    constructor(
        private fboPricesService: FbopricesService,
        private pricingTemplateService: PricingtemplatesService,
        private sharedService: SharedService,
        private customCustomerService: CustomcustomertypesService,
      private temporaryAddOnMargin: TemporaryAddOnMarginService,
        private NgxUiLoader: NgxUiLoaderService,
      private fboPricesSelectDefaultTemplateDialog: MatDialog,
        private fboFeesAndTaxesDialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.resetAll();
        this.onResize({ target: {innerWidth: window.innerWidth }});
    }

    ngAfterViewInit(): void {
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.resetAll();
                }
            }
        );

        this.tooltipSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.menuTooltipShowedEvent) {
                    this.tooltipIndex = this.priceTooltips.length - 1;
                    this.showTooltips();
                }
        });
    }

    ngOnDestroy(): void {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
        if (this.tooltipSubscription) {
            this.tooltipSubscription.unsubscribe();
        }
    }

    resetAll() {
        this.currentPrices = undefined;
        this.NgxUiLoader.startLoader(this.pricingLoader);
        this.loadFboPrices().subscribe(() => {
            this.NgxUiLoader.stopLoader(this.pricingLoader);
        });
        this.checkDefaultTemplate();
    }

    // Public Methods
    public suspendJetPricing() {
        if (this.isLoadingCost) {
            return;
        }
        this.isLoadingCost = true;
        this.fboPricesService
            .suspendJetPricing(this.sharedService.currentUser.fboId)
            .subscribe(() => {
                this.loadFboPrices().subscribe(() => {
                    this.isLoadingCost = false;
                });
            });
    }

    public suspendRetailPricing() {
        if (this.isLoadingRetail) {
            return;
        }
        this.isLoadingRetail = true;
        this.fboPricesService
            .suspendRetailPricing(this.sharedService.currentUser.fboId)
            .subscribe(() => {
                this.loadFboPrices().subscribe(() => {
                    this.isLoadingRetail = false;
                });
            });
    }

    public fboPriceRequiresUpdate(price: number, vl: string) {
        if (vl === 'JetA Retail') {
            this.jtRetail = price;
        }
        if (vl === 'JetA Cost') {
            this.jtCost = price;
        }
        if (this.jtRetail && this.jtCost) {
            if (this.jtCost < 0 || this.jtRetail < 0) {
                this.priceEntryError =
                    'Cost or Retail values cannot be negative.';
            } else if (this.jtCost > this.jtRetail) {
                this.priceEntryError =
                    'Your cost value is higher than the retail price.';
            } else {
                this.priceEntryError = '';
            }
        }
    }

    public updatePricing() {
        const effectiveFrom = moment(this.currentPricingEffectiveFrom).format('MM/DD/YYYY');
        const effectiveTo = moment(this.currentPricingEffectiveTo).format('MM/DD/YYYY');
        const newPrices = [];
        for (const price of this.currentPrices) {
            if (price.product === 'JetA Retail' && this.jtRetail > 0) {
                price.oid = 0;
                price.price = this.jtRetail;
                price.effectiveFrom = effectiveFrom;
                price.effectiveTo = effectiveTo;
                newPrices.push(price);
            }
            if (price.product === 'JetA Cost' && this.jtCost > 0) {
                price.oid = 0;
                price.price = this.jtCost;
                price.effectiveFrom = effectiveFrom;
                price.effectiveTo = effectiveTo;
                newPrices.push(price);
            }
        }
        this.savePriceChangesAll(newPrices);

        this.jtRetail = '';
        this.jtCost = '';
        this.currentPricingEffectiveTo = null;
    }

    public canUpdatePricing() {
        return this.currentPricingEffectiveTo && (this.jtRetail || this.jtCost) && !this.priceEntryError.length;
    }

    public marginValueChanged(margin: number) {
        this.TempValueJet = margin;
    }

    public updateMargin() {
        if (this.isUpdatingMargin) {
            return;
        }
        this.isUpdatingMargin = true;
        const payload: TemporaryAddOnMargin = {
            EffectiveFrom: this.TempDateFrom,
            EffectiveTo: this.TempDateTo,
            MarginJet: this.TempValueJet,
        };
        if (this.TempValueId) {
            payload.id = this.TempValueId;
            this.temporaryAddOnMargin
                .update(payload)
                .subscribe(() => {
                    this.isUpdatingMargin = false;
                });
        } else {
            payload.fboId = this.sharedService.currentUser.fboId;
            this.temporaryAddOnMargin
                .add(payload)
                .subscribe((savedTemplate: TemporaryAddOnMargin) => {
                    this.TempValueId = savedTemplate.id;
                    this.isUpdatingMargin = false;
                });
        }
    }

    public canUpdateMargin() {
        return this.TempDateFrom && this.TempDateTo && this.TempValueJet;
    }

    public clearMargin() {
        if (this.isClearingMargin) {
            return;
        }
        this.isClearingMargin = true;
        this.temporaryAddOnMargin
            .remove(this.TempValueId)
            .subscribe(() => {
                this.TempValueJet = null;
                this.TempValueId = 0;
                this.TempDateFrom = null;
                this.TempDateTo = null;
                this.isClearingMargin = false;
            });
    }

    public jetACostExists() {
        return this.currentFboPriceJetACost && this.currentFboPriceJetACost.price > 0;
    }

    public jetARetailExists() {
        return this.currentFboPriceJetARetail && this.currentFboPriceJetARetail.price > 0;
    }

    public tooltipHidden() {
        const tooltipsArr = this.priceTooltips.toArray();
        if (this.tooltipIndex >= 0) {
            setTimeout(() => {
                tooltipsArr[this.tooltipIndex].open();
                this.tooltipIndex--;
            }, 400);
        }
    }

    public lookupTail() {
        const tailLookupData = {
            icao: this.sharedService.currentUser.icao,
            tailNumber: this.tailNumber,
            fboId: this.sharedService.currentUser.fboId,
          groupId: this.sharedService.currentUser.groupId,

        };
        this.tailLookupError = false;
        this.tailLookupInfo = undefined;
        this.NgxUiLoader.startLoader(this.tailLoader);
        this.fboPricesService.getFuelPricesForCompany(tailLookupData).subscribe((data: TailLookupResponse) => {
            if (data) {
                this.tailLookupInfo = data;
                this.tailLookupError = false;
            } else {
                this.tailLookupError = true;
            }
            this.NgxUiLoader.stopLoader(this.tailLoader);
        }, (err) => {
            this.tailLookupError = true;
            this.NgxUiLoader.stopLoader(this.tailLoader);
        });
  }

  public editFeesAndTaxes(): void {
    const dialogRef = this.fboFeesAndTaxesDialog.open(
      FeeAndTaxSettingsDialogComponent,
      {
        disableClose: true
      }
    );

    dialogRef
      .afterClosed()
      .subscribe((result) => {
        if (!result)
          return;        
      });
  }

    // Private Methods
    private loadFboPrices() {
        return new Observable((observer) => {
            this.fboPricesService
                .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
                .subscribe((data: any) => {
                    this.currentPrices = data;
                    this.currentFboPrice100LLCost = this.getCurrentPriceByProduct('100LL Cost');
                    this.currentFboPrice100LLRetail = this.getCurrentPriceByProduct('100LL Retail');
                    this.currentFboPriceJetACost = this.getCurrentPriceByProduct('JetA Cost');
                    this.currentFboPriceJetARetail = this.getCurrentPriceByProduct('JetA Retail');

                    if (this.currentFboPriceJetARetail.effectiveTo) {
                        this.currentFboPriceJetARetail.effectiveTo =
                            moment(this.currentFboPriceJetARetail.effectiveTo).format('MM/DD/YYYY');
                    }

                    if (this.currentFboPriceJetACost.effectiveTo) {
                        this.currentFboPriceJetACost.effectiveTo =
                            moment(this.currentFboPriceJetACost.effectiveTo).format('MM/DD/YYYY');
                    }

                    if (data.length > 0) {
                        this.TempValueJet = data[0].tempJet;
                        this.TempValueId = data[0].tempId;
                        this.TempDateFrom = moment(data[0].tempDateFrom).toDate();
                        this.TempDateTo = moment(data[0].tempDateTo).toDate();
                    }

                    this.sharedService.emitChange('fbo-prices-loaded');
                    this.sharedService.valueChange({
                        message: SharedEvents.fboPricesUpdatedEvent,
                        JetACost: this.currentFboPriceJetACost.price,
                        JetARetail: this.currentFboPriceJetARetail.price,
                    });

                    observer.next();
                }, (error: any) => {
                    observer.error(error);
                });
        });
    }

    private savePriceChangesAll(price) {
        this.currentPrices = null;
        this.NgxUiLoader.startLoader(this.pricingLoader);
        this.fboPricesService
            .checkifExistFrboPrice(this.sharedService.currentUser.fboId, price)
            .subscribe(() => {
                this.loadFboPrices().subscribe(() => {
                    this.NgxUiLoader.stopLoader(this.pricingLoader);
                });
            });
    }

    private getCurrentPriceByProduct(product) {
        let result = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            oid: 0,
        };
        for (const fboPrice of this.currentPrices) {
            if (fboPrice.product === product) {
                result = fboPrice;
            }
        }
        return result;
    }

    private checkDefaultTemplate() {
        let currentFboId = 0;

        if (this.sharedService.currentUser.fboId !== 0) {
            currentFboId = this.sharedService.currentUser.fboId;
        } else {
            currentFboId = sessionStorage.fboId;
        }

        this.pricingTemplateService
            .checkdefaultpricingtemplates(currentFboId)
            .subscribe((response: any) => {
                if (!response) {
                    this.pricingTemplateService
                        .getByFboDefaultTemplate(currentFboId)
                        .subscribe(
                            (data: any) => {
                                this.pricingTemplates = data;
                                if (this.pricingTemplates) {
                                    let skipForDefault = false;
                                    if (this.pricingTemplates.length === 1) {
                                        if (this.pricingTemplates[0].default) {
                                            skipForDefault = true;
                                        }
                                    }

                                    if (!skipForDefault) {
                                        const dialogRef = this.fboPricesSelectDefaultTemplateDialog.open(
                                            FboPricesSelectDefaultTemplateComponent,
                                            {
                                                data: this.pricingTemplates,
                                                disableClose: true,
                                            }
                                        );

                                        dialogRef
                                            .afterClosed()
                                            .subscribe((result) => {
                                                if (result !== 'cancel') {
                                                    this.updateModel.currenttemplate = 0;
                                                    this.updateModel.fboid = currentFboId;
                                                    this.updateModel.newtemplate = result;

                                                    this.customCustomerService
                                                        .updateDefaultTemplate(
                                                            this.updateModel
                                                        )
                                                        .subscribe(() => { });
                                                }
                                            });
                                    }

                                }
                            },
                            () => {
                                this.pricingTemplates = [];
                            }
                        );
                }
            });
    }

    private showTooltips() {
        setTimeout(() => {
            const tooltipsArr = this.priceTooltips.toArray();
            tooltipsArr[this.tooltipIndex].open();
            this.tooltipIndex--;
        }, 400);
  }

    @HostListener('window:resize', ['$event'])
    private onResize(event: any) {
        if (event.target.innerWidth <= 1425) {
            this.layoutChanged = true;
        } else {
            this.layoutChanged = false;
        }
    }
}
