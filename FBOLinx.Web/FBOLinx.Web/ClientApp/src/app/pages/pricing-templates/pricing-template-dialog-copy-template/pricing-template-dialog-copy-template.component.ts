import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import {
    FormBuilder,
    FormGroup,
    Validators,
    FormControl,
} from "@angular/forms";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

// Services
import { SharedService } from "../../../layouts/shared-service";
import { RichTextEditorComponent } from "@syncfusion/ej2-angular-richtexteditor";
import { CloseConfirmationComponent } from "../../../shared/components/close-confirmation/close-confirmation.component";
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
export class PricingTemplatesDialogCopyTemplateComponent implements OnInit {
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

    ngOnInit() {
        console.log(this.data.currentPricingTemplateId);
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
