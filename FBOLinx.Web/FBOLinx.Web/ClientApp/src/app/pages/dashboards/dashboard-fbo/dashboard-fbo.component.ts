import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

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
import { AnalysisPriceOrdersChartComponent } from
    '../../../shared/components/analysis-price-orders-chart/analysis-price-orders-chart.component';
import { AnalysisFuelreqsTopCustomersFboComponent } from
    '../../../shared/components/analysis-fuelreqs-top-customers-fbo/analysis-fuelreqs-top-customers-fbo.component';
import { AnalysisFuelreqsByAircraftSizeComponent } from
    '../../../shared/components/analysis-fuelreqs-by-aircraft-size/analysis-fuelreqs-by-aircraft-size.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Dashboard',
        link: '#/default-layout/dashboard-fbo'
    }
];

@Component({
    selector: 'app-dashboard-fbo',
    templateUrl: './dashboard-fbo.component.html',
    styleUrls: ['./dashboard-fbo.component.scss']
})
/** dashboard-fbo component*/
export class DashboardFboComponent {

    public pageTitle: string = 'Dashboard';
    public breadcrumb: any[] = BREADCRUMBS;
    public statisticsOptions: any = { useCard: true };
    public dashboardSettings: any;
    public fboid: any;
    public groupid: any;

    @ViewChild('statisticsTotalOrders') private statisticsTotalOrders: StatisticsTotalOrdersComponent;
    @ViewChild('statisticsTotalCustomers') private statisticsTotalCustomers: StatisticsTotalCustomersComponent;
    @ViewChild('statisticsTotalAircraft') private statisticsTotalAircraft: StatisticsTotalAircraftComponent;
    @ViewChild('statisticsOrdersByLocation') private statisticsOrdersByLocation: StatisticsOrdersByLocationComponent;
    @ViewChild('analysisPriceOrdersChart') private analysisPriceOrdersChart: AnalysisPriceOrdersChartComponent;
    @ViewChild('analysisFuelreqsTopCustomersFbo') private analysisFuelreqsTopCustomersFbo: AnalysisFuelreqsTopCustomersFboComponent;
    @ViewChild('analysisFuelreqsByAircraftSize') private analysisFuelreqsByAircraftSize: AnalysisFuelreqsByAircraftSizeComponent;

    /** dashboard-fbo ctor */
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
        this.analysisPriceOrdersChart.refreshData();
        this.analysisFuelreqsTopCustomersFbo.refreshData();
        this.analysisFuelreqsByAircraftSize.refreshData();
    }
}
