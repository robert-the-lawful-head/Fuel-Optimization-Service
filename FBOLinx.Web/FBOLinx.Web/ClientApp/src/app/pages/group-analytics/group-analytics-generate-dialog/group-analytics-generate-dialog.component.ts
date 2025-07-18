import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import {
    MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
    MatLegacyDialog as MatDialog,
    MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';
import { GridComponent, SelectionService } from '@syncfusion/ej2-angular-grids';
import { EmailTemplate } from 'src/app/models/email-template';
import { EmailcontentService } from 'src/app/services/emailcontent.service';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { GroupAnalyticsEmailPricingDialogComponent } from '../group-analytics-email-pricing-dialog/group-analytics-email-pricing-dialog.component';
import { DecimalPrecisionPipe } from 'src/app/shared/pipes/decimal/decimal-precision.pipe';

export type GroupAnalyticsGenerateDialogData = {
    customers: any[];
    emailTemplate: EmailTemplate;
};

@Component({
    providers: [SharedService, SelectionService],
    selector: 'app-group-analytics-generate-dialog',
    styleUrls: ['./group-analytics-generate-dialog.component.scss'],
    templateUrl: './group-analytics-generate-dialog.component.html',
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
        immediateModeDelay: 1000,
        mode: 'Immediate',
    };

    constructor(
        public dialogRef: MatDialogRef<GroupAnalyticsGenerateDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: GroupAnalyticsGenerateDialogData,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService,
        private emailDialog: MatDialog,
        private emailContentService: EmailcontentService,
        private decimalPrecisionPipe: DecimalPrecisionPipe
    ) {}

    public ngOnInit(): void {
        this.dataSources = this.data.customers;
        this.selectOptions = { persistSelection: true };
        this.editSettings = { allowDeleting: true };

        this.toolbar = ['Delete'];
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onEmail() {
        const dialogRef = this.emailDialog.open<
            GroupAnalyticsEmailPricingDialogComponent,
            GroupAnalyticsGenerateDialogData
        >(GroupAnalyticsEmailPricingDialogComponent, {
            data: {
                customers: this.selectedCustomers,
                emailTemplate: this.data.emailTemplate,
            },
            height: '600px',
            width: '680px',
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                const emailTemplate: EmailTemplate = {
                    emailContentHtml: result.emailTemplate.emailContentHtml,
                    fromAddress: result.emailTemplate.fromAddress,
                    groupId: this.sharedService.currentUser.groupId,
                    oid: result.emailTemplate?.oid,
                    replyTo: result.emailTemplate.replyTo,
                    subject: result.emailTemplate.subject,
                };

                if (!emailTemplate.oid) {
                    this.emailContentService
                        .add(emailTemplate)
                        .subscribe((data: EmailTemplate) => {
                            this.data.emailTemplate = data;
                            this.onSendEmail();
                        });
                } else {
                    this.emailContentService
                        .update(emailTemplate)
                        .subscribe((data: EmailTemplate) => {
                            this.data.emailTemplate = emailTemplate;
                            this.onSendEmail();
                        });
                }
            }
        });
    }

    async onGenerate() {
        this.downloading = true;
        this.exportedCustomersCount = 0;

        const promises = this.selectedCustomers.map(
            async (selectedCustomer) => {
                const data = (await this.customerInfoByGroupService
                    .getGroupAnalytics(
                        this.sharedService.currentUser.groupId,
                        selectedCustomer.customerId
                    )
                    .toPromise()) as any;

                if (data.length > 0) {
                    const exportData: any[] = [];

                    this.populateExportDataForCustomer(
                        data,
                        exportData
                    );

                    await this.exportReportForCustomer(exportData, data[0].company);
                }
            }
        );

        await Promise.all(promises);
        this.downloading = false;
    }

    async onSendEmail() {
        this.emailing = true;
        this.exportedCustomersCount = 0;

        const promises = this.selectedCustomers.map((selectedCustomer) =>
            this.customerInfoByGroupService
                .getGroupAnalyticsAndEmail(
                    this.sharedService.currentUser.groupId,
                    selectedCustomer.customerId
                )
                .toPromise()
        );

        await Promise.all(promises);
        this.emailing = false;
    }

    applyFilter(filter: string) {
        this.filter = filter;

        this.dataSources = this.data.customers.filter((customer) =>
            customer.company.toLowerCase().includes(filter)
        );
        this.focusTextbox();
    }

    customerSelectedClicked(customers) {
        this.selectedCustomers = customers.filter(c => c.checked == true);
        this.focusTextbox();
    }

    selectAllClicked(customers) {
        if (customers.filter(c => c.checked == true).length > 0)
            this.selectedCustomers = customers;
        else
            this.selectedCustomers = [];
        this.focusTextbox();
    }

    populateExportDataForCustomer(
        data: any,
        exportData: any[]
    ) {
        for (const fbo of data) {
            for (const fboPrice of fbo.groupCustomerFbos) {
                for (let i = 0; i < fboPrice.prices.length; i++) {
                    const row: any = {};
                    row.ICAO = i === 0 ? fboPrice.icao : '';
                    row['Volume Tier'] = fboPrice.prices[i].volumeTier;
                    row["Product"] = fboPrice.prices[i].product;
                    if (fboPrice.prices[i].priceBreakdownDisplayType === 0) {
                        row.Price = this.decimalPrecisionPipe.transform(fboPrice.prices[i].domPrivate);
                    } else if (fboPrice.prices[i].priceBreakdownDisplayType === 1) {
                        row['International Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].intPrivate)
                        row['Domestic Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].domPrivate);
                    } else if (fboPrice.prices[i].priceBreakdownDisplayType === 2) {
                        row['Commercial Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].domComm);
                        row['Private Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].domPrivate);
                    } else {
                        row['Int/Comm Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].intComm);
                        row['Int/Private Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].intPrivate);
                        row['Dom/Comm Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].domComm);
                        row['Dom/Private Price'] =
                        this.decimalPrecisionPipe.transform(fboPrice.prices[i].domPrivate);
                    }
                    row['Tail Numbers'] = fbo.tailNumbers;
                    exportData.push(row);
                }
                if (!fboPrice.prices.length) {
                    exportData.push({
                        'Dom/Comm': '',
                        'Dom/Private': '',
                        ICAO: fboPrice.icao,
                        'Int/Comm': '',
                        'Int/Private': '',
                        'Tail Numbers': '',
                        'Volume Tier': '',
                    });
                }
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

    focusTextbox() {
        const searchBox = document.getElementById("searchBox") as HTMLInputElement;

        if (searchBox && searchBox.value != "") {
            setTimeout(() => {
                searchBox.focus();
            }, 1);
        }
    }

    public onFilterInput(event: any, fieldName: string, operator: string): void {
        this.grid.filterByColumn(fieldName, operator, event.target.value);
        this.focusTextbox();
    }
}
