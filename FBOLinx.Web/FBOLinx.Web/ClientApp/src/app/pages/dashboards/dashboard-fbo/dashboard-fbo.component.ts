import { Component, AfterViewInit, ViewChild, OnDestroy } from "@angular/core";

import * as moment from "moment";

// Services
import { SharedService } from "../../../layouts/shared-service";

import * as SharedEvents from "../../../models/sharedEvents";

// Components
import { StatisticsTotalOrdersComponent } from "../../../shared/components/statistics-total-orders/statistics-total-orders.component";
import { StatisticsTotalCustomersComponent } from "../../../shared/components/statistics-total-customers/statistics-total-customers.component";
import { StatisticsTotalAircraftComponent } from "../../../shared/components/statistics-total-aircraft/statistics-total-aircraft.component";
import { StatisticsOrdersByLocationComponent } from "../../../shared/components/statistics-orders-by-location/statistics-orders-by-location.component";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "/default-layout",
    },
    {
        title: "Dashboard",
        link: "/default-layout/dashboard-fbo",
    },
];

@Component({
    selector: "app-dashboard-fbo",
    templateUrl: "./dashboard-fbo.component.html",
    styleUrls: ["./dashboard-fbo.component.scss"],
})
export class DashboardFboComponent implements AfterViewInit, OnDestroy {
    public pageTitle = "Dashboard";
    public breadcrumb: any[] = BREADCRUMBS;
    public fboid: any;
    public groupid: any;
    public updatedPrice: any;
    public locationChangedSubscription: any;
    public filterStartDate: Date;
    public filterEndDate: Date;

    @ViewChild("statisticsTotalOrders")
    private statisticsTotalOrders: StatisticsTotalOrdersComponent;
    @ViewChild("statisticsTotalCustomers")
    private statisticsTotalCustomers: StatisticsTotalCustomersComponent;
    @ViewChild("statisticsTotalAircraft")
    private statisticsTotalAircraft: StatisticsTotalAircraftComponent;
    @ViewChild("statisticsOrdersByLocation")
    private statisticsOrdersByLocation: StatisticsOrdersByLocationComponent;

    constructor(private sharedService: SharedService) {
        this.filterStartDate = new Date(moment().add(-12, "M").format("MM/DD/YYYY"));
        this.filterEndDate = new Date(moment().format("MM/DD/YYYY"));
        this.fboid = this.sharedService.currentUser.fboId;
        this.groupid = this.sharedService.currentUser.groupId;
        this.sharedService.titleChange(this.pageTitle);
    }

    ngAfterViewInit() {
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.applyDateFilterChange();
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    public applyDateFilterChange() {
        this.statisticsTotalOrders.refreshData();
        this.statisticsTotalCustomers.refreshData();
        this.statisticsTotalAircraft.refreshData();
        this.statisticsOrdersByLocation.refreshData();
    }
}
