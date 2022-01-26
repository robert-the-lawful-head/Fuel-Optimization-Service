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

// Services
import { SharedService } from '../../../layouts/shared-service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FboairportsService } from '../../../services/fboairports.service';

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
        id: 'effective',
        name: 'Effective',
    },
    {
        id: 'expiration',
        name: 'Expiration',
    },
    {
        id: 'retailPap',
        name: 'Retail PAP',
    },
    {
        id: 'fuelCost',
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

@Component({
    selector: 'app-fbo-prices-update-generator-grid',
    styleUrls: ['./fbo-prices-update-generator-grid.component.scss'],
    templateUrl: './fbo-prices-update-generator-grid.component.html',
})
export class FboPricesUpdateGeneratorGridComponent implements OnInit {
    @ViewChild('updatePricesTable') table: ElementRef;
    @ViewChildren('tooltip') priceTooltips: QueryList<any>;
    @Input() fboPricesUpdateData: any[];
    @Input() fboPricesUpdateGridData: any[];
    @Output() onEditRow = new EventEmitter<pricingUpdateGridViewModel>();
    @Output() onSubmitRow = new EventEmitter<pricingUpdateGridViewModel>();

    pricesDataSource: any = null;
    columns: ColumnType[] = [];
    tooltipIndex = 0;
    timezone = "";

    constructor(private router: Router, private sharedService: SharedService, private fboPricesService: FbopricesService, private fboAirportsService: FboairportsService) {

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
        pricingUpdate.isEdit = true;
    }

    public onEffectiveFromFocus(pricingUpdate) {
        if (pricingUpdate.oidRetail > 0)
            pricingUpdate.originalEffectiveFrom = pricingUpdate.effectiveFrom;

        this.fboAirportsService.getLocalDateTime(pricingUpdate).subscribe((localdatetime: any) => {
            pricingUpdate.effectiveFrom = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
            pricingUpdate.effectiveTo = pricingUpdate.effectiveFrom;
        });
    }

    public onEffectiveToFocus(pricingUpdate) {
        if (pricingUpdate.oidRetail > 0)
            pricingUpdate.originalEffectiveTo = pricingUpdate.effectiveTo;

        this.fboAirportsService.getLocalDateTime(pricingUpdate).subscribe((localdatetime: any) => {
            pricingUpdate.effectiveTo = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
        });
    }

    public clearClicked(pricingUpdate) {
        this.fboPricesService
            .suspendPricingGenerator(pricingUpdate)
            .subscribe((data: any) => {
                this.fboAirportsService.getLocalDateTime(pricingUpdate).subscribe((localdatetime: any) => {
                    pricingUpdate.effectiveFrom = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
                    pricingUpdate.effectiveTo = pricingUpdate.effectiveFrom;
                    pricingUpdate.oidCost = 0;
                    pricingUpdate.oidPap = 0;
                    pricingUpdate.priceCost = 0;
                    pricingUpdate.pricePap = 0;
                    pricingUpdate.isEdit = true;
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
            this.fboAirportsService.getLocalDateTime(pricingUpdate).subscribe((localdatetime: any) => {
                pricingUpdate.effectiveFrom = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
                pricingUpdate.effectiveTo = pricingUpdate.effectiveFrom;
                pricingUpdate.pricePap = "";
                pricingUpdate.priceCost = "";
            });
        }
    }

    public submitPricingClicked(pricingUpdate) {
        if (pricingUpdate.pricePap > 0) {
            delete pricingUpdate.isEdit;
            delete pricingUpdate.priceEntryError;

            var currentTableData = this.pricesDataSource.data;
            this.pricesDataSource.data = new MatTableDataSource();

            pricingUpdate.effectiveFrom = moment(pricingUpdate.effectiveFrom).format("MM/DD/YYYY HH:mm");
            pricingUpdate.effectiveTo = moment(pricingUpdate.effectiveTo).format("MM/DD/YYYY HH:mm");

            this.fboPricesService
                .updatePricingGenerator(pricingUpdate)
                .subscribe((data: any) => {
                    var updatedPricing = data;
                    var product = pricingUpdate.product;
                    pricingUpdate = updatedPricing[0];
                    pricingUpdate.product = product;

                    if (pricingUpdate.oidPap > 0) {
                        pricingUpdate.effectiveFrom = moment(pricingUpdate.effectiveFrom).format("MM/DD/YYYY HH:mm");
                        pricingUpdate.effectiveTo = moment(pricingUpdate.effectiveTo).format("MM/DD/YYYY HH:mm");
                        pricingUpdate.isEdit = false;
                    }
                    else {
                        this.fboAirportsService.getLocalDateTime(pricingUpdate).subscribe((localdatetime: any) => {
                            pricingUpdate.effectiveFrom = moment(moment(new Date(localdatetime)).format("MM/DD/YYYY HH:mm")).toDate();
                            pricingUpdate.effectiveTo = pricingUpdate.effectiveFrom;
                            pricingUpdate.isEdit = true;
                        });
                    }

                    var tableData = currentTableData.find(x => x.product != pricingUpdate.product);
                    updatedPricing.push(tableData);
                    updatedPricing.sort((a, b) => (a.product < b.product ? -1 : 1));
                    this.pricesDataSource.data = updatedPricing;
                });
        }
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
