import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { difference, isEqual, keys } from 'lodash';
import * as mapboxgl from 'mapbox-gl';
import { FlightWatch } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from './aircraft-images';

@Component({
    selector: 'app-flight-watch-map',
    templateUrl: './flight-watch-map.component.html',
    styleUrls: [ './flight-watch-map.component.scss' ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FlightWatchMapComponent implements OnInit, OnChanges, OnDestroy, AfterViewInit {
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: {
        [oid: string]: FlightWatch;
    };
    @Input() isStable: boolean;
    @Output() markerClicked = new EventEmitter<FlightWatch>();

    // Map Options
    map: mapboxgl.Map;
    zoom = 8;

    keys: string[] = [];

    bounds = new google.maps.LatLngBounds(
        new google.maps.LatLng(85, -180),
        new google.maps.LatLng(-85, 180)
    );

    constructor() {
    }

    ngOnInit(): void {
        this.map = new mapboxgl.Map({
            accessToken: 'pk.eyJ1IjoiYWlyc3VwcG9ydCIsImEiOiJtTVpWZmkwIn0.CUgowMsXxx2FbX07qda6XQ',
            container: 'flight-watch-map',
            style: 'mapbox://styles/mapbox/light-v10',
            center: this.center,
            zoom: this.zoom,
        });
        this.map.on('boxzoomend', this.boundsChanged);
        this.map.on('dragend', this.boundsChanged);
        this.map.on('rotate', this.boundsChanged);
        this.map.on('resize', this.boundsChanged);
        this.map.on('load', this.boundsChanged);

        AIRCRAFT_IMAGES.forEach(image => {
            this.map.loadImage(image.url, (err, imageData) => {
                this.map.addImage(`aircraft_image_${image.id}`, imageData);
            });
        });
    }

    ngAfterViewInit(): void {
        this.map.off('boxzoomend', this.boundsChanged);
        this.map.off('dragend', this.boundsChanged);
        this.map.off('rotate', this.boundsChanged);
        this.map.off('resize', this.boundsChanged);
        this.map.off('load', this.boundsChanged);
    }

    ngOnChanges(changes: SimpleChanges): void {
        const currentData = changes.data.currentValue;
        const oldData = changes.data.previousValue;
        if (!isEqual(currentData, oldData)) {
            this.boundsChanged();
        }
    }

    ngOnDestroy(): void {
    }

    boundsChanged() {
        if (this.map) {
            this.updateKeys(this.data);
        }
    }

    updateKeys(data: { [oid: number]: FlightWatch }) {
        let newKeys = keys(data);

        if (this.map) {
            const bound = this.map.getBounds();
            newKeys = newKeys.filter(id => {
                const flightWatch = data[Number(id)];
                const flightWatchPosition: mapboxgl.LngLatLike = {
                    lat: flightWatch.latitude,
                    lng: flightWatch.longitude,
                };
                return bound.contains(flightWatchPosition);
            });
        }
        newKeys = newKeys.splice(0, 200);

        const removals = difference(this.keys, newKeys);
        removals.forEach(key => {
            this.map.removeLayer(key);
            this.map.removeSource(key);
            this.map.off('click', key, (e) => this.clickHandler(e, this));
            this.map.off('mouseenter', key, () => this.cursorPointer('pointer', this));
            this.map.off('mouseleave', key, () => this.cursorPointer('', this));
        });

        this.keys = [...newKeys];

        newKeys.forEach(key => {
            const row = this.data[key];
            let atype = row.aircraftTypeCode;
            if (!AIRCRAFT_IMAGES.find(ai => ai.id === atype)) {
                atype = 'default';
            }

            const previousSource = this.map.getSource(row.oid.toString()) as mapboxgl.GeoJSONSource;

            if (previousSource) {
                previousSource.setData({
                    type: 'Feature',
                    geometry: {
                        type: 'Point',
                        coordinates: [row.longitude, row.latitude],
                    },
                    properties: {
                        id: row.oid,
                    }
                });
            } else {
                this.map.addSource(row.oid.toString(), {
                    type: 'geojson',
                    data: {
                        type: 'Feature',
                        geometry: {
                            type: 'Point',
                            coordinates: [row.longitude, row.latitude],
                        },
                        properties: {
                            id: row.oid,
                        }
                    }
                });

                this.map.on('click', row.oid.toString(), (e) => this.clickHandler(e, this));

                this.map.on('mouseenter', row.oid.toString(), () => this.cursorPointer('pointer', this));

                // Change it back to a pointer when it leaves.
                this.map.on('mouseleave', row.oid.toString(), () => this.cursorPointer('', this));
            }

            if (!previousSource) {
                this.map.addLayer({
                    id: row.oid.toString(),
                    type: 'symbol',
                    layout: {
                        'icon-image': `aircraft_image_${atype}`,
                        'icon-size': 0.5,
                        'icon-rotate': row.trackingDegree ?? 0,
                        'icon-allow-overlap': true
                    },
                    source: row.oid.toString(),
                });
            }
        });
    }

    clickHandler(e: mapboxgl.MapMouseEvent & {
        features?: mapboxgl.MapboxGeoJSONFeature[];
    } & mapboxgl.EventData, self: any) {
        const id = e.features[0].properties.id;
        self.markerClicked.emit(this.data[id]);
    }

    cursorPointer(cursor: string, self: any) {
        self.map.getCanvas().style.cursor = cursor;
    }
}
