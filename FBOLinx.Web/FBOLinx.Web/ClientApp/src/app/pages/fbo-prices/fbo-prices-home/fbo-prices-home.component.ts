import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { DistributionService } from '../../../services/distribution.service';
import { FbofeesService } from '../../../services/fbofees.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FbopreferencesService } from '../../../services/fbopreferences.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

//Components
import { DistributionWizardMainComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout/dashboard-home'
    },
    {
        title: 'Manage Prices',
        link: ''
    }
];

@Component({
    selector: 'app-fbo-prices-home',
    templateUrl: './fbo-prices-home.component.html',
    styleUrls: ['./fbo-prices-home.component.scss']
})
/** fbo-prices-home component*/
export class FboPricesHomeComponent implements OnInit {

    //Public Members
    public pageTitle: string = 'Pricing';
    public breadcrumb: any[] = BREADCRUMBS;
    public pricingJetARetail: any;
    public pricingJetACost: any;
    public pricing100LLRetail: any;
    public pricing100LLCost: any;
    public currentPrices: any[];
    public stagedPrices: any[];
    public fboPreferences: any;
    public fboFees: any[];
    public distributionLog: any[];
    public isLoadingJetARetail: boolean;
    public isLoadingJetACost: boolean;
    public isLoading100LLRetail: boolean;
    public isLoading100LLCost: boolean;
    public isLoadingFboPreferences: boolean;
    public isLoadingFboFees: boolean;
    public requiresUpdate: boolean;
    public isSaved: boolean;
    public minimumAllowedDate: Date = new Date();
    public maximumCurrentEffectiveTo: Date = new Date();
    public minimumStagedEffectiveFrom: Date = new Date();
    public currentPricingEffectiveFrom: Date = new Date();
    public currentPricingEffectiveTo: Date = new Date();
    public stagedPricingEffectiveFrom: Date = new Date();
    public stagedPricingEffectiveTo: Date = new Date();
    public pricingTemplates: any[];

    //Additional Public Members for direct reference (date filtering/restrictions)
    public currentFboPriceJetARetail: any;
    public currentFboPriceJetACost: any;
    public currentFboPrice100LLRetail: any;
    public currentFboPrice100LLCost: any;
    public stagedFboPriceJetARetail: any;
    public stagedFboPriceJetACost: any;
    public stagedFboPrice100LLRetail: any;
    public stagedFboPrice100LLCost: any;

     /** fbo-prices-home ctor */
    constructor(private distributionService: DistributionService,
        private fboFeesService: FbofeesService,
        private fboPricesService: FbopricesService,
        private fboPreferencesService: FbopreferencesService,
        private pricingTemplateService: PricingtemplatesService,
        private sharedService: SharedService,
        public distributionDialog: MatDialog,
        public warningDialog: MatDialog) {

        this.sharedService.emitChange(this.pageTitle);
    }

    ngOnInit(): void {
        this.loadCurrentFboPrices();
        this.loadFboPreferences();
        this.loadFboFees();
        this.loadDistributionLog();
        this.loadPricingTemplates();
    }

    //Public Methods

    public distributePricingClicked() {
        const dialogRef = this.distributionDialog.open(DistributionWizardMainComponent, {
            data: {
                fboId: this.sharedService.currentUser.fboId,
                groupId: this.sharedService.currentUser.groupId
            },
            disableClose: true
        });

        dialogRef.afterClosed().subscribe(result => {
            this.loadDistributionLog();
        });
    }

    public fboPriceRequiresUpdate(price) {
        this.requiresUpdate = true;
        this.isSaved = false;
        price.requiresUpdate = true;
    }

    public fboCurrentPriceDateChange() {
        this.requiresUpdate = true;
        for (let price of this.currentPrices) {
            price.effectiveFrom = this.currentPricingEffectiveFrom;
            price.effectiveTo = this.currentPricingEffectiveTo;
            price.requiresUpdate = true;
        }
        this.stagedPricingEffectiveFrom = new Date(moment(this.currentPricingEffectiveTo).add(1, 'days').format('MM/DD/YYYY'));
        this.minimumStagedEffectiveFrom = this.currentPricingEffectiveTo;
    }

