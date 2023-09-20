import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';

@Component({
    selector: 'app-table-global-search',
    styleUrls: ['table-global-search.component.scss'],
    templateUrl: 'table-global-search.component.html',
})
export class TableGlobalSearchComponent implements OnInit {
    @Input() placeholder: string;
    @Input() matDataSource: any = null;
    @Input() SubmatDataSource: any = null
    @Input() showClearButton: boolean = true;
    @Output() filterApplied: EventEmitter<any> = new EventEmitter<any>();
    @Output() filteredDataSource: EventEmitter<any> = new EventEmitter<any>();

    public globalFilter: any = { filterValue: '', isGlobal: true };
    public userTypedFilter: string = '';

    private page: string = "";
    private idParam: string = "";

    constructor(
        private route: ActivatedRoute,
    ) { }

    ngOnInit(): void {
        //if (!this.column) {
        //    this.column = {
        //        propertyName: this.propertyName,
        //    };
        //}
        //if (!this.column.propertyName) {
        //    if (!this.propertyName) {
        //        throw new Error(
        //            'A propertyName must be provided for the column value in the data collection.'
        //        );
        //    } else {
        //        this.column.propertyName = this.propertyName;
        //    }
        //}
        //if (!this.column.columnFormat) {
        //    this.column.columnFormat = this.columnFormat;
        //}

        //if (!this.matDataSource.filterCollection) {
        //    this.matDataSource.filterCollection = [];
        //}
        //if (this.matDataSource.filterCollection.indexOf(this.filter) === -1) {
        //    this.matDataSource.filterCollection.push(this.filter);
        //}

        if (!this.matDataSource)
            return;

        this.setupFilterPredicate();
        if (!this.matDataSource.filterCollection) {
            this.matDataSource.filterCollection = [];
        }
        let hasGlobal = false;

        if (this.matDataSource.data.length > 0 && this.matDataSource.data[0].hasOwnProperty('fuelerLinxId'))
            this.page = "customer-manager-filters";
        else
            this.page = "fuel-orders-filters";

        this.route.queryParamMap.subscribe((params) => {
            this.idParam = params.get('id');
        });

        if (this.page == "fuel-orders-filters" && this.idParam != null)
            this.matDataSource.filter = "[{ \"filterValue\":\"" + this.idParam + "\", \"isGlobal\": true }]";
        else
            this.matDataSource.filter = localStorage.getItem(this.page);

        if (this.matDataSource.filter != null) {
            for (const filter of JSON.parse(this.matDataSource.filter)) {
                if (filter.isGlobal) {
                    hasGlobal = true;
                    this.globalFilter = filter;
                    this.userTypedFilter = filter.filterValue;
                    break;
                }
            }
        }

        if (!hasGlobal) {
            this.matDataSource.filterCollection.push(this.globalFilter);
        }

        this.applyFilter(this.userTypedFilter);
    }

    public applyFilter(filterValue: any) {
        if (!this.matDataSource) {
            this.filterApplied.emit(filterValue);
            return;
        }

        let existingFilters: any[];
        if (!this.matDataSource.filter) {
            existingFilters = [];
        } else {
            existingFilters = JSON.parse(this.matDataSource.filter);

        }

        let hasGlobal = false;

        for (const filter of existingFilters) {

            if (filter.isGlobal) {
                hasGlobal = true;
                this.globalFilter = filter;
                break;
            }
        }

        if (!hasGlobal) {
            existingFilters.push(this.globalFilter);
        }

        this.globalFilter.filterValue = filterValue.toLowerCase();

        if (
            !this.globalFilter.filterValue ||
            this.globalFilter.filterValue === ''
        ) {
            existingFilters.splice(
                existingFilters.indexOf(this.globalFilter),
                1
            );
        }

        this.matDataSource.filter = JSON.stringify(existingFilters);

        localStorage.setItem(
            this.page,
            this.matDataSource.filter
        );

        this.filterApplied.emit(filterValue);

        this.filteredDataSource.emit(this.matDataSource);
    }

    public clearAllFilters() {
        this.globalFilter.filterValue = '';
        this.userTypedFilter = '';
        this.matDataSource.filter = '';
        if (!this.matDataSource.filterCollection) {
            this.matDataSource.filterCollection = [];
        }

        for (const filter of this.matDataSource.filterCollection) {
            if (filter.isGlobal) {
                continue;
            }

            filter.dateFilter = {
                endDate: null,
                startDate: null,
            };
            filter.stringFilter = '';
            filter.numberRangeFilter = {
                end: null,
                start: null,
            };
            filter.isFiltered = false;
        }

        if (this.page != "") {
            localStorage.removeItem(this.page);
        }
    }

