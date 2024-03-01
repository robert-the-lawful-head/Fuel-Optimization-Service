import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AnaliticsReportType } from '../analytics-report-popup/analytics-report-popup.component';

export interface AnatylticsReports {
    type: AnaliticsReportType,
    title : string,
    description : string,
    link: string
}

@Component({
  selector: 'app-analytics-activity-reports',
  templateUrl: './analytics-activity-reports.component.html',
  styleUrls: ['./analytics-activity-reports.component.scss']
})
export class AnalyticsActivityReportsComponent implements OnInit {
    @Output() reportClicked = new EventEmitter<AnatylticsReports>();

    reports : AnatylticsReports[] = [
        {
            type: AnaliticsReportType.CustomerStatistics,
            title: 'Customer Statistics',
            description: 'This report allows you to track detailed FuelerLinx operator analytics related to their fuel orders, conversion rates, frequency of visits, etc.',
            link: 'activity-report'
        },
        {
            type: AnaliticsReportType.ArrivalsDepartures,
            title: 'Arrivals & Departures',
            description: 'This report shows airport arrivals and departures by tail number',
            link: 'activity-report'
        },
        {
            type: AnaliticsReportType.FBONetworkArrivalDepartures,
            title: 'FBO Network Arrival & Departures',
            description: 'This report shows ADS-B tracked aircraft visits to particpating FBOLinx locations within your FBO Network',
            link: 'activity-report'
        },
        {
            type: AnaliticsReportType.LostToCompetition,
            title: 'Lost to Competition',
            description: 'FuelerLinx customers that have dispatched fuel at another FBO at your airport.',
            link: 'activity-report'
        },
        {
            type: AnaliticsReportType.FuelerLinxCustomerCaptureRate,
            title: 'FuelerLinx Customer Capture Rate',
            description: 'This report is a tracks the percentage of times a FuelerLinx customer dispatched fuel at your FBO vs. your competition',
            link: 'activity-report'
        }
    ];

    constructor() { }

    ngOnInit() {
    }

    openReport(anatylticsReports: AnatylticsReports) {
        this.reportClicked.emit(anatylticsReports)
    }
}
