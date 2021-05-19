import { AfterViewInit, Component, Input, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import * as moment from 'moment';

// Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvent from '../../../models/sharedEvents';
import { MatSort } from '@angular/material/sort';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-group-analytics-customer-statistics',
    templateUrl: './group-analytics-customer-statistics.component.html',
    styleUrls: ['./group-analytics-customer-statistics.component.scss'],
})
export class GroupAnalyticsCustomerStatisticsComponent implements AfterViewInit, OnDestroy {
    @ViewChild(MatSort) sort: MatSort;
    @Input() fbos: any[];

    filterStartDate: Date;
    filterEndDate: Date;
    icao: string;
    fbo: string;
    icaoChangedSubscription: any;
    chartName = 'group-analytics-customer-statistics';
    displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'lastPullDate'];
    dataSource: MatTableDataSource<any[]>;
    filtersChanged: Subject<any> = new Subject<any>();

    selectedFbos: any[];

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
    ) {
        this.icao = this.sharedService.currentUser.icao;
        this.filterStartDate = new Date(moment().add(-12, 'M').format('MM/DD/YYYY'));
        this.filterEndDate = new Date(moment().add(7, 'd').format('MM/DD/YYYY'));

        this.filtersChanged
            .debounceTime(1000)
            .subscribe(() => this.refreshData());
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
            .getCompaniesQuotingDealStatisticsForGroupFbos(
                this.sharedService.currentUser.groupId,
                this.selectedFbos.map(fbo => fbo.oid),
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

    filterChanged() {
        this.filtersChanged.next();
    }
}
