import { Component, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

// Services
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { PricetiersService } from "../../../services/pricetiers.service";
import { Router } from "@angular/router";

export interface CopyPricingTemplateDialogData {
    currentPricingTemplateId: number;
    name: string;
}

@Component({
    selector: "copy-pricing-templates-dialog-new-template",
    templateUrl: "./pricing-template-dialog-copy-template.component.html",
    styleUrls: ["./pricing-template-dialog-copy-template.component.scss"],
})
export class PricingTemplatesDialogCopyTemplateComponent {
    constructor(
        public dialogRef: MatDialogRef<
            PricingTemplatesDialogCopyTemplateComponent
        >,
        public closeConfirmationDialog: MatDialog,
        public pricingTemplatesService: PricingtemplatesService,
        public priceTiersService: PricetiersService,
        public router: Router,
        @Inject(MAT_DIALOG_DATA) public data: CopyPricingTemplateDialogData
    ) {
        // Prevent modal close on outside click
        dialogRef.disableClose = true;
    }

    ConfirmCopy() {
        this.pricingTemplatesService.copy(this.data).subscribe((result) => {
            if (result) {
                this.router.navigate([
                    "/default-layout/pricing-templates/" + result,
                ]);
            }
        });
    }

    public onCancelClick(): void {
        this.dialogRef.close("cancel");
    }
}
