import {
    Component,
    OnInit,
    OnDestroy,
    AfterViewInit,
    ViewChild,
    ViewChildren,
    ViewEncapsulation,
    QueryList,
} from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import * as moment from "moment";
import * as _ from "lodash";

// Services
import { DistributionService } from "../../../services/distribution.service";
import { FbofeesService } from "../../../services/fbofees.service";
import { FbopricesService } from "../../../services/fboprices.service";
import { FbopreferencesService } from "../../../services/fbopreferences.service";
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { SharedService } from "../../../layouts/shared-service";
import { TemporaryAddOnMarginService } from "../../../services/temporaryaddonmargin.service";
import { CustomcustomertypesService } from "../../../services/customcustomertypes.service";

// Components
import { DeleteConfirmationComponent } from "../../../shared/components/delete-confirmation/delete-confirmation.component";
import { TemporaryAddOnMarginComponent } from "../../../shared/components/temporary-add-on-margin/temporary-add-on-margin.component";
import { FboPricesSelectDefaultTemplateComponent } from "../fbo-prices-select-default-template/fbo-prices-select-default-template.component";

import * as SharedEvents from "../../../models/sharedEvents";
// Components
import { DistributionWizardMainComponent } from "../../../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component";
import { NiCardComponent } from "../../../ni-components/ni-card/ni-card.component";

export interface DefaultTemplateUpdate {
    currenttemplate: number;
    newtemplate: number;
    fboid: number;
}

