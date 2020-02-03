import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Components
import * as moment from 'moment';

@Component({
    selector: 'app-pricing-expired-notification',
    templateUrl: './pricing-expired-notification.component.html',
    styleUrls: ['./pricing-expired-notification.component.scss']
})
/** pricing-expired-notification component*/
export class PricingExpiredNotificationComponent {

    /** pricing-expired-notification ctor */
    constructor(private router: Router,
        public dialogRef: MatDialogRef<PricingExpiredNotificationComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {

    }

    public onConfirmClicked() {
        this.router.navigate(['/default-layout/fbo-prices']);
        this.dialogRef.close();
    }

    public onRemindMeLaterClick() {
        localStorage.setItem('pricingExpiredNotification', moment().format('L'));
        this.dialogRef.close();
    }

    public onCancelClick() {
        sessionStorage.setItem('pricingExpiredNotification', moment().format('L'));
        this.dialogRef.close();
    }
}
