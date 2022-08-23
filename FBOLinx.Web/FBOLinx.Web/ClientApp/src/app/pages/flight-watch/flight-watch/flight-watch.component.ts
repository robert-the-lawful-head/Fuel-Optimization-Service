import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { MatTableDataSource } from '@angular/material/table';
import { ResizeEvent } from 'angular-resizable-element';
import { isEmpty, keyBy } from 'lodash';
import { LngLatLike } from 'mapbox-gl';
import { BehaviorSubject, Observable, Subscription, timer } from 'rxjs';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { SwimFilter } from 'src/app/models/filter';
import { Swim, SwimType } from 'src/app/models/swim';
import { User } from 'src/app/models/User';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { SwimService } from 'src/app/services/swim.service';
import { convertDMSToDEG } from 'src/utils/coordinates';

import { SharedService } from '../../../layouts/shared-service';
import { Aircraftwatch, FlightWatch, FlightWatchDictionary } from '../../../models/flight-watch';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FlightWatchMapComponent } from '../flight-watch-map/flight-watch-map.component';
import { FlightWatchMapOnlyComponent } from '../flight-watch-map-only/flight-watch-map-only.component';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/flight-watch',
        title: 'Flight Watch',
    },
];

@Component({
    selector: 'app-flight-watch',
    styleUrls: ['./flight-watch.component.scss'],
    templateUrl: './flight-watch.component.html',
})
export class FlightWatchComponent implements OnInit, OnDestroy {
    @ViewChild('map') map: FlightWatchMapComponent;
    @ViewChild('mapOnly') mapOnly: FlightWatchMapOnlyComponent;
    @ViewChild('mapfilters') public drawer: MatDrawer;

    pageTitle = 'Flight Watch';
    breadcrumb: any[] = BREADCRUMBS;

    loading = false;
    mapLoadSubscription: Subscription;
    airportWatchFetchSubscription: Subscription;
    flightWatchData: FlightWatch[];
    filteredFlightWatchData: FlightWatchDictionary;
    filter: string;
    filteredTypes: string[] = [];
    center: LngLatLike;
    selectedFlightWatch: FlightWatch;
    selectedAircraftData: Aircraftwatch;

    flightWatchDataSource: MatTableDataSource<FlightWatch>;

    flightWatchDataSubject = new BehaviorSubject<FlightWatch[]>([]);
    flightWatchDataObservable$ = this.flightWatchDataSubject.asObservable();

    AircraftLiveDatasubscription: Subscription;

    swimArrivals: Swim[];
    swimDepartures: Swim[];
    swimArrivalsAllRecords: Swim[];
    swimDeparturesAllRecords: Swim[];
    acukwikairport: AcukwikAirport[];
    airportsICAO: string[];
    selectedICAO: string;

    style: any = {};
    isMapShowing = true;

    arrivalsDeparturesProps = { initial: 0, currentWidth: 0 };

    constructor(
        private airportWatchService: AirportWatchService,
        private swimService: SwimService,
        private sharedService: SharedService,
        public dialog: MatDialog)
    {
        this.sharedService.titleChange(this.pageTitle);
        this.selectedICAO = (this.sharedService.currentUser.icao)?this.sharedService.currentUser.icao:localStorage.getItem('icao');
    }

    ngOnInit() {
        this.mapLoadSubscription = timer(0, 15000).subscribe(() =>{
            this.loadAirportWatchData();
        });
        this.mapLoadSubscription = timer(60000).subscribe(() =>{
            this.getDrawerData(this.selectedICAO, this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId);
        });
    }
    ngOnDestroy() {
        this.flightWatchDataSubject.unsubscribe();
        if (this.mapLoadSubscription) {
            this.mapLoadSubscription.unsubscribe();
        }
        if (this.airportWatchFetchSubscription) {
            this.airportWatchFetchSubscription.unsubscribe();
        }
    }
    getDrawerData(icao:string, groupId: number, fboId: number):void{
        let currentFilter: SwimFilter = { filterText : this.filter, dataType: null };

        this.swimService.getArrivals(icao, groupId, fboId).subscribe(
            arrivals => {
                this.swimArrivalsAllRecords = arrivals.result;
                if(this.filter) this.onFilterChanged(currentFilter);
                else this.swimArrivals = arrivals.result;
            }
          );
          this.swimService.getDepartures(icao, groupId, fboId).subscribe(
            departures => {
                this.swimDeparturesAllRecords = departures.result;
                if(this.filter) this.onFilterChanged(currentFilter);
                else this.swimDepartures = departures.result;
            }
          );
    }
    openDrawer(airportClicked: AcukwikAirport){
        this.getDrawerData(airportClicked.icao, this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId);
        this.drawer.toggle();
    }
    setIcaoList(airportList: AcukwikAirport[]){
        this.acukwikairport = airportList;
        let icaoList =  airportList.map((data) => {
            return data.icao;
        });
        this.airportsICAO = icaoList;

    }
    updateIcao(icao:string){
        this.map.goToAirport(icao);
        this.selectedICAO =  icao;
        this.getDrawerData(icao, this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId);
    }
    loadAirportWatchData() {
        if (!this.loading) {
            this.loading = true;
            return this.airportWatchFetchSubscription = this.airportWatchService
                .getAll(
                    this.sharedService.currentUser.groupId,
                    this.sharedService.currentUser.fboId
                )
                .subscribe((data: any) => {
                    if (!this.center) {
                        this.center = {
                            lat: data.fboLocation.latitude,
                            lng: data.fboLocation.longitude,
                        };
                    }
                    if (data && data != null) {
                        this.flightWatchData = data.flightWatchData;
                        this.setFilteredFlightWatchData();
                    }
                    this.loading = false;
                }, (error: any) => {
                    this.loading = false;
                });
        }
    }

