import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { LngLatLike } from 'mapbox-gl';
import { Subscription, timer } from 'rxjs';
import { SharedService } from 'src/app/layouts/shared-service';
import { ApiResponseWraper } from 'src/app/models/apiResponseWraper';
import { SwimFilter } from 'src/app/models/filter';
import { FlightWatch, FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { AirportWatchService } from 'src/app/services/airportwatch.service';
import { FlightWatchService } from 'src/app/services/flightwatch.service';
import { convertDMSToDEG } from 'src/utils/coordinates';
import { FlightWatchMapComponent } from '../flight-watch/flight-watch-map/flight-watch-map.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-lobby-view',
  templateUrl: './lobby-view.component.html',
  styleUrls: ['./lobby-view.component.scss']
})
export class LobbyViewComponent implements OnInit {
    center: LngLatLike;
    @Input() data: FlightWatchModelResponse[];
    @Input() selectedPopUp: FlightWatchModelResponse;
    isStable: boolean = true;
    icao: string;
    @Input() icaoList: string[];

    @ViewChild('mapfilters') public drawer: MatDrawer;
    @ViewChild('map') private map:FlightWatchMapComponent;


    showFilters: boolean = false;
    flightWatchData: FlightWatchModelResponse[];
    airportWatchFetchSubscription: Subscription;
    mapLoadSubscription: Subscription;
    loading: boolean = false;
    filter: string = "";
    arrivals: FlightWatchModelResponse[];
    departures: FlightWatchModelResponse[];
    arrivalsAllRecords: FlightWatchModelResponse[];
    departuresAllRecords: FlightWatchModelResponse[];
    filteredTypes: string[] = [];

    constructor(private sharedService: SharedService,
        private flightWatchService: FlightWatchService,
        private acukwikairportsService: AcukwikairportsService,
        ) {
            this.icao = (this.sharedService.currentUser.icao)?this.sharedService.currentUser.icao:localStorage.getItem('icao');

        }

    async ngOnInit() {
        var selectedAirport = await this.acukwikairportsService.getAcukwikAirportByICAO(this.icao).toPromise();
        if (!this.center) {
            this.center = {
                lat: convertDMSToDEG(selectedAirport.latitude),
                lng: convertDMSToDEG(selectedAirport.longitude),
            };
        }
        this.mapLoadSubscription = timer(0, 15000).subscribe(() =>{
            this.loadAirportWatchData();
        });
    }
    ngOnDestroy() {
        if (this.mapLoadSubscription) {
            this.mapLoadSubscription.unsubscribe();
        }
        if (this.airportWatchFetchSubscription) {
            this.airportWatchFetchSubscription.unsubscribe();
        }
    }

    loadAirportWatchData() {
        if (this.loading) return;
        this.loading = true;

        return this.airportWatchFetchSubscription = this.flightWatchService
        .getAirportLiveData(
            this.sharedService.currentUser.fboId,
            this.icao
        )
        .subscribe((data: ApiResponseWraper<FlightWatchModelResponse[]>) => {
            if (data.success) {
                console.log("🚀 ~ file: lobby-view.component.ts ~ line 73 ~ LobbyViewComponent ~ .subscribe ~ data.success", data)
                this.setData(data.result);
                this.isStable = true;
            } else {
                this.flightWatchData = [];
                this.isStable = false;
            }
            this.loading = false;
        }, (error: any) => {
            console.log("🚀 ~ file: lobby-view.component.ts ~ line 66 ~ LobbyViewComponent ~ .subscribe ~ error", error)
            this.loading = false;
            this.isStable = false;
        });
    }
    setData(data: FlightWatchModelResponse[]):void{
        let currentFilter: SwimFilter = { filterText: this.filter, dataType: null };

        this.arrivals = data?.filter((row: FlightWatchModelResponse) => { return row.arrivalICAO == row.focusedAirportICAO  && row.status != null });
        this.departures = data?.filter((row: FlightWatchModelResponse) => { return row.departureICAO  == row.focusedAirportICAO && row.status != null });

        this.arrivalsAllRecords = this.arrivals;
        this.departuresAllRecords = this.departures;

        this.onFilterChanged(currentFilter);
    }
    onFilterChanged(filter: SwimFilter) {
        this.filter = filter.filterText;
        this.arrivals = this.filterData(this.filter?.toLowerCase(), this.arrivalsAllRecords);
        this.departures = this.filterData(this.filter?.toLowerCase(), this.departuresAllRecords);

        this.flightWatchData = this.arrivals.concat(this.departures);
    }
    filterData(filter: string, records: FlightWatchModelResponse[]): FlightWatchModelResponse[]{

        if (this.filteredTypes.length) {
            records = records.filter((fw) =>
                this.filteredTypes.includes(fw.aircraftTypeCode)
            );
        }

        if(!filter && !filter?.trim()) return records;

        return records.filter(
            (fw) =>
            fw.tailNumber?.toLowerCase().includes(filter) ||
            fw.flightDepartment?.toLowerCase().includes(filter) ||
            fw.departureCity?.toLowerCase().includes(filter)||
            fw.arrivalCity?.toLowerCase().includes(filter)
        );
    }
    async updateButtonOnDrawerResize(){
        if(!this.drawer.opened) return;
        await this.drawer.toggle();
        this.toggleSettingsDrawer();
    }
    async toggleSettingsDrawer(){
        await this.drawer.toggle();
        this.map.resizeMap(this.drawer.opened);
    }
    isDrawerOpen(){
       return this.drawer.opened;
    }
}
