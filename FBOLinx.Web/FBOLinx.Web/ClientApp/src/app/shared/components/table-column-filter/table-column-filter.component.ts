import {
    animate,
    state,
    style,
    transition,
    trigger,
} from '@angular/animations';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import * as moment from 'moment';
import { EnumOptions } from "../../../models/enum-options";
import { StringFilterConditions } from "../../../enums/string-filter-conditions";

@Component({
    animations: [
        trigger('openClose', [
            // ...
            state(
                'open',
                style({
                    height: '*',
                    opacity: 1,
                })
            ),
            state(
                'closed',
                style({
                    height: '0',
                    opacity: 0,
                })
            ),
            transition('open => closed', [animate('.3s')]),
            transition('closed => open', [animate('0.3s')]),
        ]),
    ],
    selector: 'app-table-column-filter',
    styleUrls: ['table-column-filter.component.scss'],
    templateUrl: 'table-column-filter.component.html',
})
export class TableColumnFilterComponent implements OnInit {
    @Input() columnId: string;
    @Input() column: any;
    @Input() columnFormat = 0; // Text = 0,Number = 1,Date = 2,Time = 3,DateTime = 4,Currency = 5,Weight = 6,TimeStandard = 7, YesNo = 8, EmptyNotEmpty = 9, Multiselect = 10, Many-to-Many Multiselect = 11,
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
            endDate: null,
            startDate: null,
        },
        isFiltered: false,
        numberRangeFilter: {
            end: null,
            start: null,
        },
        optionsFilter: [],
        stringFilter: '',
        filterStringCondition: StringFilterConditions.Contains
    };
    public isHeadingOpen = false;
    public isFilterOpen = false;
    public stringFilterConditionOptions: Array<EnumOptions.EnumOption> = EnumOptions.stringFilterConditionOptions;

    private page;
    private existingFilterPassed = false;

    constructor() {}

    ngOnInit(): void {

        if (!this.column) {
            this.column = {
                propertyName: this.propertyName,
            };
        }
        if (!this.column.propertyName) {
            if (!this.propertyName) {
                throw new Error(
                    'A propertyName must be provided for the column value in the data collection.'
                );
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

        if (this.matDataSource.data.length > 0 && this.matDataSource.data[0].hasOwnProperty('fuelerLinxId'))
            this.page = "customer-manager-filters";

        var existingFilters = localStorage.getItem(this.page);
        if (existingFilters != null) {
            var filters = JSON.parse(existingFilters);
            filters.forEach((filter) => {
                if (filter.propertyName == this.column.propertyName) {
                    this.existingFilterPassed = true;
                    this.applyFilter(filter.filter, null);
                }
            });
        }
    }

    public orderData(id: string, start?: 'asc' | 'desc'): void {
        const matSort = this.matDataSource.sort;
        const disableClear = false;

        matSort.sort({ disableClear, id: null, start });
        matSort.sort({ disableClear, id, start });

        this.matDataSource.sort = this.matSort;
    }

    public editHeadingFinished(column: any): void {
        column.isEditingHeader = false;
        this.columnChanged.emit(column);
    }

    public applyFilter(filterValue: any, menuTrigger: any) {
        this.filter.isFiltered = filterValue != null;

        this.column.filter = filterValue;

        if (this.matDataSource.filter && this.matDataSource.filter !== '') {
            this.tableFilter = JSON.parse(this.matDataSource.filter);
        } else {
            this.tableFilter = [];
        }

        const existingFilter = this.tableFilter.filter(
            (column) => column.propertyName === this.column.propertyName
        );

        var existingFilterIndex = -1;

        if (existingFilter.length === 0) {
            this.tableFilter.push(this.column);
            existingFilter.push(this.column);
        } else {
            existingFilterIndex = this.tableFilter.findIndex(column => {
                return column.propertyName === existingFilter[0].propertyName
            });

            existingFilter[0].filter = this.column.filter;
        }

        if (!filterValue || filterValue === '') {
            this.tableFilter.splice(existingFilterIndex, 1);
        }

        this.matDataSource.filter = JSON.stringify(this.tableFilter);
        this.filterApplied.emit(this.column);
        if (menuTrigger) {
            menuTrigger.closeMenu();
        }

        if ((!this.existingFilterPassed && filterValue != null) || filterValue == null) {
            localStorage.setItem(
                this.page,
                this.matDataSource.filter
            );
            this.existingFilterPassed = false;
        }
    }

    public clearFilter(): void {
        this.filter.dateFilter = {
            endDate: null,
            startDate: null,
        };
        this.filter.stringFilter = '';
        this.filter.numberRangeFilter = {
            end: null,
            start: null,
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
                            if (data.hasOwnProperty('fuelVendors')) {
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
                            //    serializedData.indexOf(element.filterValue) > -1
                            //);
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
                            if (element.filter.filterStringCondition == StringFilterConditions.Contains) {
                                if (
                                    propertyValue
                                        .toString()
                                        .toLowerCase()
                                        .indexOf(element.filterValue) >
                                        -1
                                ) {
                                    return true;
                                }
                            } else if (element.filter.filterStringCondition == StringFilterConditions.DoesNotContain) {
                                if (
                                    propertyValue
                                        .toString()
                                        .toLowerCase()
                                        .indexOf(element.filterValue) === -1
                                ) {
                                    return true;
                                }
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
                        return (
                            (element.filter.stringFilter === 'true' &&
                                !columnValue) ||
                            (element.filter.stringFilter === 'false' &&
                                !!columnValue)
                        );
                    }

                    if ([10].indexOf(element.columnFormat) > -1) {
                        return (
                            !element.filter.optionsFilter.length ||
                            element.filter.optionsFilter.includes(columnValue)
                        );
                    }

                     if ([11].indexOf(element.columnFormat) > -1) {
                        return (
                            !element.filter.optionsFilter.length
                                ||
                            element.filter.optionsFilter.some((v) =>
                                columnValue.find((c) => ((c.value === v || c.name === v) && element.filter.filterStringCondition == StringFilterConditions.Contains)))
                            ||
                            (element.filter.filterStringCondition == StringFilterConditions.DoesNotContain && element.filter.optionsFilter.every((v) =>
                                    !(columnValue.find((c) => ((c.value === v || c.name === v))))))

                        );
                    }


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
                    if (element.filter.filterStringCondition == StringFilterConditions.Contains) {
                        return (
                            columnValue
                                .toLowerCase()
                                .indexOf(element.filter.stringFilter
                                    .toLowerCase()) >
                                -1
                        );
                    } else if (element.filter.filterStringCondition == StringFilterConditions.DoesNotContain) {
                        return (
                            columnValue
                                .toLowerCase()
                                .indexOf(element.filter.stringFilter
                                    .toLowerCase()) ===
                                -1
                        );
                    }
                    return true;
                }
            };

            const trueFilters = parsedFilters.filter(filterCallback);

            return trueFilters.length === parsedFilters.length;
        };
    }
}
