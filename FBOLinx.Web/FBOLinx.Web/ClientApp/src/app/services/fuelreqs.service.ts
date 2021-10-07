import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

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

    public getForFboAndDateRange(fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/fbo/' + fboId + '/daterange',
            {
                startDateTime: startDate,
                endDateTime: endDate,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getForGroupFboAndDateRange(groupId: number, fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/group/' + groupId + '/fbo/' + fboId + '/daterange',
            {
                startDateTime: startDate,
                endDateTime: endDate,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getForFboCount(fboId: number, startDate: Date) {
      return this.http.post(this.accessPointUrl + '/fbo/' + fboId + '/count' + '/startdate',
        {
          startDateTime: startDate,
        },
        {
            headers: this.headers,
        });
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
            this.accessPointUrl + '/analysis/quotes-orders-over-time/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
                fboId,
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
                startDateTime: startDate,
                endDateTime: endDate,
                fboId,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getVolumesNearbyAirport(fboId: number, startDate: Date, endDate: Date, mile: number) {
        return this.http.post(
            this.accessPointUrl + '/analysis/volumes-nearby-airport/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
                distanceMile: mile,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getMarketShareAirport(fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/analysis/market-share-airport/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getFBOCustomersBreakdown(fboId: number, startDate: Date, endDate: Date, chartType: string) {
        return this.http.post(
            this.accessPointUrl + '/analysis/customers-breakdown/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
                chartType,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getCompaniesQuotingDealStatistics(groupId: number, fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/analysis/company-quoting-deal-statistics/group/' + groupId + '/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
            },
            {
                headers: this.headers,
            }
        );
    }

    //Get Customer Analytics Id
    public getCompanyQuotingDealStatistics(groupId: number, fboId: number, startDate: Date, endDate: Date , customerId: number) {
       console.log(customerId);
        return this.http.post(
            this.accessPointUrl + '/analysis/company-quoting-deal-statistics/group/' + groupId + '/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
                customerId: customerId.toString()
            },
            {
                headers: this.headers,
            }
        );
    }

    public getFuelVendorSources(fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/analysis/fbo-fuel-vendor-sources/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
            },
            {
                headers: this.headers,
            }
        );
    }

    public getMarketShareFboAirport(fboId: number, startDate: Date, endDate: Date) {
        return this.http.post(
            this.accessPointUrl + '/analysis/market-share-fbos-airport/fbo/' + fboId,
            {
                startDateTime: startDate,
                endDateTime: endDate,
            },
            {
                headers: this.headers,
            }
        );
    }
}
