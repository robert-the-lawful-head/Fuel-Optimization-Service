import { Component, Inject, HostListener } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";

@Component({
    selector: "app-request-demo-success",
    templateUrl: "./request-demo-success.component.html",
    styleUrls: ["./request-demo-success.component.scss"],
})
export class RequestDemoSuccessComponent {
    public zohoLoader = "Zoho loader";

    constructor(
        public dialogRef: MatDialogRef<RequestDemoSuccessComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
