import {
    Component,
    Input,
    OnDestroy,
    OnInit,
} from '@angular/core';
import * as mapboxgl from 'mapbox-gl';

@Component({
    selector: 'app-fbo-geofencing-map',
    styleUrls: ['./fbo-geofencing-map.component.scss'],
    templateUrl: './fbo-geofencing-map.component.html',
})
export class FboGeofencingMapComponent implements OnInit, OnDestroy {
    //@Input() center: mapboxgl.LngLatLike;
    //@Input() isStable: boolean;

    // Map Options
    map: mapboxgl.Map;
    zoom = 13;
    keys: string[] = [];
    styleLoaded = false;
    showLayers: boolean = false;

    constructor() {}

    ngOnInit(): void {
        this.map = new mapboxgl.Map({
            accessToken:
                'pk.eyJ1IjoiZnVlbGVybGlueCIsImEiOiJja3NzODNqcG4wdHVrMm9rdHU3OGRpb2dmIn0.LvSvlGG0ej3PEDJOBpOoMQ',
            //center: this.center,
            container: 'fbo-geofencing-map',
            style: 'mapbox://styles/fuelerlinx/ckwb6l72y1c5m14p1nocmcjpc',
            zoom: this.zoom,
        });
        const eventHandler = () => this.refreshMap();
        this.map.on('zoom', eventHandler);
        this.map.on('dragend', eventHandler);
        this.map.on('rotate', eventHandler);
        this.map.on('resize', eventHandler);
        this.map.on('load', () => eventHandler());
        this.map.on('styledata', () => this.mapStyleLoaded());
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

    refreshMap() {
    }

    mapStyleLoaded() {
        this.styleLoaded = true;
    }

    mapResize() {
        this.map.resize();
    }
}
