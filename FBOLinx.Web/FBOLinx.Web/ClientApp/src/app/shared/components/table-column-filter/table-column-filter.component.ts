import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { animate, state, style, transition, trigger } from '@angular/animations';
import * as moment from 'moment';

@Component({
    selector: 'app-table-column-filter',
    templateUrl: 'table-column-filter.component.html',
    styleUrls: [ 'table-column-filter.component.scss' ],
    animations: [
        trigger('openClose', [
            // ...
            state(
                'open',
                style({
                    height: '*',
                    opacity: 1
                })
            ),
            state(
                'closed',
                style({
                    height: '0',
                    opacity: 0
                })
            ),
            transition('open => closed', [ animate('.3s') ]),
            transition('closed => open', [ animate('0.3s') ])
        ])
    ]
})
export class TableColumnFilterComponent implements OnInit {
    @Input() columnId: string;
    @Input() column: any;
    @Input() columnFormat = 0; // Text = 0,Number = 1,Date = 2,Time = 3,DateTime = 4,Currency = 5,Weight = 6,TimeStandard = 7, YesNo = 8, EmptyNotEmpty = 9, Multiselect = 10
    @Input() propertyName: string;
    @Input() matDataSource: any = null;
    @Input() matSort: MatSort;
    @Input() tableFilter: any;
    @Input() allowEditHeading = false;
    @Input() options: any[] = [];
    @Input() optionLabel: string;
    @Input() optionValue: string;
    @Output() columnChanged: EventEmitter<any> = new EventEmitter<any>();
    @Output() filterApplied: EventEmitter<any> = new EventEmitter<any>();

    public filter: any = {
        dateFilter: {
            startDate: null,
            endDate: null
        },
        stringFilter: '',
        numberRangeFilter: {
            start: null,
            end: null
        },
        optionsFilter: [],
        isFiltered: false
    };
    public isHeadingOpen = false;
    public isFilterOpen = false;

    constructor() {
    }

    ngOnInit(): void {
        if (!this.column) {
            this.column = {
                propertyName: this.propertyName
            };
        }
        if (!this.column.propertyName) {
            if (!this.propertyName) {
                throw new Error('A propertyName must be provided for the column value in the data collection.');
            } else {
                this.column.propertyName = this.propertyName;
            }
        }
        if (!this.column.columnFormat) {
            this.column.columnFormat = this.columnFormat;
        }

        if (!this.matDataSource.filterCollection) {
            this.matDataSource.filterCollection = [];
        }
        if (this.matDataSource.filterCollection.indexOf(this.filter) === -1) {
            this.matDataSource.filterCollection.push(this.filter);
        }

        this.setupFilterPredicate();
    }

    public orderData(id: string, start?: 'asc' | 'desc'): void {
        const matSort = this.matDataSource.sort;
        const disableClear = false;

        matSort.sort({ id: null, start, disableClear });
        matSort.sort({ id, start, disableClear });

        this.matDataSource.sort = this.matSort;
    }

    public editHeadingFinished(column: any): void {
        column.isEditingHeader = false;
        this.columnChanged.emit(column);
    }

    public applyFilter(filterValue: any, menuTrigger: any) {
        this.filter.isFiltered = (filterValue != null);

        this.column.filter = filterValue;

        if (this.matDataSource.filter && this.matDataSource.filter !== '') {
            this.tableFilter = JSON.parse(this.matDataSource.filter);
        } else {
            this.tableFilter = [];
        }

        const existingFilter = this.tableFilter.filter(column => column.propertyName === this.column.propertyName);
        if (existingFilter.length === 0) {
            this.tableFilter.push(this.column);
            existingFilter.push(this.column);
        } else {
            existingFilter[0].filter = this.column.filter;
        }

        if (!filterValue || filterValue === '') {
            this.tableFilter.splice(existingFilter[0], 1);
        }

        this.matDataSource.filter = JSON.stringify(this.tableFilter);
        this.filterApplied.emit(this.column);
        if (menuTrigger) {
            menuTrigger.closeMenu();
        }
    }

    public clearFilter(): void {

        this.filter.dateFilter = {
            startDate: null,
            endDate: null
        };
        this.filter.stringFilter = '';
        this.filter.numberRangeFilter = {
            start: null,
            end: null
        };
        this.filter.optionsFilter = [];

        this.applyFilter(null, null);
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
                            const serializedData = JSON.stringify(data).toLowerCase();
                            return (serializedData.indexOf(element.filterValue) > -1);
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

                    if ([9].indexOf(element.columnFormat) > -1) {
                        return (element.filter.stringFilter === 'true' && !columnValue) ||
                            (element.filter.stringFilter === 'false' && !!columnValue);
                    }

                    if ([10].indexOf(element.columnFormat) > -1) {
                        return !element.filter.optionsFilter.length || element.filter.optionsFilter.includes(columnValue);
                    }

                    if (columnValue === null || columnValue === '') {
                        return false;
                    }

                    if (element.columnFormat === 6) {
                        columnValue = columnValue.amount;
                    }
                    columnValue = columnValue.toString();

                    if ([ 2, 3, 4 ].indexOf(element.columnFormat) > -1) {
                        return (
                            (!element.filter.dateFilter.startDate ||
                                element.filter.dateFilter.startDate === '' ||
                                moment(columnValue).isAfter(
                                    element.filter.dateFilter.startDate
                                )) &&
                            (!element.filter.dateFilter.endDate ||
                                element.filter.dateFilter.endDate === '' ||
                                moment(columnValue).isBefore(element.filter.dateFilter.endDate))
                        );
                    }
                    if ([ 1, 5, 6 ].indexOf(element.columnFormat) > -1) {
                        return (
                            (!element.filter.numberRangeFilter.start ||
                                element.filter.numberRangeFilter.start <=
                                parseFloat(columnValue)) &&
                            (!element.filter.numberRangeFilter.end ||
                                element.filter.numberRangeFilter.end >= parseFloat(columnValue))
                        );
                    }
                    return (
                        columnValue
                            .toLowerCase()
                            .indexOf(element.filter.stringFilter.toString().toLowerCase()) >
                        -1
                    );
                }
            };

            const trueFilters = parsedFilters.filter(filterCallback);

            return trueFilters.length === parsedFilters.length;
        };
    }
}
