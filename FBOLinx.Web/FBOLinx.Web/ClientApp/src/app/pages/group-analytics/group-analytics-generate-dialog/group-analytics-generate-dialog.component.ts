import { Component, Inject, ViewChild, OnInit } from '@angular/core';
import * as XLSX from 'xlsx';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { GridComponent, SelectionService } from '@syncfusion/ej2-angular-grids';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';
import { GroupAnalyticsEmailPricingDialogComponent } from '../group-analytics-email-pricing-dialog/group-analytics-email-pricing-dialog.component';
import { EmailTemplate } from 'src/app/models/email-template';

export type GroupAnalyticsGenerateDialogData = {
    customers: any[];
    emailTemplate: EmailTemplate;
};

@Component({
    selector: 'app-group-analytics-generate-dialog',
    templateUrl: './group-analytics-generate-dialog.component.html',
    styleUrls: [ './group-analytics-generate-dialog.component.scss' ],
    providers: [ SharedService, SelectionService ]
})
export class GroupAnalyticsGenerateDialogComponent implements OnInit {
    @ViewChild('grid') public grid: GridComponent;

    // Public Members
    dataSources: any = {};
    filter = '';
    selectedCustomers: any[];
    downloading: boolean;
    emailing: boolean;
    exportedCustomersCount: number;
    public selectOptions: any;
    public editSettings: any;
    public toolbar: string[];
    public filterOptions = {
        mode: 'Immediate', immediateModeDelay: 1000  };
    public sortSettings: any;

    constructor(
        public dialogRef: MatDialogRef<GroupAnalyticsGenerateDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: GroupAnalyticsGenerateDialogData,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
        private emailDialog: MatDialog,
    ) {
    }

    public ngOnInit(): void {
        this.dataSources = this.data.customers;
        this.selectOptions = { persistSelection: true };
        this.editSettings = { allowDeleting: true };
        this.sortSettings = { columns: [{ field: 'company' }] };
        this.toolbar = ['Delete'];
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onEmail() {
        const dialogRef = this.emailDialog.open<GroupAnalyticsEmailPricingDialogComponent, GroupAnalyticsGenerateDialogData>(
            GroupAnalyticsEmailPricingDialogComponent,
            {
                data: {
                    customers: this.selectedCustomers,
                    emailTemplate: this.data.emailTemplate,
                },
                height: '600px',
                width: '680px',
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.onSendEmail();
            }
        });
    }

    async onGenerate() {
        this.downloading = true;
        this.exportedCustomersCount = 0;

        const promises = this.selectedCustomers.map(async (selectedCustomer) => {
            const data =
                await this.customerInfoByGroupService.getGroupAnalytics(
                    this.sharedService.currentUser.groupId,
                    selectedCustomer.customerId,
                ).toPromise() as any;

            const exportData: any[] = [];

            this.populateExportDataForCustomer(
                data.tailNumbers,
                data.groupCustomerFbos,
                exportData);

            await this.exportReportForCustomer(exportData, data.company);
        });

        await Promise.all(promises);
        this.downloading = false;
    }

    async onSendEmail() {
        this.emailing = true;
        this.exportedCustomersCount = 0;

        const promises = this.selectedCustomers.map(async (selectedCustomer) => {
            const data =
                await this.customerInfoByGroupService.getGroupAnalyticsAndEmail(
                    this.sharedService.currentUser.groupId,
                    selectedCustomer.customerId,
                ).toPromise() as any;

            const exportData: any[] = [];

            this.populateExportDataForCustomer(
                data.tailNumbers,
                data.groupCustomerFbos,
                exportData);

            await this.exportReportForCustomer(exportData, data.company);
        });

        await Promise.all(promises);
        this.emailing = false;
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

    populateExportDataForCustomer(tailNumbers: string, data: any, exportData: any[]) {
        for (const fboPrice of data) {
            for (let i = 0; i < fboPrice.prices.length; i++) {
                const row: any = {};
                row.FBO = i === 0 ? fboPrice.icao : '';
                row['Volume Tier'] = fboPrice.prices[i].volumeTier;
                if (fboPrice.prices[i].priceBreakdownDisplayType === 0) {
                    row.Price = fboPrice.prices[i].domPrivate?.toFixed(4);
                } else if (fboPrice.prices[i].priceBreakdownDisplayType === 1) {
                    row['International Price'] = fboPrice.prices[i].intPrivate?.toFixed(4);
                    row['Domestic Price'] = fboPrice.prices[i].domPrivate?.toFixed(4);
                } else if (fboPrice.prices[i].priceBreakdownDisplayType === 2) {
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
