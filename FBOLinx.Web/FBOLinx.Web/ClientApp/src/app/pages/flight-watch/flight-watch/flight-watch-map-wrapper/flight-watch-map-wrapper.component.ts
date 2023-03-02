import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { Dictionary, keyBy } from 'lodash';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { SwimFilter } from 'src/app/models/filter';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { isCommercialAircraft } from 'src/utils/aircraft';
import { FlightWatchMapService } from '../../flight-watch-map/flight-watch-map-services/flight-watch-map.service';
import { FlightWatchMapComponent } from '../../flight-watch-map/flight-watch-map.component';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    selector: 'app-flight-watch-map-wrapper',
    templateUrl: './flight-watch-map-wrapper.component.html',
    styleUrls: ['./flight-watch-map-wrapper.component.scss'],
})
export class FlightWatchMapWrapperComponent implements OnInit {
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: FlightWatchModelResponse[];
    @Input() selectedPopUp: FlightWatchModelResponse;
    @Input() isStable: boolean;
    @Input() icao: string;
    @Input() icaoList: string[];
    @Input() showFilters: boolean =  true;

    @Output() airportClick = new EventEmitter<AcukwikAirport>();
    @Output() setIcaoList = new EventEmitter<AcukwikAirport[]>();

    @Output() updateDrawerButtonPosition = new EventEmitter<any>();
    @Output() filterChanged = new EventEmitter<SwimFilter>();
    @Output() icaoChanged = new EventEmitter<string>();

    @ViewChild('map') map: FlightWatchMapComponent;

    public showLayers: boolean = false;
    public isCommercialVisible = true;
    public isShowAirportCodesEnabled = true;
    public isShowTaxiwaysEnabled = true;
    public flightWatchDictionary: Dictionary<FlightWatchModelResponse>;

    constructor(private flightWatchMapService: FlightWatchMapService) {}

    ngOnInit() {}

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.data) {
            this.flightWatchDictionary = this.getFilteredData(
                changes.data.currentValue
            );
        }
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
            (flightWatch) => !isCommercialAircraft(flightWatch.aircraftTypeCode)
        );
    }
    toggleCommercial(event: MouseEvent) {
        this.isCommercialVisible = !this.isCommercialVisible;
        this.flightWatchDictionary = this.getFilteredData(this.data);
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
    updateSelectedAircraft($event: FlightWatchModelResponse) {
        this.selectedPopUp = $event;
    }
}
