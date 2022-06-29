import { Injectable } from '@angular/core';
import { FlightWatchMapService } from './flight-watch-map.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftFlightWatchService {

constructor(private flightWatchMapService : FlightWatchMapService) { }
    public getFlightFeatureJsonData(data: any,id,focusedMarkerId): any {
        let icon = "aircraft_image_";
        if(data.isInNetwork){
            icon = `${icon}client`;
        }else{
            icon = `${icon}${this.flightWatchMapService.getDefaultAircraftType(
                data.aircraftTypeCode
            )}${id === focusedMarkerId.toString() ? '_reversed' : ''}${
                data.fuelOrder != null
                    ? '_release'
                    : data.isFuelerLinxCustomer
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
                id: data.oid,
                // description:
                //     '<app-flight-watch-aircraft-info-dialog><app-flight-watch-aircraft-info-dialog/>',
                'icon': icon,
                'rotate': data.trackingDegree ?? 0,
                'size': 0.5,
            },
            type: 'Feature',
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
                'icon-image': ['get', 'icon'],
                'icon-rotate': ['get', 'rotate'],
                'icon-size': ['get', 'size']
            },
            source: flightSourceId,
            type: 'symbol',
        };
    }
}