    public fboStagedPriceDateChange() {
        this.requiresUpdate = true;
        for (let stagedPrice of this.stagedPrices) {
            stagedPrice.effectiveFrom = this.stagedPricingEffectiveFrom;
            stagedPrice.effectiveTo = this.stagedPricingEffectiveTo;
            stagedPrice.requiresUpdate = true;
        }
        this.currentPricingEffectiveTo = new Date(moment(this.stagedPricingEffectiveFrom).add(-1, 'days').format('MM/DD/YYYY'));
        this.maximumCurrentEffectiveTo = this.stagedPricingEffectiveFrom;
    }

    public saveChangesClicked() {
        for (let price of this.currentPrices) {
            if (price.requiresUpdate) {
                price.effectiveFrom = this.currentPricingEffectiveFrom;
                price.effectiveTo = this.currentPricingEffectiveTo;
                this.savePriceChanges(price);
            }
        }

        for (let stagedPrice of this.stagedPrices) {
            if (stagedPrice.requiresUpdate) {
                stagedPrice.effectiveFrom = this.stagedPricingEffectiveFrom;
                stagedPrice.effectiveTo = this.stagedPricingEffectiveTo;
                this.savePriceChanges(stagedPrice);
            }
        }

        if (this.fboPreferences.oid > 0) {
            this.fboPreferencesService.update(this.fboPreferences).subscribe((data: any) => {});
        } else {
            this.fboPreferencesService.add(this.fboPreferences).subscribe((data: any) => {
                this.fboPreferences.oid = data.oid;
            });
        }

        this.isSaved = true;
        this.requiresUpdate = false;
    }

    public fboPreferenceChange() {
        this.requiresUpdate = true;
        this.isSaved = false;
    }

