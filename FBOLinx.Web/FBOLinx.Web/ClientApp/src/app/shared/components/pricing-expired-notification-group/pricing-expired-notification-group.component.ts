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
    selector: "app-pricing-expired-notification-group",
    templateUrl: "./pricing-expired-notification-group.component.html",
    styleUrls: ["./pricing-expired-notification-group.component.scss"],
})
export class PricingExpiredNotificationGroupComponent {
    constructor(
        private router: Router,
        public dialogRef: MatDialogRef<PricingExpiredNotificationGroupComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    public onConfirmClicked() {
        this.router.navigate(["/default-layout/dashboard-fbo"]);
        this.dialogRef.close();
    }

    ngOnInit(): void {
        console.log('in component');
        console.log(this);
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

    public onManageClick(fbo) {
        this.dialogRef.close(fbo);
    }
}
