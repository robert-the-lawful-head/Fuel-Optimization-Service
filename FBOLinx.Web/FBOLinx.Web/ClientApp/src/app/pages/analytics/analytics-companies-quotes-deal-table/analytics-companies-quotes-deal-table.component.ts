import { Router } from '@angular/router';
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import { FbosService } from '../../../services/fbos.service';
import * as SharedEvent from '../../../models/sharedEvents';
import { MatSort } from '@angular/material/sort';

@Component({
    selector: 'app-analytics-companies-quotes-deal',
    templateUrl: './analytics-companies-quotes-deal-table.component.html',
    styleUrls: ['./analytics-companies-quotes-deal-table.component.scss'],
})
export class AnalyticsCompaniesQuotesDealTableComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatSort) sort: MatSort;

    public filterStartDate: Date;
    public filterEndDate: Date;
    public icao: string;
    public fbo: string;
    public icaoChangedSubscription: any;
    public chartName = 'companies-quotes-deal-table';
    public displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'airportOrders', 'lastPullDate'];
    public dataSource: MatTableDataSource<any[]>;

    constructor(
        private fuelreqsService: FuelreqsService,
        private fbosService: FbosService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private router: Router
    ) {
        this.icao = this.sharedService.currentUser.icao;
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().add(7, 'd').format('MM/DD/YYYY'));
        this.fbosService.get({oid: this.sharedService.currentUser.fboId}).subscribe(
            (data: any) => {
                this.fbo = data.fbo;
            }
        );
    }

    ngOnInit() {
        this.refreshData();
    }

    ngAfterViewInit() {
        this.icaoChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvent.icaoChangedEvent) {
                    this.icao = this.sharedService.currentUser.icao;
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.icaoChangedSubscription) {
            this.icaoChangedSubscription.unsubscribe();
        }
    }

    refreshData() {
        this.ngxLoader.startLoader(this.chartName);
        this.fuelreqsService
            .getCompaniesQuotingDealStatistics(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate
            )
            .subscribe((data: any) => {
                this.dataSource = new MatTableDataSource(data);
                 this.dataSource.sortingDataAccessor = (item, property) => {
                    switch (property) {
                        case 'lastPullDate':
                            if (item[property] === 'N/A') {
                                if (this.sort.direction === 'asc') {
                                    return new Date(8640000000000000);
                                } else {
                                    return new Date(-8640000000000000);
                                }
                            }
                            return new Date(item[property]);
                        default:
                            return item[property];
                    }
                };
                this.dataSource.sort = this.sort;
            }, () => {
            }, () => {
                this.ngxLoader.stopLoader(this.chartName);
            });
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }


    CustomerAnalitycs(element: any )
    {

       this.router.navigate(['/default-layout/customers' , element.oid ], {state : {tab : 3  , customerId : element.customerId}  });
    }
}
