import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import * as moment from 'moment';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { AnatylticsReports, analyticsReports } from '../analytics-activity-reports/analytics-activity-reports.component';
import { AnaliticsReportType } from '../analytics-report-popup/analytics-report-popup.component';
import { ActivatedRoute } from '@angular/router';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';

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
        private sharedService: SharedService,
        private cdr: ChangeDetectorRef,
        private route: ActivatedRoute
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

        var isSingleSourceFbo: boolean = JSON.parse(
            this.sharedService
                .getCurrentUserPropertyValue(
                    localStorageAccessConstant.isSingleSourceFbo
                )
        );

        this.route.queryParams.subscribe(params => {
            const reportType = params['report'] as AnaliticsReportType;
            if(reportType == AnaliticsReportType.LostToCompetition && !isSingleSourceFbo){
                this.openReport(analyticsReports[AnaliticsReportType.LostToCompetition]);
            }
        });
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
        this.selectedRerport = null;
        this.cdr.detectChanges();
        this.selectedRerport = reportType;
        this.cdr.detectChanges();
    }
}
