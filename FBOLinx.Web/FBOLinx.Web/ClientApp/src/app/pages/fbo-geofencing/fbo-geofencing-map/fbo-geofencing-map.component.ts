import {
    Component,
    Input,
    Output,
    EventEmitter,
    OnDestroy,
    OnInit,
    Inject,
} from '@angular/core';
import { difference, isEqual, keys } from 'lodash';
import * as mapboxgl from 'mapbox-gl';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RequestDemoModalComponent } from "../../../shared/components/request-demo-modal/request-demo-modal.component";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { MatDialog } from '@angular/material/dialog';
import { FboGeofencingDialogNewClusterComponent } from
    "../fbo-geofencing-dialog-new-cluster/fbo-geofencing-dialog-new-cluster.component";

// Services
import { AcukwikairportsService } from "../../../services/acukwikairports.service";
import { AirportFboGeofenceClustersService } from "../../../services/airportfbogeofenceclusters.service";
import { AirportWatchService } from "../../../services/airportwatch.service";
import { AirportFboGeofenceClusterCoordinatesService } from
    "../../../services/airportfbogeofenceclustercoordinates.service";

// Models
import { FlightWatch } from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { AirportFboGeoFenceClusterCoordinates } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster-coordinates';
import { AirportFboGeoFenceGridViewmodel } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-grid-viewmodel';
import { DeleteConfirmationComponent, DeleteConfirmationData } from
    "../../../shared/components/delete-confirmation/delete-confirmation.component";
import { SaveConfirmationData } from "../../../shared/components/save-confirmation/save-confirmation.component";

@Component({
    selector: 'app-fbo-geofencing-map',
    styleUrls: ['./fbo-geofencing-map.component.scss'],
    templateUrl: './fbo-geofencing-map.component.html',
})
export class FboGeofencingMapComponent implements OnInit, OnDestroy {
    //@Input() center: mapboxgl.LngLatLike;
    //@Input() isStable: boolean;
    @Input() airportFboGeofenceGridItem: AirportFboGeoFenceGridViewmodel;
    @Output() onCloseEditing = new EventEmitter<AirportFboGeoFenceGridViewmodel>();

    // Map Options
    map: mapboxgl.Map;
    zoom = 13;
    keys: string[] = [];

    public acukwikFbos: any[];
    public clusters: AirportFboGeoFenceCluster[];
    public parkingOccurrences: FlightWatch[];
    
    public loaderName: string = 'fbo-geofence-map-loader';
    public isEditing: boolean = false;
    public activeCluster: AirportFboGeoFenceCluster = null;
    public styleLoaded: boolean = false;
    public initialViewRefreshed: boolean = false;
    

    //Map items
    public parkingOccurenceMarkers: mapboxgl.Marker[] = [];
    public fboClusterLayerIds: string[] = [];
    public fboClusterSourceIds: string[] = [];

    constructor(private ngxLoader: NgxUiLoaderService,
        private acukwikAirportsService: AcukwikairportsService,
        private airportFboGeoFenceClustersService: AirportFboGeofenceClustersService,
        private airportWatchService: AirportWatchService,
        private newClusterDialog: MatDialog,
        private deleteConfirmDialog: MatDialog,
        private airportFboGeofenceClusterCoordinatesService: AirportFboGeofenceClusterCoordinatesService) { }

    ngOnInit(): void {

        this.map = new mapboxgl.Map({
            accessToken:
                'pk.eyJ1IjoiZnVlbGVybGlueCIsImEiOiJja3NzODNqcG4wdHVrMm9rdHU3OGRpb2dmIn0.LvSvlGG0ej3PEDJOBpOoMQ',
            center: {
                lat: this.airportFboGeofenceGridItem.latitude,
                lng: this.airportFboGeofenceGridItem.longitude,
            },
            container: 'fbo-geofencing-map',
            //style: 'mapbox://styles/fuelerlinx/ckwb6l72y1c5m14p1nocmcjpc',
            style: 'mapbox://styles/fuelerlinx/ckszkcycz080718l7oaqoszvd',
            zoom: this.zoom,
        });
        const eventHandler = () => this.refreshMap();
        this.map.on('load', () => eventHandler());
        this.map.on('styledata', () => this.mapStyleLoaded());
        this.map.on('click', (e) => this.mapClicked(e));

        this.ngxLoader.startLoader(this.loaderName);
        this.loadAvailableFbos();
        this.loadParkingOccurrences();
        this.loadClusters();
    }

