import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { PresetDateFilterEnum, SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';
import * as moment from 'moment';
export interface CustomIcaoList{
    value: string[];
    isStandAlone: boolean;
}
export enum ReportFilterItems{
    icaoDropDown,
    presetdateFilter,
    searchInput
}
@Component({
    selector: 'app-report-filters',
    templateUrl: './report-filters.component.html',
    styleUrls: ['./report-filters.component.scss'],
})
export class ReportFiltersComponent implements OnInit {
    @Input() icao: string = '';
    @Input() dataSource: MatTableDataSource<any> =  new MatTableDataSource([]);
    @Input() selectedDateFilter: SelectedDateFilter;
    @Input() hiddenFilters: ReportFilterItems[];
    @Input() customIcaoList: CustomIcaoList = null;

    @Output() onChangeIcaoFilter: EventEmitter<string> = new EventEmitter<string>();
    @Output() onDateChange: EventEmitter<SelectedDateFilter> = new EventEmitter<SelectedDateFilter>();

    airportsICAO: string[] = [];
    nearbyMiles: number = 150;
    filterStartDate: Date;
    filterEndDate: Date;
    reportFilterItems = ReportFilterItems;

    constructor(private acukwikairportsService: AcukwikairportsService) {}

    async ngOnInit() {
        this.airportsICAO = (
            await this.acukwikairportsService
                .getNearByAcukwikAirportsByICAO(this.icao, this.nearbyMiles)
                .toPromise()
        ).map((data) => {
            return data.icao;
        });

        if(this.customIcaoList) {
            this.setCustomIcaoList(this.customIcaoList);
        }
    }
    ngOnChanges(changes: SimpleChanges): void {
        if(changes.customIcaoList && changes.customIcaoList.currentValue) {
            this.setCustomIcaoList(changes.customIcaoList.currentValue);
        }

        if(changes.SelectedDateFilter && changes.SelectedDateFilter.currentValue) {
            this.filterStartDate = changes.SelectedDateFilter.currentValue.offsetDate;
            this.filterEndDate = changes.SelectedDateFilter.currentValue.limitDate;
        }

    }
    changeIcaoFilter($event: string) {
        this.onChangeIcaoFilter.emit($event);
    }
    applyPresetDateFilter(filter: SelectedDateFilter) {
        this.selectedDateFilter = filter;
        this.onDateChange.emit(filter);
    }
    applyCustomDateFilter() {
        this.selectedDateFilter.selectedFilter = null;
        this.onDateChange.emit(this.selectedDateFilter);
    }
    isFilterHidden(filter: ReportFilterItems) {
        return this.hiddenFilters?.includes(filter);
    }
    private setCustomIcaoList(customIcaoList: CustomIcaoList) {
        if(customIcaoList.isStandAlone) {
            this.airportsICAO = customIcaoList.value;
        }else{
            this.airportsICAO = this.airportsICAO.concat(customIcaoList.value);
        }
    }
    clearFilters(){
        this.filterStartDate = new Date(
            moment().add(-1, 'M').format('MM/DD/YYYY')
        );
        this.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        this.selectedDateFilter = {
            selectedFilter: PresetDateFilterEnum.oneMonth,
            offsetDate: this.filterStartDate,
            limitDate: this.filterEndDate,
        }
        this.onDateChange.emit(this.selectedDateFilter);

    }
}
