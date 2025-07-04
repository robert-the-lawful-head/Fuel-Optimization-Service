import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import * as moment from 'moment';
import { CustomerCaptureRateReport } from '../models/customer-capture-rate-report';
import { Observable } from 'rxjs';

@Injectable()
export class FuelreqsService {
    private headers: HttpHeaders;
    private accessPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/fuelreqs';
    }

    public getForFbo(fboId: number) {
        return this.http.get(this.accessPointUrl + '/fbo/' + fboId, {
            headers: this.headers,
        });
    }

    public getForFboAndDateRange(
        fboId: number,
        startDate: Date,
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/daterange',
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getForGroupFboAndDateRange(
        groupId: number,
        fboId: number,
        startDate: Date,
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl +
                '/group/' +
                groupId +
                '/fbo/' +
                fboId +
                '/daterange',
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getForFboCount(fboId: number, startDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/count' + '/startdate',
            {
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public get(id) {
        return this.http.get(this.accessPointUrl + '/' + id, {
            headers: this.headers,
        });
    }

    public getBySourceId(sourceId, fboId) {
        return this.http.get(this.accessPointUrl + '/sourceid/' + sourceId + '/fboid/' + fboId, {
            headers: this.headers,
        });
    }


    public add(payload) {
        return this.http.post(this.accessPointUrl + '/create', payload, {
            headers: this.headers,
        });
    }

    public remove(payload) {
        return this.http.delete(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public update(payload) {
        return this.http.put(this.accessPointUrl + '/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public updateArchived(payload) {
        return this.http.put(this.accessPointUrl + '/updatearchived/' + payload.oid, payload, {
            headers: this.headers,
        });
    }

    public getAdditionalOrderNotes(associatedFuelOrderId: number, associatedServiceOrderId: number,associatedFuelerlinxTransactionId: number) {
        return this.http.get(
            this.accessPointUrl + '/ordernotes/associatedfuelorderid/' + associatedFuelOrderId + '/associatedserviceorderid/' + associatedServiceOrderId + '/associatedfuelerlinxtransactionid/' + associatedFuelerlinxTransactionId,
            {
                headers: this.headers,
            }
        );
    }

    public addAdditionalNotes(payload) {
        return this.http.post(
            this.accessPointUrl + '/ordernotes', payload,
            {
                headers: this.headers,
            }
        );
    }

    public editAdditionalNote(payload) {
        return this.http.put(
            this.accessPointUrl + '/ordernotes', payload,
            {
                headers: this.headers,
            }
        );
    }

    public deleteAdditionalNote(payload) {
        return this.http.delete(
            this.accessPointUrl + '/ordernotes/' + payload.oid,
            {
                headers: this.headers,
            }
        );
    }

    // Analysis Services

    public topCustomersForFbo(fboId, payload) {
        return this.http.post(
            this.accessPointUrl + '/analysis/top-customers/fbo/' + fboId,
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public totalOrdersByMonthForFbo(fboId, payload) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/total-orders-by-month/fbo/' +
                fboId,
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public totalOrdersByAircraftSizeForFbo(fboId, payload) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/total-orders-by-aircraft-size/fbo/' +
                fboId,
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public getOrdersByLocation(payload) {
        return this.http.post(
            this.accessPointUrl + '/analysis/fuelerlinx/orders-by-location',
            payload,
            {
                headers: this.headers,
            }
        );
    }

    public getQuotesAndOrders(fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/quotes-orders-over-time/fbo/' +
                fboId,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                fboId,
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getOrders(fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/analysis/orders-won-over-time/fbo/' + fboId,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                fboId,
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getVolumesNearbyAirport(
        fboId: number,
        startDate: Date,
        endDate: Date,
        mile: number
    ) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/volumes-nearby-airport/fbo/' +
                fboId,
            {
                distanceMile: mile,
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                icao: '',
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getMarketShareAirport(
        fboId: number,
        startDate: Date,
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl + '/analysis/market-share-airport/fbo/' + fboId,
            {
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getFBOCustomersBreakdown(
        fboId: number,
        groupId: number,
        customerId: number,
        startDate: Date,
        endDate: Date,
        chartType: string
    ) {
        return this.http.post(
            this.accessPointUrl + '/analysis/customers-breakdown/group/' +
            groupId +
            '/fbo/' +
            fboId +
            '/customer/' +
            customerId,
            {
                chartType,
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                icao: '',
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getCompaniesQuotingDealStatistics(
        groupId: number,
        fboId: number,
        startDate: Date,
        endDate: Date,
        icao?: string
    ) {
        let query = icao ? `?icao=${icao}` : '';

        return this.http.post(
            `${this.accessPointUrl}/analysis/company-quoting-deal-statistics/group/${groupId}/fbo/${fboId}${query}`,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm')
            },
            {
                headers: this.headers,
            }
        );
    }
    public getCustomerCaptureRate(
        groupId: number,
        fboId: number,
        startDate: Date,
        endDate: Date,
    ): Observable<CustomerCaptureRateReport[]>{
        let query = `?startDateTime=${startDate.toISOString()}&endDateTime=${endDate.toISOString()}`;
        return this.http.get<CustomerCaptureRateReport[]>(
            `${this.accessPointUrl}/analysis/customer-capture-rate/group/${groupId}/fbo/${fboId}${query}`,
            {
                headers: this.headers,
            }
        );
    }
    public getCompaniesQuotingDealStatisticsForGroupFbos(
        groupId: number,
        fboIds: number[],
        startDate: Date,
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl +
            '/analysis/company-quoting-deal-statistics/group/' +
            groupId,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                fboIds,
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    //Get Customer Analytics Id
    public getCompanyQuotingDealStatistics(groupId: number, fboId: number, startDate: Date, endDate: Date , customerId: number) {
        return this.http.post(
            this.accessPointUrl + '/analysis/company-quoting-deal-statistics/group/' + groupId + '/fbo/' + fboId,
            {
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                customerId: customerId.toString()
            },
            {
                headers: this.headers,
            }
        );
    }

    public getFuelVendorSources(fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/fbo-fuel-vendor-sources/fbo/' +
                fboId,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                icao: '',
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getMarketShareFboAirport(
        fboId: number,
        startDate: Date,
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/market-share-fbo-airport/fbo/' +
                fboId,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                icao: '',
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
            },
            {
                headers: this.headers,
            }
        );
    }

    public getMarketShareFbosAirports(
        groupId: number,
        startDate: Date,
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/market-share-fbos-airports/group/' +
                groupId,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
                icaos: [],
                fbos: []
            },
            {
                headers: this.headers,
            }
        );
    }

    public getFuelVendorSourcesByAirports(
        groupId: number,
        startDate: Date,
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/fbo-fuel-vendor-sources/group/' +
                groupId,
            {
                endDateTime: moment(endDate).format('MM/DD/YYYY HH:mm'),
                startDateTime: moment(startDate).format('MM/DD/YYYY HH:mm'),
                icaosfbos: {}
            },
            {
                headers: this.headers,
            }
        );
    }
    public sendOrderConfirmationNotification(payload) {
        return this.http.post(
            `${this.accessPointUrl}/send-email-confirmation`,
            payload,
            {
                headers: this.headers,
            }
        );
    }
}