    ngOnDestroy(): void {
        this.map.remove();
    }

    public closeGeofenceEdit(): void {
        this.onCloseEditing.emit(this.airportFboGeofenceGridItem);
    }

    public refreshMap(): void {
        if (!this.map)
            return;

        this.refreshParkingOccurrencesOnMap();
    }

    public mapStyleLoaded(): void {
        this.styleLoaded = true;
        if (!this.initialViewRefreshed) {
            this.initialViewRefreshed = true;
            this.refreshParkingOccurrencesOnMap();
            this.refreshClustersOnMap();
        }
    }

    public mapResize(): void {
        this.map.resize();
    }

    public newClusterClicked(): void {
        const dialogRef = this.newClusterDialog.open(FboGeofencingDialogNewClusterComponent,
            {
                data: this.airportFboGeofenceGridItem,
                width: '450px'
            });

        dialogRef.afterClosed().subscribe((result) => {
            if (!result)
                return;

            var airportFboGeoFence: AirportFboGeoFenceCluster = {
                oid: 0,
                icao: this.airportFboGeofenceGridItem.icao,
                acukwikAirportId: this.airportFboGeofenceGridItem.acukwikAirportId,
                acukwikFboHandlerId: result.acukwikFbo.handlerId,
                centerLatitude: 0,
                centerLongitude: 0,
                fboName: result.acukwikFbo.handlerLongName,
                clusterCoordinatesCollection: []
            };
            this.airportFboGeoFenceClustersService.add(airportFboGeoFence).subscribe((response: AirportFboGeoFenceCluster) => {
                if (!response)
                    return;
                this.clusters.push(response);
                this.activeCluster = response;
                this.isEditing = true;
            });
        });
    }

    public editRowClicked(cluster: AirportFboGeoFenceCluster): void {
        this.isEditing = true;
        this.activeCluster = cluster;
    }

    public stopEditingRowClicked(cluster: AirportFboGeoFenceCluster): void {
        this.isEditing = false;
        this.activeCluster = null;
        this.refreshClustersOnMap();
    }

