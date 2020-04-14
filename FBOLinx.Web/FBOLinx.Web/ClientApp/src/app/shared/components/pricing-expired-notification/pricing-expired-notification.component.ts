import { Component, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

// Components
import * as moment from "moment";

@Component({
    selector: "app-pricing-expired-notification",
    templateUrl: "./pricing-expired-notification.component.html",
    styleUrls: ["./pricing-expired-notification.component.scss"],
})
export class PricingExpiredNotificationComponent {
    constructor(
        private router: Router,
        public dialogRef: MatDialogRef<PricingExpiredNotificationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    public onConfirmClicked() {
        this.router.navigate(["/default-layout/dashboard-fbo"]);
        this.dialogRef.close();
    }

    public onRemindMeLaterClick() {
        localStorage.setItem(
            "pricingExpiredNotification",
            moment().format("L")
        );
        this.dialogRef.close();
    }

    public onCancelClick() {
        sessionStorage.setItem(
            "pricingExpiredNotification",
            moment().format("L")
        );
        this.dialogRef.close();
    }
}
