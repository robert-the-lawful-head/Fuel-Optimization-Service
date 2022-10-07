import {
    Component,
    Input,
    Output,
    EventEmitter,
    OnDestroy,
    OnInit,
} from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
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
import { MapboxglBase } from 'src/app/services/mapbox/mapboxBase';

@Component({
    selector: 'app-fbo-geofencing-map',
    styleUrls: ['./fbo-geofencing-map.component.scss'],
    templateUrl: './fbo-geofencing-map.component.html',
})
export class FboGeofencingMapComponent extends MapboxglBase implements OnInit, OnDestroy {
    @Input() airportFboGeofenceGridItem: AirportFboGeoFenceGridViewmodel;
    @Output() onCloseEditing = new EventEmitter<AirportFboGeoFenceGridViewmodel>();

    // Map Options
    public styleLoaded : boolean = false;
    public mapStyle : string =
        'mapbox://styles/fuelerlinx/ckszkcycz080718l7oaqoszvd';
    public mapContainer: string = 'fbo-geofencing-map';
    public center: mapboxgl.LngLatLike;
    public zoom = 13;

    public acukwikFbos: any[];
    public clusters: AirportFboGeoFenceCluster[];
    public clustersFiltered: AirportFboGeoFenceCluster[];
    public parkingOccurrences: FlightWatch[];

    public loaderName: string = 'fbo-geofence-map-loader';
    public isEditing: boolean = false;
    public activeCluster: AirportFboGeoFenceCluster = null;
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
        private airportFboGeofenceClusterCoordinatesService: AirportFboGeofenceClusterCoordinatesService) {
            super();
        }

    ngOnInit(): void {
        this.center = {
            lat: this.airportFboGeofenceGridItem.latitude,
            lng: this.airportFboGeofenceGridItem.longitude,
        };

        const eventHandler = () => this.refreshMap();

        this.buildMap(this.center, this.mapContainer, this.mapStyle, this.zoom)
            .onStyleData(() => this.mapStyleLoaded())
            .onMapClick((e) => this.mapClicked(e))
            .onStyleLoad(() => this.refreshClustersOnMap())
            .onLoad(async () => {
                eventHandler();
            });

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
                this.clustersFiltered.push(response);
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
                this.clustersFiltered.splice(this.clustersFiltered.indexOf(cluster), 1);
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

    public layerToggleClicked(styleId: string): void {
        if (styleId.indexOf('default') == -1)
            this.setStyle('mapbox://styles/mapbox/' + styleId);
        else
            this.setStyle('mapbox://styles/fuelerlinx/ckszkcycz080718l7oaqoszvd');
    }

    public onMouseEnterFbo(cluster: AirportFboGeoFenceCluster): void {
        var layerId = 'fbo-cluster-layer-' + cluster.oid;
        var layer = this.getLayer(layerId);
        if (!layer)
            return;
        this.setPaintProperty(layerId, 'fill-color', '#FFEC14');
    }

    public onMouseLeaveFbo(cluster: AirportFboGeoFenceCluster): void {
        var layerId = 'fbo-cluster-layer-' + cluster.oid;
        var layer = this.getLayer(layerId);
        if (!layer)
            return;
        this.setPaintProperty(layerId, 'fill-color', '#0080ff');
    }

    public onClusterFilterChanged(value: string) {
        this.clustersFiltered.length = 0;
        this.clusters.forEach((cluster: AirportFboGeoFenceCluster) => {
            if (value == null || value == '')
                this.clustersFiltered.push(cluster);
            else if (cluster.fboName.toLowerCase().indexOf(value.toLowerCase()) > -1)
                this.clustersFiltered.push(cluster);
        });
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
            let layer = this.getLayer(id);
            if (layer)  this.removeLayer(id);
        });

        this.fboClusterLayerIds = [];

        this.fboClusterSourceIds.forEach((id) => {
            let source = this.getSource(id);
            if (source) this.removeSource(id);
        });

        this.fboClusterSourceIds = [];

        this.clusters.forEach((cluster: AirportFboGeoFenceCluster) => {
            const clusterPolygonSourceId: string = 'fbo-cluster-source-' + cluster.oid;
            const clusterPolygonLayerId: string = 'fbo-cluster-layer-' + cluster.oid;

            this.addSource(clusterPolygonSourceId,
                {
                    data: {
                        geometry: {
                            coordinates: [ cluster.clusterCoordinatesCollection.map(coords => [coords.longitude,coords.latitude])],
                            type: 'Polygon',
                        },
                        properties: {
                            id: cluster.oid,
                        },
                        type: 'Feature',
                    },
                    type: 'geojson',
                });

            this.addLayer({
                id: clusterPolygonLayerId,
                layout: {},
                source: clusterPolygonSourceId,
                type: 'fill',
                paint: {
                    'fill-color': (this.activeCluster != null && this.activeCluster != cluster ? '#FFB042' : '#0080ff'),
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
                this.acukwikFbos.push(...response.sort(x => x.handlerLongName));
            });
    }

    private loadClusters(): void {
        this.airportFboGeoFenceClustersService.getClustersByAcukwikAirportId(this.airportFboGeofenceGridItem.acukwikAirportId)
            .subscribe((response: any) => {
                this.clusters = [];
                this.clustersFiltered = [];
                this.checkLoadingStatus();
                if (!response)
                    return;
                this.clusters.push(...response.sort(x => x.fboName));
                this.clustersFiltered.push(...response.sort(x => x.fboName));
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
