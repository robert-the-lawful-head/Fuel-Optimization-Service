import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
// Components
import * as moment from 'moment';

@Component({
    selector: 'app-pricing-expired-notification',
    styleUrls: ['./pricing-expired-notification.component.scss'],
    templateUrl: './pricing-expired-notification.component.html',
})
export class PricingExpiredNotificationComponent {
    @Input() hideRemindMeButton = false;

    constructor(
        private router: Router,
        public dialogRef: MatDialogRef<PricingExpiredNotificationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        if (data.hideRemindMeButton) {
            this.hideRemindMeButton = data.hideRemindMeButton;
        }
    }

    public onConfirmClicked() {
        this.router.navigate(['/default-layout/dashboard-fbo-updated']);
        this.dialogRef.close();
    }

    public onRemindMeLaterClick() {
        localStorage.setItem(
            'pricingExpiredNotification',
            moment().add(1, 'days').format('L')
        );
        this.dialogRef.close();
    }

    public onCancelClick() {
        this.dialogRef.close();
    }
}
