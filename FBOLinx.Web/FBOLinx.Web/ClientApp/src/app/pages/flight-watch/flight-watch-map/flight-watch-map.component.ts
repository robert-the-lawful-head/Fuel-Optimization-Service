import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    Output,
    SimpleChanges,
} from '@angular/core';
import { isEqual, keys } from 'lodash';
import * as mapboxgl from 'mapbox-gl';

import { SharedService } from '../../../layouts/shared-service';
import { AirportFboGeofenceClustersService } from "../../../services/airportfbogeofenceclusters.service";

import { isCommercialAircraft } from '../../../../utils/aircraft';
import { FlightWatch } from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { MapboxglBase } from 'src/app/services/mapbox/mapboxBase';
import { FlightWatchMapService } from './flight-watch-map-services/flight-watch-map.service';
import { AircraftFlightWatchService } from './flight-watch-map-services/aircraft-flight-watch.service';
import { FboFlightWatchService } from './flight-watch-map-services/fbo-flight-watch.service';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-map',
    styleUrls: ['./flight-watch-map.component.scss'],
    templateUrl: './flight-watch-map.component.html',
})
export class FlightWatchMapComponent extends MapboxglBase implements OnInit, OnChanges, OnDestroy {
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: {
        [oid: string]: FlightWatch;
    };
    @Input() isStable: boolean;
    @Output() markerClicked = new EventEmitter<FlightWatch>();

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

    ngOnInit(): void {
        const refreshMapFlight = ()=> this.updateFlightOnMap();

        this.buildMap(this.center, this.mapContainer, this.mapStyle)
        .addNavigationControls()
        .onZoomAsync(refreshMapFlight)
        .onDragendAsync(refreshMapFlight)
        .onRotateAsync(refreshMapFlight)
        .onResizeAsync(refreshMapFlight)
        .onStyleDataAsync(this.mapStyleLoaded());

        this.onLoad(async () => {
            this.flightWatchMapService.loadAircraftIcons(this.map);
            this.loadFlightOnMap();
            this.getFbosAndLoad();
        });
    }
    ngOnChanges(changes: SimpleChanges): void {
        const currentData = changes.data.currentValue;
        const oldData = changes.data.previousValue;
        if (!isEqual(currentData, oldData)) {
            this.updateFlightOnMap();
        }
    }
    ngOnDestroy(): void {
        this.mapRemove();
    }
    async loadFlightOnMap(){
        if (!this.map) return;

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
         this.applyMouseFunctions(this.flightLayerId);
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
    clickHandler(
        e: mapboxgl.MapMouseEvent & {
            features?: mapboxgl.MapboxGeoJSONFeature[];
        } & mapboxgl.EventData,
        self: FlightWatchMapComponent
    ) {
        const id = e.features[0].properties.id;
        if (self.focusedMarkerId !== Number(id)) {
            self.focusedMarkerId = id;
        }
        if (self.previousMarkerId && self.data[self.previousMarkerId] != null) {
            var previousMarker = self.data[self.previousMarkerId];
            self.setFlightWatchMarkerLayout(previousMarker);
        }

        var focusedMarker = self.data[id];
        self.markerClicked.emit(self.data[id]);

        if (self.focusedMarkerId > 0) {
            self.previousMarkerId = self.focusedMarkerId;
        } else {
            self.previousMarkerId = 0;
        }

        self.setFlightWatchMarkerLayout(focusedMarker);
    }
    private setFlightWatchMarkerLayout(marker){
        this.setLayoutProperty(
            this.flightWatchMapService.buildAircraftId(marker.oid),
            'icon-image',
            `aircraft_image_${this.flightWatchMapService.getDefaultAircraftType(marker.aircraftTypeCode)}${this.focusedMarkerId == marker.oid ? '_reversed' : ''
            }${marker?.fuelOrder != null ? '_release' : (marker?.isFuelerLinxCustomer ? '_fuelerlinx' : '')}`
        );
    }

    async getFbosAndLoad() {
        if (this.clusters)  return;

        this.airportFboGeoFenceClustersService.getClustersByIcao(this.sharedService.currentUser.icao)
            .subscribe((response: any) => {
                this.clusters = [];
                if (!response)  return;
                this.clusters.push(...response);
                this.loadFbosOnMap();
            });
    }

    async loadFbosOnMap(){
        if (!this.clusters) return;

        const markers = [];
        this.clusters.forEach((cluster) => {
            markers.push(this.fboFlightWatchService.getFbosFeatureJsonData(cluster));
        });
        this.addSource(this.fboSourceId,this.flightWatchMapService.getGeojsonFeatureSourceJsonData(markers));
        this.addLayer(this.fboFlightWatchService.getFbosLayer(this.fboLayerId,this.fboSourceId));
        this.createPopUpOnMouseEnter(this.fboLayerId);
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
