import { Component, OnInit, ViewChild } from '@angular/core';

import { NgxUiLoaderService } from 'ngx-ui-loader';

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
import { AnalyticsOrdersQuoteChartComponent } from '../../../shared/components/analytics-orders-quote-chart/analytics-orders-quote-chart.component';
import { AnalyticsOrdersOverTimeChartComponent } from '../../../shared/components/analytics-orders-over-time-chart/analytics-orders-over-time-chart.component';
import { AnalyticsVolumesNearbyAirportChartComponent } from '../../../shared/components/analytics-volumes-nearby-airport-chart/analytics-volumes-nearby-airport-chart.component';
import { AnalyticsMarketShareAirportChartComponent } from '../../../shared/components/analytics-market-share-airport-chart/analytics-market-share-airport-chart.component';
import { AnalyticsCustomerBreakdownChartComponent } from '../../../shared/components/analytics-customer-breakdown-chart/analytics-customer-breakdown-chart.component';
import { AnalyticsCompaniesQuotesDealTableComponent } from '../../../shared/components/analytics-companies-quotes-deal-table/analytics-companies-quotes-deal-table.component';
import { AnalyticsFuelVendorSourceChartComponent } from '../../../shared/components/analytics-fuel-vendor-source-chart/analytics-fuel-vendor-source-chart.component';
import { AnalyticsMarketShareFboAirportChartComponent } from '../../../shared/components/analytics-market-share-fbo-airport-chart/analytics-market-share-fbo-airport-chart.component';

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
    @ViewChild('analyticsOrdersQuoteChart')
    public analyticsOrdersQuoteChart: AnalyticsOrdersQuoteChartComponent;
    @ViewChild('analyticsOrdersOverTimeChart')
    public analyticsOrdersOverTimeChart: AnalyticsOrdersOverTimeChartComponent;
    @ViewChild('analyticsVolumesNearbyAirportChart')
    public analyticsVolumesNearbyAirportChart: AnalyticsVolumesNearbyAirportChartComponent;
    @ViewChild('analyticsMarketShareAirportChart')
    public analyticsMarketShareAirportChart: AnalyticsMarketShareAirportChartComponent;
    @ViewChild('analyticsCustomerBreakdownChart')
    public analyticsCustomerBreakdownChart: AnalyticsCustomerBreakdownChartComponent;
    @ViewChild('analyticsCompaniesQuotesDealTable')
    public analyticsCompaniesQuotesDealTable: AnalyticsCompaniesQuotesDealTableComponent;
    @ViewChild('analyticsFuelVendorSourceChart')
    public analyticsFuelVendorSourceChart: AnalyticsFuelVendorSourceChartComponent;
    @ViewChild('analyticsMarketShareFboAirportChart')
    public analyticsMarketShareFboAirportChart: AnalyticsMarketShareFboAirportChartComponent;

    public pageTitle = 'Analytics';
    public breadcrumb: any[] = BREADCRUMBS;
    public filterStartDate: Date;
    public filterEndDate: Date;
    public pastThirtyDaysStartDate: Date;

    constructor(
        private store: Store<State>,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.pastThirtyDaysStartDate = new Date(moment().add(-30, 'days').format('MM/DD/YYYY'));
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.ngxLoader.startLoader('loader-01');

        this.store.dispatch(breadcrumbSet({ breadcrumbs: BREADCRUMBS }));
    }

    public applyDateFilterChange() {
        this.statisticsTotalOrders.refreshData();
        this.statisticsTotalCustomers.refreshData();
        this.statisticsTotalAircraft.refreshData();
        this.statisticsOrdersByLocation.refreshData();
        this.analyticsOrdersQuoteChart.refreshData();
        this.analyticsOrdersOverTimeChart.refreshData();
        this.analyticsVolumesNearbyAirportChart.refreshData();
        this.analyticsCustomerBreakdownChart.refreshData();
        this.analyticsFuelVendorSourceChart.refreshData();
        this.analyticsMarketShareFboAirportChart.refreshData();
    }
}
