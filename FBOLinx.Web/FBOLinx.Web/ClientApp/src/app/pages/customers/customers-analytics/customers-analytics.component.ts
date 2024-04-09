import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { SharedService } from 'src/app/layouts/shared-service';
import { FbosService } from 'src/app/services/fbos.service';
import * as SharedEvent from '../../../constants/sharedEvents';
import { FuelreqsService } from 'src/app/services/fuelreqs.service';

@Component({
  selector: 'app-customers-analytics',
  templateUrl: './customers-analytics.component.html',
  styleUrls: ['./customers-analytics.component.scss']
})
export class CustomersAnalyticsComponent implements OnInit , AfterViewInit, OnDestroy  {
    @ViewChild(MatSort) sort: MatSort;
    @Input() customerId;
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
        console.log(this.customerId)
       this.fuelreqsService
            .getCompanyQuotingDealStatistics(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.filterStartDate,
                this.filterEndDate,
                this.customerId
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

}
