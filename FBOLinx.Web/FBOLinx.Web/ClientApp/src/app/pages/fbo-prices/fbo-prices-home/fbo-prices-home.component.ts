import { AfterViewInit, Component, Input, OnDestroy, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { interval, Observable } from 'rxjs';
import * as moment from 'moment';

// Services
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { TemporaryAddOnMarginService } from '../../../services/temporaryaddonmargin.service';
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
import { SharedService } from '../../../layouts/shared-service';


// Components
import { FboPricesSelectDefaultTemplateComponent } from '../fbo-prices-select-default-template/fbo-prices-select-default-template.component';
import { FeeAndTaxSettingsDialogComponent } from '../fee-and-tax-settings-dialog/fee-and-tax-settings-dialog.component';
import { FeeAndTaxBreakdownComponent } from '../../../shared/components/fee-and-tax-breakdown/fee-and-tax-breakdown.component';
import { PriceCheckerComponent } from '../../../shared/components/price-checker/price-checker.component';
import { ProceedConfirmationComponent } from '../../../shared/components/proceed-confirmation/proceed-confirmation.component';

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

@Component({
    selector: 'app-fbo-prices-home',
    templateUrl: './fbo-prices-home.component.html',
    styleUrls: [ './fbo-prices-home.component.scss' ]
})
export class FboPricesHomeComponent implements OnInit, OnDestroy, AfterViewInit {
    @Input() isCsr?: boolean;
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;
    @ViewChild('retailFeeAndTaxBreakdown') private retailFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;
    @ViewChild('costFeeAndTaxBreakdown') private costFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;
    @ViewChild('priceChecker') private priceChecker: PriceCheckerComponent;
    // Members
    pricingLoader = 'pricing-loader';
    stagedPricingLoader = 'staged-pricing-loader';
    currentPrices: any[];
    currentPricingEffectiveFrom = new Date();
    currentPricingEffectiveTo: any;
    stagedPrices: any[];
    stagedPricingEffectiveFrom = new Date();
    stagedPricingEffectiveTo: any;
    currentDate = new Date();
    pricingTemplates: any[];
    feesAndTaxes: Array<any>;
    jtCost: any;
    jtRetail: any;
    stagedJetRetail: any;
    stagedJetCost: any;
    isLoadingRetail = false;
    isLoadingCost = false;
    isLoadingStagedRetail = false;
    isLoadingStagedCost = false;
    TempValueJet: number;
    TempValueId = 0;
    TempDateFrom: Date;
    TempDateTo: Date;
    isUpdatingMargin = false;
    isClearingMargin = false;
    priceEntryError = '';
    stagedPriceEntryError = '';
    tooltipIndex = 0;
    updateModel: DefaultTemplateUpdate = {
        currenttemplate: 0,
        fboid: 0,
        newtemplate: 0,
    };
    // Additional Members for direct reference (date filtering/restrictions)
    currentFboPriceJetARetail: any;
    currentFboPriceJetACost: any;
    currentFboPrice100LLRetail: any;
    currentFboPrice100LLCost: any;
    stagedFboPriceJetACost: any;
    stagedFboPriceJetARetail: any;
    stagedFboPrice100LL: any;
    locationChangedSubscription: any;
    tooltipSubscription: any;
    tailNumberFormControlSubscription: any;
    priceShiftSubscription: any;
    priceShiftLoading: boolean;

    constructor(
        private feesAndTaxesService: FbofeesandtaxesService,
        private fboPricesService: FbopricesService,
        private pricingTemplateService: PricingtemplatesService,
        private sharedService: SharedService,
        private customCustomerService: CustomcustomertypesService,
        private temporaryAddOnMargin: TemporaryAddOnMarginService,
        private NgxUiLoader: NgxUiLoaderService,
        private fboPricesSelectDefaultTemplateDialog: MatDialog,
        private fboFeesAndTaxesDialog: MatDialog,
        private proceedConfirmationDialog: MatDialog
    ) {

    }

    ngOnInit(): void {
        this.resetAll();
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
        if (this.tailNumberFormControlSubscription) {
            this.tailNumberFormControlSubscription.unsubscribe();
        }
        if (this.priceShiftSubscription) {
            this.priceShiftSubscription.unsubscribe();
        }
    }

    resetAll() {
        this.currentPrices = undefined;
        this.stagedPrices = undefined;
        this.NgxUiLoader.startLoader(this.pricingLoader);
        this.loadCurrentFboPrices().subscribe(() => {
            this.loadStagedFboPrices().subscribe(() => {
                this.NgxUiLoader.stopLoader(this.pricingLoader);
            });
        });
        this.loadFeesAndTaxes();
        this.checkDefaultTemplate();
    }

    // Methods
    suspendCostPricing(price) {
        if (this.isLoadingCost) {
            return;
        }
        this.isLoadingCost = true;
        this.suspendPricing(price.oid, 'cost');
    }

    suspendRetailPricing(price) {
        if (this.isLoadingRetail) {
            return;
        }
        this.isLoadingRetail = true;
        this.suspendPricing(price.oid, 'retail');
    }

    suspendStagedJetPricing(price) {
        if (this.isLoadingStagedCost) {
            return;
        }
        this.isLoadingStagedCost = true;
        this.suspendPricing(price.oid, 'stagedcost');
    }

    suspendStagedRetailPricing(price) {
        if (this.isLoadingStagedRetail) {
            return;
        }
        this.isLoadingStagedRetail = true;
        this.suspendPricing(price.oid, 'stagedretail');
    }

    fboPriceRequiresUpdate(price: number, vl: string) {
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

    checkStagedPriceBeforeUpdating(price: number, vl: string) {
        if (vl === 'JetA Retail') {
            this.stagedJetRetail = price;
        }
        if (vl === 'JetA Cost') {
            this.stagedJetCost = price;
        }
        if (this.stagedJetRetail) {
            if (this.stagedJetRetail < 0) {
                this.stagedPriceEntryError =
                    'Retail value cannot be negative.';
            } else {
                this.stagedPriceEntryError = '';
            }
        }

        if (this.stagedJetCost) {
            if (this.stagedJetCost < 0) {
                this.stagedPriceEntryError +=
                    'Cost value cannot be negative.';
            } else if (this.stagedJetRetail > 0 && this.stagedJetCost > this.stagedJetRetail) {
                this.stagedPriceEntryError +=
                    'Your cost value is higher than the retail price.';
            } else {
                this.stagedPriceEntryError = '';
            }
        }
    }

    updatePricing() {
        const effectiveFrom = moment(this.currentPricingEffectiveFrom).format('MM/DD/YYYY');
        const effectiveTo = moment(this.currentPricingEffectiveTo).add(1, 'day').format('MM/DD/YYYY');
        const newPrices = [];
        let isRetailExist = false;
        let isCostExist = false;
        for (const price of this.currentPrices) {
            if (price.product === 'JetA Retail') {
                isRetailExist = true;
            } else if (price.product === 'Jet A Cost') {
                isCostExist = true;
            }

            if (price.product === 'JetA Retail' && this.jtRetail > 0) {
                price.oid = 0;
                price.price = this.jtRetail;
                price.effectiveFrom = effectiveFrom;
                price.effectiveTo = effectiveTo;
                newPrices.push(price);
            } else if (price.product === 'JetA Cost' && this.jtCost > 0) {
                price.oid = 0;
                price.price = this.jtCost;
                price.effectiveFrom = effectiveFrom;
                price.effectiveTo = effectiveTo;
                newPrices.push(price);
            }
        }

        if (!isRetailExist) {
            const price = {
                oid: 0,
                price: this.jtRetail,
                product: 'JetA Retail',
                effectiveFrom,
                effectiveTo,
                fboid: this.sharedService.currentUser.fboId
            };
            newPrices.push(price);
        }

        if (!isCostExist) {
            const price = {
                oid: 0,
                price: this.jtCost,
                product: 'JetA Cost',
                effectiveFrom,
                effectiveTo,
                fboid: this.sharedService.currentUser.fboId
            };
            newPrices.push(price);
        }
        this.savePriceChangesAll(newPrices);

        this.jtRetail = '';
        this.jtCost = '';
        this.currentPricingEffectiveTo = null;
    }

    canUpdatePricing() {
        return this.currentPricingEffectiveTo && (this.jtRetail || this.jtCost) && !this.priceEntryError.length;
    }

    checkDatesForStaging() {
        const effectiveFrom = moment(this.stagedPricingEffectiveFrom);

        if (this.stagedJetRetail || this.stagedJetCost) {
            if (effectiveFrom > moment(this.currentFboPriceJetARetail.effectiveTo).add(1, 'days')) {
                const dialogRef = this.proceedConfirmationDialog.open(
                    ProceedConfirmationComponent,
                    {
                        data: {
                            description: 'This effective date is after your current prices\' expiration date.',
                            buttonText: 'Proceed',
                            title: ' '
                        },
                        autoFocus: false
                    }
                );

                dialogRef.afterClosed().subscribe((result) => {
                    if (!result) {
                        return;
                    }
                    this.updateStagedPricing();
                });
            } else if (effectiveFrom < this.currentFboPriceJetARetail.effectiveTo) {
                const dialogRef = this.proceedConfirmationDialog.open(
                    ProceedConfirmationComponent,
                    {
                        data: {
                            description: 'Your staged price will take effect before your current price expires.',
                            buttonText: 'Make effective date match current expiration date',
                            title: ' '
                        },
                        autoFocus: false
                    }
                );

                dialogRef.afterClosed().subscribe((result) => {
                    if (!result) {
                        return;
                    }
                    this.stagedPricingEffectiveFrom = moment(this.currentFboPriceJetARetail.effectiveTo).add(1, 'days').toDate();
                    this.updateStagedPricing();
                });
            } else {
                this.updateStagedPricing();
            }
        } else {
            this.updateStagedPricing();
        }
    }

    canStagePricing() {
        return this.stagedPricingEffectiveFrom &&
            this.stagedPricingEffectiveTo &&
            (this.stagedJetRetail || this.stagedJetCost)
            && !this.priceEntryError.length;
    }

    marginValueChanged(margin: number) {
        this.TempValueJet = margin;
    }

    updateMargin() {
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

    canUpdateMargin() {
        return this.TempDateFrom && this.TempDateTo && this.TempValueJet;
    }

    clearMargin() {
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

    jetACostExists() {
        return this.currentFboPriceJetACost && this.currentFboPriceJetACost.price > 0;
    }

    jetARetailExists() {
        return this.currentFboPriceJetARetail && this.currentFboPriceJetARetail.price > 0;
    }

    stagedJetARetailExists() {
        return this.stagedFboPriceJetARetail && this.stagedFboPriceJetARetail.price > 0;
    }

    stagedJetACostExists() {
        return this.stagedFboPriceJetACost && this.stagedFboPriceJetACost.price > 0;
    }

    tooltipHidden() {
        const tooltipsArr = this.priceTooltips.toArray();
        if (this.tooltipIndex >= 0) {
            setTimeout(() => {
                tooltipsArr[this.tooltipIndex].open();
                this.tooltipIndex--;
            }, 400);
        }
    }

    editFeesAndTaxes(): void {
        const dialogRef = this.fboFeesAndTaxesDialog.open(
            FeeAndTaxSettingsDialogComponent, {
                disableClose: true
            }
        );

        dialogRef
            .afterClosed()
            .subscribe((result) => {
                this.loadFeesAndTaxes();
                if (this.priceChecker) {
                    this.priceChecker.refresh();
                }
                if (!result) {
                    return;
                }
            });
    }

    // Private Methods
    private suspendPricing(oid, product) {
        if (product.indexOf('staged') > -1) {
            this.fboPricesService
                .suspendPricing(oid)
                .subscribe(() => {
                    this.loadStagedFboPrices().subscribe(() => {
                        if (product === 'stagedretail') {
                            this.isLoadingStagedRetail = false;
                        } else {
                            this.isLoadingStagedCost = false;
                        }
                    });
                });
        } else {
            this.fboPricesService
                .suspendPricing(oid)
                .subscribe(() => {
                    this.loadCurrentFboPrices().subscribe(() => {
                        if (product === 'retail') {
                            this.isLoadingRetail = false;
                        }
                        this.isLoadingCost = false;
                    });
                });
        }
    }

    private loadCurrentFboPrices() {
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
                        const tempStagedPricingEffectiveFrom = moment(this.currentFboPriceJetARetail.effectiveTo);
                        this.stagedPricingEffectiveFrom = new Date(tempStagedPricingEffectiveFrom.format('MM/DD/YYYY'));

                        this.currentFboPriceJetARetail.effectiveTo =
                            moment(this.currentFboPriceJetARetail.effectiveTo).subtract(1, 'minutes');
                    }

                    if (this.currentFboPriceJetACost.effectiveTo) {
                        if (!this.stagedPricingEffectiveFrom) {
                            this.stagedPricingEffectiveFrom = new Date(this.currentFboPriceJetACost.effectiveTo);
                        }

                        this.currentFboPriceJetACost.effectiveTo =
                            moment(this.currentFboPriceJetACost.effectiveTo).subtract(1, 'minutes');
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

                    this.subscribeToPricingShift();

                    observer.next();
                }, (error: any) => {
                    observer.error(error);
                });
        });
    }

    private loadStagedFboPrices() {
        return new Observable((observer) => {
            this.fboPricesService
                .getFbopricesByFboIdStaged(this.sharedService.currentUser.fboId)
                .subscribe((data: any) => {
                    this.stagedPrices = data;
                    this.stagedFboPriceJetACost = this.getStagedPriceByProduct('JetA Cost');
                    this.stagedFboPriceJetARetail = this.getStagedPriceByProduct('JetA Retail');
                    this.subscribeToPricingShift();
                    observer.next();
                }, (error: any) => {
                    observer.error(error);
                });
        });
    }

    private updateStagedPricing() {
        const effectiveFrom = moment(this.stagedPricingEffectiveFrom).format('MM/DD/YYYY');
        const effectiveTo = moment(this.stagedPricingEffectiveTo).format('MM/DD/YYYY');

        const newPrices = [];
        let addRetail = true;
        let addCost = true;
        for (const price of this.stagedPrices) {
            if (price.product === 'JetA Retail' && this.stagedJetRetail > 0) {
                price.price = this.stagedJetRetail;
                price.effectiveFrom = effectiveFrom;
                price.effectiveTo = effectiveTo;
                newPrices.push(price);
                addRetail = false;
            } else if (price.product === 'JetA Cost' && this.stagedJetCost > 0) {
                price.price = this.stagedJetCost;
                price.effectiveFrom = effectiveFrom;
                price.effectiveTo = effectiveTo;
                newPrices.push(price);
                addCost = false;
            }
        }

        if (addRetail) {
            const price = {
                oid: 0,
                price: this.stagedJetRetail,
                product: 'JetA Retail',
                effectiveFrom,
                effectiveTo,
                fboid: this.sharedService.currentUser.fboId
            };
            newPrices.push(price);
        }

        if (addCost) {
            const price = {
                oid: 0,
                price: this.stagedJetCost,
                product: 'JetA Cost',
                effectiveFrom,
                effectiveTo,
                fboid: this.sharedService.currentUser.fboId
            };
            newPrices.push(price);
        }

        this.saveStagedPriceChangesAll(newPrices);

        this.stagedJetRetail = '';
        this.stagedJetCost = '';

        if (this.currentFboPriceJetARetail.effectiveTo) {
            const tempStagedPricingEffectiveFrom = moment(this.currentFboPriceJetARetail.effectiveTo).add(1, 'days');
            this.stagedPricingEffectiveFrom = new Date(tempStagedPricingEffectiveFrom.format('MM/DD/YYYY'));
        } else if (!this.stagedPricingEffectiveFrom && this.currentFboPriceJetACost.effectiveTo) {
            const tempStagedPricingEffectiveFrom = moment(this.currentFboPriceJetACost.effectiveTo).add(1, 'days');
            this.stagedPricingEffectiveFrom = new Date(tempStagedPricingEffectiveFrom.format('MM/DD/YYYY'));
        } else {
            const tempStagedPricingEffectiveFrom = moment(this.stagedFboPriceJetARetail.effectiveTo).add(1, 'days');
            this.stagedPricingEffectiveFrom = new Date(tempStagedPricingEffectiveFrom.format('MM/DD/YYYY'));
        }

        this.stagedPricingEffectiveTo = null;
    }

    private savePriceChangesAll(price) {
        this.currentPrices = null;
        this.NgxUiLoader.startLoader(this.pricingLoader);
        this.fboPricesService
            .checkifExistFboPrice(this.sharedService.currentUser.fboId, price)
            .subscribe(() => {
                this.loadCurrentFboPrices().subscribe(() => {
                    this.NgxUiLoader.stopLoader(this.pricingLoader);
                });
            });
    }

    private saveStagedPriceChangesAll(price) {
        this.stagedPrices = null;
        this.NgxUiLoader.startLoader(this.stagedPricingLoader);
        this.fboPricesService
            .checkifExistStagedFboPrice(this.sharedService.currentUser.fboId, price)
            .subscribe(() => {
                this.loadStagedFboPrices().subscribe(() => {
                    this.NgxUiLoader.stopLoader(this.stagedPricingLoader);
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

    private getStagedPriceByProduct(product) {
        let result = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            oid: 0,
        };
        for (const fboPrice of this.stagedPrices) {
            if (fboPrice.product === product) {
                result = fboPrice;
            }
        }
        return result;
    }

    private checkDefaultTemplate() {
        this.pricingTemplateService
            .checkdefaultpricingtemplates(this.sharedService.currentUser.fboId)
            .subscribe((response: any) => {
                if (!response) {
                    this.pricingTemplateService
                        .getByFboDefaultTemplate(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId)
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
                                            FboPricesSelectDefaultTemplateComponent, {
                                                data: this.pricingTemplates,
                                                disableClose: true,
                                            }
                                        );

                                        dialogRef
                                            .afterClosed()
                                            .subscribe((result) => {
                                                if (result !== 'cancel') {
                                                    this.updateModel.currenttemplate = 0;
                                                    this.updateModel.fboid = this.sharedService.currentUser.fboId;
                                                    this.updateModel.newtemplate = result;

                                                    this.customCustomerService
                                                        .updateDefaultTemplate(
                                                            this.updateModel
                                                        )
                                                        .subscribe(() => {
                                                        });
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

    private loadFeesAndTaxes() {
        this.feesAndTaxesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((response: any) => {
            this.feesAndTaxes = response;
            if (this.retailFeeAndTaxBreakdown) {
                this.retailFeeAndTaxBreakdown.feesAndTaxes = this.feesAndTaxes;
                this.retailFeeAndTaxBreakdown.performRecalculation();
            }
            if (this.costFeeAndTaxBreakdown) {
                this.costFeeAndTaxBreakdown.feesAndTaxes = this.feesAndTaxes;
                this.costFeeAndTaxBreakdown.performRecalculation();
            }
        });
    }

    private subscribeToPricingShift() {
        if (this.priceShiftSubscription) {
            this.priceShiftSubscription.unsubscribe();
        }
        this.priceShiftSubscription = interval(1000).subscribe(() => {
            const utcNow = moment.utc().format('MM/DD/YYYY');
            const utcTomorrow = moment.utc().add(1, 'days').format('MM/DD/YYYY');


            if (!this.priceShiftLoading) {
                if (this.currentFboPriceJetARetail) {
                    if (this.currentFboPriceJetARetail.effectiveFrom && this.currentFboPriceJetARetail.effectiveTo) {
                        const currentRetailEffectiveFrom = moment(this.currentFboPriceJetARetail.effectiveFrom).format('MM/DD/YYYY');
                        const currentRetailEffectiveTo = moment(this.currentFboPriceJetARetail.effectiveTo).format('MM/DD/YYYY');
                        if (utcTomorrow !== currentRetailEffectiveFrom && utcNow === currentRetailEffectiveTo) {
                            this.loadAllPrices();
                        }
                    }
                    if (this.currentFboPriceJetACost.effectiveFrom && this.currentFboPriceJetACost.effectiveTo) {
                        const currentCostEffectiveFrom = moment(this.currentFboPriceJetACost.effectiveFrom).format('MM/DD/YYYY');
                        const currentCostEffectiveTo = moment(this.currentFboPriceJetACost.effectiveTo).format('MM/DD/YYYY');
                        if (utcTomorrow !== currentCostEffectiveFrom && utcNow === currentCostEffectiveTo) {
                            this.loadAllPrices();
                        }
                    }
                }
                if (this.stagedFboPriceJetARetail) {
                    if (this.stagedFboPriceJetARetail.effectiveFrom) {
                        const stagedRetailEffectiveFrom = moment(this.stagedFboPriceJetARetail.effectiveFrom).format('MM/DD/YYYY');
                        if (utcNow === stagedRetailEffectiveFrom) {
                            this.loadAllPrices();
                        }
                    }
                    if (this.stagedFboPriceJetACost.effectiveFrom) {
                        const stagedCostEffectiveFrom = moment(this.stagedFboPriceJetACost.effectiveFrom).format('MM/DD/YYYY');
                        if (utcNow === stagedCostEffectiveFrom) {
                            this.loadAllPrices();
                        }
                    }
                }
            }
        });
    }

    private loadAllPrices() {
        this.priceShiftLoading = true;
        this.loadCurrentFboPrices().subscribe(() => {
            this.loadStagedFboPrices().subscribe(() => {
                this.priceShiftLoading = false;
            });
        });
    }
}
