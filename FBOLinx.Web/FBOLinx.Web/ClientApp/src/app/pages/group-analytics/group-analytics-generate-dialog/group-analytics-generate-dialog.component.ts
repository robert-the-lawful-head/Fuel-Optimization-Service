import { Component, Inject, ViewChild } from '@angular/core';
import * as XLSX from 'xlsx';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-group-analytics-generate-dialog',
    templateUrl: './group-analytics-generate-dialog.component.html',
    styleUrls: [ './group-analytics-generate-dialog.component.scss' ],
    providers: [ SharedService ]
})
export class GroupAnalyticsGenerateDialogComponent {
    @ViewChild('grid') public grid: GridComponent;

    // Public Members
    dataSources: any = {};
    filter = '';
    selectedCustomers: any[];
    loading: boolean;
    exportedCustomersCount: number;

    constructor(
        public dialogRef: MatDialogRef<GroupAnalyticsGenerateDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService
    ) {
        this.dataSources = data.customers;
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    async onGenerate() {
        this.loading = true;
        this.exportedCustomersCount = 0;

        for (var selectedCustomer of this.selectedCustomers) {
            this.customerInfoByGroupService.getGroupAnalytics(
                this.sharedService.currentUser.groupId,
                [selectedCustomer.customerId],
            ).subscribe(async (data: any[]) => {
                var exportData: any[] = [];
                for (const customerFbos of data) {
                    this.populateExportDataForCustomer(customerFbos.company,
                        customerFbos.tailNumbers,
                        customerFbos.groupCustomerFbos,
                        exportData);
                }
                if (data.length > 0)
                    await this.exportReportForCustomer(exportData, data[0].company);
                this.loading = false;
            });
        }
    }

    applyFilter(filter: string) {
        this.filter = filter;

        this.dataSources = this.data.customers.filter(customer =>
            customer.company.toLowerCase().includes(filter));
    }

    rowSelected() {
        this.selectedCustomers = this.grid.getSelectedRecords();
    }

    rowDeselected() {
        this.selectedCustomers = this.grid.getSelectedRecords();
    }

    populateExportDataForCustomer(company: string, tailNumbers: string, data: any, exportData: any[]) {
        for (const fboPrice of data) {
            for (let i = 0; i < fboPrice.prices.length; i++) {
                var row: any = {};
                row = {};
                row['FBO'] = i === 0 ? fboPrice.icao : '';
                row['Volume Tier'] = fboPrice.prices[i].volumeTier;
                if (fboPrice.prices[i].priceBreakdownDisplayType == 0) {
                    row['Price'] = fboPrice.prices[i].domPrivate?.toFixed(4);
                } else if (fboPrice.prices[i].priceBreakdownDisplayType == 1) {
                    row['International Price'] = fboPrice.prices[i].intPrivate?.toFixed(4);
                    row['Domestic Price'] = fboPrice.prices[i].domPrivate?.toFixed(4);
                } else if (fboPrice.prices[i].priceBreakdownDisplayType == 2) {
                    row['Commercial Price'] = fboPrice.prices[i].domComm?.toFixed(4);
                    row['Private Price'] = fboPrice.prices[i].domPrivate?.toFixed(4);
                } else {
                    row['Int/Comm Price'] = fboPrice.prices[i].intComm?.toFixed(4);
                    row['Int/Private Price'] = fboPrice.prices[i].intPrivate?.toFixed(4);
                    row['Dom/Comm Price'] = fboPrice.prices[i].domComm?.toFixed(4);
                    row['Dom/Private Price'] = fboPrice.prices[i].domPrivate?.toFixed(4);
                }
                row['Tail Numbers'] = tailNumbers;
                exportData.push(row);
            }
            if (!fboPrice.prices.length) {
                exportData.push({
                    FBO: fboPrice.icao,
                    'Volume Tier': '',
                    'Int/Comm': '',
                    'Int/Private': '',
                    'Dom/Comm': '',
                    'Dom/Private': '',
                    'Tail Numbers': ''
                });
            }
        }
    }

    exportReportForCustomer(exportData: any[], company: string) {
        return new Promise((resolve) => {
            // TODO
            const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
            const wb: XLSX.WorkBook = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, 'Report');

            /* save to file */
            XLSX.writeFile(wb, company + '.csv');

            setTimeout(() => {
                resolve({});
            }, 100);
        });
    }
}
