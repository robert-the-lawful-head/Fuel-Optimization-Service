import { Component, OnInit, ViewChild } from '@angular/core';

//Services
import { SharedService } from '../../../layouts/shared-service';

//Components
import { StatisticsTotalOrdersComponent } from '../../../shared/components/statistics-total-orders/statistics-total-orders.component';
import { StatisticsTotalCustomersComponent } from
    '../../../shared/components/statistics-total-customers/statistics-total-customers.component';
import { StatisticsTotalAircraftComponent } from
    '../../../shared/components/statistics-total-aircraft/statistics-total-aircraft.component';
import { StatisticsOrdersByLocationComponent } from
    '../../../shared/components/statistics-orders-by-location/statistics-orders-by-location.component';
import { AnalyticsOrdersQuoteChartComponent } from
    '../../../shared/components/analytics-orders-quote-chart/analytics-orders-quote-chart.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Analytics',
        link: '#/default-layout/analytics'
    }
];

@Component({
    selector: 'app-analytics-home',
    templateUrl: './analytics-home.component.html',
    styleUrls: ['./analytics-home.component.scss']
})
/** analytics-home component*/
export class AnalyticsHomeComponent {
    @ViewChild('statisticsTotalOrders') private statisticsTotalOrders: StatisticsTotalOrdersComponent;
    @ViewChild('statisticsTotalCustomers') private statisticsTotalCustomers: StatisticsTotalCustomersComponent;
    @ViewChild('statisticsTotalAircraft') private statisticsTotalAircraft: StatisticsTotalAircraftComponent;
    @ViewChild('statisticsOrdersByLocation') private statisticsOrdersByLocation: StatisticsOrdersByLocationComponent;
    @ViewChild('analyticsOrdersQuoteChart') private analyticsOrdersQuoteChart: AnalyticsOrdersQuoteChartComponent;

    public pageTitle: string = 'Analytics';
    public breadcrumb: any[] = BREADCRUMBS;
    public statisticsOptions: any = { useCard: true };
    public dashboardSettings: any;
    public fboid: any;
    public groupid: any;

    constructor(private sharedService: SharedService) {
        this.dashboardSettings = this.sharedService.dashboardSettings;
        this.fboid = this.sharedService.currentUser.fboId;
        this.groupid = this.sharedService.currentUser.groupId;
        this.sharedService.emitChange(this.pageTitle);
    }

    public applyDateFilterChange() {
        this.statisticsTotalOrders.refreshData();
        this.statisticsTotalCustomers.refreshData();
        this.statisticsTotalAircraft.refreshData();
        this.statisticsOrdersByLocation.refreshData();
        this.analyticsOrdersQuoteChart.refreshData();
    }
}
