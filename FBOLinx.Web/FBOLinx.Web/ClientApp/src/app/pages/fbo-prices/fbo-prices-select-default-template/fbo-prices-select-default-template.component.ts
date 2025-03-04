import { Component, Inject, OnInit } from '@angular/core';
import {
    MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
    MatLegacyDialog as MatDialog,
    MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';

@Component({
    selector: 'fbo-prices-select-default-template',
    styleUrls: ['./fbo-prices-select-default-template.component.scss'],
    templateUrl: './fbo-prices-select-default-template.component.html',
})
export class FboPricesSelectDefaultTemplateComponent implements OnInit {
    public selectedTemplate: any;

    constructor(
        public dialogRef: MatDialogRef<FboPricesSelectDefaultTemplateComponent>,
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
