import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { LngLatLike } from 'mapbox-gl';
import * as moment from 'moment';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';

// Services
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { StatisticsOrdersByLocationComponent } from '../../../shared/components/statistics-orders-by-location/statistics-orders-by-location.component';
import { StatisticsTotalAircraftComponent } from '../../../shared/components/statistics-total-aircraft/statistics-total-aircraft.component';
import { StatisticsTotalCustomersComponent } from '../../../shared/components/statistics-total-customers/statistics-total-customers.component';
// Components
import { StatisticsTotalOrdersComponent } from '../../../shared/components/statistics-total-orders/statistics-total-orders.component';
import { FlightWatchMapService } from '../../flight-watch/flight-watch-map/flight-watch-map-services/flight-watch-map.service';
import {localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';
import { GroupsService } from 'src/app/services/groups.service';
import { ManageFboGroupsService } from 'src/app/services/managefbo.service';

@Component({
    selector: 'app-dashboard-fbo-updated',
    styleUrls: ['./dashboard-fbo-updated.component.scss'],
    templateUrl: './dashboard-fbo-updated.component.html',
})
export class DashboardFboUpdatedComponent implements AfterViewInit, OnDestroy {
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
    flightWatchData: FlightWatchModelResponse[] = null;
    isStable: boolean = true;
    selectedICAO: string = "";
    isSingleSourceFbo: boolean = false;

    groupsFbosData: any = null;

    constructor(private sharedService: SharedService,
        private router: Router,
        private flightWatchMapService: FlightWatchMapService,
        private manageFboGroupsService: ManageFboGroupsService,
        private groupsService: GroupsService,
        ) {
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

        this.selectedICAO = this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao);

        this.checkfboVariables();
    }
    async checkfboVariables(){
        this.groupsFbosData = await this.groupsService.groupsAndFbos().toPromise();

        var singleSource = this.sharedService
        .getCurrentUserPropertyValue(
            localStorageAccessConstant.isSingleSourceFbo
        );

        if(!singleSource){
            let groupId = this.sharedService.getCurrentUserPropertyValue(
                localStorageAccessConstant.groupId
            );
            let icao = this.sharedService.getCurrentUserPropertyValue(
                localStorageAccessConstant.icao
            );
            this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.isNetworkFbo,this.manageFboGroupsService.isNetworkFbo(this.groupsFbosData,Number(groupId)).toString());

            var isSingleSourceFbo = await this.groupsService.isGroupFboSingleSource(icao).toPromise();

            this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.isSingleSourceFbo,isSingleSourceFbo.toString());

        }

        this.isSingleSourceFbo = JSON.parse(
            this.sharedService
                .getCurrentUserPropertyValue(
                    localStorageAccessConstant.isSingleSourceFbo
                )
                ?.toLowerCase()
        );
    }

    get isCsr() {
        return this.sharedService.currentUser.role === 5;
    }

    get isMember() {
        return this.sharedService.currentUser.role === 4;
    }
    async ngOnInit() {
        await this.setFlightWatchMapCenter();

        this.sharedService.valueChanged$.subscribe((value: {event: string, data: FlightWatchModelResponse[]}) => {
            if(value.event === SharedEvents.flightWatchDataEvent){
                if(value.data){
                    this.flightWatchData = value.data;
                    this.isStable = true;
                }else{
                    this.flightWatchData = null;
                    this.isStable = false;
                }
            }
        });
    }

    ngAfterViewInit() {
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.applyDateFilterChange();
                }else if(message == SharedEvents.icaoChangedEvent){
                    this.selectedICAO = this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao);
                    this.setFlightWatchMapCenter();
                }
            });
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }
    async setFlightWatchMapCenter(){
        if(this.selectedICAO)
            this.center = await this.flightWatchMapService.getMapCenter(this.selectedICAO);
    }
    applyDateFilterChange() {
        this.statisticsTotalOrders.refreshData();
        this.statisticsTotalCustomers.refreshData();
        this.statisticsTotalAircraft.refreshData();
        this.statisticsOrdersByLocation.refreshData();
    }

    gotoFlightWatch(){
        this.router.navigate(['/default-layout/flight-watch']);
    }
}
