import {
    ChangeDetectionStrategy,
    Component,
    ElementRef,
    EventEmitter,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    Output,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { isEqual, keys } from 'lodash';
import * as mapboxgl from 'mapbox-gl';

import { SharedService } from '../../../layouts/shared-service';
import { AirportFboGeofenceClustersService } from '../../../services/airportfbogeofenceclusters.service';

import { isCommercialAircraft } from '../../../../utils/aircraft';
import {
    Aircraftwatch,
    FlightWatch,
    FlightWatchDictionary,
} from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { MapboxglBase } from 'src/app/services/mapbox/mapboxBase';
import { FlightWatchMapService } from './flight-watch-map-services/flight-watch-map.service';
import { AircraftFlightWatchService } from './flight-watch-map-services/aircraft-flight-watch.service';
import { FboFlightWatchService } from './flight-watch-map-services/fbo-flight-watch.service';
import { AircraftImageData, AIRCRAFT_IMAGES } from './aircraft-images';
import { AircraftPopupContainerComponent } from '../aircraft-popup-container/aircraft-popup-container.component';
import { AcukwikAirport } from 'src/app/models/AcukwikAirport';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { convertDMSToDEG } from 'src/utils/coordinates';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-map',
    styleUrls: ['./flight-watch-map.component.scss'],
    templateUrl: './flight-watch-map.component.html',
})
export class FlightWatchMapComponent
    extends MapboxglBase
    implements OnInit, OnChanges, OnDestroy
{
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: FlightWatchDictionary;
    @Input() aircraftData: Aircraftwatch;
    @Input() isStable: boolean;
    @Output() markerClicked = new EventEmitter<FlightWatch>();
    @Output() airportClick = new EventEmitter<AcukwikAirport>();
    @Output() setIcaoList = new EventEmitter<AcukwikAirport[]>();

    @ViewChild('aircraftPopupContainer')
    aircraftPopupContainer: AircraftPopupContainerComponent;
    @ViewChild('aircraftPopupContainer', { read: ElementRef, static: false })
    aircraftPopupContainerRef!: ElementRef;

    public acukwikairports: AcukwikAirport[];
    public icao: any;
    nearbyMiles: number = 30;
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
        private airportFboGeoFenceClustersService: AirportFboGeofenceClustersService,
        private sharedService: SharedService,
        private flightWatchMapService: FlightWatchMapService,
        private fboFlightWatchService: FboFlightWatchService,
        private aircraftFlightWatchService: AircraftFlightWatchService,
        private acukwikairportsService: AcukwikairportsService
    ) {
        super();
    }
    ngOnInit(): void {
        const refreshMapFlight = () => {
            this.updateFlightOnMap();
            this.loadICAOIconOnMap();
        }

        this.buildMap(this.center, this.mapContainer, this.mapStyle)
            .addNavigationControls()
            .onZoomAsync(refreshMapFlight)
            .onDragendAsync(refreshMapFlight)
            .onRotateAsync(refreshMapFlight)
            .onResizeAsync(refreshMapFlight)
            .onStyleDataAsync(this.mapStyleLoaded())
            .onZoomEndAsync((e) => this.updateICAOIconOnMap())
            .onZoomStartAsync((e) => this.geolocationZoomAction(e))
            .onLoad(async () => {
                await this.loadMapIcons();
                this.loadICAOIconOnMap();
                this.loadFlightOnMap();
                this.getFbosAndLoad();
            });
    }
    geolocationZoomAction(e){
        if (e.geolocateSource) {
            var airport = this.getAirportsWithinMapBounds(this.getBounds());
            this.airportClick.emit(airport[0]);
            this.updateFlightOnMap();
        }
    }
    ngAfterViewInit() {
        this.fboId = this.sharedService.currentUser.fboId;
        this.groupId = this.sharedService.currentUser.groupId;
        this.icao = this.sharedService.currentUser.icao;
        this.aircraftPopupContainer.getCustomersList(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId
        );
    }
    async loadICAOIconOnMap() {
        this.acukwikairports = await this.acukwikairportsService.getNearByAcukwikAirportsByICAO(this.icao,this.nearbyMiles).toPromise();
        this.setIcaoList.emit(this.acukwikairports);
        
        var markers: any[] = this.getAirportsWithinMapBounds(this.getBounds()).map((data) => {
            this.aircraftFlightWatchService.getAirportFeatureJsonData(data)
            return {
                geometry: {
                    coordinates: [convertDMSToDEG(data.longitude), convertDMSToDEG(data.latitude)],
                    type: 'Point',
                },
                properties: {
                    id: data.oid,
                    'icon-image': 'airport-icon',
                    'size': 0.5,
                },
                type: 'Feature'
            };
        });
        this.map.addSource(this.airportSourceId, this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers));

        this.map.addLayer(this.aircraftFlightWatchService.getAirportLayerJsonData(this.airportLayerId, this.airportSourceId));
        
        this.addHoverPointerActions(this.airportLayerId);
        this.onClick(this.airportLayerId, (e) => this.clickActionOnAirportICon(e) );
    }
    updateICAOIconOnMap() {        
        var markers: any[] = this.getAirportsWithinMapBounds(this.getBounds()).map((data) => {
            this.aircraftFlightWatchService.getAirportFeatureJsonData(data)
            return {
                geometry: {
                    coordinates: [convertDMSToDEG(data.longitude), convertDMSToDEG(data.latitude)],
                    type: 'Point',
                },
                properties: {
                    id: data.oid,
                    'icon-image': 'airport-icon',
                    'size': 0.5,
                },
                type: 'Feature'
            };
        });

        const data: any = {
            type: 'FeatureCollection',
            features: markers,
        };
        const source = this.getSource(this.airportSourceId);

        source.setData(data);
        this.addHoverPointerActions(this.airportLayerId);
        this.onClick(this.airportLayerId, (e) => this.clickActionOnAirportICon(e) );
    }
    clickActionOnAirportICon(e: any): void{
        let id = e.features[0].properties.id;
        var clickedAirport = this.acukwikairports.filter((airport) => {
            return airport.oid == id;
        });
        this.airportClick.emit(clickedAirport[0]);
    }
    async loadMapIcons(): Promise<unknown> {
        var promisesArray = []
        promisesArray.push(this.loadICAOIcon());
        promisesArray.concat(this.loadAircraftIcon());

        return Promise.all(promisesArray);
    }
    async loadICAOIcon(): Promise<unknown>{
        let imageName = `airport-icon`;
        let imageUrl = `/assets/img/swim-airport.png`;
        return this.loadPNGImageAsync(imageUrl, imageName);
    }
    async loadAircraftIcon(): Promise<unknown> {
        let aircraftIconPromises = []
        return Promise.all(
            AIRCRAFT_IMAGES.map(
                async (image: AircraftImageData) => {
                    let imageName = `aircraft_image_${image.id}`;
                    let img1 = this.loadSVGImageAsync(
                        image.size,
                        image.size,
                        image.url,
                        imageName
                    );

                    if (image.id == 'client') {
                        await Promise.all([img1]);
                        return;
                    }

                    imageName = `aircraft_image_${image.id}_reversed`;
                    let img2 = this.loadSVGImageAsync(
                        image.size,
                        image.size,
                        image.reverseUrl,
                        imageName
                    );

                    imageName = `aircraft_image_${image.id}_release`;
                    let img3 = this.loadSVGImageAsync(
                        image.size,
                        image.size,
                        image.blueUrl,
                        imageName
                    );

                    imageName = `aircraft_image_${image.id}_reversed_release`;
                    let img4 = this.loadSVGImageAsync(
                        image.size,
                        image.size,
                        image.blueReverseUrl,
                        imageName
                    );

                    imageName = `aircraft_image_${image.id}_fuelerlinx`;
                    let img5 = this.loadSVGImageAsync(
                        image.size,
                        image.size,
                        image.fuelerlinxUrl,
                        imageName
                    );

                    imageName = `aircraft_image_${image.id}_reversed_fuelerlinx`;

                    let img6 = this.loadSVGImageAsync(
                        image.size,
                        image.size,
                        image.fuelerlinxReverseUrl,
                        imageName
                    );
                    aircraftIconPromises.concat([img1, img2, img3, img4, img5, img6]);
                }
            )
        );
    }
    ngOnChanges(changes: SimpleChanges): void {
        const currentData = changes.data?.currentValue;
        const oldData = changes.data?.previousValue;
        if (currentData && !isEqual(currentData, oldData)) {
            this.updateFlightOnMap();
        } else this.setPopUpContainerData(changes);
    }
    ngOnDestroy(): void {
        this.mapRemove();
    }
    setPopUpContainerData(changes) {
        if (!changes.aircraftData?.currentValue) return;
        let aircraft = changes.aircraftData.currentValue;

        this.popupData = aircraft;
        this.isAircraftDataLoading = false;
    }
    loadFlightOnMap() {
        var newflightsInMapBounds = this.getFlightsWithinMapBounds(
            this.getBounds()
        );
        const markers = this.getFlightSourcerFeatureMarkers(
            newflightsInMapBounds
        );
        this.addSource(
            this.flightSourceId,
            this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers)
        );
        this.addLayer(
            this.aircraftFlightWatchService.getFlightLayerJsonData(
                this.flightLayerId,
                this.flightSourceId
            )
        );
        this.applyMouseFunctions(this.flightLayerId);
    }
    updateFlightOnMap() {
        if (!this.map) return;

        var newflightsInMapBounds = this.getFlightsWithinMapBounds(
            this.getBounds()
        );

        const source = this.getSource(this.flightSourceId);

        const data: any = {
            type: 'FeatureCollection',
            features: this.getFlightSourcerFeatureMarkers(
                newflightsInMapBounds
            ),
        };

        source.setData(data);
        if (this.currentPopup.isPopUpOpen) {
            this.closeAllPopUps();
            this.setDefaultPopUpOpen(newflightsInMapBounds);
            this.currentPopup.isPopUpOpen = true;
        }
        this.applyMouseFunctions(this.flightLayerId);
    }
    setDefaultPopUpOpen(flightsIdsOnMap: string[]): void {
        let selectedFlight = flightsIdsOnMap.find(
            (key) => this.data[key].oid == this.currentPopup.popupId
        );

        if (!selectedFlight) return;

        this.currentPopup.coordinates = [
            this.data[selectedFlight].longitude,
            this.data[selectedFlight].latitude,
        ];
        this.openPopupRenderComponent(
            this.currentPopup.coordinates,
            this.aircraftPopupContainerRef,
            this.currentPopup
        );
    }
    getFlightsWithinMapBounds(bound: mapboxgl.LngLatBounds): any {
        return keys(this.data).filter((id) => {
            const flightWatch = this.data[Number(id)];
            const flightWatchPosition: mapboxgl.LngLatLike = {
                lat: flightWatch.latitude,
                lng: flightWatch.longitude,
            };
            return (
                bound.contains(flightWatchPosition) &&
                (this.isCommercialVisible ||
                    !isCommercialAircraft(
                        flightWatch.aircraftTypeCode,
                        flightWatch.atcFlightNumber
                    ))
            );
        });
    }
    getAirportsWithinMapBounds(bound: mapboxgl.LngLatBounds): any {
        return this.acukwikairports.filter((airport) => {
            const airportPosition: mapboxgl.LngLatLike = {
                lat: convertDMSToDEG(airport.latitude),
                lng: convertDMSToDEG(airport.longitude),
            };
            return bound.contains(airportPosition);
        });
    }
    getFlightSourcerFeatureMarkers(flights: any): any {
        return flights.map((key) => {
            const row = this.data[key];
            const id = this.flightWatchMapService.buildAircraftId(row.oid);
            return this.aircraftFlightWatchService.getFlightFeatureJsonData(
                row,
                id,
                this.selectedAircraft
            );
        });
    }
    applyMouseFunctions(id: string): void {
        this.onClick(id, (e) => this.clickHandler(e, this));
        this.addHoverPointerActions(this.flightLayerId);
    }
    async clickHandler(
        e: mapboxgl.MapMouseEvent & {
            features?: mapboxgl.MapboxGeoJSONFeature[];
        } & mapboxgl.EventData,
        self: FlightWatchMapComponent
    ) {
        const id = e.features[0].properties.id;
        self.selectedAircraft = id;
        self.currentPopup.isPopUpOpen = true;
        self.currentPopup.coordinates = [
            this.data[id].longitude,
            this.data[id].latitude,
        ];
        self.currentPopup.popupId = id;
        self.isAircraftDataLoading = true;
        self.markerClicked.emit(this.data[id]);
        self.openPopupRenderComponent(
            self.currentPopup.coordinates,
            self.aircraftPopupContainerRef,
            self.currentPopup
        );
    }
    getFbosAndLoad() {
        if (this.clusters) return;
        this.airportFboGeoFenceClustersService
            .getClustersByIcao(this.sharedService.currentUser.icao)
            .subscribe((response: any) => {
                this.clusters = [];
                if (!response) return;
                this.clusters.push(...response);
                this.loadFbosOnMap();
            });
    }

    loadFbosOnMap() {
        if (!this.clusters) return;

        const markers = [];
        this.clusters.forEach((cluster) => {
            markers.push(
                this.fboFlightWatchService.getFbosFeatureJsonData(cluster)
            );
        });
        this.addSource(
            this.fboSourceId,
            this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers)
        );
        this.addLayer(
            this.fboFlightWatchService.getFbosLayer(
                this.fboLayerId,
                this.fboSourceId
            )
        );
        this.createPopUpOnMouseEnterFromDescription(this.fboLayerId);
    }

    mapStyleLoaded() {
        this.styleLoaded = true;
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

        const visibility = this.getLayoutProperty(layers[0], 'visibility');

        // Toggle layer visibility by changing the layout object's visibility property.
        if (visibility === 'visible' || visibility === undefined) {
            layers.forEach((layer) => {
                this.setLayoutProperty(layer, 'visibility', 'none');
            });
        } else {
            layers.forEach((layer) => {
                this.setLayoutProperty(layer, 'visibility', 'visible');
            });
        }
        if (type == 'icao')
            this.isShowAirportCodesEnabled = !this.isShowAirportCodesEnabled;
        else if (type == 'taxiway')
            this.isShowTaxiwaysEnabled = !this.isShowTaxiwaysEnabled;
    }

    toggleCommercial(event: MouseEvent) {
        this.isCommercialVisible = !this.isCommercialVisible;
        this.updateFlightOnMap();
    }
    updateAircraft(event: Aircraftwatch): void {
        this.data[this.selectedAircraft].isInNetwork = true;
        this.data[this.selectedAircraft].company = event.company;
        this.data[this.selectedAircraft].aircraftMakeModel =
            event.aircraftMakeModel;
        this.markerClicked.emit(this.data[this.selectedAircraft]);
        this.updateFlightOnMap();
    }
    goToAirport(icao: string){
        let airport = this.acukwikairports.find( x => x.icao == icao);
        let flyToCenter = {
            lat: convertDMSToDEG(airport.latitude),
            lng: convertDMSToDEG(airport.longitude),
        };
        this.flyTo(flyToCenter);
    }
    openAircraftPopUpByTailNumber(tailNumber: string): void {
        let selectedFlight = keys(this.data).find(
            (key) => this.data[key].tailNumber == tailNumber
        );

        if (!selectedFlight) return;

        this.currentPopup.coordinates = [
            this.data[selectedFlight].longitude,
            this.data[selectedFlight].latitude,
        ];
        this.openPopupRenderComponent(
            this.currentPopup.coordinates,
            this.aircraftPopupContainerRef,
            this.currentPopup
        );
    }
}