import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { MatTableDataSource } from '@angular/material/table';
import { ResizeEvent } from 'angular-resizable-element';
import { isEmpty, keyBy } from 'lodash';
import { LngLatLike } from 'mapbox-gl';
import { BehaviorSubject, Observable, Subscription, timer } from 'rxjs';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { Swim } from 'src/app/models/swim';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { SwimService } from 'src/app/services/swim.service';
import { convertDMSToDEG } from 'src/utils/coordinates';

import { SharedService } from '../../../layouts/shared-service';
import { Aircraftwatch, FlightWatch, FlightWatchDictionary } from '../../../models/flight-watch';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FlightWatchMapComponent } from '../flight-watch-map/flight-watch-map.component';

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

    constructor(
        private airportWatchService: AirportWatchService,
        private swimService: SwimService,
        private sharedService: SharedService,
        private acukwikairportsService: AcukwikairportsService,
        public dialog: MatDialog)
    {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.mapLoadSubscription = timer(0, 15000).subscribe(() =>
            this.loadAirportWatchData()
        );
    }
    ngAfterViewInit(){
        this.selectedICAO = this.sharedService.currentUser.icao;
        this.getDrawerData(this.selectedICAO);
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
    getDrawerData(icao:string):void{
        this.swimService.getArrivals(icao).subscribe(
            arrivals => {
                this.swimArrivals = arrivals.result;
                this.swimArrivalsAllRecords = arrivals.result;
            }
          );
          this.swimService.getDepartures(icao).subscribe(
            departures => {
                this.swimDepartures = departures.result;
                this.swimDeparturesAllRecords = departures.result;
            }
          );
    }
    openDrawer(airportClicked: AcukwikAirport){
        this.getDrawerData(airportClicked.icao);
        this.drawer.toggle();
    }
    setIcaoList(airportList: AcukwikAirport[]){
        this.acukwikairport = airportList;
        let icaoList =  airportList.map((data) => {
            return data.icao;
        })
        this.airportsICAO = icaoList;
        
    }
    updateIcao(icao:string){
        this.map.goToAirport(icao);
        this.selectedICAO =  icao;
        this.getDrawerData(icao);
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

    onFilterChanged(filter: string) {
        this.filter = filter;
        this.setFilteredFlightWatchData();
        this.filterArrivals(this.filter);
    }
    filterArrivals(filter){
        const loweredFilter = filter.toLowerCase();
        this.swimArrivals = this.swimArrivalsAllRecords.filter(
            (fw) =>
                fw.tailNumber.toLowerCase().includes(loweredFilter) ||
                fw.flightDepartment.toLowerCase().includes(loweredFilter)
        );
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
            this.map.mapResize();
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
}
