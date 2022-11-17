import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { AircraftImageData, AIRCRAFT_IMAGES } from '../aircraft-images';

@Injectable({
    providedIn: 'root',
})
export class FlightWatchMapService {
    constructor() {}
    public getDefaultAircraftType(flight: FlightWatchModelResponse): string {
        if (!flight.isActiveFuelRelease && !flight.isFuelerLinxClient && flight.isInNetwork) return "client";
        let atype = flight.aircraftTypeCode;
        if (!AIRCRAFT_IMAGES.find((ai) => ai.id === flight.aircraftTypeCode)) {
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
            }
        };
    }
    public buildAircraftId(aircraftId: any): string {
        return `aircraft_${aircraftId}`;
    }
}
