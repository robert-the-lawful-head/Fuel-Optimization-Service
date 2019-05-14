import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Fuel Orders',
        link: '#/default-layout/fuelreqs'
    }
];

@Component({
    selector: 'app-fuelreqs-home',
    templateUrl: './fuelreqs-home.component.html',
    styleUrls: ['./fuelreqs-home.component.scss']
})
/** fuelreqs-home component*/
export class FuelreqsHomeComponent {

    //Public Members
    public pageTitle: string = 'Fuel Orders';
    public breadcrumb: any[] = BREADCRUMBS;
    public fuelreqsData: Array<any>;

    /** fuelreqs-home ctor */
    constructor(private fuelReqService: FuelreqsService, private sharedService: SharedService) {
        this.sharedService.emitChange(this.pageTitle);
        this.loadFuelReqData();
    }

    public dateFilterChanged(event) {
        this.loadFuelReqData();
    }

    private loadFuelReqData() {
        this.fuelreqsData = null;
        this.fuelReqService.getForFboAndDateRange(this.sharedService.currentUser.fboId, this.sharedService.dashboardSettings.filterStartDate, this.sharedService.dashboardSettings.filterEndDate).subscribe((data: any) => {
            this.fuelreqsData = data;
        });
    }
}
