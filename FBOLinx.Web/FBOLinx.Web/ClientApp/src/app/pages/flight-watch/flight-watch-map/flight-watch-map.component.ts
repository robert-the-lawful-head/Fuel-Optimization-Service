import {
    ChangeDetectionStrategy,
    Component,
    ElementRef,
    EventEmitter,
    Input,
    OnDestroy,
    OnInit,
    Output,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { isEqual, keys } from 'lodash';

import { SharedService } from '../../../layouts/shared-service';

import { isCommercialAircraft } from '../../../../utils/aircraft';
import {
    Aircraftwatch,
    FlightWatch,
    FlightWatchDictionary,
} from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { FlightWatchMapService } from './flight-watch-map-services/flight-watch-map.service';
import { AircraftFlightWatchService } from './flight-watch-map-services/aircraft-flight-watch.service';
import { FboFlightWatchService } from './flight-watch-map-services/fbo-flight-watch.service';
import { AircraftImageData, AIRCRAFT_IMAGES } from './aircraft-images';
import { AircraftPopupContainerComponent } from '../aircraft-popup-container/aircraft-popup-container.component';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { convertDMSToDEG } from 'src/utils/coordinates';
import { FlightWatchMapOnlyComponent } from '../flight-watch-map-only/flight-watch-map-only.component';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-map',
    styleUrls: ['./flight-watch-map.component.scss'],
    templateUrl: './flight-watch-map.component.html',
})
export class FlightWatchMapComponent
    implements OnInit, OnDestroy
{
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: FlightWatchDictionary;
    @Input() aircraftData: Aircraftwatch;
    @Input() isStable: boolean;
    @Input() flightWatchData: FlightWatch[];
    @Input() selectedAircraftData: Aircraftwatch;
    @Input() filteredFlightWatchData: FlightWatchDictionary;
    //@Output() markerClicked = new EventEmitter<FlightWatch>();
    //@Output() airportClick = new EventEmitter<AcukwikAirport>();
    //@Output() setIcaoList = new EventEmitter<AcukwikAirport[]>();

    @ViewChild('aircraftPopupContainer')
    aircraftPopupContainer: AircraftPopupContainerComponent;
    @ViewChild('aircraftPopupContainer', { read: ElementRef, static: false })
    aircraftPopupContainerRef!: ElementRef;
    @ViewChild('map') map: FlightWatchMapOnlyComponent;

    public acukwikairports: AcukwikAirport[];
    public icao: any;
    nearbyMiles: number = 150;
    //popup
    public popupData: Aircraftwatch = {
        customerInfoBygGroupId: 0,
        tailNumber: '',
        atcFlightNumber: '',
        aircraftTypeCode: '',
        isAircraftOnGround: false,
        company: '',
        aircraftMakeModel: '',
        lastQuote: '',
        currentPricing: '',
    };
    public isAircraftDataLoading: boolean = true;
    public fboId: any;
    public groupId: any;
    public selectedAircraft: number = 0;

    // Map Options
    public styleLoaded = false;
    public isCommercialVisible = true;
    public isShowAirportCodesEnabled = true;
    public isShowTaxiwaysEnabled = true;
    public showLayers: boolean = false;
    public mapStyle: string =
        'mapbox://styles/fuelerlinx/ckszkcycz080718l7oaqoszvd';
    public mapContainer: string = 'flight-watch-map';

    public clusters: AirportFboGeoFenceCluster[];

    // Mapbox and layers IDs
    public flightSourceId: string = 'flights';
    public flightLayerId: string = 'flightsLayer';
    public fboLayerId: string = 'fbosLayerId';
    public fboSourceId: string = 'fbosSourceId';
    public airportLayerId: string = 'airportLayerId';
    public airportSourceId: string = 'airportSourceId';

    constructor(
        private sharedService: SharedService,
        private flightWatchMapService: FlightWatchMapService,
        private fboFlightWatchService: FboFlightWatchService,
        private aircraftFlightWatchService: AircraftFlightWatchService,
        private acukwikairportsService: AcukwikairportsService
    ) {
        /*super();*/
    }
    ngOnInit(): void {
    }

    ngAfterViewInit() {
        this.fboId = this.sharedService.currentUser.fboId;
        this.groupId = this.sharedService.currentUser.groupId;
        this.icao = this.sharedService.currentUser.icao;
        //this.aircraftPopupContainer.getCustomersList(
        //    this.sharedService.currentUser.groupId,
        //    this.sharedService.currentUser.fboId
        //);
    }

    ngOnDestroy(): void {
       
    }

    getLayersFromType(type: LayerType) {
        const airwayLayers = ['airways-lines', 'airways-labels'];
        const taxwayLayers = ['taxiways-lines', 'taxiways-labels'];
        const styleLayers = this.map
            .getStyle()
            .layers.filter(
                (layer) =>
                    !layer.id.startsWith('aircraft_') &&
                    !airwayLayers.includes(layer.id)
            )
            .map((layer) => layer.id);
        const icaoLayers = ['airports-names'];

        if (type === 'airway') {
            return airwayLayers;
        }
        if (type === 'taxiway') {
            return taxwayLayers;
        }
        if (type === 'streetview') {
            return styleLayers;
        }
        if (type === 'icao') {
            return icaoLayers;
        }
        return [];
    }

    toggleLayer(type: LayerType, event: MouseEvent) {
        const layers = this.getLayersFromType(type);

        //const visibility = this.getLayoutProperty(layers[0], 'visibility');

        //// Toggle layer visibility by changing the layout object's visibility property.
        //if (visibility === 'visible' || visibility === undefined) {
        //    layers.forEach((layer) => {
        //        this.setLayoutProperty(layer, 'visibility', 'none');
        //    });
        //} else {
        //    layers.forEach((layer) => {
        //        this.setLayoutProperty(layer, 'visibility', 'visible');
        //    });
        //}
        //if (type == 'icao')
        //    this.isShowAirportCodesEnabled = !this.isShowAirportCodesEnabled;
        //else if (type == 'taxiway')
        //    this.isShowTaxiwaysEnabled = !this.isShowTaxiwaysEnabled;
    }

    toggleCommercial(event: MouseEvent) {
        this.isCommercialVisible = !this.isCommercialVisible;
/*        this.updateFlightOnMap();*/
    }

    goToAirport(icao: string){
        let airport = this.acukwikairports.find( x => x.icao == icao);
        let flyToCenter = {
            lat: convertDMSToDEG(airport.latitude),
            lng: convertDMSToDEG(airport.longitude),
        };
/*        this.flyTo(flyToCenter);*/
    }

    goToCurrentIcao(){
        this.goToAirport(this.sharedService.currentUser.icao);
    }
}
