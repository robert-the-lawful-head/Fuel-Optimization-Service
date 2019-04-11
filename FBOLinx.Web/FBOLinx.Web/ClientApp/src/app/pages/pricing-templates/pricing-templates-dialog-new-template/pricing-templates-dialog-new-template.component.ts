import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface NewPricingTemplateDialogData {
    oid: number;
    name: string;
    fboId: number;
}

@Component({
    selector: 'app-pricing-templates-dialog-new-template',
    templateUrl: './pricing-templates-dialog-new-template.component.html',
    styleUrls: ['./pricing-templates-dialog-new-template.component.scss']
})
/** pricing-templates-dialog-new-template component*/
export class PricingTemplatesDialogNewTemplateComponent {
    /** pricing-templates-dialog-new-template ctor */
    constructor(public dialogRef: MatDialogRef<PricingTemplatesDialogNewTemplateComponent>, @Inject(MAT_DIALOG_DATA) public data: NewPricingTemplateDialogData) {

    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
