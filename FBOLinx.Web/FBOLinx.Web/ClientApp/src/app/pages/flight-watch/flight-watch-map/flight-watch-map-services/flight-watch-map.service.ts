import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { AircraftImageData, AIRCRAFT_IMAGES } from '../aircraft-images';

@Injectable({
    providedIn: 'root',
})
export class FlightWatchMapService {
    constructor() {}
    public getDefaultAircraftType(atype: string): string {
        if (!AIRCRAFT_IMAGES.find((ai) => ai.id === atype)) {
            atype = 'default';
        }
        return atype;
    }
    public getGeojsonFeatureSourceJsonData(
        features: any[]
    ): mapboxgl.AnySourceData {
        return {
            type: 'geojson',
            data: {
                type: 'FeatureCollection',
                features: features,
            },
        };
    }
    public buildAircraftId(aircraftId: number): string {
        return `aircraft_${aircraftId}`;
    }
}
