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
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { SwimFilter } from 'src/app/models/filter';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import {
    FlightWatchDictionary,
    FlightWatchModelResponse,
} from '../../../models/flight-watch';
import { FlightWatchMapService } from '../flight-watch-map/flight-watch-map-services/flight-watch-map.service';
import { FlightWatchMapWrapperComponent } from './flight-watch-map-wrapper/flight-watch-map-wrapper.component';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';
import { isCommercialAircraft } from 'src/utils/aircraft';
import { FlightWatchSettingTableComponent } from '../flight-watch-settings/flight-watch-setting-table/flight-watch-setting-table.component';

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
    @ViewChild('flightwatchSettings') public flightwatchSettings: FlightWatchSettingTableComponent;

    pageTitle = 'Flight Watch';
    breadcrumb: any[] = BREADCRUMBS;

    isStable = true;
    loading = false;
    center: LngLatLike = null;
    flightWatchDataSource: MatTableDataSource<FlightWatchModelResponse>;

    flightWatchData: FlightWatchModelResponse[];
    filteredFlightWatchData: FlightWatchDictionary;
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
        this.sharedService.valueChanged$.subscribe((value: {event: string, data: any}) => {
            if(!value.data) return;
            if(value.event === SharedEvents.flightWatchDataEvent){
                if(value.data){
                    this.flightWatchData = value.data?.filter((row: FlightWatchModelResponse) => {
                        return (row.arrivalICAO == row.focusedAirportICAO) ||
                        (row.departureICAO == row.focusedAirportICAO &&
                            row.status != null);
                    });
                    this.isStable = true;
                }else{
                    this.flightWatchData = [];
                    this.isStable = false;
                }
                this.loading = false;
            }
            if (value.event == SharedEvents.flyToOnMapEvent) {
                this.center = this.flightWatchMapService.getMapCenterByCoordinates(value.data.latitude,value.data.longitude);
            }
        });
    }
    ngAfterContentChecked() {
        this.cdref.detectChanges();
    }

    async ngOnInit() {
        if(this.center == null)
            this.center = await this.flightWatchMapService.getMapCenter(this.selectedICAO);
    }
    ngOnDestroy() {
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

    onTextFilterChanged(filter: string): void {
        this.currentFilters.filterText = filter;
        this.applyFiltersToData();
    }
    applyFiltersToData(): void {
        this.flightWatchData = this.filterData(
            this.currentFilters.filterText?.toLowerCase(),
            this.flightWatchData
        );
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
        console.log("🚀 ~ file: flight-watch.component.ts:211 ~ FlightWatchComponent ~ openAircraftPopup ~ tailNumber:", tailNumber)
        this.mapWrapper.map.openAircraftPopUpByTailNumber(tailNumber);
    }
    closedAircraftPopup(tailNumber: string) {
        console.log("🚀 ~ file: flight-watch.component.ts:215 ~ FlightWatchComponent ~ closedAircraftPopup ~ tailNumber:", tailNumber)
        this.mapWrapper.map.closeAircraftPopUpByTailNumber(tailNumber);
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
    onAircraftClick(flightWatch: FlightWatchModelResponse) {
        this.flightwatchSettings.expandRow(flightWatch.tailNumber);
    }
    onPopUpClosed(flightWatch: FlightWatchModelResponse) {
        this.flightwatchSettings.collapseRow(flightWatch.tailNumber);
    }
}
