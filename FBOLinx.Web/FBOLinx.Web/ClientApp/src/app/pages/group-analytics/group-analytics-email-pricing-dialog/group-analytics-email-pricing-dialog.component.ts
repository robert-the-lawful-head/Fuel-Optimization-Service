import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
    DetailRowService,
    FilterSettingsModel,
    GridComponent,
    GridModel,
    SelectionSettingsModel,
} from '@syncfusion/ej2-angular-grids';
import { ImageSettingsModel } from '@syncfusion/ej2-angular-richtexteditor';

import { SharedService } from '../../../layouts/shared-service';
import { GroupAnalyticsGenerateDialogData } from '../group-analytics-generate-dialog/group-analytics-generate-dialog.component';

@Component({
    providers: [SharedService, DetailRowService],
    selector: 'app-group-analytics-email-pricing-dialog',
    styleUrls: ['./group-analytics-email-pricing-dialog.component.scss'],
    templateUrl: './group-analytics-email-pricing-dialog.component.html',
})
export class GroupAnalyticsEmailPricingDialogComponent implements OnInit {
    @ViewChild('grid') public grid: GridComponent;
    childGrid: GridModel;

    selectionOptions: SelectionSettingsModel = {
        checkboxMode: 'ResetOnRowClick',
    };
    pageSettings: any = {
        pageSize: 25,
        pageSizes: [25, 50, 100, 'All'],
    };
    filterSettings: FilterSettingsModel = { type: 'Menu' };

    gridData: any[] = [];

    public insertImageSettings: ImageSettingsModel = { saveFormat: 'Base64' }

    constructor(
        public dialogRef: MatDialogRef<GroupAnalyticsEmailPricingDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: GroupAnalyticsGenerateDialogData
    ) {}

    public ngOnInit(): void {
        this.data.customers
            .filter((customer) => customer.contacts.length > 0)
            .forEach((customer) => {
                this.gridData.push({
                    ...customer,
                    isCustomer: true,
                });
                customer.contacts.forEach((contact) => {
                    this.gridData.push({
                        ...contact,
                        company: '',
                        isCustomer: false,
                    });
                });
            });
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
