import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as moment from 'moment';

@Component({
    selector: 'app-table-global-search',
    styleUrls: ['table-global-search.component.scss'],
    templateUrl: 'table-global-search.component.html',
})
export class TableGlobalSearchComponent implements OnInit {
    @Input() placeholder: string;
    @Input() matDataSource: any = null;
    @Input() SubmatDataSource : any = null
    @Output() filterApplied: EventEmitter<any> = new EventEmitter<any>();

    public globalFilter: any = { filterValue: '', isGlobal: true };
    private page: string = "";

    constructor() {}

    ngOnInit(): void {
        this.setupFilterPredicate();
        if (!this.matDataSource.filterCollection) {
            this.matDataSource.filterCollection = [];
        }
        let hasGlobal = false;

        if (this.matDataSource.data[0].hasOwnProperty('fuelerLinxId'))
            this.page = "customer-manager-filters";
        this.matDataSource.filter = localStorage.getItem(this.page);

        for (const filter of JSON.parse(this.matDataSource.filter)) {
            if (filter.isGlobal) {
                hasGlobal = true;
                this.globalFilter = filter;
                break;
            }
        }
        if (!hasGlobal) {
            this.matDataSource.filterCollection.push(this.globalFilter);
        }

        this.applyFilter(this.globalFilter.filterValue);
    }

    public applyFilter(filterValue: any) {
        //localStorage.setItem(
        //    this.tableLocalStorageKey,
        //    filterValue
        //);

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
    }

    public clearAllFilters() {
        this.globalFilter.filterValue = '';
        this.matDataSource.filter = '';

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
                            const serializedData =
                                JSON.stringify(data).toLowerCase();
                            return (
                                serializedData.indexOf(element.filterValue) > -1
                            );
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
