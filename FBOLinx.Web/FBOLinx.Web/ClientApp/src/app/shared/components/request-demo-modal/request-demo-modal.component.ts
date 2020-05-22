import { Component, Inject, HostListener } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { NgxUiLoaderService } from "ngx-ui-loader";

@Component({
    selector: "app-request-demo-modal",
    templateUrl: "./request-demo-modal.component.html",
    styleUrls: ["./request-demo-modal.component.scss"],
})
export class RequestDemoModalComponent {
    public zohoLoader = "Zoho loader";

    constructor(
        public dialogRef: MatDialogRef<RequestDemoModalComponent>,
        private ngxLoader: NgxUiLoaderService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.ngxLoader.startLoader(this.zohoLoader);
    }

    @HostListener("window:message", ["$event"])
    MessageUpdated(event: MessageEvent) {
        if (event.isTrusted && event.returnValue) {
            this.data.succeed = true;
            this.dialogRef.close(this.data);
        }
    }
    
    onCancelClick(): void {
        this.dialogRef.close();
    }

    public zohoLoaded() {
        this.ngxLoader.stopLoader(this.zohoLoader);
    }
}
