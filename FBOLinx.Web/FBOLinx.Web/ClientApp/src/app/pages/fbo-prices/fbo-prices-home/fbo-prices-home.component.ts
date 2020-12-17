import {
  Component,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ViewChildren,
  QueryList,
  HostListener,
  Input,
  ViewChild
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Observable } from 'rxjs';
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
  rampFee: any;
}

@Component({
  selector: 'app-fbo-prices-home',
  templateUrl: './fbo-prices-home.component.html',
  styleUrls: ['./fbo-prices-home.component.scss']
})
export class FboPricesHomeComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input() isCsr?: boolean;

  @ViewChildren('tooltip') priceTooltips: QueryList<any>;
  // Members
  pricingLoader = 'pricing-loader';
  // tailLoader = 'tail-loader';

  currentPrices: any[];
  currentPricingEffectiveFrom = new Date();
  currentPricingEffectiveTo: any;
  pricingTemplates: any[];
  public feesAndTaxes: Array<any>;
  jtCost: any;
  jtRetail: any;
  isLoadingRetail = false;
  isLoadingCost = false;

  TempValueJet: number;
  TempValueId = 0;
  TempDateFrom: Date;
  TempDateTo: Date;
  isUpdatingMargin = false;
  isClearingMargin = false;

  priceEntryError = '';

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
  stagedFboPriceJetA: any[];
  stagedFboPrice100LL: any;

  locationChangedSubscription: any;
  tooltipSubscription: any;
  tailNumberFormControlSubscription: any;

  layoutChanged: boolean;

  @ViewChild('retailFeeAndTaxBreakdown') private retailFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;
    @ViewChild('costFeeAndTaxBreakdown') private costFeeAndTaxBreakdown: FeeAndTaxBreakdownComponent;
    @ViewChild('priceChecker') private priceChecker: PriceCheckerComponent;

  constructor(
      private feesAndTaxesService: FbofeesandtaxesService,
      private fboPricesService: FbopricesService,
      private pricingTemplateService: PricingtemplatesService,
      private sharedService: SharedService,
      private customCustomerService: CustomcustomertypesService,
      private temporaryAddOnMargin: TemporaryAddOnMarginService,
      private NgxUiLoader: NgxUiLoaderService,
      private fboPricesSelectDefaultTemplateDialog: MatDialog,
      private fboFeesAndTaxesDialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    this.resetAll();
    this.onResize({
      target: {
        innerWidth: window.innerWidth
      }
    });
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
  }

  resetAll() {
    this.currentPrices = undefined;
    this.NgxUiLoader.startLoader(this.pricingLoader);
    this.loadFboPrices().subscribe(() => {
      this.NgxUiLoader.stopLoader(this.pricingLoader);
    });
    this.loadFeesAndTaxes();
    this.checkDefaultTemplate();
  }

  // Methods
  suspendJetPricing() {
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

  suspendRetailPricing() {
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

  updatePricing() {
    const effectiveFrom = moment.utc(this.currentPricingEffectiveFrom).format();
    const effectiveTo = moment.utc(this.currentPricingEffectiveTo).add(1, 'day').format();
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

  canUpdatePricing() {
    return this.currentPricingEffectiveTo && (this.jtRetail || this.jtCost) && !this.priceEntryError.length;
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
              moment(moment.utc(this.currentFboPriceJetARetail.effectiveTo).subtract(1, 'day')).local().format('MM/DD/YYYY');
          }

          if (this.currentFboPriceJetACost.effectiveTo) {
            this.currentFboPriceJetACost.effectiveTo =
              moment(moment.utc(this.currentFboPriceJetACost.effectiveTo).subtract(1, 'day')).local().format('MM/DD/YYYY');
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
                            .subscribe(() => {});
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

  @HostListener('window:resize', ['$event'])
  private onResize(event: any) {
    if (event.target.innerWidth <= 1425) {
      this.layoutChanged = true;
    } else {
      this.layoutChanged = false;
    }
  }
}
