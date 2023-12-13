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
import { Dictionary, keys } from 'lodash';
import * as mapboxgl from 'mapbox-gl';

import { SharedService } from '../../../layouts/shared-service';
import { AirportFboGeofenceClustersService } from '../../../services/airportfbogeofenceclusters.service';
import {
    Aircraftwatch,
    FlightWatchModelResponse,
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
import { FlightWatchHelper } from '../FlightWatchHelper.service';
import { MapMarkerInfo, MapMarkers } from 'src/app/models/swim';
import { localStorageAccessConstant } from 'src/app/models/LocalStorageAccessConstant';
import { Subscription } from 'rxjs';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-map',
    styleUrls: ['./flight-watch-map.component.scss'],
    templateUrl: './flight-watch-map.component.html',
})
export class FlightWatchMapComponent
    extends MapboxglBase
    implements OnInit, OnChanges
{
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: Dictionary<FlightWatchModelResponse>;
    @Input() selectedPopUp: FlightWatchModelResponse;
    @Input() isStable: boolean;
    @Input() icao: string;

    @Output() markerClicked = new EventEmitter<FlightWatchModelResponse>();
    @Output() setIcaoList = new EventEmitter<AcukwikAirport[]>();
    @Output() popUpClosed = new EventEmitter<FlightWatchModelResponse>();

    @ViewChild('aircraftPopupContainer')
    aircraftPopupContainer: AircraftPopupContainerComponent;
    @ViewChild('aircraftPopupContainer', { read: ElementRef, static: false })
    aircraftPopupContainerRef!: ElementRef;

    public acukwikairports: AcukwikAirport[];
    nearbyMiles: number = 150;
    //popup
    public popupData: Aircraftwatch;
    public fboId: any;
    public groupId: any;
    public selectedAircraft: string[] = [];

    // Map Options
    public styleLoaded = false;
    public mapStyle: string =
        'mapbox://styles/fuelerlinx/ckszkcycz080718l7oaqoszvd';
    public mapContainer: string = 'flight-watch-map';

    public clusters: AirportFboGeoFenceCluster[];

    public isMapDataLoaded: boolean = false;

    // Mapbox and layers IDs
    public mapMarkers: MapMarkers= {
        flights : {
            sourceId: 'flightSource',
            layerId: 'flightLayer',
            data: null
        },
        flightsReversed : {
            sourceId: 'flightReversedSource',
            layerId: 'flightReversedLayer',
            data: null
        },
        fbos: {
            sourceId: 'fbosSourceId',
            layerId: 'fbosLayerId',
            data: null
        },
        airports: {
            sourceId: 'airportSourceId',
            layerId: 'airportLayerId',
            data: null
        }
    }

    subscription: Subscription;

    constructor(
        private airportFboGeoFenceClustersService: AirportFboGeofenceClustersService,
        private sharedService: SharedService,
        private flightWatchMapService: FlightWatchMapService,
        private fboFlightWatchService: FboFlightWatchService,
        private aircraftFlightWatchService: AircraftFlightWatchService,
        private acukwikairportsService: AcukwikairportsService,
        private flightWatchHelper: FlightWatchHelper
    ) {
        super();
        this.fboId = this.sharedService.currentUser.fboId;
        this.groupId = this.sharedService.currentUser.groupId;
        this.icao = (this.icao == null) ?
            this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao)
            :  this.icao;
    }
    ngOnInit(): void {
        this.buildMap(this.center, this.mapContainer, this.mapStyle)
            .addNavigationControls()
            .onStyleData(async () => {
                this.styleLoaded = true;
            })
            .onLoad(async () => {
                this.resizeMap();
                await this.loadMapIcons();
                this.loadFlightOnMap();
                this.loadICAOIconOnMap(this.icao);
                this.getFbosAndLoad();
                this.isMapDataLoaded = true;
            })
            .onSourcedata(async () => {
                let flightslayer = this.map.getLayer(this.mapMarkers.flights.layerId);
                let flightsReversedlayer = this.map.getLayer(this.mapMarkers.flightsReversed.layerId);
                let airportlayer = this.map.getLayer(this.mapMarkers.airports.layerId);
                let fbolayer = this.map.getLayer(this.mapMarkers.fbos.layerId);
                if(flightslayer && airportlayer && fbolayer && flightsReversedlayer){
                    this.map.moveLayer(this.mapMarkers.fbos.layerId,this.mapMarkers.airports.layerId);
                    this.map.moveLayer(this.mapMarkers.flights.layerId);
                    this.map.moveLayer(this.mapMarkers.flightsReversed.layerId);
                }

            });
    }

    ngAfterViewInit() {
        this.aircraftPopupContainer.getCustomersList(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId
        );
    }
    ngOnDestroy(): void {
        this.mapRemove();
        if(this.subscription) this.subscription.unsubscribe();
    }
    async loadICAOIconOnMap(currentIcao: string): Promise<void>{
        this.acukwikairports = await this.acukwikairportsService.getNearByAcukwikAirportsByICAO(this.icao,this.nearbyMiles).toPromise();
        this.setIcaoList.emit(this.acukwikairports);

        var markers: any[] = this.acukwikairports.map((data) => {
            return this.aircraftFlightWatchService.getAirportFeatureJsonData(data, currentIcao);
        });

        this.addSource(this.mapMarkers.airports.sourceId, this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers));

        this.addLayer(this.aircraftFlightWatchService.getAirportLayerJsonData(this.mapMarkers.airports.layerId, this.mapMarkers.airports.sourceId));
    }
    async updateICAOIconOnMap(currentIcao: string): Promise<void>{
        this.acukwikairports = await this.acukwikairportsService.getNearByAcukwikAirportsByICAO(this.icao,this.nearbyMiles).toPromise();
        this.setIcaoList.emit(this.acukwikairports);

        var markers: any[] = this.acukwikairports.map((data) => {
            return this.aircraftFlightWatchService.getAirportFeatureJsonData(data, currentIcao);
        });
        const data: any = {
            type: 'FeatureCollection',
            features: markers,
        };
        const source = this.getSource(this.mapMarkers.airports.sourceId);

        if(!source) return;

        source.setData(data);
    }
    async loadMapIcons(): Promise<unknown> {
        var promisesArray = []
        promisesArray.push(this.loadICAOIcon());
        promisesArray.push(this.loadActiveICAOIcon());
        promisesArray.concat(this.loadAircraftIcon());

        return Promise.all(promisesArray);
    }
    async loadICAOIcon(): Promise<unknown>{
        let imageName = `airport-icon`;
        let imageUrl = `/assets/img/swim-airport.png`;
        return this.loadPNGImageAsync(imageUrl, imageName);
    }
    async loadActiveICAOIcon(): Promise<unknown>{
        let imageName = `airport-icon-active`;
        let imageUrl = `/assets/img/swim-airport-active.png`;
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

                    let imageNameReversed = `${imageName}_reversed`;
                    let img2 = this.loadSVGImageAsync(
                        image.size,
                        image.size,
                        image.reverseUrl,
                        imageNameReversed
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
        if(!this.map) return;

        if (changes.data) {
            this.setMapMarkersData(keys(changes.data.currentValue));
            this.updateFlightOnMap(this.mapMarkers.flights);
            this.updateFlightOnMap(this.mapMarkers.flightsReversed,true);
        }
        if(changes.selectedPopUp)  this.setPopUpContainerData(changes.selectedPopUp.currentValue);
        if(changes.center)
            this.flyTo(this.center);
    }
    setPopUpContainerData(selectedPopUp: FlightWatchModelResponse) {
        var makemodelstr = this.flightWatchHelper.getSlashSeparationDisplayString(selectedPopUp.make,selectedPopUp.model);
        makemodelstr =  this.flightWatchHelper.getEmptyorDefaultStringText(makemodelstr);
        let obj : Aircraftwatch= {
            customerInfoByGroupId: selectedPopUp.customerInfoByGroupId,
            tailNumber: selectedPopUp.tailNumber,
            atcFlightNumber: selectedPopUp.atcFlightNumber,
            aircraftTypeCode: selectedPopUp.aircraftTypeCode,
            isAircraftOnGround: selectedPopUp.isAircraftOnGround,
            flightDepartment: selectedPopUp.flightDepartment,
            aircraftMakeModel: makemodelstr,
            lastQuote: selectedPopUp.lastQuote,
            currentPricing: selectedPopUp.currentPricing,
            aircraftICAO: selectedPopUp.icaoAircraftCode,
            faaRegisteredOwner: selectedPopUp.faaRegisteredOwner,
            origin: selectedPopUp.departureICAO,
            destination: selectedPopUp.arrivalICAO
        };

        this.popupData = Object.assign({}, obj);
    }
    loadFlightOnMap() {
        this.setMapMarkersData(keys(this.data));

        this.loadFlightMarkersOnMap(this.mapMarkers.flights);
        this.loadFlightMarkersOnMap(this.mapMarkers.flightsReversed,true);
    }
    loadFlightMarkersOnMap(marker: MapMarkerInfo, isReversedLayers = false) {
        const markers = this.getFlightSourcerFeatureMarkers(
            marker.data,isReversedLayers
        );
        this.addSource(
            marker.sourceId,
            this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers)
        );
        this.addLayer(
            this.aircraftFlightWatchService.getFlightLayerJsonData(
                marker.layerId,
                marker.sourceId,
                isReversedLayers
            )
        );
        if(!isReversedLayers)
            this.applyMouseFunctions(marker.layerId);
    }
    updateFlightOnMap(marker: MapMarkerInfo, isReversedLayers = false) {
        if (!this.map || !this.isMapDataLoaded) return;

        const source = this.getSource(marker.sourceId);

        const dataFeatures = this.getFlightSourcerFeatureMarkers(marker.data, isReversedLayers);

        const data: any = {
            type: 'FeatureCollection',
            features: dataFeatures,
        };

        source.setData(data);

        if (Object.keys(this.openedPopUps).length > 0) {
            let selectedFlight = marker.data.find(
                (key) => this.data[key].tailNumber in this.openedPopUps
            );
            if (!selectedFlight){
                this.closeAllPopUps();
            }else{
                this.setDefaultPopUpOpen(selectedFlight);
            }
        }
        if(!isReversedLayers)
            this.applyMouseFunctions(marker.layerId);
    }
    setDefaultPopUpOpen(selectedFlightId: string): void {
        if (!selectedFlightId){
            this.closeAllPopUps();
            return;
        }

        this.openedPopUps[selectedFlightId].coordinates = [
            this.data[selectedFlightId].longitude,
            this.data[selectedFlightId].latitude,
        ];
        this.openedPopUps[selectedFlightId].popupInstance.setLngLat(this.openedPopUps[selectedFlightId].coordinates);
    }
    setMapMarkersData(flights: string[]): void{
        let activeFuelRelease = flights.filter((key) => { return this.data[key].isActiveFuelRelease }) || [];

        let fuelerLinxClient = flights.filter((key) => { return this.data[key].isFuelerLinxClient && !this.data[key].isActiveFuelRelease}) || [];

        let inNetwork = flights.filter((key) => { return this.data[key].isInNetwork && !this.data[key].isActiveFuelRelease && !this.data[key].isFuelerLinxClient}) || [];

        let outOfNetwork = flights.filter((key) => { return !this.data[key].isInNetwork && !this.data[key].isActiveFuelRelease && !this.data[key].isFuelerLinxClient }) || [];

        this.mapMarkers.flights.data = [].concat(outOfNetwork,inNetwork,fuelerLinxClient,activeFuelRelease);
        this.mapMarkers.flightsReversed.data = [].concat(outOfNetwork,inNetwork,fuelerLinxClient,activeFuelRelease);

    }
    getFlightSourcerFeatureMarkers(flights: string[], isReversedLayout = false): any[] {
        return flights.map((key) => {
            const row = this.data[key];
            return this.aircraftFlightWatchService.getFlightFeatureJsonData(
                row,
                isReversedLayout
            );
        });
    }
    applyMouseFunctions(layerid: string): void {
        this.onClick(layerid, (e) => this.clickHandler(e, this));
        this.addHoverPointerActions(layerid);
    }
    async clickHandler(
        e: mapboxgl.MapMouseEvent & {
            features?: mapboxgl.MapboxGeoJSONFeature[];
        } & mapboxgl.EventData,
        self: FlightWatchMapComponent
    ) {
        const id = e.features[0].properties.id;

        if (self.selectedAircraft.includes(id)) {
            return;
        }

        self.selectedAircraft.push(id);

        this.createPopUp(self, id);

        self.map.setFilter(self.mapMarkers.flightsReversed.layerId, ['==', 'id', id])
    }
    private async createPopUp(self: FlightWatchMapComponent, id: string): Promise<void>{
        self.markerClicked.emit(this.data[id]);

        self.openedPopUps[id] = {...this.popUpPropsNewInstance}

        self.openedPopUps[id].coordinates = [
            self.data[id].longitude,
            self.data[id].latitude,
        ];

        self.map.setFilter(self.mapMarkers.flightsReversed.layerId, ['==', 'id', id])

        await new Promise(f => setTimeout(f, 500));

        let html =  self.aircraftPopupContainerRef.nativeElement.innerHTML;
        self.openedPopUps[id].popupInstance = self.openPopupRenderComponent(
            self.openedPopUps[id].coordinates,
            html
        );
        self.openedPopUps[id].popupInstance.on('close', function(event) {
            self.selectedAircraft = self.selectedAircraft.filter(e => e != id);
            self.popUpClosed.emit(self.data[id]);
            self.openedPopUps[id].isOpen = false;
            try {
                self.map.setFilter(self.mapMarkers.flightsReversed.layerId, ['==', 'id', ''])
            } catch (err) {
                console.log("attempt to filter on an undefined map");
            }
        });
        self.map.setFilter(self.mapMarkers.flightsReversed.layerId, ['==', 'id', id]);
    }
    getFbosAndLoad() {
        if (this.clusters) return;
        this.airportFboGeoFenceClustersService
            .getClustersByIcao(this.icao)
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
            this.mapMarkers.fbos.sourceId,
            this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers)
        );
        this.addLayer(
            this.fboFlightWatchService.getFbosLayer(
                this.mapMarkers.fbos.layerId,
                this.mapMarkers.fbos.sourceId
            )
        );
        this.createPopUpOnMouseEnterFromDescription(this.mapMarkers.fbos.layerId);
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

    updateAircraft(event: Aircraftwatch): void {
        let selectedFlight = keys(this.data).find(
            (key) => this.data[key].tailNumber == event.tailNumber
        );

        this.data[selectedFlight].isInNetwork = true;
        this.data[selectedFlight].flightDepartment = event.flightDepartment;
        this.data[selectedFlight].aircraftTypeCode = event.aircraftTypeCode;
        this.markerClicked.emit(this.data[selectedFlight]);
    }
    goToAirport(icao: string){
        let airport = this.acukwikairports.find( x => x.icao == icao);
        this.flyToCoordinates(airport.latitudeInDegrees,airport.longitudeInDegrees);
    }
    flyToCoordinates(latitudeInDegrees: number, longitudeInDegrees: number): void{
        this.flyTo(this.flightWatchMapService.getMapCenterByCoordinates(latitudeInDegrees,longitudeInDegrees));
    }
    openAircraftPopUpByTailNumber(tailNumber: string): void {
        let selectedFlight = keys(this.data).find(
            (key) => this.data[key].tailNumber == tailNumber
        );

        if (!selectedFlight) return;

        this.selectedAircraft.push(selectedFlight);

       this.createPopUp(this, selectedFlight);
    }
    closeAircraftPopUpByTailNumber(tailNumber: string): void {
        let selectedFlight = keys(this.data).find(
            (key) => this.data[key].tailNumber == tailNumber
        );

        if (!selectedFlight) return;

        this.openedPopUps[selectedFlight].popupInstance.remove();

        delete this.openedPopUps[selectedFlight];
    }
    goToCurrentIcao(){
        this.goToAirport(this.icao);
    }
    toggleLayer(type: LayerType) {
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
    }
    resizeMap(isopen: boolean = false){
        var mapCanvas = document.getElementsByClassName('mapboxgl-canvas')[0] as HTMLElement;
        var mapDiv = document.getElementById(this.mapContainer);
        var mapWrapper = document.getElementById('map-wrapper');

        if(isopen){
            mapDiv.style.width = mapWrapper.offsetWidth + 'px';
            mapCanvas.style.width = '100%';

        }else{
            mapDiv.style.width = '100%';
            mapCanvas.style.width = '100%';
        }

        this.map.resize();
    }
}
