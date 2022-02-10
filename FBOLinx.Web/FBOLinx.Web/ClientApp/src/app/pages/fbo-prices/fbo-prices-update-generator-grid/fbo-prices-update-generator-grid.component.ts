import {
    Component, ElementRef, Input, Output, OnInit, EventEmitter, ViewChild,
    ViewChildren, QueryList, } from '@angular/core';

import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { forEach } from 'lodash';
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import * as moment from 'moment';
import { NgxMatDateFormats, NGX_MAT_DATE_FORMATS } from '@angular-material-components/datetime-picker';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { DateTimeService } from '../../../services/datetime.service';

// Models
import { PricingUpdateGridViewModel as pricingUpdateGridViewModel } from '../../../models/pricing/pricing-update-grid-viewmodel';

const initialColumns: ColumnType[] = [
    {
        id: 'status',
        name: 'Status',
    },
    {
        id: 'product',
        name: 'Product'
    },
    {
        id: 'effectiveFrom',
        name: 'Effective',
    },
    {
        id: 'effectiveTo',
        name: 'Expiration',
    },
    {
        id: 'pricePap',
        name: 'Retail PAP',
    },
    {
        id: 'priceCost',
        name: 'Fuel Cost',
    },
    {
        id: 'edit',
        name: 'Edit',
    },
    {
        id: 'remove',
        name: 'Remove',
    }
];

const INTL_DATE_INPUT_FORMAT = {
    year: 'numeric',
    month: 'numeric',
    day: 'numeric',
    hourCycle: 'h23',
    hour: '2-digit',
    minute: '2-digit',
};

const MAT_DATE_FORMATS: NgxMatDateFormats = {
    parse: {
        dateInput: INTL_DATE_INPUT_FORMAT,
    },
    display: {
        dateInput: INTL_DATE_INPUT_FORMAT,
        monthYearLabel: { year: 'numeric', month: 'short' },
        dateA11yLabel: { year: 'numeric', month: 'long', day: 'numeric' },
        monthYearA11yLabel: { year: 'numeric', month: 'long' },
    },
};

@Component({
    providers: [{ provide: NGX_MAT_DATE_FORMATS, useValue: MAT_DATE_FORMATS }],
    selector: 'app-fbo-prices-update-generator-grid',
    styleUrls: ['./fbo-prices-update-generator-grid.component.scss'],
    templateUrl: './fbo-prices-update-generator-grid.component.html',
})
export class FboPricesUpdateGeneratorGridComponent implements OnInit {
    @ViewChild('updatePricesTable') table: ElementRef;
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;
    @Input() fboPricesUpdateData: any[];
    @Input() fboPricesUpdateGridData: any[];
    @Output() onSubmitRow = new EventEmitter<pricingUpdateGridViewModel>();

    pricesDataSource: any = null;
    columns: ColumnType[] = [];
    tooltipIndex = 0;
    timezone = "";

    constructor(private router: Router, private sharedService: SharedService, private fboPricesService: FbopricesService, private fboAirportsService: FboairportsService, private dateTimeService: DateTimeService) {

    }