export interface TemporaryAddOnMargin {
    id: any;
    fboId: any;
    EffectiveFrom: any;
    EffectiveTo: any;
    MarginJet: any;
}
@Component({
    selector: "app-fbo-prices-home",
    templateUrl: "./fbo-prices-home.component.html",
    styleUrls: ["./fbo-prices-home.component.scss"],
    encapsulation: ViewEncapsulation.None,
})
export class FboPricesHomeComponent
    implements OnInit, OnDestroy, AfterViewInit {
    @ViewChild(NiCardComponent) niCard: NiCardComponent;
    @ViewChildren("tooltip") tooltips: QueryList<any>;
    // Public Members
    public currentPrices: any[];
    public dateFrom: any;
    public dateTo: any;
    public todayDate: Date = new Date();
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
    public isLoadingFboPreferences: boolean;
    public isLoadingFboFees: boolean;
    public requiresUpdate = true;
    public isSaved: boolean;
    public show: boolean;
    public saveOk = true;
    public minimumAllowedDate: Date = new Date();
    public currentPricingEffectiveFrom: any;
    public currentPricingEffectiveTo: any;
    public pricingTemplates: any[];
    public activePrice = false;
    public newRetail: any;
    public newCost: any;
    public jtCost: any;
    public jtRetail: any;
    public priceGroup: number;
    public buttonTextValue: any;
    public costPlusText = "";
    public priceEntryError = "";

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

    public staticCurrentFboPriceJetARetail: any;
    public staticCurrentFboPriceJetACost: any;

    public staticCurrentFboPriceJetAEffectiveFromRetail: any;
    public staticCurrentFboPriceJetAEffectiveToRetail: any;

    public staticCurrentFboPriceJetAEffectiveFromCost: any;
    public staticCurrentFboPriceJetAEffectiveToCost: any;

    public tooltipIndex = 0;
    public menuTooltipSubscription: any;
    public locationChangedSubscription: any;

    constructor(
        private temporaryAddOnMargin: TemporaryAddOnMarginService,
        private distributionService: DistributionService,
        private fboFeesService: FbofeesService,
        private fboPricesService: FbopricesService,
        private fboPreferencesService: FbopreferencesService,
        private pricingTemplateService: PricingtemplatesService,
        private sharedService: SharedService,
        private customCustomerService: CustomcustomertypesService,
        private distributionDialog: MatDialog,
        private warningDialog: MatDialog,
        private tempAddOnMargin: MatDialog,
        private deleteFBODialog: MatDialog,
        private fboPricesSelectDefaultTemplateDialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.buttonTextValue = "Update Live Pricing";
        this.resetAll();
    }

    ngAfterViewInit(): void {
        this.menuTooltipSubscription = this.sharedService.loadedEmitted$.subscribe(
            (message) => {
                if (message === "menu-tooltips-showed") {
                    this.showTooltips();
                }
            }
        );
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.resetAll();
                }
            }
        );
    }

    ngOnDestroy(): void {
        if (this.menuTooltipSubscription) {
            this.menuTooltipSubscription.unsubscribe();
        }
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    resetAll() {
        this.currentPrices = undefined;
        this.loadCurrentFboPrices();
        this.loadFboPreferences();
        this.loadFboFees();
        this.loadDistributionLog();
        this.loadPricingTemplates();
        this.checkCostPlusMargins();
        this.checkDefaultTemplate();
    }

    // Public Methods
    jetChangedHandler(event: TemporaryAddOnMargin) {
        this.dateTo = new Date(moment.utc(event.EffectiveTo).toDate());
        const now = new Date();
        now.setHours(0, 0, 0, 0);
        if (this.dateTo >= new Date(moment.utc(now).toDate())) {
            this.TempValueId = event.id;
            this.TempValueJet = event.MarginJet;
            this.TempDateFrom = moment(
                moment.utc(event.EffectiveFrom).toDate()
            ).format("MM/DD/YYYY");
            this.TempDateTo = moment(
                moment.utc(event.EffectiveTo).toDate()
            ).format("MM/DD/YYYY");
        }

        this.show = true;
        this.isSaved = true;
        this.delay(5000).then(() => {
            this.show = false;
            this.isSaved = false;
        });
    }

    public suspendJetPricing() {
        this.fboPricesService
            .suspendJetPricing(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.loadCurrentFboPrices();
            });
    }

    public suspendRetailPricing() {
        this.fboPricesService
            .suspendRetailPricing(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.loadCurrentFboPrices();
            });
    }

    public distributePricingClicked() {
        const dialogRef = this.distributionDialog.open(
            DistributionWizardMainComponent,
            {
                data: {
                    fboId: this.sharedService.currentUser.fboId,
                    groupId: this.sharedService.currentUser.groupId,
                },
                disableClose: true,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            this.loadDistributionLog();
        });
    }

    public fboPriceRequiresUpdate(price, vl) {
        if (!(price === 0 || price === null) && this.requiresUpdate) {
            this.isSaved = false;
            if (vl === "JetA Retail") {
                this.jtRetail = price;
                this.currentFboPriceJetARetail.price = price;
            }
            if (vl === "JetA Cost") {
                this.jtCost = price;
                this.currentFboPriceJetACost.price = price;
            }
        } else {
            if (vl === "JetA Retail") {
                this.jtRetail = price;
                this.newRetail = price;
            }
            if (vl === "JetA Cost") {
                this.jtCost = price;
                this.newCost = price;
            }
        }
        if (this.jtRetail && this.jtCost) {
            if (this.jtCost < 0 || this.jtRetail < 0) {
                this.priceEntryError =
                    "Cost or Retail values cannot be negative.";
            } else if (this.jtCost > this.jtRetail) {
                this.priceEntryError =
                    "Your cost value is higher than the retail price.";
            } else {
                this.priceEntryError = "";
            }
        }
        if (vl === "JetA Retail") {
            this.newRetail = price;
            this.checkOkPriceSave(
                this.currentPricingEffectiveFrom,
                this.currentPricingEffectiveTo,
                this.jtRetail,
                this.jtCost
            );
        }
        if (vl === "JetA Cost") {
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
        const CurrentDate = new Date();
        if (this.currentPricingEffectiveFrom > CurrentDate) {
            this.buttonTextValue = "Stage Price";
        } else {
            this.buttonTextValue = "Update Live Pricing";
        }
        this.requiresUpdate = false;
        for (const price of this.currentPrices) {
            if (
                moment(moment.utc(price.effectiveFrom).toDate()).format(
                    "MM/DD/YYYY"
                ) ===
                    moment(
                        moment.utc(this.currentPricingEffectiveFrom).toDate()
                    ).format("MM/DD/YYYY") &&
                moment(moment.utc(price.effectiveTo).toDate()).format(
                    "MM/DD/YYYY"
                ) ===
                    moment(
                        moment.utc(this.currentPricingEffectiveTo).toDate()
                    ).format("MM/DD/YYYY")
            ) {
                this.requiresUpdate = true;
            }
            price.effectiveFrom = moment(
                moment.utc(this.currentPricingEffectiveFrom).toDate()
            ).format("MM/DD/YYYY");
            price.effectiveTo = moment(
                moment.utc(this.currentPricingEffectiveTo).toDate()
            ).format("MM/DD/YYYY");
        }

        if (event !== "hide") {
            // this.currentPricingEffectiveTo = new Date(
            //    moment(this.currentPricingEffectiveFrom)
            //        .add(6, "days")
            //        .format("MM/DD/YYYY")
            // );
            this.minimumAllowedDate = this.currentPricingEffectiveFrom;
            for (const price of this.currentPrices) {
                price.effectiveFrom = moment(
                    moment.utc(this.currentPricingEffectiveFrom).toDate()
                ).format("MM/DD/YYYY");
                price.effectiveTo = moment(
                    moment.utc(this.currentPricingEffectiveTo).toDate()
                ).format("MM/DD/YYYY");
            }
        }

        this.checkOkPriceSave(
            this.currentPricingEffectiveFrom,
            this.currentPricingEffectiveTo,
            this.jtRetail,
            this.jtCost
        );
    }

    public saveChangesClicked() {
        const arr = [];

        if (this.requiresUpdate === false) {
            for (const price of this.currentPrices) {
                price.oid = 0;
                price.effectiveFrom = this.currentPricingEffectiveFrom;
                price.effectiveTo = this.currentPricingEffectiveTo;
                price.price =
                    price.product === "JetA Retail"
                        ? this.newRetail === undefined
                            ? this.currentFboPriceJetARetail.price
                            : this.newRetail
                        : price.product === "JetA Cost"
                        ? this.newCost === undefined
                            ? this.currentFboPriceJetACost.price
                            : this.newCost
                        : null;
                if (price.price) {
                    arr.push(price);
                }
            }
            this.savePriceChangesAll(arr);
            const dateCost = this.currentFboPriceJetACost.effectiveTo;
            const priceCost = this.currentFboPriceJetACost.price;
            const dateRetasil = this.currentFboPriceJetARetail.effectiveTo;
            const priceRetail = this.currentFboPriceJetARetail.price;
            this.currentFboPriceJetACost.effectiveTo = dateCost;
            this.currentFboPriceJetACost.price = priceCost;
            this.currentFboPriceJetARetail.effectiveTo = dateRetasil;
            this.currentFboPriceJetARetail.price = priceRetail;
            this.sharedService.NotifyPricingSavedComponent("update");
        } else {
            for (const price of this.currentPrices) {
                if (price.price) {
                    price.effectiveFrom = this.currentPricingEffectiveFrom;
                    price.effectiveTo = this.currentPricingEffectiveTo;

                    price.price =
                        price.product === "JetA Retail"
                            ? this.currentFboPriceJetARetail.price
                            : price.product === "JetA Cost"
                            ? this.currentFboPriceJetACost.price
                            : null;
                    this.savePriceChanges(price);
                }
            }

            this.show = true;
            this.isSaved = true;
            this.delay(5000).then(() => {
                this.isSaved = false;
                this.requiresUpdate = true;
                this.show = false;
                this.sharedService.NotifyPricingSavedComponent("update");
            });
            this.loadCurrentFboPrices();
        }
        // this.currentFboPriceJetARetail.price = this.jtRetail;
        // this.currentFboPriceJetACost.price = this.jtCost;
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
        this.buttonTextValue = "Update Live Pricing";

        this.jtRetail = "";
        this.jtCost = "";
        this.currentPricingEffectiveFrom = null;
        this.currentPricingEffectiveTo = null;
        this.saveOk = true;
        this.niCard.checkPricing();
    }

    public fboPreferenceChange() {
        this.requiresUpdate = true;
        this.isSaved = false;
        this.show = false;
    }

    public checkOkPriceSave(startDate, endDate, cost, retail) {
        if (startDate && endDate && (retail || cost)) {
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
        let templatesAffected = 0;
        for (const template of this.pricingTemplates) {
            if (template.marginType === 0) {
                templatesAffected += 1;
            }
        }
        const customText =
            "Disabling JetA Cost will make " +
            templatesAffected +
            " margin templates invalid.";
        const dialogRef = this.warningDialog.open(DeleteConfirmationComponent, {
            data: {
                item: this.fboPreferences,
                customText,
                customTitle: "Warning",
            },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                this.fboPreferences.omitJetACost = false;
            } else {
                this.fboPreferences.omitJetACost = true;
                this.fboPreferenceChange();
            }
        });
    }
    async delay(ms: number) {
        await new Promise((resolve) =>
            setTimeout(() => resolve(), ms)
        ).then(() => {});
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
                    update: true,
                },
                autoFocus: false,
                panelClass: "my-panel",
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.TempDateFrom = moment(
                moment.utc(result.EffectiveFrom).toDate()
            ).format("MM/DD/YYYY");
            this.TempDateTo = moment(
                moment.utc(result.EffectiveTo).toDate()
            ).format("MM/DD/YYYY");
            this.TempValueAvgas = result.MarginAvgas;
            this.TempValueJet = result.MarginJet;
            const now = new Date();
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
            this.delay(5000).then(() => {
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
                    description: "temporary add-on margin",
                    customTitle: "Delete Temporary Margin?",
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
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
        let templatesAffected = 0;
        for (const template of this.pricingTemplates) {
            if (template.marginType === 1) {
                templatesAffected += 1;
            }
        }
        const customText =
            "Disabling JetA Retail will make " +
            templatesAffected +
            " margin templates invalid.";
        const dialogRef = this.warningDialog.open(DeleteConfirmationComponent, {
            data: {
                item: this.fboPreferences,
                customText,
                customTitle: "Warning",
            },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                this.fboPreferences.omitJetARetail = false;
            } else {
                this.fboPreferences.omitJetARetail = true;
                this.fboPreferenceChange();
            }
        });
    }

    // Private Methods
    private loadCurrentFboPrices() {
        this.fboPricesService
            .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.currentPrices = data;
                this.currentFboPrice100LLCost = this.getCurrentPriceByProduct(
                    "100LL Cost"
                );
                this.currentFboPrice100LLRetail = this.getCurrentPriceByProduct(
                    "100LL Retail"
                );
                this.currentFboPriceJetACost = this.getCurrentPriceByProduct(
                    "JetA Cost"
                );
                this.currentFboPriceJetARetail = this.getCurrentPriceByProduct(
                    "JetA Retail"
                );

                this.staticCurrentFboPriceJetARetail = this.currentFboPriceJetARetail.price;
                this.staticCurrentFboPriceJetACost = this.currentFboPriceJetACost.price;
                if (this.currentFboPriceJetARetail.effectiveFrom) {
                    this.staticCurrentFboPriceJetAEffectiveFromRetail = this.currentFboPriceJetARetail.effectiveFrom = moment(
                        this.currentFboPriceJetARetail.effectiveFrom
                    ).format("MM/DD/YYYY");
                }
                if (this.currentFboPriceJetARetail.effectiveTo) {
                    this.staticCurrentFboPriceJetAEffectiveToRetail = this.currentFboPriceJetARetail.effectiveTo = moment(
                        this.currentFboPriceJetARetail.effectiveTo
                    ).format("MM/DD/YYYY");
                }
                
                if (this.currentFboPriceJetACost.effectiveFrom) {
                    this.staticCurrentFboPriceJetAEffectiveFromCost = moment(
                        this.currentFboPriceJetACost.effectiveFrom
                    ).format("MM/DD/YYYY");
                }

                if (this.currentFboPriceJetACost.effectiveTo) {
                    this.staticCurrentFboPriceJetAEffectiveToCost = moment(
                        this.currentFboPriceJetACost.effectiveTo
                    ).format("MM/DD/YYYY");
                }
                
                this.currentFboPriceJetARetail.effectiveTo = moment(
                    this.currentFboPriceJetARetail.effectiveTo
                ).format("MM/DD/YYYY");
                if (data.length > 0) {
                    this.TempValueJet = data[0].tempJet;
                    this.TempValueAvgas = data[0].tempAvg;
                    this.TempValueId = data[0].tempId;
                    this.lcl = moment(data[0].tempDateFrom).utc();
                    this.TempDateFrom = moment(
                        moment.utc(data[0].tempDateFrom).toDate()
                    ).format("MM/DD/YYYY");
                    this.TempDateTo = moment(
                        moment.utc(data[0].tempDateTo).toDate()
                    ).format("MM/DD/YYYY");
                }
                if (this.currentFboPriceJetACost.effectiveFrom !== null) {
                    this.staticCurrentFboPriceJetAEffectiveFromCost = this.currentFboPriceJetACost.effectiveFrom;
                    if (
                        this.currentFboPriceJetACost.effectiveFrom <=
                            new Date() &&
                        this.currentFboPriceJetACost.effectiveTo > new Date()
                    ) {
                        this.activePrice = true;
                    }
                } else {
                    this.staticCurrentFboPriceJetAEffectiveFromCost = null;
                }

                if (this.currentFboPriceJetACost.effectiveTo !== null) {
                    this.staticCurrentFboPriceJetAEffectiveToCost = new Date(moment(this.currentFboPriceJetACost.effectiveTo).format(
                        "MM/DD/YYYY"
                    ));
                } 

                this.loadStagedFboPrices();

                this.sharedService.loadedChange("fbo-prices-loaded");
            });
    }

    public loadStagedFboPrices() {
        this.fboPricesService
            .getFbopricesByFboIdStaged(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.stagedPrices = data;
                this.stagedFboPriceJetA = this.getStagedPriceByProduct("JetA");
                this.stagedFboPrice100LL = this.getStagedPriceByProduct(
                    "100LL"
                );
            });
    }

    private loadFboPreferences() {
        this.fboPreferencesService
            .getForFbo(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.fboPreferences = data;
                    if (!this.fboPreferences) {
                        this.fboPreferences = {
                            fboId: this.sharedService.currentUser.fboId,
                        };
                    }
                    this.isLoadingFboPreferences = false;
                },
                (error: any) => {
                    this.fboPreferences = {
                        fboId: this.sharedService.currentUser.fboId,
                    };
                    this.isLoadingFboPreferences = false;
                }
            );
    }

    public removeStaged(data) {
        this.fboPricesService.remove(data).subscribe(() => {
            this.isSaved = true;
            this.show = true;
            this.delay(3000).then(() => {
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
                    if (!this.fboFees) {
                        this.fboFees = [];
                    }
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
                    if (!this.distributionLog) {
                        this.distributionLog = [];
                    }
                },
                (error: any) => {
                    this.distributionLog = [];
                }
            );
    }

    private loadPricingTemplates() {
        
        if (this.sharedService.currentUser.fboId !== 0) {
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
        else {
            this.pricingTemplates = [];
        }
    }

    private checkCostPlusMargins() {
        this.pricingTemplateService
            .getcostpluspricingtemplates(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                if (data) {
                    if (data.exist) {
                        this.costPlusText =
                            "Your cost value is confidential and not publicly displayed.  It is used for Cost+ margin templates.";
                    } else {
                        this.costPlusText =
                            "Cost value is not currently required (no customers are assigned to Cost+ pricing templates)";
                    }
                } else {
                    this.costPlusText =
                        "Cost value is not currently required (no customers are assigned to Cost+ pricing templates)";
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
            .subscribe(() => {
                this.delay(2000).then(() => {
                    this.show = true;
                    this.isSaved = true;
                });

                this.delay(6000).then(() => {
                    this.isSaved = false;
                    this.requiresUpdate = true;
                    this.show = false;
                });
                this.loadCurrentFboPrices();
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
        const result = [];
        this.stagedPrices.forEach((value) => {
            if (
                ((value.product === "JetA Retail" ||
                    value.product === "JetA Cost") &&
                    product === "JetA") ||
                ((value.product === "100LL Retail" ||
                    value.product === "100LL Cost") &&
                    product === "100LL")
            ) {
                const item = result.find((x) => x.groupId === value.groupId);
                let pm;
                if (item === undefined) {
                    pm = {
                        RetailPrice: 0,
                        CostPrice: 0,
                        oid: 0,
                        fboid: 0,
                        product: "",
                        description: "",
                        effectiveFrom: null,
                        effectiveTo: null,
                        timestamp: null,
                        salesTaxCost: 0,
                        currencyCost: 0,
                        salesTaxRetail: 0,
                        currencyRetail: 0,
                        groupId: null,
                    };
                    pm.oid = value.oid;
                    pm.fboid = value.fboid;
                    pm.effectiveFrom = moment(
                        moment.utc(value.effectiveFrom).toDate()
                    ).format("MM/DD/YYYY");
                    pm.effectiveTo = moment(
                        moment.utc(value.effectiveTo).toDate()
                    ).format("MM/DD/YYYY");
                    pm.timestamp = value.timestamp;
                    pm.product = value.product;
                    pm.description = product;
                    pm.groupId = value.groupId;
                }
                if (
                    (value.product === "JetA Retail" && product === "JetA") ||
                    (value.product === "100LL Retail" && product === "100LL")
                ) {
                    if (item === undefined) {
                        pm.RetailPrice = value.price;
                        pm.currencyRetail = value.currency;
                    } else {
                        item.RetailPrice = value.price;
                        item.currencyRetail = value.currency;
                    }
                }
                if (
                    (value.product === "JetA Cost" && product === "JetA") ||
                    (value.product === "100LL Cost" && product === "100LL")
                ) {
                    if (item === undefined) {
                        pm.CostPrice = value.price;
                        pm.currencyCost = value.currency;
                    } else {
                        item.CostPrice = value.price;
                        item.currencyCost = value.currency;
                    }
                }
                if (item === undefined) {
                    result.push(pm);
                }
            }
        });
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
                                                if (result !== "cancel") {
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
                            (error: any) => {
                                this.pricingTemplates = [];
                            }
                        );
                }
            });
    }

    private showTooltips() {
        setTimeout(() => {
            const tooltipsArr = this.tooltips.toArray();
            this.tooltipIndex = tooltipsArr.length - 1;
            if (this.tooltipIndex >= 0) {
                tooltipsArr[this.tooltipIndex].open();
                this.tooltipIndex--;
            }
        }, 400);
    }

    public tooltipHidden() {
        const tooltipsArr = this.tooltips.toArray();
        if (this.tooltipIndex >= 0 && tooltipsArr.length > this.tooltipIndex) {
            setTimeout(() => {
                tooltipsArr[this.tooltipIndex].open();
                this.tooltipIndex--;
            }, 400);
        } else {
            this.sharedService.loadedChange("price-tooltips-showed");
        }
    }
}
