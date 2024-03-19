import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Aircraftwatch, FlightWatchModelResponse } from '../models/flight-watch';
import { AirportWatchHistoricalParking } from '../models/airport-watch-historical-parking';

import {
    AirportWatchHistoricalDataRequest,
    FlightWatchHistorical,
} from '../models/flight-watch-historical';
import { IntraNetworkVisitsReportItem } from '../models/intra-network-visits-report-item';
import { Observable } from 'rxjs';

@Injectable()
export class AirportWatchService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/airportwatch';
    }
    public logBackwards(body: FlightWatchModelResponse) {
        return this.http.post(
            `${this.accessPointUrl}/log-backwards`,
            body,
            { headers: this.headers }
        );
    }
    public getAll(groupId: number, fboId: number) {
        return this.http.get<any>(
            `${this.accessPointUrl}/list/group/${groupId}/fbo/${fboId}`,
            { headers: this.headers }
        );
    }

    public getArrivalsDepartures(
        groupId: number,
        fboId: number,
        body: AirportWatchHistoricalDataRequest,
        icao?: string
    ) : Observable<FlightWatchHistorical[]>{
        let query = icao ? `?icao=${icao}` : '';

        return this.http.post<FlightWatchHistorical[]>(
            `${this.accessPointUrl}/group/${groupId}/fbo/${fboId}/arrivals-depatures${query}`,
            body,
            { headers: this.headers }
        );
    }

    public getArrivalsDeparturesSwim(
        groupId: number,
        fboId: number,
        body: AirportWatchHistoricalDataRequest
    ) {
        return this.http.post<FlightWatchHistorical[]>(
            this.accessPointUrl +
            '/group/' +
            groupId +
            '/fbo/' +
            fboId +
            '/arrivals-depatures-swim',
            body,
            { headers: this.headers }
        );
    }

    public getVisits(
        groupId: number,
        fboId: number,
        body: AirportWatchHistoricalDataRequest
    ) {
        return this.http.post<FlightWatchHistorical[]>(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/visits',
            body,
            { headers: this.headers }
        );
    }

    public getStartDate() {
        return this.http.get<any>(this.accessPointUrl + '/start-date', {
            headers: this.headers,
        });
    }

    public getParkingOccurrencesAtAirport(icao, startDateTime = null, endDateTime = null) {
        var route = this.accessPointUrl + '/parking-occurrences/' + icao;
        if (startDateTime != null) {
            route += '?startDateTime=' + startDateTime.toString();
            if (endDateTime != null) {
                route += 'endDateTime=' + endDateTime.toString();
            }
        }
        return this.http.get(route,
            {
            });
    }
    public getAircraftLiveData(groupId: number, fboid: number, tailNumber: string) {
        return this.http.get<Aircraftwatch>(
            `${this.accessPointUrl}/aircraftLiveData/${groupId}/${fboid}/${tailNumber}`
        );
    }

    public getAntennaStatusData() {
        return this.http.get<any>(
            `${this.accessPointUrl}/allAntennas`,
            { headers: this.headers }
        );
    }

    public getUnassignedAntennaBoxes(antennaName: string) {
        var name = antennaName == null || antennaName == "" ? "none" : antennaName;
        return this.http.get<any>(
            `${this.accessPointUrl}/unassignedAntennas/${name}`,
            { headers: this.headers }
        );
    }

    public getIntraNetworkVisitsReport(groupId: number,fboId: number, startDateTimeUtc: Date, endDateTimeUtc: Date) {
        let query = `?startDateTimeUtc=${startDateTimeUtc.toISOString()}&endDateTimeUtc=${endDateTimeUtc.toISOString()}`;
        return this.http.get<IntraNetworkVisitsReportItem[]>(
            `${this.accessPointUrl}/intra-network/visits-report/group/${groupId}/fbo/${fboId}${query}`,
            { headers: this.headers }
        );
    }

    public createHistoricalParking(record: FlightWatchHistorical) {
        return this.http.post<FlightWatchHistorical>(
            `${this.accessPointUrl}/historical-parking`,
            record,
            { headers: this.headers }
        );
    }

    public updateHistoricalParking(record: FlightWatchHistorical) {
        return this.http.put<FlightWatchHistorical>(
            `${this.accessPointUrl}/historical-parking`,
            record,
            { headers: this.headers }
        );
    }
}
