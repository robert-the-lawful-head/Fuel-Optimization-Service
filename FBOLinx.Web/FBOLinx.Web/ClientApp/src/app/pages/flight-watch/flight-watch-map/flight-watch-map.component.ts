import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { difference, isEqual, keys } from 'lodash';
import * as mapboxgl from 'mapbox-gl';
import { FlightWatch } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from './aircraft-images';
import { isCommercialAircraft } from '../../../../utils/aircraft';

type LayerType = 'airway' | 'streetview' | 'icao';

@Component({
    selector: 'app-flight-watch-map',
    templateUrl: './flight-watch-map.component.html',
    styleUrls: [ './flight-watch-map.component.scss' ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FlightWatchMapComponent implements OnInit, OnChanges, OnDestroy {
    @Input() center: mapboxgl.LngLatLike;
    @Input() data: {
        [oid: string]: FlightWatch;
    };
    @Input() isStable: boolean;
    @Output() markerClicked = new EventEmitter<FlightWatch>();

    // Map Options
    map: mapboxgl.Map;
    zoom = 13;
    keys: string[] = [];
    styleLoaded = false;
    isCommercialInvisible = true;

    constructor() {
    }

    ngOnInit(): void {
        this.map = new mapboxgl.Map({
            accessToken: 'pk.eyJ1IjoidGJyZWVzZSIsImEiOiJja280a3M3dDEwMzAyMnFwbjMwZ2VleWdxIn0.CyG67L4gTlEHV9oJiH7FFw',
            container: 'flight-watch-map',
            style: 'mapbox://styles/tbreese/ckoj6y81613y818qfsngeei08',
            center: this.center,
            zoom: this.zoom,
        });
        const eventHandler = () => this.refreshMap();
        this.map.on('zoom', eventHandler);
        this.map.on('dragend', eventHandler);
        this.map.on('rotate', eventHandler);
        this.map.on('resize', eventHandler);
        this.map.on('load', () => eventHandler());
        this.map.on('styledata', () => this.mapStyleLoaded());

        AIRCRAFT_IMAGES.forEach(image => {
            this.map.loadImage(image.url, (err, imageData) => {
                this.map.addImage(`aircraft_image_${image.id}`, imageData);
            });
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        const currentData = changes.data.currentValue;
        const oldData = changes.data.previousValue;
        if (!isEqual(currentData, oldData)) {
            this.refreshMap();
        }
    }

    ngOnDestroy(): void {
        const eventHandler = () => this.refreshMap();
        this.map.off('zoom', eventHandler);
        this.map.off('dragend', eventHandler);
        this.map.off('rotate', eventHandler);
        this.map.off('resize', eventHandler);
        this.map.off('load', eventHandler);
        this.map.off('styledata', () => this.mapStyleLoaded());
    }

    refreshMap() {
        if (this.map) {
            const bound = this.map.getBounds();

            const newKeys = keys(this.data).filter(id => {
                const flightWatch = this.data[Number(id)];
                const flightWatchPosition: mapboxgl.LngLatLike = {
                    lat: flightWatch.latitude,
                    lng: flightWatch.longitude,
                };
                return bound.contains(flightWatchPosition) &&
                    (!this.isCommercialInvisible || !isCommercialAircraft(flightWatch.aircraftTypeCode, flightWatch.atcFlightNumber));
            });

            const removals = difference(this.keys, newKeys);
            removals.forEach(key => {
                const id = `aircraft_${key}`;
                this.map.removeLayer(id);
                this.map.removeSource(id);
                this.map.off('click', id, (e) => this.clickHandler(e, this));
                this.map.off('mouseenter', id, () => this.cursorPointer('pointer', this));
                this.map.off('mouseleave', id, () => this.cursorPointer('', this));
            });

            this.keys = [...newKeys];

            newKeys.forEach(key => {
                const row = this.data[key];
                const id = `aircraft_${row.oid}`;
                let atype = row.aircraftTypeCode;
                if (!AIRCRAFT_IMAGES.find(ai => ai.id === atype)) {
                    atype = 'default';
                }

                const previousSource = this.map.getSource(id) as mapboxgl.GeoJSONSource;

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
                    this.map.addSource(id, {
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

                    this.map.on('click', id, (e) => this.clickHandler(e, this));

                    this.map.on('mouseenter', id, () => this.cursorPointer('pointer', this));

                    // Change it back to a pointer when it leaves.
                    this.map.on('mouseleave', id, () => this.cursorPointer('', this));
                }

                if (!previousSource) {
                    this.map.addLayer({
                        id,
                        type: 'symbol',
                        layout: {
                            'icon-image': `aircraft_image_${atype}`,
                            'icon-size': 0.5,
                            'icon-rotate': row.trackingDegree ?? 0,
                            'icon-allow-overlap': true
                        },
                        source: id,
                    });
                }
            });
        }
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

    mapStyleLoaded() {
        this.styleLoaded = true;
    }

    getLayersFromType(type: LayerType) {
        const airwayLayers = ['airways-lines', 'airways-labels'];
        const styleLayers = this.map
            .getStyle()
            .layers
            .filter(layer => !layer.id.startsWith('aircraft_') && !airwayLayers.includes(layer.id))
            .map(layer => layer.id);
        const icaoLayers = ['airports-names'];

        if (type === 'airway') {
            return airwayLayers;
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
        event.preventDefault();
        event.stopPropagation();

        const layers = this.getLayersFromType(type);

        const visibility = this.map.getLayoutProperty(
            layers[0],
            'visibility'
        );

        // Toggle layer visibility by changing the layout object's visibility property.
        if (visibility === 'visible' || visibility === undefined) {
            layers.forEach(layer => {
                this.map.setLayoutProperty(
                    layer,
                    'visibility',
                    'none'
                );
            });
            (event.target as any).className = '';
        } else {
            (event.target as any).className = 'active';
            layers.forEach(layer => {
                this.map.setLayoutProperty(
                    layer,
                    'visibility',
                    'visible'
                );
            });
        }
    }

    toggleCommercial(event: MouseEvent) {
        event.preventDefault();
        event.stopPropagation();

        this.isCommercialInvisible = !this.isCommercialInvisible;

        if (this.isCommercialInvisible) {
            (event.target as any).className = '';
        } else {
            (event.target as any).className = 'active';
        }

        this.refreshMap();
    }

    mapResize() {
        this.map.resize();
    }
}
