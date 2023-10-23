import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { FlightWatchMapService } from './flight-watch-map.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftFlightWatchService {

constructor(private flightWatchMapService : FlightWatchMapService) { }
    public getFlightFeatureJsonData(data: FlightWatchModelResponse,isSelectedAircraft:boolean): any {
        let icon = `aircraft_image_${this.flightWatchMapService.getDefaultAircraftType(
            data
        )}`;

        icon += `${isSelectedAircraft ? '_reversed' : ''}${
            data.isActiveFuelRelease
                ? '_release'
                : data.isFuelerLinxClient
                ? '_fuelerlinx'
                : ''
        }`;

        return {
            geometry: {
                coordinates: [(data.previousLongitude)?data.previousLongitude:data.longitude??0, (data.previousLatitude)?data.previousLatitude:data.latitude??0],
                type: 'Point',
            },
            properties: {
                'origin-coordinates': [data.previousLongitude, data.previousLatitude],
                'destination-coordinates': [data.longitude??0, data.latitude??0],
                id: data.tailNumber,
                'default-icon-image': icon,
                'rotate': data.trackingDegree ?? 0,
                'size': 0.5,
            },
            type: 'Feature'
        };
    }
    public getFlightLayerJsonData(
        layerId: string,
        flightSourceId: any,
        isReversedLayer: boolean
    ): mapboxgl.AnyLayer {

        let json : mapboxgl.AnyLayer  = {
            id : layerId,
            layout: {
                'icon-allow-overlap': true,
                'icon-image':['get', 'default-icon-image'],
                'icon-rotate': ['get', 'rotate'],
                // 'icon-rotate': ['get', 'bearing'],
                // 'icon-rotation-alignment': 'map',
                'icon-size': ['get', 'size'],
                'symbol-z-order': 'source'
            },
            source: flightSourceId,
            type: 'symbol',

        };

        if(isReversedLayer) json['filter'] = ["==", "id", ""];
        return json;

    }
    public getAirportLayerJsonData(
        layerId: string,
        airportSourceId: any,
    ): mapboxgl.AnyLayer {
        return {
            id : layerId,
            layout: {
                'icon-allow-overlap': true,
                'icon-image': ['get', 'icon-image'],
                'icon-size': ['get', 'size']
            },
            source: airportSourceId,
            type: 'symbol',
        }
    }
    public getAirportFeatureJsonData(data: any, currentIcao: string): any {
        return {
            geometry: {
                coordinates: [data.longitudeInDegrees, data.latitudeInDegrees],
                type: 'Point',
            },
            properties: {
                id: data.oid,
                'icon-image': (data.icao == currentIcao)?'airport-icon-active':'airport-icon',
                'size': 0.5,
            },
            type: 'Feature'
        };
    }
}