    // PRIVATE METHODS
    private setupFilterPredicate() {
        this.matDataSource.filterPredicate = (data: any, filters: string) => {
            const parsedFilters: any[] = JSON.parse(filters);

            if (parsedFilters === null || parsedFilters.length === 0) {
                return true;
            }

            const filterCallback = (element, index, array): boolean => {
                if (element.isGlobal) {
                    if (!element.columns) {
                        if (!data) {
                            return true;
                        } else {
                            if (data.hasOwnProperty('fboName')) {
                                let tempDataString = JSON.stringify(data)
                                let tempData = JSON.parse(tempDataString);
                                delete tempData.icao;
                                delete tempData.timeStandard;
                                delete tempData.fbo;
                                delete tempData.fuelOn;
                                delete tempData.source;
                                delete tempData.fboName;

                                return (Object.values(tempData).toString().toLowerCase().indexOf(element.filterValue) > -1);
                            }
                            else if (data.hasOwnProperty('fuelVendors')) {
                                let tempDataString = JSON.stringify(data)
                                let tempData = JSON.parse(tempDataString);
                                delete tempData.fuelVendors;

                                return (Object.values(tempData).toString().toLowerCase().indexOf(element.filterValue) > -1);
                            }
                            else
                                return (Object.values(data).toString().toLowerCase().indexOf(element.filterValue) > -1);

                            //const serializedData =
                            //    JSON.stringify(data).toLowerCase();
                            //return (
                            //    serializedData.trim().indexOf(element.filterValue.trim()) > -1
                            //);

                            //Object.keys(data).some((k) => {
                            //    return data[k] == null ? false : data[k].toString().indexOf(element.filterValue) > -1;
                            //});

                            //return false;
                            //for (let key in data) {
                            //    if (data[key] != null) {
                            //        var value = data[key].toString();
                            //        if (value.indexOf(element.filterValue) > -1) {
                            //            return true;
                            //        }
                            //        else return false;
                            //    }
                            //}

                            //return false;

                            //return (
                            //    (Object.values(data).indexOf(element.filterValue) > -1));
                            //return (
                            //Object.values(data).includes(element.filterValue));
                        }
                    }
                    for (const column of element.columns) {
                        if (!column.isVisible) {
                            continue;
                        }
                        const propertyValue: any = data[column.propertyName];
                        if (!propertyValue) {
                            continue;
                        }
                        if (
                            typeof propertyValue === 'number' ||
                            typeof propertyValue === 'string'
                        ) {
                            if (
                                propertyValue
                                    .toString()
                                    .toLowerCase()
                                    .indexOf(element.filterValue) > -1
                            ) {
                                return true;
                            }
                        } else if (propertyValue.amount) {
                            if (
                                propertyValue.amount
                                    .toString()
                                    .toLowerCase()
                                    .indexOf(element.filterValue) > -1
                            ) {
                                return true;
                            }
                        }
                    }
                    return false;
                } else {
                    if (!element.filter) {
                        return true;
                    }

                    let columnValue = data[element.propertyName];

                    if (columnValue === null || columnValue === '') {
                        return false;
                    }

                    if (element.columnFormat === 6) {
                        columnValue = columnValue.amount;
                    }
                    columnValue = columnValue.toString();

                    if ([2, 3, 4].indexOf(element.columnFormat) > -1) {
                        return (
                            (!element.filter.dateFilter.startDate ||
                                element.filter.dateFilter.startDate === '' ||
                                moment(columnValue).isAfter(
                                    element.filter.dateFilter.startDate
                                )) &&
                            (!element.filter.dateFilter.endDate ||
                                element.filter.dateFilter.endDate === '' ||
                                moment(columnValue).isBefore(
                                    element.filter.dateFilter.endDate
                                ))
                        );
                    }
                    if ([1, 5, 6].indexOf(element.columnFormat) > -1) {
                        return (
                            (!element.filter.numberRangeFilter.start ||
                                element.filter.numberRangeFilter.start <=
                                    parseFloat(columnValue)) &&
                            (!element.filter.numberRangeFilter.end ||
                                element.filter.numberRangeFilter.end >=
                                    parseFloat(columnValue))
                        );
                    }
                    return (
                        columnValue
                            .toLowerCase()
                            .indexOf(
                                element.filter.stringFilter
                                    .toString()
                                    .toLowerCase()
                            ) > -1
                    );
                }
            };

            const trueFilters = parsedFilters.filter(filterCallback);

            return trueFilters.length === parsedFilters.length;
        };
    }
}
