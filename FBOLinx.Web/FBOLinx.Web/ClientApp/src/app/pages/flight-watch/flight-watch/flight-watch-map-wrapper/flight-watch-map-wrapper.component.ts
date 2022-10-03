import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { SwimFilter } from 'src/app/models/filter';
import {
    Aircraftwatch,
    FlightWatch,
    FlightWatchDictionary,
} from 'src/app/models/flight-watch';
import { FlightWatchMapComponent } from '../../flight-watch-map/flight-watch-map.component';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    selector: 'app-flight-watch-map-wrapper',
    templateUrl: './flight-watch-map-wrapper.component.html',
    styleUrls: ['./flight-watch-map-wrapper.component.scss'],
})
export class FlightWatchMapWrapperComponent implements OnInit {
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: FlightWatchDictionary;
    @Input() aircraftData: Aircraftwatch;
    @Input() isStable: boolean;
    @Input() icao: string;
    @Input() icaoList: string[];


    @Output() markerClicked = new EventEmitter<FlightWatch>();
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

    constructor() {}

    ngOnInit() {}

    toggleCommercial(event: MouseEvent) {
        this.isCommercialVisible = !this.isCommercialVisible;
        this.map.updateFlightOnMap();
    }
    toggleLayer(type: LayerType) {
        this.toggleLayer(type);
        if (type == 'icao')
            this.isShowAirportCodesEnabled = !this.isShowAirportCodesEnabled;
        else if (type == 'taxiway')
            this.isShowTaxiwaysEnabled = !this.isShowTaxiwaysEnabled;
    }
    resizeMap(isopen: boolean){
        this.map.resizeMap(isopen);
    }
}
