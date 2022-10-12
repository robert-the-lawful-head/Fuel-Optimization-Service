import { Injectable } from '@angular/core';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { convertDMSToDEG } from 'src/utils/coordinates';
import { FlightWatchMapService } from './flight-watch-map.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftFlightWatchService {

constructor(private flightWatchMapService : FlightWatchMapService) { }
    public getFlightFeatureJsonData(data: FlightWatchModelResponse,id,selectedAircraft): any {
        let icon = "aircraft_image_";
        if (data.isInNetwork && !data.fuelOrderId && !data.isFuelerLinxClient){
            icon = `${icon}client`;
        }else{
            icon = `${icon}${this.flightWatchMapService.getDefaultAircraftType(
                data.aircraftTypeCode
            )}${id === selectedAircraft.toString() ? '_reversed' : ''}${
                data.fuelOrderId != null
                    ? '_release'
                    : data.isFuelerLinxClient
                    ? '_fuelerlinx'
                    : ''
            }`
        }
        return {
            geometry: {
                coordinates: [data.longitude, data.latitude],
                type: 'Point',
            },
            properties: {
                id: data.tailNumber,
               'icon-image': icon,
                'rotate': data.trackingDegree ?? 0,
                'size': 0.5,
            },
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
                'icon-image': ['get', 'icon-image'],
                'icon-rotate': ['get', 'rotate'],
                'icon-size': ['get', 'size']
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
