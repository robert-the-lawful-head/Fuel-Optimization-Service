import { Component, OnInit, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { Store } from '@ngrx/store';

import { State } from '../../../store/reducers';
import { breadcrumbSet } from '../../../store/actions';

// Services
import { SharedService } from '../../../layouts/shared-service';

// Components
import { StatisticsTotalOrdersComponent } from '../../../shared/components/statistics-total-orders/statistics-total-orders.component';
import { StatisticsTotalCustomersComponent } from '../../../shared/components/statistics-total-customers/statistics-total-customers.component';
import { StatisticsTotalAircraftComponent } from '../../../shared/components/statistics-total-aircraft/statistics-total-aircraft.component';
import { StatisticsOrdersByLocationComponent } from '../../../shared/components/statistics-orders-by-location/statistics-orders-by-location.component';

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
    styleUrls: ['./analytics-home.component.scss'],
})
export class AnalyticsHomeComponent implements OnInit {
    @ViewChild('statisticsTotalOrders')
    public statisticsTotalOrders: StatisticsTotalOrdersComponent;
    @ViewChild('statisticsTotalCustomers')
    public statisticsTotalCustomers: StatisticsTotalCustomersComponent;
    @ViewChild('statisticsTotalAircraft')
    public statisticsTotalAircraft: StatisticsTotalAircraftComponent;
    @ViewChild('statisticsOrdersByLocation')
    public statisticsOrdersByLocation: StatisticsOrdersByLocationComponent;

    public pageTitle = 'Analytics';
    public breadcrumb: any[] = BREADCRUMBS;
    public filterStartDate: Date;
    public filterEndDate: Date;
    public pastThirtyDaysStartDate: Date;

    constructor(
        private store: Store<State>,
        private sharedService: SharedService
    ) {
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.pastThirtyDaysStartDate = new Date(moment().add(-30, 'days').format('MM/DD/YYYY'));
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.store.dispatch(breadcrumbSet({ breadcrumbs: BREADCRUMBS }));
    }

    public applyDateFilterChange() {
        this.statisticsTotalOrders.refreshData();
        this.statisticsTotalCustomers.refreshData();
        this.statisticsTotalAircraft.refreshData();
        this.statisticsOrdersByLocation.refreshData();
    }
}