    public deleteRowClicked(cluster: AirportFboGeoFenceCluster): void {
        const dialogRef = this.deleteConfirmDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: {
                    item: cluster,
                    description: 'geo-fence'
                } as DeleteConfirmationData,
            }
        );

        dialogRef.afterClosed().subscribe((result: DeleteConfirmationData) => {
            if (!result)
                return;
            this.ngxLoader.startLoader(this.loaderName);
            this.airportFboGeoFenceClustersService.remove(cluster).subscribe((response: any) => {
                this.ngxLoader.stopLoader(this.loaderName);
                this.clusters.splice(this.clusters.indexOf(cluster), 1);
                this.refreshClustersOnMap();
            });
        });
    }

    public mapClicked(e: any): void {
        if (this.isEditing && this.activeCluster != null) {
            this.ngxLoader.startLoader(this.loaderName);
            var clusterCoordinate: AirportFboGeoFenceClusterCoordinates = {
                oid: 0,
                clusterId: this.activeCluster.oid,
                latitude: e.lngLat.lat,
                longitude: e.lngLat.lng,
                longitudeLatitudeAsList: null
            };
            this.airportFboGeofenceClusterCoordinatesService.add(clusterCoordinate).subscribe((response:
                AirportFboGeoFenceClusterCoordinates) => {
                this.ngxLoader.stopLoader(this.loaderName);
                this.activeCluster.clusterCoordinatesCollection.push(response);
                this.refreshClustersOnMap();
            });
        }
    }

    public onMouseEnterFbo(cluster: AirportFboGeoFenceCluster): void {
        var layerId = 'fbo-cluster-layer-' + cluster.oid;
        var layer = this.map.getLayer(layerId);
        if (!layer)
            return;
        this.map.setPaintProperty(layerId, 'fill-color', '#FFEC14');
    }

    public onMouseLeaveFbo(cluster: AirportFboGeoFenceCluster): void {
        var layerId = 'fbo-cluster-layer-' + cluster.oid;
        var layer = this.map.getLayer(layerId);
        if (!layer)
            return;
        this.map.setPaintProperty(layerId, 'fill-color', '#0080ff');
    }

    private refreshParkingOccurrencesOnMap(): void {
        if (!this.styleLoaded)
            return;

        if (!this.parkingOccurrences)
            return;

        this.parkingOccurenceMarkers.forEach(marker => {
            marker.remove();
        });

        this.parkingOccurenceMarkers = [];

        this.parkingOccurrences.forEach(occurrence => {
            const marker = new mapboxgl.Marker({
                    draggable: false
            }).setLngLat([occurrence.longitude, occurrence.latitude])
                .addTo(this.map);
        });
    }

    private refreshClustersOnMap(): void {
        if (!this.styleLoaded)
            return;

        if (!this.clusters)
            return;

        this.fboClusterLayerIds.forEach((id) => {
            this.map.removeLayer(id);
        });

        this.fboClusterLayerIds = [];

        this.fboClusterSourceIds.forEach((id) => {
            this.map.removeSource(id);
        });

        this.fboClusterSourceIds = [];

        this.clusters.forEach((cluster: AirportFboGeoFenceCluster) => {
            if (this.activeCluster != null && this.activeCluster != cluster)
                return;

            const clusterPolygonSourceId: string = 'fbo-cluster-source-' + cluster.oid;
            const clusterPolygonLayerId: string = 'fbo-cluster-layer-' + cluster.oid;
            this.map.addSource(clusterPolygonSourceId,
                {
                    data: {
                        geometry: {
                            coordinates: [ cluster.clusterCoordinatesCollection.map(coords => coords.longitudeLatitudeAsList)],
                            //coordinates: cluster.clusterCoordinatesCollection.map(coords => new mapboxgl.LngLat(coords.longitude, coords.latitude)),
                            type: 'Polygon',
                        },
                        properties: {
                            id: cluster.oid,
                        },
                        type: 'Feature',
                    },
                    type: 'geojson',
                });

            this.map.addLayer({
                id: clusterPolygonLayerId,
                layout: {},
                source: clusterPolygonSourceId,
                type: 'fill',
                paint: {
                    'fill-color': '#0080ff',
                    'fill-opacity': 0.5
                }
            });

            this.fboClusterLayerIds.push(clusterPolygonLayerId);
            this.fboClusterSourceIds.push(clusterPolygonSourceId);
        });
    }

    private loadAvailableFbos(): void {
        this.acukwikAirportsService.getAcukwikFboHandlerDetailByIcao(this.airportFboGeofenceGridItem.icao)
            .subscribe((response: any) => {
                this.acukwikFbos = [];
                this.checkLoadingStatus();
                if (!response)
                    return;
                this.acukwikFbos.push(...response);
            });
    }

    private loadClusters(): void {
        this.airportFboGeoFenceClustersService.getClustersByAcukwikAirportId(this.airportFboGeofenceGridItem.acukwikAirportId)
            .subscribe((response: any) => {
                this.clusters = [];
                this.checkLoadingStatus();
                if (!response)
                    return;
                this.clusters.push(...response);
                this.refreshClustersOnMap();
            });
    }

    private loadParkingOccurrences(): void {
        this.airportWatchService.getParkingOccurrencesAtAirport(this.airportFboGeofenceGridItem.icao)
            .subscribe((response: any) => {
                this.parkingOccurrences = [];
                this.checkLoadingStatus();
                if (!response)
                    return;
                this.parkingOccurrences.push(...response);
                this.refreshParkingOccurrencesOnMap();
            });
    }

    private checkLoadingStatus(): void {
        if (!this.parkingOccurrences)
            return;
        if (!this.clusters)
            return;
        if (!this.acukwikFbos)
            return
        this.ngxLoader.stopLoader(this.loaderName);
    }
}
