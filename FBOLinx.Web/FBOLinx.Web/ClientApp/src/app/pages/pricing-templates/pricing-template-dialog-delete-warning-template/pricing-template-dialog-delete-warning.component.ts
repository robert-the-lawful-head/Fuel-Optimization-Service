import { Component, Inject, OnInit } from '@angular/core';
import {
    MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
    MatLegacyDialog as MatDialog,
    MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';
import { PricingTemplate } from '../../../models';
export interface PricingTemplatesDialogDeleteWarningComponentData {
    action: string;
    data: PricingTemplate[]
}

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
    ) { dialogRef.disableClose = true; }

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
