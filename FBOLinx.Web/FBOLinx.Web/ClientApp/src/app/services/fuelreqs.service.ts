import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

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
                endDateTime: endDate,
                startDateTime: startDate,
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
                endDateTime: endDate,
                startDateTime: startDate,
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
                startDateTime: startDate,
            },
            {
                headers: this.headers,
            }
        );
    }

    public get(payload) {
        return this.http.get(this.accessPointUrl + '/' + payload.oid, {
            headers: this.headers,
        });
    }

    public add(payload) {
        return this.http.post(this.accessPointUrl, payload, {
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
                endDateTime: endDate,
                fboId,
                startDateTime: startDate,
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
                endDateTime: endDate,
                fboId,
                startDateTime: startDate,
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
                endDateTime: endDate,
                icao: '',
                startDateTime: startDate,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getFBOCustomersBreakdown(
        fboId: number,
        startDate: Date,
        endDate: Date,
        chartType: string
    ) {
        return this.http.post(
            this.accessPointUrl + '/analysis/customers-breakdown/fbo/' + fboId,
            {
                chartType,
                endDateTime: endDate,
                icao: '',
                startDateTime: startDate,
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
        endDate: Date
    ) {
        return this.http.post(
            this.accessPointUrl +
                '/analysis/company-quoting-deal-statistics/group/' +
                groupId +
                '/fbo/' +
                fboId,
            {
                endDateTime: endDate,
                startDateTime: startDate,
            },
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
                endDateTime: endDate,
                fboIds,
                startDateTime: startDate,
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
                endDateTime: endDate,
                icao: '',
                startDateTime: startDate,
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
                endDateTime: endDate,
                icao: '',
                startDateTime: startDate,
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
                endDateTime: endDate,
                startDateTime: startDate,
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
                endDateTime: endDate,
                startDateTime: startDate,
            },
            {
                headers: this.headers,
            }
        );
    }
}
