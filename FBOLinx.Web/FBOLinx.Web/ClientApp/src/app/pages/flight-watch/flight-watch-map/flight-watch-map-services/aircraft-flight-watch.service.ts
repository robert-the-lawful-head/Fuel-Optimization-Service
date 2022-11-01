import { Injectable } from '@angular/core';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { convertDMSToDEG } from 'src/utils/coordinates';
import { FlightWatchMapService } from './flight-watch-map.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftFlightWatchService {

constructor(private flightWatchMapService : FlightWatchMapService) { }
    public getFlightFeatureJsonData(data: FlightWatchModelResponse,isSelectedAircraft:boolean): any {
        let icon = "aircraft_image_";

        let iconDefault = `${icon}${this.flightWatchMapService.getDefaultAircraftType(
            data.aircraftTypeCode
        )}`;

        let iconReversed = `${iconDefault}_reversed`;
        return {
            geometry: {
                coordinates: [data.longitude, data.latitude],
                type: 'Point',
            },
            properties: {
                id: data.tailNumber,
                'default-icon-image': isSelectedAircraft ? iconReversed : iconDefault,
                'rotate': data.trackingDegree ?? 0,
                'size': 0.5,
            },
            id: data.tailNumber,
            type: 'Feature'
        };
    }
    public getFlightLayerJsonData(
        layerId: string,
        flightSourceId: any,
    ): mapboxgl.AnyLayer {
        return {
            id : layerId,
            layout: {
                'icon-allow-overlap': true,
                'icon-image':['get', 'default-icon-image'],
                'icon-rotate': ['get', 'rotate'],
                'icon-size': ['get', 'size'],
                'symbol-z-order': 'source'
            },
            source: flightSourceId,
            type: 'symbol',
        };
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
                coordinates: [convertDMSToDEG(data.longitude), convertDMSToDEG(data.latitude)],
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
