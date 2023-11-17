import { Injectable } from '@angular/core';
import { Dictionary, keyBy } from 'lodash';
import * as mapboxgl from 'mapbox-gl';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { convertDMSToDEG } from 'src/utils/coordinates';
import { AircraftImageData, AIRCRAFT_IMAGES } from '../aircraft-images';

@Injectable({
    providedIn: 'root',
})
export class FlightWatchMapService {
    constructor(private acukwikairportsService: AcukwikairportsService
        ) {}
    async getMapCenter(icao: string): Promise<mapboxgl.LngLatLike> {
        var selectedAirport = await this.acukwikairportsService.getAcukwikAirportByICAO(icao).toPromise();
        return {
                lat: convertDMSToDEG(selectedAirport.latitude),
                lng: convertDMSToDEG(selectedAirport.longitude),
            };
    }
    getMapCenterByCoordinates(latitudeInDegrees: number, longitudeInDegrees: number): mapboxgl.LngLatLike {
            return {
                lat: latitudeInDegrees,
                lng: longitudeInDegrees,
            };
    }
    getDictionaryByTailNumberAsKey(
        data: FlightWatchModelResponse[]
    ): Dictionary<FlightWatchModelResponse> {
        return keyBy(data, (fw) => {
            return fw.tailNumber;
        });
    }
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
            },
            buffer: 0,
            tolerance: 10,
        };
    }
    public buildAircraftId(aircraftId: any): string {
        return `aircraft_${aircraftId}`;
    }
}
