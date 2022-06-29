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
import { AirportFboGeofenceClustersService } from "../../../services/airportfbogeofenceclusters.service";

import { isCommercialAircraft } from '../../../../utils/aircraft';
import { Aircraftwatch, FlightWatch, FlightWatchDictionary } from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { MapboxglBase } from 'src/app/services/mapbox/mapboxBase';
import { FlightWatchMapService } from './flight-watch-map-services/flight-watch-map.service';
import { AircraftFlightWatchService } from './flight-watch-map-services/aircraft-flight-watch.service';
import { FboFlightWatchService } from './flight-watch-map-services/fbo-flight-watch.service';
import { AircraftImageData, AIRCRAFT_IMAGES } from './aircraft-images';
import { AircraftPopupContainerComponent } from '../aircraft-popup-container/aircraft-popup-container.component';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-map',
    styleUrls: ['./flight-watch-map.component.scss'],
    templateUrl: './flight-watch-map.component.html',
})
export class FlightWatchMapComponent extends MapboxglBase implements OnInit, OnChanges, OnDestroy {
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: FlightWatchDictionary;
    @Input() aircraftData: FlightWatch;    
    @Input() isStable: boolean;
    @Output() markerClicked = new EventEmitter<FlightWatch>();

    @ViewChild('aircraftPopupContainer') aircraftPopupContainer: AircraftPopupContainerComponent;
    @ViewChild('aircraftPopupContainer', {read: ElementRef, static: false}) aircraftPopupContainerRef!: ElementRef;

    //popup
    public popupData: Aircraftwatch = {
        customerInfoBygGroupId : 0,
        tailNumber: '',
        atcFlightNumber: '',
        aircraftTypeCode: '',
        isAircraftOnGround: false,
        company: '',
        aircraftMakeModel: '',
        lastQuote: '',
        currentPricing: ''
    };
    public isAircraftDataLoading: boolean = true;
    public fboId: any;
    public groupId: any;


    // Map Options
    public styleLoaded = false;
    public isCommercialVisible = true;
    public isShowAirportCodesEnabled = true;
    public isShowTaxiwaysEnabled = true;
    public previousMarkerId: number = 0;
    public focusedMarkerId: number = 0;
    public showLayers: boolean = false;
    public mapStyle: string = 'mapbox://styles/fuelerlinx/ckszkcycz080718l7oaqoszvd';
    public mapContainer: string = 'flight-watch-map';


    public clusters: AirportFboGeoFenceCluster[];

    // Mapbox and layers IDs
    public flightSourceId: string = 'flights';
    public flightLayerId: string = 'flightsLayer';
    public fboLayerId: string = 'fbosLayerId';
    public fboSourceId: string = 'fbosSourceId';

