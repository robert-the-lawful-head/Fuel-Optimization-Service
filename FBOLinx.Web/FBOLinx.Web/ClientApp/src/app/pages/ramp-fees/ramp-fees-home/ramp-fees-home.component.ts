import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material';

//Services
import { RampfeesService } from '../../../services/rampfees.service';
import { AircraftsService } from '../../../services/aircrafts.service';
import { SharedService } from '../../../layouts/shared-service';
import { Parametri } from '../../../services/paremeters.service';

import * as SharedEvents from '../../../models/sharedEvents';

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
export class RampFeesHomeComponent implements OnInit, AfterViewInit, OnDestroy {

    public pageTitle: string = 'Ramp Fees';
    public breadcrumb: any[] = BREADCRUMBS;
    public rampFees: any[];
    public requiresUpdate: boolean = false;
    public expirationDate: any;
    public locationChangedSubscription: any;
    public aircraftTypes: any[];

    /** ramp-fees-home ctor */
    constructor(
        private rampFeesService: RampfeesService,
        private sharedService: SharedService,
        private aircraftsService: AircraftsService,
        public newRampFeeDialog: MatDialog,
        private messageService: Parametri
    ) {
        this.sharedService.emitChange(this.pageTitle);
        this.aircraftsService.getAll().subscribe((data: any) => this.aircraftTypes = data);
    }

    ngOnInit() {
        this.initRampfees();
    }

    ngAfterViewInit() {
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(message => {
            if (message === SharedEvents.locationChangedEvent) {
                this.rampFees = null;
                this.initRampfees();
            }
        });
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    public initRampfees() {
        this.rampFeesService.getForFbo({ oid: this.sharedService.currentUser.fboId }).subscribe((data: any) => {
            this.rampFees = data;
            this.expirationDate = data[1].expirationDate;
            this.messageService.updateMessage(this.expirationDate);
            this.messageService.getMessage().subscribe((mymessage: any) => this.expirationDate = mymessage);
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

    public rampFeeRequiresUpdate() {
        this.rampFees.forEach(fee => {
            fee.ExpirationDate = this.expirationDate;
                this.updateRampFee(fee);

        });
        this.messageService.updateMessage(this.expirationDate);
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
