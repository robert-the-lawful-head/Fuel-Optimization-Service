import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { AirportFboGeoFenceCluster } from 'src/app/models/fbo-geofencing/airport-fbo-geo-fence-cluster';

@Injectable({
  providedIn: 'root'
})
export class FboFlightWatchService {

constructor() { }
    public getFbosFeatureJsonData(
        fbos: AirportFboGeoFenceCluster
    ): any {
        return {
                geometry: {
                    coordinates: [
                        fbos.clusterCoordinatesCollection.map(
                            (coords) => coords.longitudeLatitudeAsList
                        ),
                    ],
                    type: 'Polygon',
                },
                properties: {
                    id: fbos.oid,
                    description: fbos.fboName,
                },
                type: 'Feature',
            };
    }
    public getFbosLayer(
        fboLayerId: string,
        fboSourceId: string
    ): mapboxgl.AnyLayer {
        return {
            id: fboLayerId,
            layout: {},
            source: fboSourceId,
            type: 'fill',
            paint: {
                'fill-color': '#0080ff',
                'fill-opacity': 0.5,
            },
        };
    }
}
