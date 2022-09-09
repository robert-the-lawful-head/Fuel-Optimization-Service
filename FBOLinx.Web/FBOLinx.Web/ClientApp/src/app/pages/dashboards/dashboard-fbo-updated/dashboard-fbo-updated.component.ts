import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { LngLatLike } from 'mapbox-gl';
import * as moment from 'moment';
import { Subscription, timer } from 'rxjs';
import { FlightWatch } from 'src/app/models/flight-watch';
import { AirportWatchService } from 'src/app/services/airportwatch.service';

// Services
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { StatisticsOrdersByLocationComponent } from '../../../shared/components/statistics-orders-by-location/statistics-orders-by-location.component';
import { StatisticsTotalAircraftComponent } from '../../../shared/components/statistics-total-aircraft/statistics-total-aircraft.component';
import { StatisticsTotalCustomersComponent } from '../../../shared/components/statistics-total-customers/statistics-total-customers.component';
// Components
import { StatisticsTotalOrdersComponent } from '../../../shared/components/statistics-total-orders/statistics-total-orders.component';

@Component({
    selector: 'app-dashboard-fbo-updated',
    styleUrls: ['./dashboard-fbo-updated.component.scss'],
    templateUrl: './dashboard-fbo-updated.component.html',
})
export class DashboardFboUpdatedComponent implements AfterViewInit, OnDestroy {
    public breadcrumb: any[];
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

    //flghtWatch
    center: LngLatLike;
    flightWatchData: FlightWatch[];
    airportWatchFetchSubscription: Subscription;
    mapLoadSubscription: Subscription;
    isMapLoading: boolean = true;

    constructor(private sharedService: SharedService,
        private router: Router,
        private airportWatchService: AirportWatchService) {
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

        this.breadcrumb = [
            {
                link: '/default-layout',
                title: 'Main',
            },
        ];
        if (!this.isCsr) {
            this.breadcrumb.push({
                link: '/default-layout/dashboard-fbo-updated',
                title: 'Dashboard',
            });
        } else {
            this.breadcrumb.push({
                link: '/default-layout/dashboard-csr',
                title: 'CSR Dashboard',
            });
        }
    }

    get isCsr() {
        return this.sharedService.currentUser.role === 5;
    }

    get isMember() {
        return this.sharedService.currentUser.role === 4;
    }
    ngOnInit() {
        this.mapLoadSubscription = timer(0, 15000).subscribe(() =>{
            this.loadAirportWatchData();
        });
    }

    ngAfterViewInit() {
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.applyDateFilterChange();

                    this.isMapLoading = true;
                    this.loadAirportWatchData();
                }
            });
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
        if (this.mapLoadSubscription) this.mapLoadSubscription.unsubscribe();
    }

    applyDateFilterChange() {
        this.statisticsTotalOrders.refreshData();
        this.statisticsTotalCustomers.refreshData();
        this.statisticsTotalAircraft.refreshData();
        this.statisticsOrdersByLocation.refreshData();
    }
    loadAirportWatchData() {
        return this.airportWatchFetchSubscription = this.airportWatchService
            .getAll(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((data: any) => {
                this.isMapLoading = false;
                this.center = {
                    lat: data.fboLocation.latitude,
                    lng: data.fboLocation.longitude,
                };
                this.flightWatchData = data.flightWatchData;
            }, (error: any) => {
                this.isMapLoading = false;
            });
    }
    gotoFlightWatch(){
        this.router.navigate(['/default-layout/flight-watch']);
    }
}
