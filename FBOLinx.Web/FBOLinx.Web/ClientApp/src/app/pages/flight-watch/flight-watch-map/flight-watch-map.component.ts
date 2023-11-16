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
import * as turf from '@turf/turf';
import { environment } from 'src/environments/environment';

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
    implements OnInit, OnChanges, OnDestroy
{
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: Dictionary<FlightWatchModelResponse>;
    @Input() selectedPopUp: FlightWatchModelResponse;
    @Input() isStable: boolean;
    @Input() icao: string;

    @Output() markerClicked = new EventEmitter<FlightWatchModelResponse>();
    @Output() setIcaoList = new EventEmitter<AcukwikAirport[]>();

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
    public selectedAircraft: any = null;

    // Map Options
    public styleLoaded = false;
    public mapStyle: string =
        'mapbox://styles/fuelerlinx/ckszkcycz080718l7oaqoszvd';
    public mapContainer: string = 'flight-watch-map';

    public clusters: AirportFboGeoFenceCluster[];

    public isMapDataLoaded: boolean = false;

    public startTime: number = Date.now();
    private animationFrameIds: number[] = [];

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
                let airportlayer = this.map.getLayer(this.mapMarkers.airports.layerId);
                let fbolayer = this.map.getLayer(this.mapMarkers.fbos.layerId);
                if(flightslayer && airportlayer && fbolayer){
                    this.map.moveLayer(this.mapMarkers.fbos.layerId,this.mapMarkers.airports.layerId);
                    this.map.moveLayer(this.mapMarkers.flights.layerId);
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

        if (changes.data&& this.styleLoaded) {
            this.startTime = Date.now();
            this.setMapMarkersData(keys(changes.data.currentValue));
            this.checkForPopupOpen();
            this.updateFlightOnMap(this.mapMarkers.flights);
        }
        if(changes.selectedPopUp)  this.setPopUpContainerData(changes.selectedPopUp.currentValue);
        if(changes.center)
            this.flyTo(this.center);
    }
    private checkForPopupOpen(): void {
        if(!this.currentPopup.isPopUpOpen && this.currentPopup.popupInstance == null) return;
        if(this.mapMarkers.flights.data.filter(x => x == this.currentPopup.popupId).length > 0) return;
        this.resetCurrentPopUpState()
    }
    private resetCurrentPopUpState(): void {
        this.currentPopup.isPopUpOpen = false;
        this.currentPopup.popupId = null;
        this.currentPopup.coordinates = null;
        this.currentPopup.popupInstance = null;
        this.selectedAircraft = null;
        this.closeAllPopUps();
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
    }
    loadFlightMarkersOnMap(marker: MapMarkerInfo) {
        const markers = this.getFlightSourcerFeatureMarkers(
            marker.data
        );
        this.addSource(
            marker.sourceId,
            this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers)
        );
        this.addLayer(
            this.aircraftFlightWatchService.getFlightLayerJsonData(
                marker.layerId,
                marker.sourceId
            )
        );
        this.applyMouseFunctions(marker.layerId);
    }
    updateFlightOnMap(marker: MapMarkerInfo) {
        if (!this.map || !this.isMapDataLoaded) return;

        const source = this.getSource(marker.sourceId);

        const dataFeatures = this.getFlightSourcerFeatureMarkers(marker.data);

        const data: any = {
            type: 'FeatureCollection',
            features: dataFeatures,
        };

        this.cancelExistingAnimationFames();
        this.animateAircrafts(source,data);
        this.applyMouseFunctions(marker.layerId);
    }
    private animateAircrafts(source: mapboxgl.GeoJSONSource, data: any): void {
        const animate = () => {
            let elapsedTime = Date.now() - this.startTime;
            let progress = elapsedTime / environment.flightWatch.apiCallInterval;
            let popUpCoordinates: number[] = null;

            if (progress < 1) {
                data.features.forEach((pointSource) => {
                    var coordinates = pointSource.properties['origin-coordinates'];
                    var targetCoordinates = pointSource.properties['destination-coordinates'];

                    if(coordinates == targetCoordinates) return;

                    let lng = coordinates[0] + (targetCoordinates[0] - coordinates[0]) * progress;
                    let lat = coordinates[1] + (targetCoordinates[1] - coordinates[1]) * progress;

                    pointSource.geometry.coordinates = [lng, lat];
                    //need to update the icon image change on animation
                    //working with some lag, need to seach for better solution
                    if(this.currentPopup.popupId == pointSource.properties.id){
                        popUpCoordinates = pointSource.geometry.coordinates;
                        const reverseIcon = this.aircraftFlightWatchService.getAricraftIcon(true,this.data[pointSource.properties.id]);
                        pointSource.properties['default-icon-image'] = reverseIcon;
                    }else{
                        const defaultIcon = this.aircraftFlightWatchService.getAricraftIcon(false,this.data[pointSource.properties.id]);
                        pointSource.properties['default-icon-image'] = defaultIcon;
                    }

                    pointSource.properties.bearing = turf.bearing(
                        turf.point(pointSource.geometry.coordinates),
                        turf.point(targetCoordinates)
                        );
                });

                source.setData(data);

                this.refreshPopUp(popUpCoordinates)

                const animationFrameId = requestAnimationFrame(animate);
                this.animationFrameIds.push(animationFrameId);
            }
        };
        const animationFrameId = requestAnimationFrame(animate);

        this.animationFrameIds.push(animationFrameId);
    }
    private refreshPopUp(popUpCoordinates: number[]): void {
        if (!this.currentPopup.isPopUpOpen && this.currentPopup.popupId == null) return;
        if (!popUpCoordinates == null){
            this.resetCurrentPopUpState();
        } else {
            this.updateOpenedPopUpCoordinates(popUpCoordinates);
        }
    }
    private cancelExistingAnimationFames(): void {
        for (const id of this.animationFrameIds) {
            cancelAnimationFrame(id);
        }
        this.animationFrameIds = [];
    }
    updateOpenedPopUpCoordinates(coordinates: any): void {
        this.currentPopup.coordinates = coordinates;
        let LngLat: mapboxgl.LngLatLike = {lng: coordinates[0], lat: coordinates[1]}
        this.currentPopup.popupInstance.setLngLat(LngLat);
    }
    setMapMarkersData(flights: string[]): void{
        let activeFuelRelease = flights.filter((key) => { return this.data[key].isActiveFuelRelease }) || [];

        let fuelerLinxClient = flights.filter((key) => { return this.data[key].isFuelerLinxClient && !this.data[key].isActiveFuelRelease}) || [];

        let inNetwork = flights.filter((key) => { return this.data[key].isInNetwork && !this.data[key].isActiveFuelRelease && !this.data[key].isFuelerLinxClient}) || [];

        let outOfNetwork = flights.filter((key) => { return !this.data[key].isInNetwork && !this.data[key].isActiveFuelRelease && !this.data[key].isFuelerLinxClient }) || [];

        this.mapMarkers.flights.data = [].concat(outOfNetwork,inNetwork,fuelerLinxClient,activeFuelRelease);
        this.mapMarkers.flightsReversed.data = [].concat(outOfNetwork,inNetwork,fuelerLinxClient,activeFuelRelease);

    }
    getFlightSourcerFeatureMarkers(flights: string[]): any[] {
        return flights.map((key) => {
            const row = this.data[key];
            return this.aircraftFlightWatchService.getFlightFeatureJsonData(
                row
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

        if (self.selectedAircraft == id) {
            return;
        }

        self.selectedAircraft = id;
        self.markerClicked.emit(this.data[id]);

        self.currentPopup.isPopUpOpen = true;
        self.currentPopup.coordinates = [
            this.data[id].longitude,
            this.data[id].latitude,
        ];
        self.currentPopup.popupId = id;
        self.currentPopup.popupInstance = self.openPopupRenderComponent(
            self.currentPopup.coordinates,
            self.aircraftPopupContainerRef,
            self.currentPopup
        );
        self.currentPopup.popupInstance.on('close', function(event) {
            self.selectedAircraft =  null;
            self.currentPopup.isPopUpOpen = false;
            self.currentPopup.popupId = null;
        });
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
        this.data[this.selectedAircraft].isInNetwork = true;
        this.data[this.selectedAircraft].flightDepartment = event.flightDepartment;
        this.data[this.selectedAircraft].aircraftTypeCode = event.aircraftTypeCode;
        this.markerClicked.emit(this.data[this.selectedAircraft]);
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
