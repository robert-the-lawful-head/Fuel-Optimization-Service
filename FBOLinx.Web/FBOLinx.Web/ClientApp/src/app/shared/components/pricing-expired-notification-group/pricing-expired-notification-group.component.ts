import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
// Components
import * as moment from 'moment';

@Component({
    selector: 'app-pricing-expired-notification-group',
    styleUrls: ['./pricing-expired-notification-group.component.scss'],
    templateUrl: './pricing-expired-notification-group.component.html',
})
export class PricingExpiredNotificationGroupComponent {
    constructor(
        private router: Router,
        public dialogRef: MatDialogRef<PricingExpiredNotificationGroupComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

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

    public onManageClick(fbo) {
        this.dialogRef.close(fbo);
    }
}
