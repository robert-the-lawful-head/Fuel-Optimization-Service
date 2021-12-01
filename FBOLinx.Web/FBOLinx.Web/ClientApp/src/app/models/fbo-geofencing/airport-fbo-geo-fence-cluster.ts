import { AirportFboGeoFenceClusterCoordinates } from './airport-fbo-geo-fence-cluster-coordinates';

export type AirportFboGeoFenceCluster = {
    oid: number;
    acukwikAirportId: number;
    acukwikFboHandlerId: number;
    centerLatitude: number;
    centerLongitude: number;
    icao: string;
    fboName: string;
    clusterCoordinatesCollection: AirportFboGeoFenceClusterCoordinates[];
};
