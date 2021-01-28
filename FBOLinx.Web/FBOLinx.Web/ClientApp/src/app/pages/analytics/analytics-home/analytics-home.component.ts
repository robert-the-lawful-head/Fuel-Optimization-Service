import { Component } from '@angular/core';
import * as moment from 'moment';

// Services
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'Analytics',
        link: '/default-layout/analytics',
    },
];

@Component({
    selector: 'app-analytics-home',
    templateUrl: './analytics-home.component.html',
    styleUrls: [ './analytics-home.component.scss' ],
})
export class AnalyticsHomeComponent {
    public pageTitle = 'Analytics';
    public breadcrumb: any[] = BREADCRUMBS;
    public filterStartDate: Date;
    public filterEndDate: Date;
    public pastThirtyDaysStartDate: Date;

    constructor(
        private sharedService: SharedService
    ) {
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.pastThirtyDaysStartDate = new Date(moment().add(-30, 'days').format('MM/DD/YYYY'));
        this.sharedService.titleChange(this.pageTitle);
    }
}
