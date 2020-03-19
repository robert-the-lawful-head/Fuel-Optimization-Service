import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


@Component({
    selector: 'app-pricing-templates-delete-warning-template',
    templateUrl: './pricing-template-dialog-delete-warning.component.html',
    styleUrls: ['./pricing-template-dialog-delete-warning.component.scss']
})

export class PricingTemplatesDialogDeleteWarningComponent implements OnInit {

    public selectedTemplate: any;

    constructor(
        public dialogRef: MatDialogRef<PricingTemplatesDialogDeleteWarningComponent>,
        @Inject(MAT_DIALOG_DATA) public data,
        public closeConfirmationDialog: MatDialog,
    ) {

        
    }

    ngOnInit() {
        
    }

    onSelect() {
        if (this.selectedTemplate) {
            this.dialogRef.close(this.selectedTemplate);
        }
    }

    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }
}