    ngOnInit() {
        this.columns = initialColumns;
        this.refreshFboPricingDataSource();
        this.getTimeZone();
    }

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }

    public editRowClicked(pricingUpdate) {
        pricingUpdate.effectiveFrom = moment(pricingUpdate.effectiveFrom).toDate();
        pricingUpdate.effectiveTo = moment(pricingUpdate.effectiveTo).toDate();
        pricingUpdate.submitStatus = "Stage";
        pricingUpdate.isEdit = true;
    }

    public onEffectiveFromFocus(pricingUpdate) {
        if (pricingUpdate.oidRetail > 0)
            pricingUpdate.originalEffectiveFrom = pricingUpdate.effectiveFrom;

        this.fboAirportsService.getLocalDateTime(pricingUpdate.fboid).subscribe((localDateTime: any) => {
            pricingUpdate.effectiveFrom = moment(moment(new Date(localDateTime)).format("MM/DD/YYYY HH:mm")).toDate();
            pricingUpdate.currentDateTime = (moment(new Date(localDateTime))).add("10", "minutes");
        });
    }

    public onEffectiveFromChange(pricingUpdate) {
        var effectiveFromDate = moment(pricingUpdate.effectiveFrom).format("MM-DD-YYYY");
        this.dateTimeService.getNextTuesdayDate(effectiveFromDate).subscribe((nextTuesdayDate: any) => {
            pricingUpdate.effectiveTo = moment(nextTuesdayDate).toDate();

            if (moment(pricingUpdate.effectiveFrom) <= moment(pricingUpdate.currentDateTime))
                pricingUpdate.submitStatus = "Publish";
            else
                pricingUpdate.submitStatus = "Stage";
        });
    }

    public onEffectiveToFocus(pricingUpdate) {
        if (pricingUpdate.oidRetail > 0)
            pricingUpdate.originalEffectiveTo = pricingUpdate.effectiveTo;

        this.fboAirportsService.getLocalDateTime(pricingUpdate.fboid).subscribe((localdatetime: any) => {
            pricingUpdate.effectiveTo = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
        });
    }

    public clearClicked(pricingUpdate) {
        this.fboPricesService
            .suspendPricingGenerator(pricingUpdate)
            .subscribe((data: any) => {
                this.fboAirportsService.getLocalDateTime(pricingUpdate.fboid).subscribe((localdatetime: any) => {
                    this.dateTimeService.getNextTuesdayDate(localdatetime).subscribe((nextTuesdayDate: any) => {
                        pricingUpdate.effectiveFrom = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
                        pricingUpdate.effectiveTo = moment(nextTuesdayDate).toDate();
                        pricingUpdate.oidCost = 0;
                        pricingUpdate.oidPap = 0;
                        pricingUpdate.priceCost = 0;
                        pricingUpdate.pricePap = 0;
                        pricingUpdate.submitStatus = "Publish";
                        pricingUpdate.isEdit = true;
                    });
                });
            });
    }

    public cancelClicked(pricingUpdate) {
        if (pricingUpdate.oidPap > 0) {
            pricingUpdate.effectiveFrom = moment(pricingUpdate.originalEffectiveFrom == undefined ? pricingUpdate.effectiveFrom : pricingUpdate.originalEffectiveFrom).format("MM/DD/YYYY HH:mm");
            pricingUpdate.effectiveTo = moment(pricingUpdate.originalEffectiveTo == undefined ? pricingUpdate.effectiveTo : pricingUpdate.originalEffectiveTo).format("MM/DD/YYYY HH:mm");
            pricingUpdate.isEdit = false;
        }
        else {
            this.fboAirportsService.getLocalDateTime(pricingUpdate.fboid).subscribe((localdatetime: any) => {
                pricingUpdate.effectiveFrom = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
                pricingUpdate.effectiveTo = pricingUpdate.effectiveFrom;
                pricingUpdate.pricePap = "";
                pricingUpdate.priceCost = "";
            });
        }
    }

    public submitPricing(pricingUpdate) {
        this.onSubmitRow.emit({
            oidCost: pricingUpdate.oidCost,
            oidPap: pricingUpdate.oidPap,
            product: pricingUpdate.product,
            effectiveTo: pricingUpdate.effectiveTo,
            effectiveFrom: pricingUpdate.effectiveFrom,
            pricePap: pricingUpdate.pricePap,
            priceCost: pricingUpdate.priceCost
        });
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

    fboPriceRequiresUpdate(price: number, pricing: any, vl: string) {
        if (vl == 'Retail') {
            pricing.pricePap = price;
        }
        else {
            pricing.priceCost = price;
        }

        if (pricing.pricePap && pricing.priceCost) {
            if (pricing.priceCost < 0 || pricing.pricePap < 0) {
                pricing.priceEntryError =
                    'Cost or Retail values cannot be negative.';
            } else if (pricing.priceCost > pricing.pricePap) {
                pricing.priceEntryError =
                    'Your cost value is higher than the retail price.';
            } else {
                pricing.priceEntryError = '';
            }
        }
    }

    private refreshFboPricingDataSource() {
        if (!this.pricesDataSource) {
            this.pricesDataSource = new MatTableDataSource();
        }

        this.pricesDataSource.data = this.fboPricesUpdateGridData;
    }

    private getTimeZone() {
        if (this.timezone == "") {
            this.fboAirportsService.getLocalTimeZone(this.sharedService.currentUser.fboId).subscribe((data: any) => {
                this.timezone = data;
            });
        }
    }
}
