import { Component, Inject, OnInit } from '@angular/core';
import {
    MAT_DIALOG_DATA,
    MatDialog,
    MatDialogRef,
} from '@angular/material/dialog';

@Component({
    selector: 'app-pricing-templates-delete-warning-template',
    styleUrls: ['./pricing-template-dialog-delete-warning.component.scss'],
    templateUrl: './pricing-template-dialog-delete-warning.component.html',
})
export class PricingTemplatesDialogDeleteWarningComponent implements OnInit {
    public selectedTemplate: any;

    constructor(
        public dialogRef: MatDialogRef<PricingTemplatesDialogDeleteWarningComponent>,
        @Inject(MAT_DIALOG_DATA) public data,
        public closeConfirmationDialog: MatDialog
    ) {}

    ngOnInit() {}

    onSelect() {
        if (this.selectedTemplate) {
            this.dialogRef.close(this.selectedTemplate);
        }
    }

    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }
}
