import { Component, OnInit, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

@Component({
    selector: "fbo-prices-select-default-template",
    templateUrl: "./fbo-prices-select-default-template.component.html",
    styleUrls: ["./fbo-prices-select-default-template.component.scss"],
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
        this.dialogRef.close("cancel");
    }
}
