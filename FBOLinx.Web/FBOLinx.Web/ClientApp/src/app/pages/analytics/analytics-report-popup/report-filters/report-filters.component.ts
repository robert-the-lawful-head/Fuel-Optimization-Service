import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';

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
        let filter: SelectedDateFilter = {
            selectedFilter: null,
            offsetDate: this.filterStartDate,
            limitDate: this.filterEndDate,
        };
        this.onDateChange.emit(filter);
    }
    isFilterHidden(filter: ReportFilterItems) {
        console.log("🚀 ~ ReportFiltersComponent ~ isFilterHidden ~ filter:", filter)
        console.log("🚀 ~ ReportFiltersComponent ~ isFilterHidden ~ this.hiddenFilters:", this.hiddenFilters)

        return this.hiddenFilters.includes(filter);
    }
    private setCustomIcaoList(customIcaoList: CustomIcaoList) {
        if(customIcaoList.isStandAlone) {
            this.airportsICAO = customIcaoList.value;
        }else{
            this.airportsICAO = this.airportsICAO.concat(customIcaoList.value);
        }
    }
}
