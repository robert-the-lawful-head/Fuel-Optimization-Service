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
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';
import { Subscription } from 'rxjs';
import { AirportWatchService } from 'src/app/services/airportwatch.service';
import { FlightLegStatus } from 'src/app/enums/flight-watch.enum';
import { FlightWatchMapSharedService } from '../services/flight-watch-map-shared.service';
import * as SharedEvents from 'src/app/constants/sharedEvents';

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
    @Output() popUpClosed = new EventEmitter<FlightWatchModelResponse>();
    @Output() updatePopUpData = new EventEmitter<FlightWatchModelResponse>();

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

    public isMapDataLoading: boolean = false;

    public startTime: number = Date.now();
    private animationFrameIds: number[] = [];

    private previousFlightData: Record<string,FlightWatchModelResponse> = {};

    private backwardLogs: Record<string, FlightWatchModelResponse> = {};

    private popupUpdatesTracking: Record<string, number[]> = {};

    private aicraftDataFeatures: GeoJSON.Feature<GeoJSON.Point,GeoJSON.GeoJsonProperties>[] = [];
    private aicraftDataSource: mapboxgl.GeoJSONSource = null;

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

    constructor(
        private airportFboGeoFenceClustersService: AirportFboGeofenceClustersService,
        private sharedService: SharedService,
        private flightWatchMapService: FlightWatchMapService,
        private fboFlightWatchService: FboFlightWatchService,
        private aircraftFlightWatchService: AircraftFlightWatchService,
        private acukwikairportsService: AcukwikairportsService,
        private flightWatchHelper: FlightWatchHelper,
        private airportWatchService: AirportWatchService,
        private flightWatchMapSharedService: FlightWatchMapSharedService
    ) {
        super();
        this.fboId = this.sharedService.currentUser.fboId;
        this.groupId = this.sharedService.currentUser.groupId;
        this.icao = (this.icao == null) ?
            this.sharedService.getCurrentUserPropertyValue(localStorageAccessConstant.icao)
            :  this.icao;
        this.sharedService.emitChange(SharedEvents.fetchFlighWatchDataEvent);
    }
    ngOnInit(): void {
        if(this.center == null) return;
        this.sharedService.emitChange(SharedEvents.fetchFlighWatchDataEvent);
        this.loadMap();
    }

    ngAfterViewInit() {
        this.aircraftPopupContainer.getCustomersList(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId
        );
    }
    ngOnDestroy(): void {
        this.mapRemove();
    }
    ngOnChanges(changes: SimpleChanges): void {
        if(this.center && !this.map && !this.isMapDataLoading){
            this.loadMap();
            return;
        }

        if(!this.map) return;

        if(changes.center){
            this.flyTo(this.center);
            this.updateAicraftsOnMapBounds(changes.data.currentValue);
        }

        if (changes.data && this.styleLoaded) {
            this.startTime = Date.now();
            this.popupUpdatesTracking = {};
            this.previousFlightData = {};
            if(changes.data.previousValue){
                for (let key in changes.data.previousValue) {
                    this.previousFlightData[key] = changes.data.previousValue[key];
                }
            }
            else{
                for (let key in changes.data.currentValue) {
                    this.previousFlightData[key] = changes.data.currentValue[key];
                }
            }
            this.updateAicraftsOnMapBounds(changes.data.currentValue);
        }

        if(changes.selectedPopUp)
            this.setPopUpContainerData(changes.selectedPopUp.currentValue);
    }
    private loadMap(): void {
        this.buildMap(this.center, this.mapContainer, this.mapStyle)
        .addNavigationControls()
        .onStyleData(async () => {
            this.styleLoaded = true;
        })
        .onLoad(async () => {
            console.log("start redering map data");
            this.isMapDataLoading = true;
            this.resizeMap();
            await this.loadMapIcons();
            await this.loadMapDataAsync();
            this.isMapDataLoading = false;
            this.sharedService.emitChange(SharedEvents.fetchFlighWatchDataEvent);
        })
        .onSourcedata(async () => {
            let flightslayer = this.map.getLayer(this.mapMarkers.flights.layerId);
            let airportlayer = this.map.getLayer(this.mapMarkers.airports.layerId);
            let fbolayer = this.map.getLayer(this.mapMarkers.fbos.layerId);
            if(flightslayer && airportlayer && fbolayer){
                this.map.moveLayer(this.mapMarkers.fbos.layerId,this.mapMarkers.airports.layerId);
                this.map.moveLayer(this.mapMarkers.flights.layerId);
            }
        })
        .onZoomEnd(async () => {
            this.updateAicraftsOnMapBounds(this.data);
        })
        .onDragend(async () => {
            this.updateAicraftsOnMapBounds(this.data);
        });
    }
    private updateAicraftsOnMapBounds(flights: Record<string,FlightWatchModelResponse> ): void {
        this.setMapMarkersData(keys(flights));
        this.checkForPopupOpen();
        this.updateFlightOnMap(this.mapMarkers.flights);
    }
    async loadMapDataAsync(): Promise<unknown> {
        var promisesArray = []
        promisesArray.push(this.getFncAsAsync(this.loadFlightOnMap()));
        promisesArray.push(this.getFncAsAsync(this.loadICAOIconOnMap(this.icao)));
        promisesArray.push(this.getFncAsAsync(this.getFbosAndLoad()));
        promisesArray.concat();

        return Promise.all(promisesArray);
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
    private checkForPopupOpen(): void {
        this.selectedAircraft?.forEach(selectedFlightId => {
            if(!this.openedPopUps[selectedFlightId].isOpen && this.openedPopUps[selectedFlightId].popupInstance == null) return;
            if(this.mapMarkers.flights.data.filter(x => x == this.openedPopUps[selectedFlightId].popupId).length > 0) return;
            this.resetCurrentPopUpState(selectedFlightId);
        });
    }
    private resetCurrentPopUpState(selectedFlightId: string): void {
        this.selectedAircraft = this.selectedAircraft.filter(e => e != selectedFlightId);
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
    setPopUpData(selectedPopUp: Aircraftwatch) {
        this.popupData = Object.assign({}, selectedPopUp);
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
        this.aicraftDataSource = this.getSource(marker.sourceId);
        this.applyMouseFunctions(marker.layerId);
    }
    updateFlightOnMap(marker: MapMarkerInfo) {
        this.aicraftDataFeatures = this.getFlightSourcerFeatureMarkers(marker.data);

        this.cancelExistingAnimationFames();
        this.animateAircrafts();
    }
    readonly animate = (self: any) => {
        let elapsedTime = Date.now() - this.startTime;
        let progress = elapsedTime / environment.flightWatch.apiCallInterval;
        let popUpCoordinates: number[] = null;
        if (progress < 1) {
            this.aicraftDataFeatures.forEach((pointSource) => {
                var coordinates = pointSource.properties['origin-coordinates'];
                var targetCoordinates = pointSource.properties['destination-coordinates'];

                if(coordinates[0] == targetCoordinates[0] && coordinates[1] == targetCoordinates[1]) return;

                let lng = coordinates[0] + (targetCoordinates[0] - coordinates[0]) * progress;
                let lat = coordinates[1] + (targetCoordinates[1] - coordinates[1]) * progress;
                let currentCoordinates = [lng, lat];

                let liveBearing = turf.bearingToAzimuth(turf.bearing(
                    turf.point(currentCoordinates),
                    turf.point(targetCoordinates)
                    ));

                var previousCoordinates = [self.previousFlightData[pointSource.properties.id].previousLongitude, self.previousFlightData[pointSource.properties.id].previousLatitude];
                var previoustargetCoordinates = [self.previousFlightData[pointSource.properties.id].longitude, self.previousFlightData[pointSource.properties.id].latitude]

                let previousiveBearing = turf.bearingToAzimuth(turf.bearing(
                        turf.point(previousCoordinates),
                        turf.point(previoustargetCoordinates)
                        ));

                pointSource = self.updateIconImage(pointSource,popUpCoordinates,currentCoordinates);

                if(liveBearing == 0)return;

                let isBackwards = self.IsBackwardsBearing(previousiveBearing,liveBearing);

                if(isBackwards && [FlightLegStatus.EnRoute].includes(pointSource.properties.status)){
                    self.data[pointSource.properties.id].liveBearing = liveBearing;
                    self.data[pointSource.properties.id].currentCoordinates = currentCoordinates;
                    self.data[pointSource.properties.id].targetCoordinates = targetCoordinates;
                    self.data[pointSource.properties.id].backEndBearing = pointSource.properties.bearing;
                    self.data[pointSource.properties.id].liveCaulculatedBearing = liveBearing;
                    self.data[pointSource.properties.id].previousCorrectModel = self.previousFlightData[pointSource.properties.id];

                    self.data[pointSource.properties.id].previousCorrectModel.currentCoordinates = previousCoordinates;
                    self.data[pointSource.properties.id].previousCorrectModel.targetCoordinates = previoustargetCoordinates;
                    self.data[pointSource.properties.id].previousCorrectModel.liveBearing = previousiveBearing;

                    self.backwardLogs[pointSource.properties.id] = self.data[pointSource.properties.id];
                    return;
                }

                pointSource.geometry.coordinates = currentCoordinates;

                pointSource.properties.bearing = liveBearing;

            });

            self.aicraftDataSource.setData(
                {
                    type: 'FeatureCollection',
                    features: this.aicraftDataFeatures,
                }
            );

            for(let prop in this.popupUpdatesTracking) {
                self.refreshPopUp(this.popupUpdatesTracking[prop],prop);
            }

            let callbackFrameId = requestAnimationFrame(() => self.animate(self));
            self.animationFrameIds.push(callbackFrameId);
        }else{
            self.aicraftDataFeatures = null;
            self.sharedService.emitChange(SharedEvents.fetchFlighWatchDataEvent);
            self.cancelExistingAnimationFames();
        }
    }
    private animateAircrafts(): void {
        let frameid = requestAnimationFrame(() => this.animate(this));
        this.animationFrameIds.push(frameid);
    }
    private updateIconImage(pointSource: any, popUpCoordinates: number[], currentCoordinates: number[]): any {
        //need to update the icon image change on animation
        //working with some lag, need to seach for better solution
        if(this.selectedAircraft?.includes(pointSource.properties.id)){
            popUpCoordinates = currentCoordinates;
            const reverseIcon = this.aircraftFlightWatchService.getAricraftIcon(true,this.data[pointSource.properties.id]);
            pointSource.properties['default-icon-image'] = reverseIcon;
            this.popupUpdatesTracking[pointSource.properties.id] = popUpCoordinates;
        }else{
            const defaultIcon = this.aircraftFlightWatchService.getAricraftIcon(false,this.data[pointSource.properties.id]);
            pointSource.properties['default-icon-image'] = defaultIcon;
        }
        return pointSource;
    }
    private IsBackwardsBearing(bearing: number,liveBearing: number): boolean {
        let start = turf.bearingToAzimuth( bearing+160);
        let end = turf.bearingToAzimuth(bearing+200);
        return this.isBearingInRange(liveBearing,start,end);
    }
    private isBearingInRange(bearing: number, rangeStart: number, rangeEnd: number): boolean {
        if (rangeStart < rangeEnd) {
            return bearing >= rangeStart && bearing <= rangeEnd;
        } else {
            // Handling the case where the range spans across 360 degrees
            return bearing >= rangeStart || bearing <= rangeEnd;
        }
    }
    private refreshPopUp(popUpCoordinates: number[],selectedFlightId: string): void {
            if (!this.openedPopUps[selectedFlightId].isOpen && this.openedPopUps[selectedFlightId].popupId == null) return;

            if (!popUpCoordinates == null){
                this.resetCurrentPopUpState(selectedFlightId);
            } else {
                this.updateOpenedPopUpCoordinates(popUpCoordinates, selectedFlightId);
            }
    }
    private cancelExistingAnimationFames(): void {
        // for(let prop in this.backwardLogs) {
        //     this.airportWatchService.logBackwards(this.backwardLogs[prop]).subscribe((response: any) => {
        //     });
        // }
        this.backwardLogs = {};
        for (const id of this.animationFrameIds) {
            cancelAnimationFrame(id);
        }
        this.animationFrameIds = [];
    }
    updateOpenedPopUpCoordinates(coordinates: any, selectedFlightId: string): void {
        this.openedPopUps[selectedFlightId].coordinates = coordinates;
        let LngLat: mapboxgl.LngLatLike = {lng: coordinates[0], lat: coordinates[1]};

        this.openedPopUps[selectedFlightId].popupInstance.setLngLat(LngLat);
    }
    setMapMarkersData(flights: string[]): void{
        flights =  flights.filter((key) => {
            if(this.previousFlightData[key] == null) return false;
            return this.map.getBounds().contains([this.previousFlightData[key].longitude, this.previousFlightData[key].latitude]) || this.map.getBounds().contains([this.data[key].longitude, this.data[key].latitude])  }) || [];

        let activeFuelRelease = flights.filter((key) => { return this.data[key].isActiveFuelRelease }) || [];

        let fuelerLinxClient = flights.filter((key) => { return this.data[key].isFuelerLinxClient && !this.data[key].isActiveFuelRelease}) || [];

        let inNetwork = flights.filter((key) => { return this.data[key].isInNetwork && !this.data[key].isActiveFuelRelease && !this.data[key].isFuelerLinxClient}) || [];

        let outOfNetwork = flights.filter((key) => { return !this.data[key].isInNetwork && !this.data[key].isActiveFuelRelease && !this.data[key].isFuelerLinxClient }) || [];

        this.mapMarkers.flights.data = [].concat(outOfNetwork,inNetwork,fuelerLinxClient,activeFuelRelease);
        this.mapMarkers.flightsReversed.data = [].concat(outOfNetwork,inNetwork,fuelerLinxClient,activeFuelRelease);

    }
    getFlightSourcerFeatureMarkers(flights: string[]): GeoJSON.Feature<GeoJSON.Point,GeoJSON.GeoJsonProperties>[] {
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

        if (self.selectedAircraft.includes(id)) {
            return;
        }
        self.markerClicked.emit(this.data[id]);

        self.createPopUp(self, id);

    }
    private async createPopUp(self: FlightWatchMapComponent, id: string): Promise<void>{
        this.flightWatchMapSharedService.getAndUpdateAircraftWithHistorical(this.fboId, this.icao, this.data[id]);
        this.updatePopUpData.emit(this.data[id]);

        self.selectedAircraft = [id];
        self.closeAllPopUps();

        self.openedPopUps[id] = {...this.popUpPropsNewInstance}
        self.openedPopUps[id].popupId = id;
        self.openedPopUps[id].coordinates = [
            self.data[id].longitude,
            self.data[id].latitude,
        ];

        self.openedPopUps[id].popupInstance = self.openPopupRenderComponent(
            self.openedPopUps[id].coordinates,
            self.aircraftPopupContainerRef
        );
        self.openedPopUps[id].popupInstance.on('close', function(event) {
            self.selectedAircraft = self.selectedAircraft.filter(e => e != id);
            self.openedPopUps[id].isOpen = false;
            self.openedPopUps[id].popupId = null;
            self.popUpClosed.emit(self.data[id]);
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
        let selectedFlight = keys(this.data).find(
            (key) => this.data[key].tailNumber == event.tailNumber
        );

        this.data[selectedFlight].isInNetwork = true;
        this.data[selectedFlight].flightDepartment = event.flightDepartment;
        this.data[selectedFlight].aircraftTypeCode = event.aircraftTypeCode;

        this.markerClicked.emit(this.data[selectedFlight]);
    }
    goToAirport(icao: string){
        if(!this.acukwikairports){
            console.log('acukwikairports not loaded');
            return;
        }
        let airport = this.acukwikairports.find( x => x.icao == icao);
        this.flyToCoordinates(airport.latitudeInDegrees,airport.longitudeInDegrees);
    }
    flyToCoordinates(latitudeInDegrees: number, longitudeInDegrees: number): void{
        this.flyTo(this.flightWatchMapService.getMapCenterByCoordinates(latitudeInDegrees,longitudeInDegrees));
        this.updateAicraftsOnMapBounds(this.data);
    }
    openAircraftPopUpByTailNumber(tailNumber: string): void {
        let selectedFlight = keys(this.data).find(
            (key) => this.data[key].tailNumber == tailNumber
        );

        if (!selectedFlight) return;


       this.createPopUp(this, selectedFlight);

       this.flyTo(this.flightWatchMapService.getMapCenterByCoordinates(this.data[selectedFlight].latitude,this.data[selectedFlight].longitude));
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