    public costJetAToggled() {
        if (!this.fboPreferences.omitJetACost) {
            this.fboPreferenceChange();
            return;
        }
        var templatesAffected = 0;
        for (let template of this.pricingTemplates) {
            if (template.marginType == 0)
                templatesAffected += 1;
        }
        var customText = 'Disabling JetA Cost will make ' + templatesAffected + ' margin templates invalid.';
        const dialogRef = this.warningDialog.open(DeleteConfirmationComponent, {
            data: { item: this.fboPreferences, customText: customText, customTitle: 'Warning' }
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

    public retailJetAToggled() {
        if (!this.fboPreferences.omitJetARetail) {
            this.fboPreferenceChange();
            return;
        }
        var templatesAffected = 0;
        for (let template of this.pricingTemplates) {
            if (template.marginType == 1)
                templatesAffected += 1;
        }
        var customText = 'Disabling JetA Retail will make ' + templatesAffected + ' margin templates invalid.';
        const dialogRef = this.warningDialog.open(DeleteConfirmationComponent, {
            data: { item: this.fboPreferences, customText: customText, customTitle: 'Warning' }
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
        this.fboPricesService.getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.currentPrices = data;
                this.currentFboPrice100LLCost = this.getCurrentPriceByProduct('100LL Cost');
                this.currentFboPrice100LLRetail = this.getCurrentPriceByProduct('100LL Retail');
                this.currentFboPriceJetACost = this.getCurrentPriceByProduct('JetA Cost');
                this.currentFboPriceJetARetail = this.getCurrentPriceByProduct('JetA Retail');
                if (this.currentFboPrice100LLCost.effectiveFrom != null) {
                    this.currentPricingEffectiveFrom = this.currentFboPrice100LLCost.effectiveFrom;
                } else {
                    this.currentPricingEffectiveFrom = new Date();
                }
                if (this.currentFboPrice100LLCost.effectiveTo != null) {
                    this.currentPricingEffectiveTo = this.currentFboPrice100LLCost.effectiveTo;
                } else {
                    this.currentPricingEffectiveTo = new Date(moment(this.currentPricingEffectiveFrom).add(6, 'days').format('MM/DD/YYYY'));
                }
                this.loadStagedFboPrices();
            });
    }

    public loadStagedFboPrices() {
        this.fboPricesService.getFbopricesByFboIdStaged(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.stagedPrices = data;
            this.stagedFboPrice100LLCost = this.getStagedPriceByProduct('100LL Cost');
            this.stagedFboPrice100LLRetail = this.getStagedPriceByProduct('100LL Retail');
            this.stagedFboPriceJetACost = this.getStagedPriceByProduct('JetA Cost');
            this.stagedFboPriceJetARetail = this.getStagedPriceByProduct('JetA Retail');
            if (this.stagedFboPrice100LLCost.effectiveFrom != null) {
                this.stagedPricingEffectiveFrom = this.stagedFboPrice100LLCost.effectiveFrom;
                this.stagedPricingEffectiveTo = this.stagedFboPrice100LLCost.effectiveTo;
            } else {
                this.stagedPricingEffectiveFrom = new Date(moment(this.currentPricingEffectiveTo).add(1, 'days').format('MM/DD/YYYY'));
                this.stagedPricingEffectiveTo = new Date(moment(this.stagedPricingEffectiveFrom).add(6, 'days').format('MM/DD/YYYY'));
            }
            this.minimumStagedEffectiveFrom = this.currentPricingEffectiveTo;
            this.maximumCurrentEffectiveTo = this.stagedPricingEffectiveFrom;
            this.setAllDateLimits();
        });
    }

    private loadFboPreferences() {
        this.fboPreferencesService.getForFbo(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.fboPreferences = data;
                if (!this.fboPreferences)
                    this.fboPreferences = { fboId: this.sharedService.currentUser.fboId };
                this.isLoadingFboPreferences = false;
            });
    }

    private loadFboFees() {
        this.fboFeesService.getFbofeesForFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.fboFees = data;
            if (!this.fboFees)
                this.fboFees = [];
            this.isLoadingFboFees = false;
        });
    }

    private loadDistributionLog() {
        this.distributionService.getDistributionLogForFbo(this.sharedService.currentUser.fboId, 50).subscribe((data:
                any) => {
            this.distributionLog = data;
                if (!this.distributionLog)
                    this.distributionLog = [];
            }
        );
    }

    private loadPricingTemplates() {
        this.pricingTemplateService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.pricingTemplates = data;
        });
    }

    private savePriceChanges(price) {
        if (!price.oid || price.oid == null) 
            this.fboPricesService.add(price).subscribe((data: any) => price.oid = data.oid);
        else
            this.fboPricesService.update(price).subscribe((data: any) => {});
    }

    private saveFboPreferences() {
        if (!this.fboPreferences.oid || this.fboPreferences.oid == 0) {
            this.fboPreferencesService.add(this.fboPreferences).subscribe((data: any) => this.loadFboPreferences());
        } else {
            this.fboPreferencesService.update(this.fboPreferences).subscribe((data: any) => this.loadFboPreferences());
        }
    }

    private getCurrentPriceByProduct(product) {
        var result = {fboId: this.sharedService.currentUser.fboId, groupId: this.sharedService.currentUser.groupId, oid: 0 };
        for (let fboPrice of this.currentPrices) {
            if (fboPrice.product == product)
                result = fboPrice;
        }
        return result;
    }

    private getStagedPriceByProduct(product) {
        var result = { fboId: this.sharedService.currentUser.fboId, groupId: this.sharedService.currentUser.groupId, oid: 0 };
        for (let fboPrice of this.stagedPrices) {
            if (fboPrice.product == product)
                result = fboPrice;
        }
        return result;
    }

    private setAllDateLimits() {
        this.setEffectiveDateValidation(this.stagedFboPrice100LLCost, this.currentFboPrice100LLCost);
        this.setEffectiveDateValidation(this.stagedFboPrice100LLRetail, this.currentFboPrice100LLRetail);
        this.setEffectiveDateValidation(this.stagedFboPriceJetACost, this.currentFboPriceJetACost);
        this.setEffectiveDateValidation(this.stagedFboPriceJetARetail, this.currentFboPriceJetARetail);
    }

    private setEffectiveDateValidation(currentFboPrice, stagedFboPrice) {
        if (!stagedFboPrice || !currentFboPrice)
            return;
        if (stagedFboPrice.effectiveFrom != null) {
            currentFboPrice.maxDate = new Date(moment(stagedFboPrice.effectiveFrom).format('MM/DD/YYYY'));
        }
        if (currentFboPrice.effectiveTo != null) {
            stagedFboPrice.minDate = new Date(moment(currentFboPrice.effectiveTo).format('MM/DD/YYYY'));
        }
    }
}
