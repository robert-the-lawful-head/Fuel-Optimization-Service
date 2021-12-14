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

import { SharedService } from '../../../layouts/shared-service';
import { AirportFboGeofenceClustersService } from "../../../services/airportfbogeofenceclusters.service";
import { AirportFboGeofenceClusterCoordinatesService } from
    "../../../services/airportfbogeofenceclustercoordinates.service";

import { isCommercialAircraft } from '../../../../utils/aircraft';
import { FlightWatch } from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { AIRCRAFT_IMAGES } from './aircraft-images';

type LayerType = 'airway' | 'streetview' | 'icao' | 'taxiway';

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
    isCommercialVisible = true;
    isShowAirportCodesEnabled = true;
    isShowTaxiwaysEnabled = true;
    previousMarkerId: number = 0;
    focusedMarkerId: number = 0;
    showLayers: boolean = false;

    public clusters: AirportFboGeoFenceCluster[];
    public fboClusterLayerIds: string[] = [];
    public fboClusterSourceIds: string[] = [];
    public activeCluster: AirportFboGeoFenceCluster = null;

    constructor(private airportFboGeoFenceClustersService: AirportFboGeofenceClustersService,
        private airportFboGeofenceClusterCoordinatesService: AirportFboGeofenceClusterCoordinatesService,
        private sharedService: SharedService) { }

    ngOnInit(): void {
        this.map = new mapboxgl.Map({
            accessToken:
                'pk.eyJ1IjoiZnVlbGVybGlueCIsImEiOiJja3NzODNqcG4wdHVrMm9rdHU3OGRpb2dmIn0.LvSvlGG0ej3PEDJOBpOoMQ',
            center: this.center,
            container: 'flight-watch-map',
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

            const releaseImg = new Image(image.size, image.size);
            releaseImg.onload = () => {
                this.map.addImage(`aircraft_image_${image.id}_release`, releaseImg);
            };
            releaseImg.src = image.blueUrl;

            const releaseReversedImg = new Image(image.size, image.size);
            releaseReversedImg.onload = () => {
                this.map.addImage(
                    `aircraft_image_${image.id}_reversed_release`,
                    releaseReversedImg
                );
            };
            releaseReversedImg.src = image.blueReverseUrl;

            const fuelerlinxImg = new Image(image.size, image.size);
            fuelerlinxImg.onload = () => {
                this.map.addImage(`aircraft_image_${image.id}_fuelerlinx`, fuelerlinxImg);
            };
            fuelerlinxImg.src = image.fuelerlinxUrl;

            const fuelerlinxReversedImg = new Image(image.size, image.size);
            fuelerlinxReversedImg.onload = () => {
                this.map.addImage(
                    `aircraft_image_${image.id}_reversed_fuelerlinx`,
                    fuelerlinxReversedImg
                );
            };
            fuelerlinxReversedImg.src = image.fuelerlinxReverseUrl;
        });

        this.loadClusters();
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
                    (this.isCommercialVisible ||
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
                                id === this.focusedMarkerId.toString()
                                    ? '_reversed'
                                    : ''
                                }${row.fuelOrder != null ? '_release' : (row.isFuelerLinxCustomer ? '_fuelerlinx' : '')}`,
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
        const id = e.features[0].properties.id;
        if (self.focusedMarkerId !== Number(id)) {
            self.focusedMarkerId = id;
        }
        if (self.previousMarkerId && self.data[self.previousMarkerId] != null) {
            var previousMarker = self.data[self.previousMarkerId];
            self.setFlightWatchMarkerLayout(previousMarker);

            //this.map.setLayoutProperty(
            //    `aircraft_${previousMarker.oid}`,
            //    'icon-image',
            //    `aircraft_image_${self.getAircraftTypeCode(
            //        previousMarker
            //    )}${previousMarker.fuelOrder != null ? '_release' : (previousMarker.isFuelerLinxCustomer ? '_fuelerlinx' : '')}`
            //);
        }

        var focusedMarker = self.data[id];
        self.markerClicked.emit(self.data[id]);

        if (self.focusedMarkerId > 0) {
            self.previousMarkerId = self.focusedMarkerId;
        } else {
            self.previousMarkerId = 0;
        }

        self.setFlightWatchMarkerLayout(focusedMarker);

        //this.map.setLayoutProperty(
        //    `aircraft_${id}`,
        //    'icon-image',
        //    `aircraft_image_${self.getAircraftTypeCode(self.data[id])}${
        //    self.focusedMarkerId > 0 ? '_reversed' : ''
        //    }${focusedMarker?.fuelOrder != null ? '_release' : (focusedMarker?.isFuelerLinxCustomer ? '_fuelerlinx' : '')}`
        //);
    }

    setFlightWatchMarkerLayout(marker: FlightWatch) {
        try {
            this.map.setLayoutProperty(
                `aircraft_${marker.oid}`,
                'icon-image',
                `aircraft_image_${this.getAircraftTypeCode(marker)}${this.focusedMarkerId == marker.oid ? '_reversed' : ''
                }${marker?.fuelOrder != null ? '_release' : (marker?.isFuelerLinxCustomer ? '_fuelerlinx' : '')}`
            );
        } catch (e) {
            //Do nothing
        }
    }

    cursorPointer(cursor: string, self: any) {
        self.map.getCanvas().style.cursor = cursor;
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

        const visibility = this.map.getLayoutProperty(layers[0], 'visibility');

        // Toggle layer visibility by changing the layout object's visibility property.
        if (visibility === 'visible' || visibility === undefined) {
            layers.forEach((layer) => {
                this.map.setLayoutProperty(layer, 'visibility', 'none');
            });
        } else {
            layers.forEach((layer) => {
                this.map.setLayoutProperty(layer, 'visibility', 'visible');
            });
        }
        if (type == "icao")
            this.isShowAirportCodesEnabled = !this.isShowAirportCodesEnabled;
        else if (type == "taxiway")
            this.isShowTaxiwaysEnabled = !this.isShowTaxiwaysEnabled;
    }

    toggleCommercial(event: MouseEvent) {
        this.isCommercialVisible = !this.isCommercialVisible;
        this.refreshMap();
    }

    mapResize() {
        this.map.resize();
    }

    private loadClusters(): void {
        this.airportFboGeoFenceClustersService.getClustersByIcao(this.sharedService.currentUser.icao)
            .subscribe((response: any) => {
                this.clusters = [];
                if (!response)
                    return;
                this.clusters.push(...response);
                this.refreshClustersOnMap();
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
                            coordinates: [cluster.clusterCoordinatesCollection.map(coords => coords.longitudeLatitudeAsList)],
                            //coordinates: cluster.clusterCoordinatesCollection.map(coords => new mapboxgl.LngLat(coords.longitude, coords.latitude)),
                            type: 'Polygon',
                        },
                        properties: {
                            id: cluster.oid,
                            description: cluster.fboName
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

            // Create a popup, but don't add it to the map yet.
            const popup = new mapboxgl.Popup({
                closeButton: false,
                closeOnClick: false
            });

            this.map.on('mouseenter', clusterPolygonLayerId, (e) => {
                // Change the cursor style as a UI indicator.
                this.map.getCanvas().style.cursor = 'pointer';

                // Copy coordinates array.
                const coordinates = e.lngLat;
                const description = e.features[0].properties.description;

                // Ensure that if the map is zoomed out such that multiple
                // copies of the feature are visible, the popup appears
                // over the copy being pointed to.
                while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
                    coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
                }

                // Populate the popup and set its coordinates
                // based on the feature found.
                popup.setLngLat(coordinates).setHTML(description).addTo(this.map);
            });

            this.map.on('mouseleave', clusterPolygonLayerId, () => {
                this.map.getCanvas().style.cursor = '';
                popup.remove();
            });

            this.fboClusterLayerIds.push(clusterPolygonLayerId);
            this.fboClusterSourceIds.push(clusterPolygonSourceId);
        });
    }
}
