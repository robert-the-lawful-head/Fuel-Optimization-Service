import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { NavigationEnd, Router } from '@angular/router';

import { SharedService } from '../../../../layouts/shared-service';
import { Subscription } from 'rxjs';

@Component({
    providers: [SharedService],
    selector: 'app-distribution-wizard-review',
    styleUrls: ['./distribution-wizard-review.component.scss'],
    templateUrl: './distribution-wizard-review.component.html',
})
export class DistributionWizardReviewComponent {
    @Output() idChanged1: EventEmitter<any> = new EventEmitter();

    public navigationSubscription: any;
    public previewEmail: string;
    routerSubscription: Subscription;
    constructor(
        public dialogRef: MatDialogRef<DistributionWizardReviewComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private router: Router
    ) {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;

        this.routerSubscription = this.router.events.subscribe((evt) => {
            if (evt instanceof NavigationEnd) {
                // trick the Router into believing it's last link wasn't previously loaded
                this.router.navigated = false;
                // if you need to scroll back to top, here is the right place
                window.scrollTo(0, 0);
            }
        });
    }
    ngOnDestroy() {
        this.routerSubscription?.unsubscribe();
    }
    public closeDialog() {
        this.dialogRef.close();
    }
}
