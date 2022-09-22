import {
    AfterViewInit,
    Component,
    Input,
    OnDestroy,
    OnInit,
    QueryList,
    ViewChild,
    ViewChildren,
    Output,
    EventEmitter,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { interval, Observable, Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { DateTimeService } from '../../../services/datetime.service';

import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
// Services
import { FboairportsService } from '../../../services/fboairports.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { TemporaryAddOnMarginService } from '../../../services/temporaryaddonmargin.service';

// Components
import { FboPricesSelectDefaultTemplateComponent } from '../fbo-prices-select-default-template/fbo-prices-select-default-template.component';
import { FeeAndTaxSettingsDialogComponent } from '../fee-and-tax-settings-dialog/fee-and-tax-settings-dialog.component';
import { FeeAndTaxBreakdownComponent } from '../../../shared/components/fee-and-tax-breakdown/fee-and-tax-breakdown.component';
import { ProceedConfirmationComponent } from '../../../shared/components/proceed-confirmation/proceed-confirmation.component';

//Models
import { PricingUpdateGridViewModel as pricingUpdateGridViewModel } from '../../../models/pricing/pricing-update-grid-viewmodel';
export interface DefaultTemplateUpdate {
    currenttemplate: number;
    newtemplate: number;
    fboid: number;
}

//export interface TemporaryAddOnMargin {
//    id?: any;
//    fboId?: any;
//    EffectiveFrom?: any;
//    EffectiveTo?: any;
//    MarginJet?: any;
//}



@Component({
    selector: 'app-fbo-prices-update-generator',
    styleUrls: ['./fbo-prices-update-generator.component.scss'],
    templateUrl: './fbo-prices-update-generator.component.html',
})
export class FboPricesUpdateGeneratorComponent implements OnInit {
    @Input() isMember?: boolean;
    @Input() isCsr?: boolean;
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;
    @ViewChild('retailFeeAndTaxBreakdown')
    private retailFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;
    @ViewChild('costFeeAndTaxBreakdown')
    private costFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;

    public fboPricesUpdateData: any[];
    public fboPricesUpdateGridData: any[];
    public fboPricesUpdateGridItem: pricingUpdateGridViewModel;

    // Members
    pricingLoader = 'pricing-loader';
    currentPrices: any[];
    //currentPricingEffectiveFrom = new Date();
    //currentPricingEffectiveTo: any;
    stagedPrices: any[];
    //currentDate = new Date();
    pricingTemplates: any[];
    feesAndTaxes: Array<any>;
    //isLoadingRetail = false;
    //isLoadingCost = false;
    //isLoadingStagedRetail = false;
    //isLoadingStagedCost = false;
    //priceEntryError = '';
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
    currentFboPriceSafRetail: any;
    currentFboPriceSafCost: any;
    //tailNumberFormControlSubscription: any;
    priceShiftSubscription: any;
    priceShiftLoading: boolean;

    changedSubscription: any;
    subscriptions: Subscription[] = [];
    timezone: string = "";
    expirationDate: any;
    localDateTime: any;

    constructor(
       private feesAndTaxesService: FbofeesandtaxesService,
       private fboPricesService: FbopricesService,
       private pricingTemplateService: PricingtemplatesService,
       private sharedService: SharedService,
       private fboairportsService: FboairportsService,
       private customCustomerService: CustomcustomertypesService,
       private NgxUiLoader: NgxUiLoaderService,
       private fboPricesSelectDefaultTemplateDialog: MatDialog,
       private fboFeesAndTaxesDialog: MatDialog,
        private dateTimeService: DateTimeService,
       private notification: MatDialog,
        //    private proceedConfirmationDialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.resetAll();
    }

    ngAfterViewInit(): void {
        this.changedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.resetAll();
                }
                else if (message === SharedEvents.fboProductPreferenceChangeEvent || message === SharedEvents.fboPricesClearedEvent) {
                    this.loadStagedFboPrices();
                }
                else if (message === SharedEvents.menuTooltipShowedEvent) {
                    this.tooltipIndex = this.priceTooltips.length - 1;
                    this.showTooltips();
                }
            });
    }

    ngOnDestroy(): void {
        this.changedSubscription?.unsubscribe();
        this.priceShiftSubscription?.unsubscribe();
        this.subscriptions.forEach((subscription) =>
            subscription.unsubscribe()
        );
    }

    resetAll() {
        this.currentPrices = undefined;
        this.stagedPrices = undefined;
        this.loadAllPrices();
        this.loadFeesAndTaxes();
        this.fixCustomCustomerTypes();
        this.checkDefaultTemplate();
    }

    // Methods
    public onSubmitFboPrice(event): void {
        if (event.pricePap > 0) {
            event.fboId = this.sharedService.currentUser.fboId;
            event.effectiveFrom = moment(event.effectiveFrom).format("MM/DD/YYYY HH:mm");
            event.effectiveTo = moment(event.effectiveTo).format("MM/DD/YYYY HH:mm");

            var _this = this;
            this.fboPricesService.updatePricingGenerator(event)
                .subscribe((data: any) => {
                    this.fboairportsService.getLocalDateTime(this.sharedService.currentUser.fboId).subscribe((localDateTime: any) => {
                        _this.localDateTime = localDateTime;
                        _this.subscriptions.push(
                            _this.loadCurrentFboPrices().subscribe(() => {
                            }));

                        var currentUpdatedPrice = _this.fboPricesUpdateGridData.findIndex(f => {
                            return f.product === event.product;
                        });

                        if (data.status == "published") {
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].effectiveFrom = moment(new Date(event.effectiveTo).getTime() + 1 * 60000).toDate();
                            var effectiveFromDate = moment(_this.fboPricesUpdateGridData[currentUpdatedPrice].effectiveFrom).format("MM-DD-YYYY");
                            this.dateTimeService.getNextTuesdayDate(effectiveFromDate).subscribe((nextTuesdayDate: any) => {
                                _this.fboPricesUpdateGridData[currentUpdatedPrice].effectiveTo = moment(nextTuesdayDate).toDate();
                            });

                            _this.fboPricesUpdateGridData[currentUpdatedPrice].pricePap = null;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].priceCost = null;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].oidPap = 0;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].oidCost = 0;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].status = "Editing";
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].submitStatus = "Stage";
                        }
                        else {
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].oidPap = data.oidPap;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].oidCost = data.oidCost;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].effectiveFrom = event.effectiveFrom;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].effectiveTo = event.effectiveTo;
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].status = "Staged";
                            _this.fboPricesUpdateGridData[currentUpdatedPrice].isEdit = false;
                        }

                        _this.notification.open(NotificationComponent, {
                            data: {
                                text: "Your prices have been " + data.status + "!",
                                title: 'Success',
                            },
                        });
                    });
                });
        }
    }

    //checkDatesForStaging() {
    //    const effectiveFrom = moment(this.stagedPricingEffectiveFrom);

    //    if (this.stagedJetRetail || this.stagedJetCost) {
    //        if (
    //            effectiveFrom >
    //            moment(this.currentFboPriceJetARetail.effectiveTo).add(
    //                1,
    //                'days'
    //            )
    //        ) {
    //            const dialogRef = this.proceedConfirmationDialog.open(
    //                ProceedConfirmationComponent,
    //                {
    //                    autoFocus: false,
    //                    data: {
    //                        buttonText: 'Proceed',
    //                        description:
    //                            "This effective date is after your current prices' expiration date.",
    //                        title: ' ',
    //                    },
    //                }
    //            );

    //            dialogRef.afterClosed().subscribe((result) => {
    //                if (!result) {
    //                    return;
    //                }
    //                this.updateStagedPricing();
    //            });
    //        } else if (
    //            effectiveFrom < this.currentFboPriceJetARetail.effectiveTo
    //        ) {
    //            const dialogRef = this.proceedConfirmationDialog.open(
    //                ProceedConfirmationComponent,
    //                {
    //                    autoFocus: false,
    //                    data: {
    //                        buttonText:
    //                            'Make effective date match current expiration date',
    //                        description:
    //                            'Your staged price will take effect before your current price expires.',
    //                        title: ' ',
    //                    },
    //                }
    //            );

    //            dialogRef.afterClosed().subscribe((result) => {
    //                if (!result) {
    //                    return;
    //                }
    //                this.stagedPricingEffectiveFrom = moment(
    //                    this.currentFboPriceJetARetail.effectiveTo
    //                )
    //                    .add(1, 'days')
    //                    .toDate();
    //                this.updateStagedPricing();
    //            });
    //        } else {
    //            this.updateStagedPricing();
    //        }
    //    } else {
    //        this.updateStagedPricing();
    //    }
    //}

    //canStagePricing() {
    //    return (
    //        this.stagedPricingEffectiveFrom &&
    //        this.stagedPricingEffectiveTo &&
    //        (this.stagedJetRetail || this.stagedJetCost) &&
    //        !this.priceEntryError.length
    //    );
    //}

    //jetACostExists() {
    //    return (
    //        this.currentFboPriceJetACost &&
    //        this.currentFboPriceJetACost.price > 0
    //    );
    //}

    //jetARetailExists() {
    //    return (
    //        this.currentFboPriceJetARetail &&
    //        this.currentFboPriceJetARetail.price > 0
    //    );
    //}

    //stagedJetARetailExists() {
    //    return (
    //        this.stagedFboPriceJetARetail &&
    //        this.stagedFboPriceJetARetail.price > 0
    //    );
    //}

    //stagedJetACostExists() {
    //    return (
    //        this.stagedFboPriceJetACost && this.stagedFboPriceJetACost.price > 0
    //    );
    //}

    editFeesAndTaxes(): void {
        const dialogRef = this.fboFeesAndTaxesDialog.open(
            FeeAndTaxSettingsDialogComponent,
            {
                disableClose: true,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
        });
    }

    // Private Methods
    private loadCurrentFboPrices() {
        return new Observable((observer) => {
            this.subscriptions.push(
                this.fboPricesService
                    .getFbopricesByFboIdCurrent(
                        this.sharedService.currentUser.fboId
                    )
                    .subscribe(
                        (data: any) => {
                            this.currentPrices = data;
                            this.currentFboPrice100LLCost =
                                this.getCurrentPriceByProduct('100LL Cost');
                            this.currentFboPrice100LLRetail =
                                this.getCurrentPriceByProduct('100LL Retail');
                            this.currentFboPriceJetACost =
                                this.getCurrentPriceByProduct('JetA Cost');
                            this.currentFboPriceJetARetail =
                                this.getCurrentPriceByProduct('JetA Retail');
                            this.currentFboPriceSafCost =
                                this.getCurrentPriceByProduct('SAF Cost');
                            this.currentFboPriceSafRetail =
                                this.getCurrentPriceByProduct('SAF Retail');


                            //if (this.currentFboPriceJetARetail.effectiveTo) {
                            //    const tempStagedPricingEffectiveFrom = moment(
                            //        this.currentFboPriceJetARetail.effectiveTo
                            //    );
                            //    this.stagedPricingEffectiveFrom = new Date(
                            //        tempStagedPricingEffectiveFrom.format(
                            //            'MM/DD/YYYY'
                            //        )
                            //    );

                            //    this.currentFboPriceJetARetail.effectiveTo =
                            //        moment(
                            //            this.currentFboPriceJetARetail
                            //                .effectiveTo
                            //        ).subtract(1, 'minutes');
                            //}

                            //if (this.currentFboPriceJetACost.effectiveTo) {
                            //    if (!this.stagedPricingEffectiveFrom) {
                            //        this.stagedPricingEffectiveFrom = new Date(
                            //            this.currentFboPriceJetACost.effectiveTo
                            //        );
                            //    }

                            //    this.currentFboPriceJetACost.effectiveTo =
                            //        moment(
                            //            this.currentFboPriceJetACost.effectiveTo
                            //        ).subtract(1, 'minutes');
                            //}

                            //if (data.length > 0) {
                            //    this.TempValueJet = data[0].tempJet;
                            //    this.TempValueId = data[0].tempId;
                            //    this.TempDateFrom = moment(
                            //        data[0].tempDateFrom
                            //    ).toDate();
                            //    this.TempDateTo = moment(
                            //        data[0].tempDateTo
                            //    ).toDate();
                            //}

                            this.sharedService.emitChange('fbo-prices-loaded');
                            this.sharedService.valueChange({
                                JetACost: this.currentFboPriceJetACost.price,
                                JetARetail:
                                    this.currentFboPriceJetARetail.price,
                                SafCost: this.currentFboPriceSafCost.price,
                                SafRetail: this.currentFboPriceSafRetail.price,
                                PriceExpirationSaf: moment(this.currentFboPriceSafRetail.effectiveTo).format("M/D/YY") == "12/31/99" || this.currentFboPriceSafRetail.source == "1" ? "Updated by X1 POS" : "Expires " + moment(this.currentFboPriceSafRetail.effectiveTo).format("M/D/YY @ HH:mm") + " " + this.timezone,
                                PriceExpirationJetA: moment(this.currentFboPriceJetARetail.effectiveTo).format("M/D/YY") == "12/31/99" || this.currentFboPriceJetARetail.source == "1"  ? "Updated by X1 POS" : "Expires " + moment(this.currentFboPriceJetARetail.effectiveTo).format("M/D/YY @ HH:mm") + " " + this.timezone,
                                message: SharedEvents.fboPricesUpdatedEvent,
                            });

                            //this.subscribeToPricingShift();

                            /*observer.next();*/
                            this.NgxUiLoader.stopLoader(this.pricingLoader);
                        },
                        (error: any) => {
                            observer.error(error);
                        }
                    )
            );
        });
    }

    private loadStagedFboPrices() {
        this.fboPricesUpdateGridData = null;
        var _this = this;

        this.fboPricesService
            .getFbopricesByFboIdAllStaged(
                this.sharedService.currentUser.fboId
            )
            .subscribe(
                (data: any) => {
                    this.stagedPrices = data;
                    this.fboPricesUpdateGridData = data;

                    this.fboPricesUpdateGridData.forEach(function (fboPrice) {
                        if (fboPrice.effectiveFrom && (fboPrice.oidPap == 0 || fboPrice.oidPap == undefined)) {
                            if (moment(fboPrice.effectiveTo).format("YYYY") == "9999" || fboPrice.source == "1") {
                                fboPrice.effectiveFrom = moment(fboPrice.effectiveFrom).format("MM/DD/YYYY HH:mm");
                                fboPrice.effectiveTo = "Updated by X1 POS";
                                fboPrice.submitStatus = "Automated";
                                fboPrice.status = "Automated";
                                fboPrice.isEdit = false;
                            }
                            else if (fboPrice.effectiveFrom == "0001-01-01T00:00:00") {
                                fboPrice.effectiveTo = moment(_this.expirationDate).toDate();
                                fboPrice.submitStatus = "Publish";
                                fboPrice.status = "Editing";
                            }
                            else {
                                fboPrice.effectiveTo = moment(fboPrice.effectiveTo).toDate();
                                fboPrice.submitStatus = "Stage";
                                fboPrice.status = "Editing";
                            }

                            if (fboPrice.status != "Automated") {
                                fboPrice.effectiveFrom = moment(moment(fboPrice.effectiveFrom == "0001-01-01T00:00:00" ? _this.localDateTime : fboPrice.effectiveFrom).format("MM/DD/YYYY HH:mm")).toDate();
                                fboPrice.isEdit = true;
                            }
                        }
                        else {
                            fboPrice.effectiveFrom = moment(fboPrice.effectiveFrom).format("MM/DD/YYYY HH:mm");
                            fboPrice.effectiveTo = moment(fboPrice.effectiveTo).format("MM/DD/YYYY HH:mm");
                            fboPrice.status = "Staged";
                            fboPrice.isEdit = false;
                        }
                    });
                });
    }

    private getExpirationDate() {
        return new Observable((observer) => {
            this.subscriptions.push(
                this.fboairportsService.getLocalDateTime(this.sharedService.currentUser.fboId).subscribe((localDateTime: any) => {
                    this.localDateTime = localDateTime;

                    this.dateTimeService.getNextTuesdayDate(moment(this.localDateTime).format("MM-DD-YYYY")).subscribe((nextTuesdayDate: any) => {
                        this.expirationDate = nextTuesdayDate;

                        observer.next();
                    })
                },
                    (error: any) => {
                        observer.error(error);
                    }
                )
            )
        });
    }

    private getTimeZone() {
        return new Observable((observer) => {
            this.subscriptions.push(
                this.fboPricesService
                    .getFbopricesByFboIdCurrent(
                        this.sharedService.currentUser.fboId
                    )
                    .subscribe(
                        (data: any) => {
                            if (this.timezone == "") {
                                var _this = this;
                                this.fboairportsService.getLocalTimeZone(_this.sharedService.currentUser.fboId).subscribe((data: any) => {
                                    this.timezone = data;
                                    _this.loadStagedFboPrices();
                                });
                            }
                            else {
                                this.loadStagedFboPrices();
                            }

                            observer.next();
                        },
                        (error: any) => {
                            observer.error(error);
                        }
                    ));
        });
    }

    private getCurrentPriceByProduct(product) {
        let result = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            oid: 0,
            price:null
        };
        for (const fboPrice of this.currentPrices) {
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
                        .getByFboDefaultTemplate(
                            this.sharedService.currentUser.groupId,
                            this.sharedService.currentUser.fboId
                        )
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
                                        const dialogRef =
                                            this.fboPricesSelectDefaultTemplateDialog.open(
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
                                                    this.updateModel.fboid =
                                                        this.sharedService.currentUser.fboId;
                                                    this.updateModel.newtemplate =
                                                        result;

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
            var toolTip = tooltipsArr[this.tooltipIndex];
            if (toolTip != undefined) {
                tooltipsArr[this.tooltipIndex].open();
                this.tooltipIndex--;
            }
        }, 400);
    }

    private loadFeesAndTaxes() {
        this.feesAndTaxesService
            .getByFbo(this.sharedService.currentUser.fboId)
            .subscribe((response: any) => {
                this.feesAndTaxes = response;
                console.log(response)
                if (this.retailFeeAndTaxBreakdown) {
                    this.retailFeeAndTaxBreakdown.feesAndTaxes =
                        this.feesAndTaxes;
                    this.retailFeeAndTaxBreakdown.performRecalculation();
                }
                if (this.costFeeAndTaxBreakdown) {
                    this.costFeeAndTaxBreakdown.feesAndTaxes =
                        this.feesAndTaxes;
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
            const utcTomorrow = moment
                .utc()
                .add(1, 'days')
                .format('MM/DD/YYYY');

            if (!this.priceShiftLoading) {
                if (this.currentFboPriceJetARetail) {
                    if (
                        this.currentFboPriceJetARetail.effectiveFrom &&
                        this.currentFboPriceJetARetail.effectiveTo
                    ) {
                        const currentRetailEffectiveFrom = moment(
                            this.currentFboPriceJetARetail.effectiveFrom
                        ).format('MM/DD/YYYY');
                        const currentRetailEffectiveTo = moment(
                            this.currentFboPriceJetARetail.effectiveTo
                        ).format('MM/DD/YYYY');
                        if (
                            utcTomorrow !== currentRetailEffectiveFrom &&
                            utcNow === currentRetailEffectiveTo
                        ) {
                            this.loadAllPrices();
                        }
                    }
                    if (
                        this.currentFboPriceJetACost.effectiveFrom &&
                        this.currentFboPriceJetACost.effectiveTo
                    ) {
                        const currentCostEffectiveFrom = moment(
                            this.currentFboPriceJetACost.effectiveFrom
                        ).format('MM/DD/YYYY');
                        const currentCostEffectiveTo = moment(
                            this.currentFboPriceJetACost.effectiveTo
                        ).format('MM/DD/YYYY');
                        if (
                            utcTomorrow !== currentCostEffectiveFrom &&
                            utcNow === currentCostEffectiveTo
                        ) {
                            this.loadAllPrices();
                        }
                    }
                }
                //if (this.stagedFboPriceJetARetail) {
                //    if (this.stagedFboPriceJetARetail.effectiveFrom) {
                //        const stagedRetailEffectiveFrom = moment(
                //            this.stagedFboPriceJetARetail.effectiveFrom
                //        ).format('MM/DD/YYYY');
                //        if (utcNow === stagedRetailEffectiveFrom) {
                //            this.loadAllPrices();
                //        }
                //    }
                //    if (this.stagedFboPriceJetACost.effectiveFrom) {
                //        const stagedCostEffectiveFrom = moment(
                //            this.stagedFboPriceJetACost.effectiveFrom
                //        ).format('MM/DD/YYYY');
                //        if (utcNow === stagedCostEffectiveFrom) {
                //            this.loadAllPrices();
                //        }
                //    }
                //}
            }
        });
    }

    private loadAllPrices() {
        this.priceShiftLoading = true;
        this.NgxUiLoader.startLoader(this.pricingLoader);
        this.getExpirationDate().subscribe((currentExpirationDate: any) => {
        this.subscriptions.push(
            this.getTimeZone().subscribe(() => {
                this.subscriptions.push(
                    this.loadCurrentFboPrices().subscribe(() => {
                        this.priceShiftLoading = false;
                    }))
            }));
        });
    }

    private fixCustomCustomerTypes() {
        this.pricingTemplateService
            .fixCustomCustomerTypes(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId)
            .subscribe((response: any) => {
             
            });
    }
}