    constructor(private airportFboGeoFenceClustersService: AirportFboGeofenceClustersService,
        private sharedService: SharedService,
        private flightWatchMapService : FlightWatchMapService,
        private fboFlightWatchService : FboFlightWatchService,
        private aircraftFlightWatchService : AircraftFlightWatchService) {
        super();
        
    }
    ngAfterViewInit(){
        this.fboId = this.sharedService.currentUser.fboId;
        this.groupId = this.sharedService.currentUser.groupId;
    }
    ngOnInit(): void {        
        const refreshMapFlight = () => this.updateFlightOnMap();

        this.buildMap(this.center, this.mapContainer, this.mapStyle)
        .addNavigationControls()
        .onZoomAsync(refreshMapFlight)
        .onDragendAsync(refreshMapFlight)
        .onRotateAsync(refreshMapFlight)
        .onResizeAsync(refreshMapFlight)
        .onStyleDataAsync(this.mapStyleLoaded());

        this.onLoad( async() => {
            await this.loadAircraftIcon();
            this.loadFlightOnMap();
            this.getFbosAndLoad();
        });
    }
    async loadAircraftIcon(){
        return Promise.all(AIRCRAFT_IMAGES.map( async (image: AircraftImageData,idx:number) => {
            let imageName = `aircraft_image_${image.id}`;
            let img1 = this.loadSVGImageAsync(image.size,image.size,image.url,imageName);
            
            imageName = `aircraft_image_${image.id}_reversed`;
            let img2 = this.loadSVGImageAsync(image.size,image.size,image.reverseUrl,imageName);

            imageName = `aircraft_image_${image.id}_release`;
            let img3 = this.loadSVGImageAsync(image.size,image.size,image.blueUrl,imageName);

            imageName = `aircraft_image_${image.id}_reversed_release`;
            let img4 = this.loadSVGImageAsync(image.size,image.size,image.blueReverseUrl,imageName);

            imageName = `aircraft_image_${image.id}_fuelerlinx`;
            let img5 = this.loadSVGImageAsync(image.size,image.size,image.fuelerlinxUrl,imageName);

            imageName = `aircraft_image_${image.id}_reversed_fuelerlinx`;
            let img6 = this.loadSVGImageAsync(image.size,image.size,image.fuelerlinxReverseUrl,imageName);
            await Promise.all([img1,img2,img3,img4,img5,img6]);
  
        }));
    }
    ngOnChanges(changes: SimpleChanges): void {
        if(changes.aircraftData) this.setPopUpContainerData(changes);

        const currentData = changes.data?.currentValue;
        const oldData = changes.data?.previousValue;
        if (currentData && !isEqual(currentData, oldData)) {
            this.updateFlightOnMap();
        }
    }
    ngOnDestroy(): void {
        this.mapRemove();
    }
    setPopUpContainerData(changes){
        this.popupData = changes.aircraftData.currentValue;
        this.isAircraftDataLoading = false;
    }
    loadFlightOnMap(){
        var newflightsInMapBounds = this.getFlightsWithinMapBounds(this.getBounds());
        const markers = this.getFlightSourcerFeatureMarkers(newflightsInMapBounds);
        this.addSource(this.flightSourceId, this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers));
        this.addLayer(this.aircraftFlightWatchService.getFlightLayerJsonData(this.flightLayerId,this.flightSourceId));
        this.applyMouseFunctions(this.flightLayerId);
    }
    async updateFlightOnMap(){
        if (!this.map) return;

        var newflightsInMapBounds = this.getFlightsWithinMapBounds(this.getBounds());

        const source = this.getSource(this.flightSourceId);

        const deita : any = {
            type: 'FeatureCollection',
            features: this.getFlightSourcerFeatureMarkers(newflightsInMapBounds)
        };

         source.setData(deita);
         this.closeAllPopUps();
         this.applyMouseFunctions(this.flightLayerId);
         this.setDefaultPopUpOpen(newflightsInMapBounds);
    }
    setDefaultPopUpOpen(flightsIdsOnMap:string[]):void{
        let selectedFlight = flightsIdsOnMap.find(key => this.data[key].oid == this.currentPopup.popupId);

        if (!selectedFlight) return;

        this.popupData = {...this.data[selectedFlight]};

        this.currentPopup.coordinates =[this.data[selectedFlight].longitude, this.data[selectedFlight].latitude];
        this.openPopupRenderComponent(this.currentPopup.coordinates,this.aircraftPopupContainerRef,this.currentPopup);
    }
    getFlightsWithinMapBounds(bound: mapboxgl.LngLatBounds): any{
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
    getFlightSourcerFeatureMarkers(flights: any): any{
        return flights.map((key) => {
            const row = this.data[key];
            const id = this.flightWatchMapService.buildAircraftId(row.oid);
            return this.aircraftFlightWatchService.getFlightFeatureJsonData(row,id,this.focusedMarkerId);
        });
    }
    applyMouseFunctions(id: string):void{
        this.onClick(id,(e) => this.clickHandler(e, this));
        this.addHoverPointerActions(this.flightLayerId);
    }
    async clickHandler(
        e: mapboxgl.MapMouseEvent & {
            features?: mapboxgl.MapboxGeoJSONFeature[];
        } & mapboxgl.EventData,
        self: FlightWatchMapComponent
    ) {
        const id = e.features[0].properties.id;
        self.markerClicked.emit(self.data[id]);

        this.closeAllPopUps();
        self.currentPopup.isPopUpOpen = true;
        self.currentPopup.coordinates = [this.data[id].longitude, this.data[id].latitude];
        self.currentPopup.popupId = id;
        this.isAircraftDataLoading = true;

        self.openPopupRenderComponent(self.currentPopup.coordinates,self.aircraftPopupContainerRef,self.currentPopup);

    }

    getFbosAndLoad() {
        if (this.clusters)  return;
        this.airportFboGeoFenceClustersService.getClustersByIcao(this.sharedService.currentUser.icao)
            .subscribe((response: any) => {
                this.clusters = [];
                if (!response)  return;
                this.clusters.push(...response);
                this.loadFbosOnMap();
            });
    }

    loadFbosOnMap(){
        if (!this.clusters) return;

        const markers = [];
        this.clusters.forEach((cluster) => {
            markers.push(this.fboFlightWatchService.getFbosFeatureJsonData(cluster));
        });
        this.addSource(this.fboSourceId,this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers));
        this.addLayer(this.fboFlightWatchService.getFbosLayer(this.fboLayerId,this.fboSourceId));
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
        if (type == "icao")
            this.isShowAirportCodesEnabled = !this.isShowAirportCodesEnabled;
        else if (type == "taxiway")
            this.isShowTaxiwaysEnabled = !this.isShowTaxiwaysEnabled;
    }

    toggleCommercial(event: MouseEvent) {
        this.isCommercialVisible = !this.isCommercialVisible;
        this.updateFlightOnMap();
    }
}
