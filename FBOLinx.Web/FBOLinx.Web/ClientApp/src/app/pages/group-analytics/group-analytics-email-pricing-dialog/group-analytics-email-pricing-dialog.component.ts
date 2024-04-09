import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
    AbstractControl,
    FormControl,
    FormGroup,
    ValidationErrors,
    Validators,
} from '@angular/forms';
import { validateEmail } from 'src/utils/email';
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

    form: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<GroupAnalyticsEmailPricingDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: GroupAnalyticsGenerateDialogData
    ) {
        this.form = new FormGroup({
            emailContentHtml: new FormControl(
                this.data.emailTemplate.emailContentHtml
            ),
            fromAddress: new FormControl(this.data.emailTemplate.fromAddress, [
                this.fromAddressValidator,
            ]),
            replyTo: new FormControl(
                this.data.emailTemplate.replyTo,
                Validators.pattern(
                    /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                )
            ),
            subject: new FormControl(this.data.emailTemplate.subject, Validators.required),
        });
    }

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

    onSendClick(): void {
        this.data.emailTemplate.emailContentHtml = this.form.value.emailContentHtml;
        this.data.emailTemplate.fromAddress = this.form.value.fromAddress;
        this.data.emailTemplate.replyTo = this.form.value.replyTo;
        this.data.emailTemplate.subject = this.form.value.subject;
        this.dialogRef.close(this.data);
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    fromAddressValidator(control: AbstractControl): ValidationErrors | null {
        const value = control.value;

        if (!value) {
            return null;
        }

        return validateEmail(value + '@fbolinx.com')
            ? null
            : { emailNotValid: true };
    }
}
