import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { AnatylticsReports } from '../analytics-activity-reports/analytics-activity-reports.component';

@Component({
    selector: 'app-analytics-home',
    styleUrls: ['./analytics-home.component.scss'],
    templateUrl: './analytics-home.component.html',
})
export class AnalyticsHomeComponent implements OnInit {
    public pageTitle = 'Analytics';
    public filterStartDate: Date;
    public filterEndDate: Date;
    public pastThirtyDaysStartDate: Date;
    public tailNumbers: any[] = [];
    public authenticatedIcao: string = '';
    public selectedRerport: AnatylticsReports;

    constructor(
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService
    ) {
        this.filterStartDate = new Date(
            moment().add(-12, 'M').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.pastThirtyDaysStartDate = new Date(
            moment().add(-30, 'days').format('MM/DD/YYYY')
        );
        this.sharedService.titleChange(this.pageTitle);
        this.authenticatedIcao = this.sharedService.currentUser.icao;
    }

    ngOnInit() {
        this.getAircrafts();
    }

    getAircrafts() {
        this.customerAircraftsService
            .getAircraftsListByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((tailNumbers: any[]) => {
                this.tailNumbers = tailNumbers;
            });
    }
    openReport(reportType: AnatylticsReports) {
        this.selectedRerport = reportType;
    }
}
