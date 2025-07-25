import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { Dictionary } from 'lodash';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { Aircraftwatch, FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { isCommercialAircraft } from 'src/utils/aircraft';
import { FlightWatchMapService } from '../flight-watch-map/flight-watch-map-services/flight-watch-map.service';
import { FlightWatchMapComponent } from '../flight-watch-map/flight-watch-map.component';
import { FlightWatchMapSharedService } from '../services/flight-watch-map-shared.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { SharedService } from 'src/app/layouts/shared-service';
import * as SharedEvents from 'src/app/constants/sharedEvents';
import { Subscription } from 'rxjs';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    selector: 'app-flight-watch-map-wrapper',
    templateUrl: './flight-watch-map-wrapper.component.html',
    styleUrls: ['./flight-watch-map-wrapper.component.scss'],
})
export class FlightWatchMapWrapperComponent implements OnInit {
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: FlightWatchModelResponse[];
    @Input() isStable: boolean;
    @Input() icao: string;
    @Input() icaoList: string[];
    @Input() showFilters: boolean =  true;

    @Output() setIcaoList = new EventEmitter<AcukwikAirport[]>();

    @Output() updateDrawerButtonPosition = new EventEmitter<any>();
    @Output() textFilterChanged = new EventEmitter<string>();
    @Output() icaoChanged = new EventEmitter<string>();
    @Output() showCommercialAircraftFilter = new EventEmitter<boolean>();
    @Output() aicraftClick = new EventEmitter<FlightWatchModelResponse>();
    @Output() popUpClosed = new EventEmitter<FlightWatchModelResponse>();


    @ViewChild('map') map: FlightWatchMapComponent;

    public showLayers: boolean = false;
    public isCommercialVisible = true;
    public isShowAirportCodesEnabled = true;
    public isShowTaxiwaysEnabled = true;
    public flightWatchDictionary: Dictionary<FlightWatchModelResponse>;
    public lastUpdatedData: FlightWatchModelResponse[];
    public selectedPopUp: FlightWatchModelResponse;

    public chartName = 'map-wrapper';

    aicraftDetailsSubscription: Subscription;
    aicraftCompanyAssignSubscription: Subscription;

    constructor(private flightWatchMapService: FlightWatchMapService,
        flightWatchMapSharedService: FlightWatchMapSharedService,
        private ngxLoader: NgxUiLoaderService,
        private sharedService: SharedService,) {
        this.aicraftDetailsSubscription = flightWatchMapSharedService.aicraftDetails$.subscribe( (data: FlightWatchModelResponse) => {
            this.updatePopUpData(data);
        });
        this.aicraftCompanyAssignSubscription = flightWatchMapSharedService.aicraftCompanyAssign$.subscribe( (data: Aircraftwatch) => {
            this.updateAircraftCompanyAssignData(data);
        });
        this.sharedService.valueChange(SharedEvents.locationChangedEvent);
    }

    ngOnInit() {
        this.ngxLoader.startLoader(this.chartName);
    }
    ngOnDestroy() {
        this.aicraftDetailsSubscription?.unsubscribe();
        this.aicraftCompanyAssignSubscription?.unsubscribe();
    }
    ngOnChanges(changes: SimpleChanges): void {
        if (!changes.data && !changes.data?.currentValue)
            return;

        if(!changes.data.firstChange){
            this.processFlightWatchData(changes.data.currentValue,changes.data.previousValue);
        }
        this.lastUpdatedData = changes.data.currentValue;

        this.ngxLoader.stopLoader(this.chartName);
    }
    private processFlightWatchData(currentFlights: FlightWatchModelResponse [], previousFlights: FlightWatchModelResponse [] = []):void {
        for (let currentData of currentFlights) {
            let previousData = previousFlights?.find((obj) => obj.tailNumber == currentData.tailNumber);

            currentData.previousAircraftPositionDateTimeUtc = previousData?.aircraftPositionDateTimeUtc;

            let isDateTimeSyncronized = (currentData.aircraftPositionDateTimeUtc > currentData.previousAircraftPositionDateTimeUtc);

            currentData.previousLongitude = isDateTimeSyncronized ? previousData?.longitude ?? currentData.longitude : currentData.longitude;
            currentData.previousLatitude =  isDateTimeSyncronized ? previousData?.latitude ?? currentData.latitude : currentData.latitude;

        }
    }
    getLatestData($event: any = null){
        this.flightWatchDictionary = this.getFilteredData(this.lastUpdatedData);
    }
    getFilteredData(
        data: FlightWatchModelResponse[]
    ): Dictionary<FlightWatchModelResponse> {
        let filtered = this.filterComercialFlights(data);
        return this.flightWatchMapService.getDictionaryByTailNumberAsKey(filtered);
    }
    filterComercialFlights(
        flightWatch: FlightWatchModelResponse[]
    ): FlightWatchModelResponse[] {
        if (this.isCommercialVisible) return flightWatch;

        return flightWatch.filter(
            (flightWatch) => !isCommercialAircraft(flightWatch.atcFlightNumber)
        );
    }
    toggleCommercial(event: MouseEvent) {
        this.isCommercialVisible = !this.isCommercialVisible;
        this.flightWatchDictionary = this.getFilteredData(this.data);
        this.showCommercialAircraftFilter.emit(this.isCommercialVisible);
    }
    toggleLayer(type: LayerType) {
        this.map.toggleLayer(type);
        if (type == 'icao')
            this.isShowAirportCodesEnabled = !this.isShowAirportCodesEnabled;
        else if (type == 'taxiway')
            this.isShowTaxiwaysEnabled = !this.isShowTaxiwaysEnabled;
    }
    resizeMap(isopen: boolean) {
        this.map.resizeMap(isopen);
    }
    updatePopUpData($event: FlightWatchModelResponse) {
        this.selectedPopUp = $event;
    }
    updateAircraftCompanyAssignData(aicarftWatch: Aircraftwatch){
        this.map.updateAircraft(aicarftWatch);
        this.map.setPopUpData(aicarftWatch);
        this.map.openAircraftPopUpByTailNumber(aicarftWatch.tailNumber);
    }
}
