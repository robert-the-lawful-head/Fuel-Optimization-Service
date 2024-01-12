import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import * as moment from 'moment';

// Services
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { StatisticsOrdersByLocationComponent } from '../../../shared/components/statistics-orders-by-location/statistics-orders-by-location.component';
import { StatisticsTotalAircraftComponent } from '../../../shared/components/statistics-total-aircraft/statistics-total-aircraft.component';
import { StatisticsTotalCustomersComponent } from '../../../shared/components/statistics-total-customers/statistics-total-customers.component';
// Components
import { StatisticsTotalOrdersComponent } from '../../../shared/components/statistics-total-orders/statistics-total-orders.component';

@Component({
    selector: 'app-dashboard-fbo',
    styleUrls: ['./dashboard-fbo.component.scss'],
    templateUrl: './dashboard-fbo.component.html',
})
export class DashboardFboComponent implements AfterViewInit, OnDestroy {
    public pageTitle = 'Dashboard';
    public fboid: any;
    public groupid: any;
    public updatedPrice: any;
    public locationChangedSubscription: any;
    public filterStartDate: Date;
    public filterEndDate: Date;
    public pastThirtyDaysStartDate: Date;
    @ViewChild('statisticsTotalOrders')
    private statisticsTotalOrders: StatisticsTotalOrdersComponent;
    @ViewChild('statisticsTotalCustomers')
    private statisticsTotalCustomers: StatisticsTotalCustomersComponent;
    @ViewChild('statisticsTotalAircraft')
    private statisticsTotalAircraft: StatisticsTotalAircraftComponent;
    @ViewChild('statisticsOrdersByLocation')
    private statisticsOrdersByLocation: StatisticsOrdersByLocationComponent;

    constructor(private sharedService: SharedService) {
        location.href = '/default-layout/dashboard-fbo-updated/';

        this.filterStartDate = new Date(
            moment().add(-12, 'M').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.pastThirtyDaysStartDate = new Date(
            moment().add(-30, 'days').format('MM/DD/YYYY')
        );
        this.fboid = this.sharedService.currentUser.fboId;
        this.groupid = this.sharedService.currentUser.groupId;
        this.sharedService.titleChange(this.pageTitle);
    }

    get isCsr() {
        return this.sharedService.currentUser.role === 5;
    }

    ngAfterViewInit() {
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.applyDateFilterChange();
                }
            });
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    applyDateFilterChange() {
        this.statisticsTotalOrders.refreshData();
        this.statisticsTotalCustomers.refreshData();
        this.statisticsTotalAircraft.refreshData();
        this.statisticsOrdersByLocation.refreshData();
    }
}