    onTabClick(event) {
        if (event.tab.textLabel == "Takeoff/Landing Cycles")
            this.isMapShowing = false;
        else
            this.isMapShowing = true;
    }

    async onFlightWatchClick(flightWatch: FlightWatch) {
        if(!flightWatch.tailNumber) {
            this.selectedAircraftData = {
                customerInfoBygGroupId : 0,
                tailNumber: flightWatch?.tailNumber,
                atcFlightNumber: flightWatch?.atcFlightNumber,
                aircraftTypeCode: flightWatch?.aircraftTypeCode,
                isAircraftOnGround: flightWatch?.isAircraftOnGround,
                company: flightWatch?.company,
                aircraftMakeModel: flightWatch?.aircraftMakeModel,
                lastQuote: flightWatch?.lastQuote,
                currentPricing: flightWatch?.currentPricing
            };
            return;
        }

        if ( this.AircraftLiveDatasubscription ) {
            this.AircraftLiveDatasubscription.unsubscribe();
        }
        this.AircraftLiveDatasubscription = this.airportWatchService.getAircraftLiveData(this.sharedService.currentUser.groupId,this.sharedService.currentUser.fboId, flightWatch.tailNumber)
        .subscribe((res)=> {
            this.selectedAircraftData = res;
        });
    }

    onAircraftInfoClose() {
        this.selectedFlightWatch = undefined;
    }

    onFilterChanged(filter: SwimFilter) {
        this.filter = filter.filterText;
        this.setFilteredFlightWatchData();
        this.filterArrivals(this.filter?.toLowerCase());
        this.filterDepartures(this.filter?.toLowerCase());
    }
    filterArrivals(filter){
        if(filter && filter.trim())
        {
            this.swimArrivals = this.swimArrivalsAllRecords.filter(
                (fw) =>
                    fw.tailNumber?.toLowerCase().includes(filter) ||
                    fw.flightDepartment?.toLowerCase().includes(filter) ||
                    fw.departureCity?.toLowerCase().includes(filter)||
                    fw.arrivalCity?.toLowerCase().includes(filter)
            );
            return;
        }
        this.swimDepartures = this.swimDeparturesAllRecords;
    }
    filterDepartures(filter){
        if(filter && filter.trim())
        {
            this.swimDepartures = this.swimDeparturesAllRecords.filter(
                (fw) =>
                fw.tailNumber?.toLowerCase().includes(filter) ||
                fw.flightDepartment?.toLowerCase().includes(filter) ||
                fw.departureCity?.toLowerCase().includes(filter)||
                fw.arrivalCity?.toLowerCase().includes(filter)
            );
            return;
        }
        this.swimArrivals = this.swimArrivalsAllRecords;
    }
    setFilteredFlightWatchData() {
        let originalData: FlightWatch[];
        if (!this.filter) {
            originalData = this.flightWatchData;
        } else {
            const loweredFilter = this.filter.toLowerCase();
            originalData = this.flightWatchData.filter(
                (fw) =>
                    fw.aircraftHexCode.toLowerCase().includes(loweredFilter) ||
                    fw.atcFlightNumber.toLowerCase().includes(loweredFilter)
            );
        }

        if (this.filteredTypes.length) {
            originalData = originalData.filter((fw) =>
                this.filteredTypes.includes(fw.aircraftTypeCode)
            );
        }

        this.filteredFlightWatchData = keyBy(originalData, (fw) => fw.oid);

        this.flightWatchDataSubject.next(originalData);
        this.flightWatchDataSubject.asObservable();

        if (this.selectedFlightWatch) {
            this.selectedFlightWatch =
                this.filteredFlightWatchData[this.selectedFlightWatch.oid];
        }
    }

    validate(event: ResizeEvent): boolean {
        const MAX_DIMENSIONS_PX = 800;
        const MIN_DIMENSIONS_PX = 0;
        if (
            event.rectangle.width &&
            (event.rectangle.width > MAX_DIMENSIONS_PX ||
                event.rectangle.width < MIN_DIMENSIONS_PX)
        ) {
            return false;
        }
        return true;
    }

    onResizeEnd(event: ResizeEvent): void {
        this.style = {
            width: `${event.rectangle.width}px`,
        };

        setTimeout(() => {
            //this.map.mapResize();
        });
    }

    get mapWidth() {
        if (isEmpty(this.style)) {
            return 'calc(100% - 400px)';
        }
        return `calc(100% - ${this.style.width})`;
    }

    onTypesFilterChanged(filteredTypes: string[]) {
        this.filteredTypes = filteredTypes;
        this.setFilteredFlightWatchData();
    }
    openAircraftPopup(tailNumber: string){
        this.mapOnly.openAircraftPopUpByTailNumber(tailNumber);
    }
    async updateButtonOnDrawerResize(){
        if(!this.drawer.opened) return;
          this.drawer.toggle();
        await this.delay(50);
        this.drawer.toggle();
    }
    delay(time) {
        return new Promise(resolve => setTimeout(resolve, time));
    }
}
