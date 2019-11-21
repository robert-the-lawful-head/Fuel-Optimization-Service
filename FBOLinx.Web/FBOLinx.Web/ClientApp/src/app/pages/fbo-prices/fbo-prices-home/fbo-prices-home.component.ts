import { Component, OnInit, Inject, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

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

//const BREADCRUMBS: any[] = [
//    {
//        title: 'Main',
//        link: '#/default-layout/dashboard-home'
//    },
//    {
//        title: 'Manage Prices',
//        link: ''
//    }
//];
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

	//Public Members
	public pageTitle: string = 'Pricing';
	//public breadcrumb: any[] = BREADCRUMBS;
	public pricingJetARetail: any;
	public pricingJetACost: any;
	public pricing100LLRetail: any;
	public pricing100LLCost: any;
	public currentPrices: any[];
	public dateFrom: any;
	public dateTo : any;
	public TempValueJet: any;
	public TempValueAvgas: any;
	public TempValueId: any;
	public TempDateFrom: any;
	public TempDateTo: any;
	public stagedPrices: any[];
	public fboPreferences: any;
	public fboFees: any[];
	public lcl : any;
	public distributionLog: any[];
	public isLoadingJetARetail: boolean;
	public isLoadingJetACost: boolean;
	public isLoading100LLRetail: boolean;
	public isLoading100LLCost: boolean;
	public isLoadingFboPreferences: boolean;
	public isLoadingFboFees: boolean;
	public requiresUpdate: boolean;
	public isSaved: boolean;
	public show: boolean;
	public minimumAllowedDate: Date = new Date();
	//public maximumCurrentEffectiveTo: Date = new Date();
	public minimumStagedEffectiveFrom: Date = new Date();
	public currentPricingEffectiveFrom: Date = new Date();
	public currentPricingEffectiveTo: Date = new Date();
	public stagedPricingEffectiveFrom: Date = new Date();
	public stagedPricingEffectiveTo: Date = new Date();
	public pricingTemplates: any[];
	public activePrice: boolean = false;

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
	constructor(private distributionService: DistributionService,
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
		public notificationDialog: MatDialog) {

		//this.sharedService.emitChange(this.pageTitle);
	}

	ngOnInit(): void {
		this.loadCurrentFboPrices();
		this.loadFboPreferences();
		this.loadFboFees();
		this.loadDistributionLog();
		this.loadPricingTemplates();
	}



	//Public Methods
	jetChangedHandler(event: temporaryAddOnMargin) {
		//this.TempValueId = null;
		//this.dateFrom = new Date(moment(event.EffectiveFrom).local().format("MM/DD/YYYY"));
		this.dateTo = new Date(moment.utc(event.EffectiveTo).toDate());
		//alert($event.MarginJet);var now = new Date();
		var now = new Date();
		now.setHours(0, 0, 0, 0);
		if (this.dateTo >= new Date(moment.utc(now).toDate())) {
			this.TempValueId = event.id;
			this.TempValueJet = event.MarginJet;
			this.TempDateFrom = moment(moment.utc(event.EffectiveFrom).toDate()).format('MM/DD/YYYY');// moment(event.EffectiveFrom).format("MM/DD/YYYY");
			this.TempDateTo = moment(moment.utc(event.EffectiveTo).toDate()).format('MM/DD/YYYY');;// moment(event.EffectiveTo).format("MM/DD/YYYY");
		}

		this.show = true;
		this.isSaved = true;
		this.delay(5000).then(any => {
			this.show = false;
			this.isSaved = false;
		});
	}

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
		if (price.price == 0 || price.price == null) {
			this.requiresUpdate = false;
			price.requiresUpdate = false;
		}
		else {
			this.requiresUpdate = true;
			this.isSaved = false;
			price.requiresUpdate = true;
		}
		//this.requiresUpdate = true;
		//this.isSaved = false;
		//price.requiresUpdate = true;
	}

	public fboCurrentPriceDateChange(event) {
		this.requiresUpdate = true;
		for (let price of this.currentPrices) {
			price.effectiveFrom = this.currentPricingEffectiveFrom;
			price.effectiveTo = this.currentPricingEffectiveTo;
			price.requiresUpdate = true;
		}
	   // this.stagedPricingEffectiveFrom = new Date(moment(this.currentPricingEffectiveTo).add(1, 'days').format('MM/DD/YYYY'));
		//this.minimumStagedEffectiveFrom = this.currentPricingEffectiveTo;
		if (event != 'hide') {
			this.currentPricingEffectiveTo = new Date(moment(this.currentPricingEffectiveFrom).add(6, 'days').format('MM/DD/YYYY'));
			this.minimumAllowedDate = this.currentPricingEffectiveFrom;
		}
	}

   /* public fboStagedPriceDateChange() {
		this.requiresUpdate = true;
		for (let stagedPrice of this.stagedPrices) {
			stagedPrice.effectiveFrom = this.stagedPricingEffectiveFrom;
			stagedPrice.effectiveTo = this.stagedPricingEffectiveTo;
			stagedPrice.requiresUpdate = true;
		}
		this.currentPricingEffectiveTo = new Date(moment(this.stagedPricingEffectiveFrom).add(-1, 'days').format('MM/DD/YYYY'));
		//this.maximumCurrentEffectiveTo = this.stagedPricingEffectiveFrom;
	}*/

	public saveChangesClicked() {
		for (let price of this.currentPrices) {
			//if (price.requiresUpdate) {
				price.effectiveFrom = this.currentPricingEffectiveFrom;
			price.effectiveTo = this.currentPricingEffectiveTo;
				this.savePriceChanges(price);
		}

		this.loadCurrentFboPrices();
		if (this.fboPreferences.oid > 0) {
			this.fboPreferencesService.update(this.fboPreferences).subscribe((data: any) => {});
		} else {
			this.fboPreferencesService.add(this.fboPreferences).subscribe((data: any) => {
				this.fboPreferences.oid = data.oid;
			});
		}
		this.isSaved = true;
		this.delay(5000).then(any => {
			this.isSaved = false;
		});
		//this.requiresUpdate = false;


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
	async delay(ms: number) {
		await new Promise(resolve => setTimeout(() => resolve(), ms)).then(() => console.log("fired"));
	}
	public editTempAd() {
		const dialogRef = this.tempAddOnMargin.open(TemporaryAddOnMarginComponent, {
			data: { MarginJet: this.TempValueJet,EffectiveFrom: this.TempDateFrom, EffectiveTo: this.TempDateTo, id: this.TempValueId, MarginAvgas: this.TempValueAvgas, fboId: this.sharedService.currentUser.fboId,update: true  },
			autoFocus: false,
			panelClass: 'my-panel'
		});

		dialogRef.afterClosed().subscribe(result => {
			if (!result)
				return;
			this.TempDateFrom = moment(moment.utc(result.EffectiveFrom).toDate()).format('MM/DD/YYYY');
			this.TempDateTo = moment(moment.utc(result.EffectiveTo).toDate()).format('MM/DD/YYYY');
			this.TempValueAvgas = result.MarginAvgas;
			this.TempValueJet = result.MarginJet;
			var now = new Date();
			now.setHours(0, 0, 0, 0);
			if (new Date(moment.utc(result.EffectiveTo).toDate()) < new Date(moment.utc(now).toDate())) {
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
		const dialogRef = this.deleteFBODialog.open(DeleteConfirmationComponent, {
			data: { item: tempaddonmargin, description: 'temporary add-on margin', customTitle: 'Delete Temporary Margin?' }
		});

		dialogRef.afterClosed().subscribe(result => {
			if (!result)
				return;
			this.temporaryAddOnMargin.remove(tempaddonmargin).subscribe((data: any) => {
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
				this.TempValueJet = data[0].tempJet;;
				this.TempValueAvgas = data[0].tempAvg;
				this.TempValueId = data[0].tempId;
				this.currentFboPriceJetACost.effectiveTo = moment(this.currentFboPriceJetACost.effectiveTo).format("MM/DD/YYYY");
				this.currentFboPriceJetARetail.effectiveTo = moment(this.currentFboPriceJetARetail.effectiveTo).format("MM/DD/YYYY");
				console.log(moment(data[0].tempDateFrom).utc());
				this.lcl = moment(data[0].tempDateFrom).utc();
				this.TempDateFrom = moment(moment.utc(data[0].tempDateFrom).toDate()).format('MM/DD/YYYY');
				this.TempDateTo = moment(moment.utc(data[0].tempDateTo).toDate()).format('MM/DD/YYYY');
				if (this.currentFboPrice100LLCost.effectiveFrom != null) {
					this.currentPricingEffectiveFrom = this.currentFboPrice100LLCost.effectiveFrom;
					if (this.currentFboPrice100LLCost.effectiveFrom <= new Date() && this.currentFboPrice100LLCost.effectiveTo > new Date()) {
						this.activePrice = true;
					}
				} else {
					this.currentPricingEffectiveFrom = new Date();
				}
				if (this.currentFboPrice100LLCost.effectiveTo != null) {
					this.currentPricingEffectiveTo = this.currentFboPrice100LLCost.effectiveTo;
				} else {
					this.currentPricingEffectiveTo = new Date(moment(this.currentPricingEffectiveFrom).add(6, 'days').format('MM/DD/YYYY'));
				}
				this.loadStagedFboPrices();
				
				if (this.currentFboPriceJetARetail) {
					this.staticCurrentFboPriceJetARetail = this.currentFboPriceJetARetail.price;
				}
				if (this.currentFboPriceJetACost) {
					this.staticCurrentFboPriceJetACost = this.currentFboPriceJetACost.price;
				}
			});
	}

	public loadStagedFboPrices() {
		this.fboPricesService.getFbopricesByFboIdStaged(this.sharedService.currentUser.fboId).subscribe((data: any) => {
			this.stagedPrices = data;
			this.stagedFboPriceJetA = this.getStagedPriceByProduct('JetA');
			this.stagedFboPrice100LL = this.getStagedPriceByProduct('100LL');
			//this.minimumStagedEffectiveFrom = this.currentPricingEffectiveTo;
			//this.maximumCurrentEffectiveTo = this.stagedPricingEffectiveFrom;
			//this.setAllDateLimits();
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

	public removeStaged(data) {
		this.fboPricesService.remove(data).subscribe((data: any) => {
			this.loadCurrentFboPrices();
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
		console.log(result);
		return result;
	}

	private getStagedPriceByProduct(product) {
		let result = [];
		this.stagedPrices.forEach(function (value) {
			if (((value.product == 'JetA Retail' || value.product == 'JetA Cost') && product == 'JetA') || ((value.product == '100LL Retail' || value.product == '100LL Cost') && product == '100LL')) {
				var item = result.find(x => moment(moment.utc(x.effectiveFrom).toDate()).format('MM/DD/YYYY') == moment(moment.utc(value.effectiveFrom).toDate()).format('MM/DD/YYYY') && moment(moment.utc(x.effectiveTo).toDate()).format('MM/DD/YYYY') == moment(moment.utc(value.effectiveTo).toDate()).format('MM/DD/YYYY'));
				//alert(JSON.stringify(value) + ' product-' + product);
				//result.find(x => x.oid == value.oid);
				if (item == undefined) {
					var pm = { RetailPrice: 0, CostPrice: 0, oid: 0, fboid: 0, product: '', effectiveFrom: null, effectiveTo: null, timestamp: null, salesTaxCost: 0, currencyCost: 0, salesTaxRetail: 0, currencyRetail: 0 };
					pm.oid = value.oid;
					pm.fboid = value.fboid;
					pm.effectiveFrom = moment(moment.utc(value.effectiveFrom).toDate()).format('MM/DD/YYYY');
					pm.effectiveTo = moment(moment.utc(value.effectiveTo).toDate()).format('MM/DD/YYYY');
					pm.timestamp = value.timestamp;

					pm.product = product;
				}
				if ((value.product == 'JetA Retail' && product == 'JetA') || (value.product == '100LL Retail' && product == '100LL')) {
					if (item == undefined) {
						pm.RetailPrice = value.price;
						pm.currencyRetail = value.currency;
					}
					else {
						item.RetailPrice = value.price;
						item.currencyRetail = value.currency;
					}
				}
				if ((value.product == 'JetA Cost' && product == 'JetA') || (value.product == '100LL Cost' && product == '100LL')) {
					if (item == undefined) {
						pm.CostPrice = value.price;
						pm.currencyCost = value.currency;
					}
					else {
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

   /* private setAllDateLimits() {
		this.setEffectiveDateValidation(this.stagedFboPrice100LLCost, this.currentFboPrice100LLCost);
		this.setEffectiveDateValidation(this.stagedFboPrice100LLRetail, this.currentFboPrice100LLRetail);
		this.setEffectiveDateValidation(this.stagedFboPriceJetACost, this.currentFboPriceJetACost);
		this.setEffectiveDateValidation(this.stagedFboPriceJetARetail, this.currentFboPriceJetARetail);
	}*/

	/*private setEffectiveDateValidation(currentFboPrice, stagedFboPrice) {
		if (!stagedFboPrice || !currentFboPrice)
			return;
		if (stagedFboPrice.effectiveFrom != null) {
			currentFboPrice.maxDate = new Date(moment(stagedFboPrice.effectiveFrom).format('MM/DD/YYYY'));
		}
		if (currentFboPrice.effectiveTo != null) {
			stagedFboPrice.minDate = new Date(moment(currentFboPrice.effectiveTo).format('MM/DD/YYYY'));
		}
	}*/
}
