import {
    ChangeDetectorRef,
    Component,
    Input,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { MatTableDataSource } from '@angular/material/table';
import { ResizeEvent } from 'angular-resizable-element';
import { isEmpty } from 'lodash';
import { LngLatLike } from 'mapbox-gl';
import { Subscription } from 'rxjs';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { SwimFilter } from 'src/app/models/filter';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../constants/sharedEvents';
import {
    FlightWatchDictionary,
    FlightWatchModelResponse,
} from '../../../models/flight-watch';
import { FlightWatchMapService } from '../flight-watch-map/flight-watch-map-services/flight-watch-map.service';
import { FlightWatchMapWrapperComponent } from './flight-watch-map-wrapper/flight-watch-map-wrapper.component';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';
import { isCommercialAircraft } from 'src/utils/aircraft';

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
    @Input() showBreadcrumb: boolean = true;
    @Input() showFilters: boolean = true;
    @Input() showLegend: boolean = true;
    @Input() isLobbyView: boolean = false;

    @ViewChild(FlightWatchMapWrapperComponent)
    private mapWrapper: FlightWatchMapWrapperComponent;
    @ViewChild('mapfilters') public drawer: MatDrawer;

    pageTitle = 'Flight Watch';
    breadcrumb: any[] = BREADCRUMBS;

    isStable = true;
    loading = false;
    center: LngLatLike;
    selectedFlightWatch: FlightWatchModelResponse;
    flightWatchDataSource: MatTableDataSource<FlightWatchModelResponse>;

    AircraftLiveDatasubscription: Subscription;

    flightWatchData: FlightWatchModelResponse[];
    filteredFlightWatchData: FlightWatchDictionary;
    arrivals: FlightWatchModelResponse[];
    departures: FlightWatchModelResponse[];
    arrivalsAllRecords: FlightWatchModelResponse[];
    departuresAllRecords: FlightWatchModelResponse[];
    acukwikairport: AcukwikAirport[];
    airportsICAO: string[];
    selectedICAO: string;

    style: any = {};
    isMapShowing = true;

    currentFilters: SwimFilter = {
        filterText: '',
        dataType: null,
        isCommercialAircraftVisible: true,
        filteredTypes: [],
    };

    constructor(
        private flightWatchMapService: FlightWatchMapService,
        private sharedService: SharedService,
        public dialog: MatDialog,
        private cdref: ChangeDetectorRef
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.selectedICAO = this.sharedService.getCurrentUserPropertyValue(
            localStorageAccessConstant.icao
        );
        this.sharedService.valueChanged$.subscribe((value: {event: string, data: FlightWatchModelResponse[]}) => {
            if(value.event === SharedEvents.flightWatchDataEvent){
                if(value.data){
                    this.setData(value.data);
                    this.isStable = true;
                }else{
                    this.flightWatchData = [];
                    this.isStable = false;
                }
                this.loading = false;
            }
        });
    }
    ngAfterContentChecked() {
        this.cdref.detectChanges();
    }

    async ngOnInit() {
        this.center = await this.flightWatchMapService.getMapCenter(
            this.selectedICAO
        );
    }
    ngOnDestroy() {
    }
    setData(data: FlightWatchModelResponse[]): void {
        this.arrivals = data?.filter((row: FlightWatchModelResponse) => {
            return row.arrivalICAO == row.focusedAirportICAO;
        });
        this.departures = data?.filter((row: FlightWatchModelResponse) => {
            return (
                row.departureICAO == row.focusedAirportICAO &&
                row.status != null
            );
        });

        this.arrivalsAllRecords = this.arrivals;
        this.departuresAllRecords = this.departures;

        this.applyFiltersToData();
    }
    setIcaoList(airportList: AcukwikAirport[]) {
        this.acukwikairport = airportList;
        let icaoList = airportList.map((data) => {
            return data.icao;
        });
        this.airportsICAO = icaoList;
    }
    updateIcao(icao: string) {
        this.mapWrapper.map.goToAirport(icao);
        this.mapWrapper.map.updateICAOIconOnMap(icao);
        this.selectedICAO = icao;
        this.sharedService.emitChange(SharedEvents.icaoChangedEvent);
    }
    onTabClick(event) {
        if (event.tab.textLabel == 'Takeoff/Landing Cycles')
            this.isMapShowing = false;
        else this.isMapShowing = true;
    }

    onAircraftInfoClose() {
        this.selectedFlightWatch = undefined;
    }

    onTextFilterChanged(filter: string): void {
        this.currentFilters.filterText = filter;
        this.applyFiltersToData();
    }
    applyFiltersToData(): void {
        this.arrivals = this.filterData(
            this.currentFilters.filterText?.toLowerCase(),
            this.arrivalsAllRecords
        );
        this.departures = this.filterData(
            this.currentFilters.filterText?.toLowerCase(),
            this.departuresAllRecords
        );

        this.flightWatchData = this.arrivals.concat(this.departures);
    }
    filterData(
        filter: string,
        records: FlightWatchModelResponse[]
    ): FlightWatchModelResponse[] {
        if (this.currentFilters.filteredTypes.length) {
            records = records.filter((fw) =>
                this.currentFilters.filteredTypes.includes(fw.aircraftTypeCode)
            );
        }
        if (!this.currentFilters.isCommercialAircraftVisible) {
            records = records.filter(
                (flightWatch) =>
                    !isCommercialAircraft(flightWatch.atcFlightNumber)
            );
        }

        if (!filter && !filter?.trim()) return records;

        return records.filter(
            (fw) =>
                fw.tailNumber?.toLowerCase().includes(filter) ||
                fw.flightDepartment?.toLowerCase().includes(filter) ||
                fw.departureCity?.toLowerCase().includes(filter) ||
                fw.arrivalCity?.toLowerCase().includes(filter)
        );
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
            this.mapWrapper.map.mapResize();
        });
    }

    get mapWidth() {
        if (isEmpty(this.style)) {
            return 'calc(100% - 400px)';
        }
        return `calc(100% - ${this.style.width})`;
    }

    onTypesFilterChanged(filteredTypes: string[]) {
        this.currentFilters.filteredTypes = filteredTypes;
        this.applyFiltersToData();
    }
    openAircraftPopup(tailNumber: string) {
        this.mapWrapper.map.openAircraftPopUpByTailNumber(tailNumber);
    }
    async updateButtonOnDrawerResize() {
        if (!this.drawer.opened) return;
        await this.drawer.toggle();
        this.toggleSettingsDrawer();
    }
    async toggleSettingsDrawer() {
        await this.drawer.toggle();
        this.mapWrapper.resizeMap(this.drawer.opened);
    }
    isDrawerOpen() {
        return this.drawer.opened;
    }
    filterCommercialAircrafts(isCommercialAircraftVisible: boolean) {
        this.currentFilters.isCommercialAircraftVisible =
            isCommercialAircraftVisible;
        this.applyFiltersToData();
    }
}
