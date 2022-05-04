import { Injectable } from '@angular/core';
import { FlightWatchMapService } from './flight-watch-map.service';

@Injectable({
  providedIn: 'root'
})
export class AircraftFlightWatchService {

constructor(private flightWatchMapService : FlightWatchMapService) { }
    public getFlightFeatureJsonData(data: any,id,focusedMarkerId): any {
        return {
            geometry: {
                coordinates: [data.longitude, data.latitude],
                type: 'Point',
            },
            properties: {
                id: data.oid,
                description:
                    '<strong>test popup' +
                    data.oid +
                    '</strong><p><a href="http://www.mtpleasantdc.com/makeitmtpleasant" target="_blank" title="Opens in a new window">Make it Mount Pleasant</a></p>',
                'icon': `aircraft_image_${this.flightWatchMapService.getDefaultAircraftType(
                    data.aircraftTypeCode
                )}${id === focusedMarkerId.toString() ? '_reversed' : ''}${
                    data.fuelOrder != null
                        ? '_release'
                        : data.isFuelerLinxCustomer
                        ? '_fuelerlinx'
                        : ''
                }`,
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
