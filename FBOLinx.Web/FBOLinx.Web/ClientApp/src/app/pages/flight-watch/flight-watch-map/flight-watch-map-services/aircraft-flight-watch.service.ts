import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { FlightWatchMapService } from './flight-watch-map.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftFlightWatchService {

constructor(private flightWatchMapService : FlightWatchMapService) { }

    public getAricraftIcon(isSelectedAircraft: boolean,data: FlightWatchModelResponse): string {
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
        return icon;
    }
    public getFlightFeatureJsonData(data: FlightWatchModelResponse): GeoJSON.Feature<GeoJSON.Point,GeoJSON.GeoJsonProperties>  {
        let icon = this.getAricraftIcon(false,data);

        return {
            geometry: {
                coordinates:  [data.previousLongitude, data.previousLatitude],
                type: 'Point',
            },
            properties: {
                'origin-coordinates': [data.previousLongitude, data.previousLatitude],
                'destination-coordinates': [data.longitude, data.latitude],
                id: data.tailNumber,
                'default-icon-image': icon,
                'bearing': data.trackingDegree,
                'size': 0.5
                // 'status': data.status,
            },
            id: data.tailNumber,
            type: 'Feature'
        };
    }
    public getFlightLayerJsonData(
        layerId: string,
        flightSourceId: any
    ): mapboxgl.AnyLayer {

        let json : mapboxgl.AnyLayer  = {
            id : layerId,
            layout: {
                'icon-allow-overlap': true,
                'icon-image':['get', 'default-icon-image'],
                'icon-rotate': ['get', 'bearing'],
                'icon-size': ['get', 'size'],
                'symbol-z-order': 'source'
            },
            source: flightSourceId,
            type: 'symbol',

        };

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

    public getAirportFeatureJsonData(data: any, currentIcao: string): GeoJSON.Feature<GeoJSON.Geometry,GeoJSON.GeoJsonProperties> {
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
