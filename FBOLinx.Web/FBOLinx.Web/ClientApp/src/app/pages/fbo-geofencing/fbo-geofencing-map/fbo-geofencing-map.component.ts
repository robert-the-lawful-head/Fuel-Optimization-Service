import {
    Component,
    Input,
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
import { SharedService } from '../../../layouts/shared-service';
import { AcukwikairportsService } from "../../../services/acukwikairports.service";
import { AirportFboGeofenceClustersService } from "../../../services/airportfbogeofenceclusters.service";
import { AirportWatchService } from "../../../services/airportwatch.service";

// Models
import { FlightWatch } from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { AirportFboGeoFenceGridViewmodel } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-grid-viewmodel';

@Component({
    selector: 'app-fbo-geofencing-map',
    styleUrls: ['./fbo-geofencing-map.component.scss'],
    templateUrl: './fbo-geofencing-map.component.html',
})
export class FboGeofencingMapComponent implements OnInit, OnDestroy {
    //@Input() center: mapboxgl.LngLatLike;
    //@Input() isStable: boolean;
    @Input() airportFboGeofenceGridItem: AirportFboGeoFenceGridViewmodel;

    // Map Options
    map: mapboxgl.Map;
    zoom = 13;
    keys: string[] = [];
    styleLoaded = false;
    showLayers: boolean = false;
    
    public acukwikFbos: any[];
    public clusters: AirportFboGeoFenceCluster[];
    public parkingOccurrences: FlightWatch[];
    
    public loaderName: string = 'fbo-geofence-map-loader';
    public isEditing: boolean = false;
    public activeCluster: AirportFboGeoFenceCluster = null;

    //Map items
    public parkingOccurenceMarkers: mapboxgl.Marker[] = [];

    constructor(private ngxLoader: NgxUiLoaderService,
        private acukwikAirportsService: AcukwikairportsService,
        private airportFboGeoFenceClusters: AirportFboGeofenceClustersService,
        private airportWatchService: AirportWatchService,
        private newClusterDialog: MatDialog) { }

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
        this.map.on('zoom', eventHandler);
        this.map.on('dragend', eventHandler);
        this.map.on('rotate', eventHandler);
        this.map.on('resize', eventHandler);
        this.map.on('load', () => eventHandler());
        this.map.on('styledata', () => this.mapStyleLoaded());

        this.ngxLoader.startLoader(this.loaderName);
        this.loadAvailableFbos();
        this.loadParkingOccurrences();
        this.loadClusters();
    }

    //ngOnChanges(changes: SimpleChanges): void {
    //    const currentData = changes.data.currentValue;
    //    const oldData = changes.data.previousValue;
    //    if (!isEqual(currentData, oldData)) {
    //        this.refreshMap();
    //    }
    //}

    ngOnDestroy(): void {
        this.map.remove();
    }

    public refreshMap(): void {
        if (!this.map)
            return;

        this.refreshParkingOccurrencesOnMap();
    }

    public mapStyleLoaded(): void {
        this.styleLoaded = true;
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
            this.airportFboGeoFenceClusters.add(airportFboGeoFence).subscribe((response: AirportFboGeoFenceCluster) => {
                if (!response)
                    return;
                this.airportFboGeoFenceClusters.add(response);
                this.activeCluster = response;
            });
        });
    }

    public editRowClicked(cluster): void {
        this.isEditing = true;
        this.activeCluster = cluster;
    }

    public finishedEditingClicked(): void {
        this.isEditing = false;
        this.activeCluster = null;
    }

    private refreshParkingOccurrencesOnMap(): void {
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
        this.airportFboGeoFenceClusters.getClustersByAcukwikAirportId(this.airportFboGeofenceGridItem.acukwikAirportId)
            .subscribe((response: any) => {
                this.clusters = [];
                this.checkLoadingStatus();
                if (!response)
                    return;
                this.clusters.push(...response);
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
