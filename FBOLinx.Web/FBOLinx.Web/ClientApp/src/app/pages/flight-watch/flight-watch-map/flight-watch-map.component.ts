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
import { difference, isEqual, keys } from 'lodash';
import * as mapboxgl from 'mapbox-gl';

import { isCommercialAircraft } from '../../../../utils/aircraft';
import { FlightWatch } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from './aircraft-images';

type LayerType = 'airway' | 'streetview' | 'icao';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-map',
    styleUrls: ['./flight-watch-map.component.scss'],
    templateUrl: './flight-watch-map.component.html',
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
    previousMarker: FlightWatch;
    focusedMarker: FlightWatch;

    constructor() {}

    ngOnInit(): void {
        this.map = new mapboxgl.Map({
            accessToken:
                'pk.eyJ1IjoidGJyZWVzZSIsImEiOiJja280a3M3dDEwMzAyMnFwbjMwZ2VleWdxIn0.CyG67L4gTlEHV9oJiH7FFw',
            center: this.center,
            container: 'flight-watch-map',
            style: 'mapbox://styles/tbreese/ckoj6y81613y818qfsngeei08',
            zoom: this.zoom,
        });
        const eventHandler = () => this.refreshMap();
        this.map.on('zoom', eventHandler);
        this.map.on('dragend', eventHandler);
        this.map.on('rotate', eventHandler);
        this.map.on('resize', eventHandler);
        this.map.on('load', () => eventHandler());
        this.map.on('styledata', () => this.mapStyleLoaded());

        AIRCRAFT_IMAGES.forEach((image) => {
            const img = new Image(image.size, image.size);
            img.onload = () => {
                this.map.addImage(`aircraft_image_${image.id}`, img);
            };
            img.src = image.url;

            const reversedImg = new Image(image.size, image.size);
            reversedImg.onload = () => {
                this.map.addImage(
                    `aircraft_image_${image.id}_reversed`,
                    reversedImg
                );
            };
            reversedImg.src = image.reverseUrl;

            const blueImg = new Image(image.size, image.size);
            blueImg.onload = () => {
                this.map.addImage(`aircraft_image_${image.id}_blue`, blueImg);
            };
            blueImg.src = image.blueUrl;

            const blueReversedImg = new Image(image.size, image.size);
            blueReversedImg.onload = () => {
                this.map.addImage(
                    `aircraft_image_${image.id}_reversed_blue`,
                    blueReversedImg
                );
            };
            blueReversedImg.src = image.blueReverseUrl;
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
        this.map.remove();
    }

    refreshMap() {
        if (this.map) {
            const bound = this.map.getBounds();

            const newKeys = keys(this.data).filter((id) => {
                const flightWatch = this.data[Number(id)];
                const flightWatchPosition: mapboxgl.LngLatLike = {
                    lat: flightWatch.latitude,
                    lng: flightWatch.longitude,
                };
                return (
                    bound.contains(flightWatchPosition) &&
                    (!this.isCommercialInvisible ||
                        !isCommercialAircraft(
                            flightWatch.aircraftTypeCode,
                            flightWatch.atcFlightNumber
                        ))
                );
            });

            const removals = difference(this.keys, newKeys);
            removals.forEach((key) => {
                const id = `aircraft_${key}`;
                this.map.removeLayer(id);
                this.map.removeSource(id);
                this.map.off('click', id, (e) => this.clickHandler(e, this));
                this.map.off('mouseenter', id, () =>
                    this.cursorPointer('pointer', this)
                );
                this.map.off('mouseleave', id, () =>
                    this.cursorPointer('', this)
                );
            });

            this.keys = [...newKeys];

            newKeys.forEach((key) => {
                const row = this.data[key];
                const id = `aircraft_${row.oid}`;
                let atype = row.aircraftTypeCode;
                if (!AIRCRAFT_IMAGES.find((ai) => ai.id === atype)) {
                    atype = 'default';
                }

                const previousSource = this.map.getSource(
                    id
                ) as mapboxgl.GeoJSONSource;

                if (previousSource) {
                    previousSource.setData({
                        geometry: {
                            coordinates: [row.longitude, row.latitude],
                            type: 'Point',
                        },
                        properties: {
                            id: row.oid,
                        },
                        type: 'Feature',
                    });
                } else {
                    this.map.addSource(id, {
                        data: {
                            geometry: {
                                coordinates: [row.longitude, row.latitude],
                                type: 'Point',
                            },
                            properties: {
                                id: row.oid,
                            },
                            type: 'Feature',
                        },
                        type: 'geojson',
                    });

                    this.map.on('click', id, (e) => this.clickHandler(e, this));

                    this.map.on('mouseenter', id, () =>
                        this.cursorPointer('pointer', this)
                    );

                    // Change it back to a pointer when it leaves.
                    this.map.on('mouseleave', id, () =>
                        this.cursorPointer('', this)
                    );
                }

                if (!previousSource) {
                    this.map.addLayer({
                        id,
                        layout: {
                            'icon-allow-overlap': true,
                            'icon-image': `aircraft_image_${atype}${
                                id === this.focusedMarker?.oid.toString()
                                    ? '_reversed'
                                    : ''
                            }${row.hasFuelOrders ? '_blue' : ''}`,
                            'icon-rotate': row.trackingDegree ?? 0,
                            'icon-size': 0.5,
                        },
                        source: id,
                        type: 'symbol',
                    });
                }
            });
        }
    }

    getAircraftTypeCode(row: FlightWatch) {
        let atype = row.aircraftTypeCode;
        if (!AIRCRAFT_IMAGES.find((ai) => ai.id === atype)) {
            atype = 'default';
        }
        return atype;
    }

    clickHandler(
        e: mapboxgl.MapMouseEvent & {
            features?: mapboxgl.MapboxGeoJSONFeature[];
        } & mapboxgl.EventData,
        self: FlightWatchMapComponent
    ) {
        if (self.previousMarker) {
            this.map.setLayoutProperty(
                `aircraft_${self.previousMarker.oid}`,
                'icon-image',
                `aircraft_image_${self.getAircraftTypeCode(
                    self.previousMarker
                )}${self.previousMarker.hasFuelOrders ? '_blue' : ''}`
            );
        }

        const id = e.features[0].properties.id;
        if (self.focusedMarker?.oid === Number(id)) {
            self.focusedMarker = undefined;
        } else {
            self.focusedMarker = self.data[id];
        }
        self.markerClicked.emit(self.data[id]);

        if (self.focusedMarker) {
            self.previousMarker = Object.assign({}, self.focusedMarker);
        } else {
            self.previousMarker = undefined;
        }

        this.map.setLayoutProperty(
            `aircraft_${id}`,
            'icon-image',
            `aircraft_image_${self.getAircraftTypeCode(self.data[id])}${
                self.focusedMarker ? '_reversed' : ''
            }${self.focusedMarker?.hasFuelOrders ? '_blue' : ''}`
        );
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

        const visibility = this.map.getLayoutProperty(layers[0], 'visibility');

        // Toggle layer visibility by changing the layout object's visibility property.
        if (visibility === 'visible' || visibility === undefined) {
            layers.forEach((layer) => {
                this.map.setLayoutProperty(layer, 'visibility', 'none');
            });
            (event.target as any).className = '';
        } else {
            (event.target as any).className = 'active';
            layers.forEach((layer) => {
                this.map.setLayoutProperty(layer, 'visibility', 'visible');
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
