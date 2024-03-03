import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { AcukwikairportsService } from 'src/app/services/acukwikairports.service';
import { SelectedDateFilter } from 'src/app/shared/components/preset-date-filter/preset-date-filter.component';

@Component({
    selector: 'app-report-filters',
    templateUrl: './report-filters.component.html',
    styleUrls: ['./report-filters.component.scss'],
})
export class ReportFiltersComponent implements OnInit {
    @Input() icao: string = '';
    @Input() dataSource: MatTableDataSource<any> =  new MatTableDataSource([]);
    @Input() selectedDateFilter: SelectedDateFilter;

    @Output() onChangeIcaoFilter: EventEmitter<string> = new EventEmitter<string>();
    @Output() onDateChange: EventEmitter<SelectedDateFilter> = new EventEmitter<SelectedDateFilter>();

    airportsICAO: string[] = [];
    nearbyMiles: number = 150;
    filterStartDate: Date;
    filterEndDate: Date;

    constructor(private acukwikairportsService: AcukwikairportsService) {}

    async ngOnInit() {
        this.airportsICAO = (
            await this.acukwikairportsService
                .getNearByAcukwikAirportsByICAO(this.icao, this.nearbyMiles)
                .toPromise()
        ).map((data) => {
            return data.icao;
        });
    }
    ngOnChanges(changes: SimpleChanges): void {
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
}
