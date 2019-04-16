import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { RampfeesService } from '../../../services/rampfees.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { RampFeesDialogNewFeeComponent } from '../ramp-fees-dialog-new-fee/ramp-fees-dialog-new-fee.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Ramp Fees',
        link: '#/default-layout/ramp-fees'
    }
];

@Component({
    selector: 'app-ramp-fees-home',
    templateUrl: './ramp-fees-home.component.html',
    styleUrls: ['./ramp-fees-home.component.scss']
})
/** ramp-fees-home component*/
export class RampFeesHomeComponent implements OnInit {

    public pageTitle: string = 'Ramp Fees';
    public breadcrumb: any[] = BREADCRUMBS;
    public rampFees: any[];
    public requiresUpdate: boolean = false;

    /** ramp-fees-home ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private rampFeesService: RampfeesService,
        private sharedService: SharedService,
        public newRampFeeDialog: MatDialog) {
        this.sharedService.emitChange(this.pageTitle);
    }

    ngOnInit() {
        this.rampFeesService.getForFbo({ oid: this.sharedService.currentUser.fboId }).subscribe((data: any) => {
            this.rampFees = data;
        });
    }

    public addNewRampFeeClicked() {
        const dialogRef = this.newRampFeeDialog.open(RampFeesDialogNewFeeComponent, {
            data: {}
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            result.fboId = this.sharedService.currentUser.fboId;
            this.rampFeesService.add(result).subscribe((data: any) => {
                this.rampFees = null;
                this.loadRampFees();
            });
        });
    }

    public rampFeeFieldChanged() {
        this.requiresUpdate = true;
    }

    public rampFeeDeleted() {
        this.loadRampFees();
    }

    public saveChanges() {
        this.rampFees.forEach(fee => {
            if (fee.requiresUpdate) {
                this.updateRampFee(fee);
            }

        });
        this.requiresUpdate = false;
    }

    //Private Methods
    private updateRampFee(fee) {
        if (fee.oid && fee.oid > 0) {
            this.rampFeesService.update(fee).subscribe((data: any) => {
                
            });
        } else {
            this.rampFeesService.add(fee).subscribe((data: any) => {
                fee.oid = data.oid;
            });
        }
    }

    private loadRampFees() {
        this.rampFeesService.getForFbo({ oid: this.sharedService.currentUser.fboId }).subscribe((data: any) => {
            this.rampFees = data;
        });
    }
}
