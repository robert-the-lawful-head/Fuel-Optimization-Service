import {
    Component,
    OnInit,
    Inject,
    ViewChild,
    ViewEncapsulation,
    Output,
    EventEmitter
} from '@angular/core';
import { MatDialog } from '@angular/material';

//Services
import { DistributionService } from '../../../services/distribution.service';
import { FbofeesService } from '../../../services/fbofees.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FbopreferencesService } from '../../../services/fbopreferences.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';
import { TemporaryAddOnMarginService } from '../../../services/temporaryaddonmargin.service';

//Components
import * as moment from 'moment';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';
import { TemporaryAddOnMarginComponent } from '../../../shared/components/temporary-add-on-margin/temporary-add-on-margin.component';

//Components
import { DistributionWizardMainComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component';
import { getDate } from 'date-fns';
import { NiCardComponent } from '../../../ni-components/ni-card/ni-card.component';

export interface temporaryAddOnMargin {
    id: any;
    fboId: any;
    EffectiveFrom: any;
    EffectiveTo: any;
    MarginJet: any;
}
@Component({
    selector: 'app-fbo-prices-home',
    templateUrl: './fbo-prices-home.component.html',
    styleUrls: ['./fbo-prices-home.component.scss'],
    encapsulation: ViewEncapsulation.None
})
/** fbo-prices-home component*/
export class FboPricesHomeComponent implements OnInit {
    @ViewChild(NiCardComponent) niCard: NiCardComponent;
    @Output() priceUpdated = new EventEmitter<any>();
    //Public Members
    public pageTitle: string = 'Pricing';
    public pricingJetARetail: any;
    public pricingJetACost: any;
    public pricing100LLRetail: any;
    public pricing100LLCost: any;
    public currentPrices: any[];
    public currentPircesAll: any[];
    public pom: any[];
    public dateFrom: any;
    public dateTo: any;
    public TempValueJet: any;
    public TempValueAvgas: any;
    public TempValueId: any;
    public TempDateFrom: any;
    public TempDateTo: any;
    public stagedPrices: any[];
    public fboPreferences: any;
    public fboFees: any[];
    public price: any;
    public lcl: any;
    public distributionLog: any[];
    public isLoadingJetARetail: boolean;
    public isLoadingJetACost: boolean;
    public isLoading100LLRetail: boolean;
    public isLoading100LLCost: boolean;
    public isLoadingFboPreferences: boolean;
    public isLoadingFboFees: boolean;
    public requiresUpdate: boolean = true;
    public isSaved: boolean;
    public show: boolean;
    public greska: boolean = false;
    public saveOk: boolean = true;
    public minimumAllowedDate: Date = new Date();
    public minimumStagedEffectiveFrom: Date = new Date();
    public currentPricingEffectiveFrom: Date = new Date();
    public currentPricingEffectiveTo: Date = new Date();
    public stagedPricingEffectiveFrom: Date = new Date();
    public stagedPricingEffectiveTo: Date = new Date();
    public todayDate: Date = new Date();
    public pricingTemplates: any[];
    public activePrice: boolean = false;
    public newRetail: any;
    public newCost: any;
    public jtCost: any;
    public jtRetail: any;
    public priceGroup: number;
    public buttonTextValue: any;
    public costPlusText: string = '';
    public priceEntryError: string = '';

    //Additional Public Members for direct reference (date filtering/restrictions)
    public currentFboPriceJetARetail: any;
    public currentFboPriceJetACost: any;
    public currentFboPrice100LLRetail: any;
    public currentFboPrice100LLCost: any;
    public stagedFboPriceJetA: any[];
    public stagedFboPrice100LL: any;

    public staticCurrentFboPriceJetARetail: any;
    public staticCurrentFboPriceJetACost: any;

    /** fbo-prices-home ctor */
    constructor(
        private distributionService: DistributionService,
        private fboFeesService: FbofeesService,
        private fboPricesService: FbopricesService,
        private fboPreferencesService: FbopreferencesService,
        private pricingTemplateService: PricingtemplatesService,
        private temporaryAddOnMargin: TemporaryAddOnMarginService,
        private sharedService: SharedService,
        public distributionDialog: MatDialog,
        public warningDialog: MatDialog,
        public tempAddOnMargin: MatDialog,
        public deleteFBODialog: MatDialog,
        public notificationDialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.buttonTextValue = 'Update Live Pricing';
        this.loadCurrentFboPrices();
        this.loadFboPreferences();
        this.loadFboFees();
        this.loadDistributionLog();
        this.loadPricingTemplates();
        this.checkCostPlusMargins();
    }

    //Public Methods
    jetChangedHandler(event: temporaryAddOnMargin) {
        this.dateTo = new Date(moment.utc(event.EffectiveTo).toDate());
        var now = new Date();
        now.setHours(0, 0, 0, 0);
        if (this.dateTo >= new Date(moment.utc(now).toDate())) {
            this.TempValueId = event.id;
            this.TempValueJet = event.MarginJet;
            this.TempDateFrom = moment(
                moment.utc(event.EffectiveFrom).toDate()
            ).format('MM/DD/YYYY');
            this.TempDateTo = moment(
                moment.utc(event.EffectiveTo).toDate()
            ).format('MM/DD/YYYY');
        }

        this.show = true;
        this.isSaved = true;
        this.delay(5000).then(any => {
            this.show = false;
            this.isSaved = false;
        });
    }

    pricesSuspended() {
        this.loadCurrentFboPrices();
    }

    public distributePricingClicked() {
        const dialogRef = this.distributionDialog.open(
            DistributionWizardMainComponent,
            {
                data: {
                    fboId: this.sharedService.currentUser.fboId,
                    groupId: this.sharedService.currentUser.groupId
                },
                disableClose: true
            }
        );

        dialogRef.afterClosed().subscribe(result => {
            this.loadDistributionLog();
        });
    }

    public fboPriceRequiresUpdate(price, vl) {
        if (!(price == 0 || price == null) && this.requiresUpdate) {
            this.isSaved = false;
            if (vl == 'JetA Retail') {
                this.jtRetail = price;
                this.currentFboPriceJetARetail.price = price;
            }
            if (vl == 'JetA Cost') {
                this.jtCost = price;
                this.currentFboPriceJetACost.price = price;
            }
        } else {
            if (vl == 'JetA Retail') {
                this.jtRetail = price;
                this.newRetail = price;
            }
            if (vl == 'JetA Cost') {
                this.jtCost = price;
                this.newCost = price;
            }
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
        if (vl == 'JetA Retail') {
            this.jtRetail = price;
            this.newRetail = price;
            this.checkOkPriceSave(
                this.currentPricingEffectiveFrom,
                this.currentPricingEffectiveTo,
                this.jtRetail,
                this.jtCost
            );
        }
        if (vl == 'JetA Cost') {
            this.jtCost = price;
            this.newCost = price;
            this.checkOkPriceSave(
                this.currentPricingEffectiveFrom,
                this.currentPricingEffectiveTo,
                this.jtRetail,
                this.jtCost
            );
        }
    }

    public fboCurrentPriceDateChange(event) {
        var CurrentDate = new Date();
        if (this.currentPricingEffectiveFrom > CurrentDate) {
            this.buttonTextValue = 'Stage Price';
        } else {
            this.buttonTextValue = 'Update Live Pricing';
        }
        this.requiresUpdate = false;
        for (let price of this.currentPrices) {
            if (
                moment(moment.utc(price.effectiveFrom).toDate()).format(
                    'MM/DD/YYYY'
                ) ==
                    moment(
                        moment.utc(this.currentPricingEffectiveFrom).toDate()
                    ).format('MM/DD/YYYY') &&
                moment(moment.utc(price.effectiveTo).toDate()).format(
                    'MM/DD/YYYY'
                ) ==
                    moment(
                        moment.utc(this.currentPricingEffectiveTo).toDate()
                    ).format('MM/DD/YYYY')
            ) {
                this.requiresUpdate = true;
            }
            price.effectiveFrom = moment(
                moment.utc(this.currentPricingEffectiveFrom).toDate()
            ).format('MM/DD/YYYY');
            price.effectiveTo = moment(
                moment.utc(this.currentPricingEffectiveTo).toDate()
            ).format('MM/DD/YYYY');
        }

        if (event != 'hide') {
            this.currentPricingEffectiveTo = new Date(
                moment(this.currentPricingEffectiveFrom)
                    .add(6, 'days')
                    .format('MM/DD/YYYY')
            );
            this.minimumAllowedDate = this.currentPricingEffectiveFrom;
            for (let price of this.currentPrices) {
                price.effectiveFrom = moment(
                    moment.utc(this.currentPricingEffectiveFrom).toDate()
                ).format('MM/DD/YYYY');
                price.effectiveTo = moment(
                    moment.utc(this.currentPricingEffectiveTo).toDate()
                ).format('MM/DD/YYYY');
            }
        }

        this.checkOkPriceSave(
            this.currentPricingEffectiveFrom,
            this.currentPricingEffectiveTo,
            this.currentFboPriceJetARetail.price,
            this.currentFboPriceJetACost.price
        );
    }

    public saveChangesClicked() {
        let arr = [];

        if (this.requiresUpdate == false) {
            for (let price of this.currentPrices) {
                price.oid = 0;
                price.effectiveFrom = this.currentPricingEffectiveFrom;
                price.effectiveTo = this.currentPricingEffectiveTo;
                price.price =
                    price.product == 'JetA Retail'
                        ? this.newRetail === undefined
                            ? this.currentFboPriceJetARetail.price
                            : this.newRetail
                        : price.product == 'JetA Cost'
                        ? this.newCost === undefined
                            ? this.currentFboPriceJetACost.price
                            : this.newCost
                        : null;
                if (price.price) {
                    arr.push(price);
                }
            }
            this.savePriceChangesAll(arr);
            let dateCost = this.currentFboPriceJetACost.effectiveTo;
            let priceCost = this.currentFboPriceJetACost.price;
            let dateRetasil = this.currentFboPriceJetARetail.effectiveTo;
            let priceRetail = this.currentFboPriceJetARetail.price;
            this.currentFboPriceJetACost.effectiveTo = dateCost;
            this.currentFboPriceJetACost.price = priceCost;
            this.currentFboPriceJetARetail.effectiveTo = dateRetasil;
            this.currentFboPriceJetARetail.price = priceRetail;
            this.sharedService.NotifyPricingSavedComponent('update');
        } else {
            for (let price of this.currentPrices) {
                if (price.price) {
                    price.effectiveFrom = this.currentPricingEffectiveFrom;
                    price.effectiveTo = this.currentPricingEffectiveTo;

                    price.price =
                        price.product == 'JetA Retail'
                            ? this.currentFboPriceJetARetail.price
                            : price.product == 'JetA Cost'
                            ? this.currentFboPriceJetACost.price
                            : null;
                    this.savePriceChanges(price);
                }

                //  this.loadCurrentFboPrices();
            }

            this.show = true;
            this.isSaved = true;
            this.delay(5000).then(any => {
                this.isSaved = false;
                this.requiresUpdate = true;
                this.show = false;
                this.sharedService.NotifyPricingSavedComponent('update');
            });
            this.loadCurrentFboPrices();
        }
        this.currentFboPriceJetARetail.price = this.jtRetail;
        this.currentFboPriceJetACost.price = this.jtCost;
        if (this.fboPreferences.oid > 0) {
            this.fboPreferencesService
                .update(this.fboPreferences)
                .subscribe((data: any) => {});
        } else {
            this.fboPreferencesService
                .add(this.fboPreferences)
                .subscribe((data: any) => {
                    this.fboPreferences.oid = data.oid;
                });
        }
        this.staticCurrentFboPriceJetACost = this.currentFboPriceJetACost.price;
        this.staticCurrentFboPriceJetARetail = this.currentFboPriceJetARetail.price;
        this.priceGroup = null;
        this.buttonTextValue = 'Update Live Pricing';

        this.jtRetail = this.jtCost = '';
        this.saveOk = true;
        this.niCard.checkPricing();
    }

    public fboPreferenceChange() {
        this.requiresUpdate = true;
        this.isSaved = false;
        this.show = false;
    }

    public checkOkPriceSave(startDate, endDate, cost, retail) {
        if (startDate && endDate && retail && cost) {
            this.saveOk = false;
        } else {
            this.saveOk = true;
        }
    }

    public costJetAToggled() {
        if (!this.fboPreferences.omitJetACost) {
            this.fboPreferenceChange();
            return;
        }
        var templatesAffected = 0;
        for (let template of this.pricingTemplates) {
            if (template.marginType == 0) templatesAffected += 1;
        }
        var customText =
            'Disabling JetA Cost will make ' +
            templatesAffected +
            ' margin templates invalid.';
        const dialogRef = this.warningDialog.open(DeleteConfirmationComponent, {
            data: {
                item: this.fboPreferences,
                customText: customText,
                customTitle: 'Warning'
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result) {
                this.fboPreferences.omitJetACost = false;
            } else {
                this.fboPreferences.omitJetACost = true;
                this.fboPreferenceChange();
            }
        });
    }
    async delay(ms: number) {
        await new Promise(resolve => setTimeout(() => resolve(), ms)).then(() => {});
    }
    public editTempAd() {
        const dialogRef = this.tempAddOnMargin.open(
            TemporaryAddOnMarginComponent,
            {
                data: {
                    MarginJet: this.TempValueJet,
                    EffectiveFrom: this.TempDateFrom,
                    EffectiveTo: this.TempDateTo,
                    id: this.TempValueId,
                    MarginAvgas: this.TempValueAvgas,
                    fboId: this.sharedService.currentUser.fboId,
                    update: true
                },
                autoFocus: false,
                panelClass: 'my-panel'
            }
        );

        dialogRef.afterClosed().subscribe(result => {
            if (!result) return;
            this.TempDateFrom = moment(
                moment.utc(result.EffectiveFrom).toDate()
            ).format('MM/DD/YYYY');
            this.TempDateTo = moment(
                moment.utc(result.EffectiveTo).toDate()
            ).format('MM/DD/YYYY');
            this.TempValueAvgas = result.MarginAvgas;
            this.TempValueJet = result.MarginJet;
            var now = new Date();
            now.setHours(0, 0, 0, 0);
            if (
                new Date(moment.utc(result.EffectiveTo).toDate()) <
                new Date(moment.utc(now).toDate())
            ) {
                this.TempValueId = null;
                this.TempValueJet = null;
                this.TempValueAvgas = null;
            }
            this.show = true;
            this.isSaved = true;
            this.delay(5000).then(any => {
                this.show = false;
                this.isSaved = false;
            });
        });
    }

    public deleteMargin(tempaddonmargin) {
        const dialogRef = this.deleteFBODialog.open(
            DeleteConfirmationComponent,
            {
                data: {
                    item: tempaddonmargin,
                    description: 'temporary add-on margin',
                    customTitle: 'Delete Temporary Margin?'
                }
            }
        );

        dialogRef.afterClosed().subscribe(result => {
            if (!result) return;
            this.temporaryAddOnMargin
                .remove(tempaddonmargin)
                .subscribe((data: any) => {
                    dialogRef.close();
                    this.TempValueJet = null;
                    this.TempValueId = null;
                    this.TempValueAvgas = null;
                    this.TempDateFrom = null;
                    this.TempDateTo = null;
                });
        });
    }

    public retailJetAToggled() {
        if (!this.fboPreferences.omitJetARetail) {
            this.fboPreferenceChange();
            return;
        }
        var templatesAffected = 0;
        for (let template of this.pricingTemplates) {
            if (template.marginType == 1) templatesAffected += 1;
        }
        var customText =
            'Disabling JetA Retail will make ' +
            templatesAffected +
            ' margin templates invalid.';
        const dialogRef = this.warningDialog.open(DeleteConfirmationComponent, {
            data: {
                item: this.fboPreferences,
                customText: customText,
                customTitle: 'Warning'
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result) {
                this.fboPreferences.omitJetARetail = false;
            } else {
                this.fboPreferences.omitJetARetail = true;
                this.fboPreferenceChange();
            }
        });
    }

    //Private Methods
    private loadCurrentFboPrices() {
        this.fboPricesService
            .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.currentPrices = data;
                this.currentFboPrice100LLCost = this.getCurrentPriceByProduct(
                    '100LL Cost'
                );
                this.currentFboPrice100LLRetail = this.getCurrentPriceByProduct(
                    '100LL Retail'
                );
                this.currentFboPriceJetACost = this.getCurrentPriceByProduct(
                    'JetA Cost'
                );
                this.currentFboPriceJetARetail = this.getCurrentPriceByProduct(
                    'JetA Retail'
                );
                if (this.currentFboPriceJetARetail.price) {
                    this.jtRetail = this.currentFboPriceJetARetail.price;
                } else {
                    this.jtRetail = null;
                }
                if (this.currentFboPriceJetACost.price) {
                    this.jtCost = this.currentFboPriceJetACost.price;
                } else {
                    this.jtCost = null;
                }

                this.staticCurrentFboPriceJetARetail = this.currentFboPriceJetARetail.price;
                this.staticCurrentFboPriceJetACost = this.currentFboPriceJetACost.price;
                this.currentFboPriceJetACost.effectiveTo = moment(
                    this.currentFboPriceJetACost.effectiveTo
                ).format('MM/DD/YYYY');
                this.currentFboPriceJetARetail.effectiveTo = moment(
                    this.currentFboPriceJetARetail.effectiveTo
                ).format('MM/DD/YYYY');
                if (data.length > 0) {
                    this.TempValueJet = data[0].tempJet;
                    this.TempValueAvgas = data[0].tempAvg;
                    this.TempValueId = data[0].tempId;
                    this.lcl = moment(data[0].tempDateFrom).utc();
                    this.TempDateFrom = moment(
                        moment.utc(data[0].tempDateFrom).toDate()
                    ).format('MM/DD/YYYY');
                    this.TempDateTo = moment(
                        moment.utc(data[0].tempDateTo).toDate()
                    ).format('MM/DD/YYYY');
                }
                if (this.currentFboPriceJetACost.effectiveFrom != null) {
                    this.currentPricingEffectiveFrom = this.currentFboPriceJetACost.effectiveFrom;
                    if (
                        this.currentFboPriceJetACost.effectiveFrom <=
                            new Date() &&
                        this.currentFboPriceJetACost.effectiveTo > new Date()
                    ) {
                        this.activePrice = true;
                    }
                } else {
                    this.currentPricingEffectiveFrom = new Date();
                }

                if (this.currentFboPriceJetACost.effectiveTo != null) {
                    this.currentPricingEffectiveTo = new Date(
                        moment(this.currentFboPriceJetACost.effectiveTo).format(
                            'MM/DD/YYYY'
                        )
                    );
                } else {
                    if (this.currentPricingEffectiveFrom) {
                        this.currentPricingEffectiveTo = new Date(
                            moment(this.currentPricingEffectiveFrom)
                                .add(6, 'days')
                                .format('MM/DD/YYYY')
                        );
                    } else {
                        this.currentPricingEffectiveTo = new Date(
                            moment(new Date())
                                .add(6, 'days')
                                .format('MM/DD/YYYY')
                        );
                    }
                }

                this.loadStagedFboPrices();

                this.priceUpdated.emit({
                    retailPrice: this.currentFboPriceJetARetail.price,
                    costPrice: this.currentFboPriceJetACost.price
				});
				
				this.sharedService.loadedChange('fbo-prices-loaded');
            });
    }

    public loadStagedFboPrices() {
        this.fboPricesService
            .getFbopricesByFboIdStaged(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.stagedPrices = data;
                this.stagedFboPriceJetA = this.getStagedPriceByProduct('JetA');
                this.stagedFboPrice100LL = this.getStagedPriceByProduct(
                    '100LL'
                );
            });
    }

    private loadFboPreferences() {
        this.fboPreferencesService
            .getForFbo(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.fboPreferences = data;
                    if (!this.fboPreferences)
                        this.fboPreferences = {
                            fboId: this.sharedService.currentUser.fboId
                        };
                    this.isLoadingFboPreferences = false;
                },
                (error: any) => {
                    this.fboPreferences = {
                        fboId: this.sharedService.currentUser.fboId
                    };
                    this.isLoadingFboPreferences = false;
                }
            );
    }

    public removeStaged(data) {
        this.fboPricesService.remove(data).subscribe((data: any) => {
            this.isSaved = true;
            this.show = true;
            this.delay(3000).then(any => {
                this.isSaved = false;
                this.show = false;
            });
            this.loadCurrentFboPrices();
        });
    }

    private loadFboFees() {
        this.fboFeesService
            .getFbofeesForFbo(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.fboFees = data;
                    if (!this.fboFees) this.fboFees = [];
                    this.isLoadingFboFees = false;
                },
                (error: any) => {
                    this.fboFees = [];
                    this.isLoadingFboFees = false;
                }
            );
    }

    private loadDistributionLog() {
        this.distributionService
            .getDistributionLogForFbo(this.sharedService.currentUser.fboId, 50)
            .subscribe(
                (data: any) => {
                    this.distributionLog = data;
                    if (!this.distributionLog) this.distributionLog = [];
                },
                (error: any) => {
                    this.distributionLog = [];
                }
            );
    }

    private loadPricingTemplates() {
        this.pricingTemplateService
            .getByFbo(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.pricingTemplates = data;
                },
                (error: any) => {
                    this.pricingTemplates = [];
                }
            );
    }

    private checkCostPlusMargins() {
        this.pricingTemplateService
            .getcostpluspricingtemplates(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                if (data) {
                    if (data.length > 0) {
                        this.costPlusText =
                            'Your cost value is confidential and not publicly displayed.  It is used for Cost+ margin templates.';
                    } else {
                        this.costPlusText =
                            'Cost value is not currently required (no customers are assigned to Cost+ pricing templates)';
                    }
                } else {
                    this.costPlusText =
                        'Cost value is not currently required (no customers are assigned to Cost+ pricing templates)';
                }
            });
    }

    private savePriceChanges(price) {
        if (this.requiresUpdate) {
            this.fboPricesService.update(price).subscribe((data: any) => {});
        }
    }

    private savePriceChangesAll(price) {
        this.fboPricesService
            .checkifExistFrboPrice(this.sharedService.currentUser.fboId, price)
            .subscribe((data: any) => {
                this.delay(2000).then(any => {
                    this.show = true;
                    this.isSaved = true;
                });

                this.delay(6000).then(any => {
                    this.isSaved = false;
                    this.requiresUpdate = true;
                    this.show = false;
                });
                this.loadCurrentFboPrices();
            });
    }

    private getCurrentPriceByProduct(product) {
        var result = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            oid: 0
        };
        for (let fboPrice of this.currentPrices) {
            if (fboPrice.product == product) result = fboPrice;
        }
        return result;
    }

    private getStagedPriceByProduct(product) {
        let result = [];
        this.stagedPrices.forEach(function(value) {
            if (
                ((value.product == 'JetA Retail' ||
                    value.product == 'JetA Cost') &&
                    product == 'JetA') ||
                ((value.product == '100LL Retail' ||
                    value.product == '100LL Cost') &&
                    product == '100LL')
            ) {
                var item = result.find(x => x.groupId == value.groupId);
                if (item == undefined) {
                    var pm = {
                        RetailPrice: 0,
                        CostPrice: 0,
                        oid: 0,
                        fboid: 0,
                        product: '',
                        description: '',
                        effectiveFrom: null,
                        effectiveTo: null,
                        timestamp: null,
                        salesTaxCost: 0,
                        currencyCost: 0,
                        salesTaxRetail: 0,
                        currencyRetail: 0,
                        groupId: null
                    };
                    pm.oid = value.oid;
                    pm.fboid = value.fboid;
                    pm.effectiveFrom = moment(
                        moment.utc(value.effectiveFrom).toDate()
                    ).format('MM/DD/YYYY');
                    pm.effectiveTo = moment(
                        moment.utc(value.effectiveTo).toDate()
                    ).format('MM/DD/YYYY');
                    pm.timestamp = value.timestamp;
                    pm.product = value.product;
                    pm.description = product;
                    pm.groupId = value.groupId;
                }
                if (
                    (value.product == 'JetA Retail' && product == 'JetA') ||
                    (value.product == '100LL Retail' && product == '100LL')
                ) {
                    if (item == undefined) {
                        pm.RetailPrice = value.price;
                        pm.currencyRetail = value.currency;
                    } else {
                        item.RetailPrice = value.price;
                        item.currencyRetail = value.currency;
                    }
                }
                if (
                    (value.product == 'JetA Cost' && product == 'JetA') ||
                    (value.product == '100LL Cost' && product == '100LL')
                ) {
                    if (item == undefined) {
                        pm.CostPrice = value.price;
                        pm.currencyCost = value.currency;
                    } else {
                        item.CostPrice = value.price;
                        item.currencyCost = value.currency;
                    }
                }
                if (item == undefined) {
                    result.push(pm);
                }
            }
        });
        return result;
    }
}
