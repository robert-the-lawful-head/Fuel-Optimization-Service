import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';

export enum PresetDateFilterEnum {
    sevenDays = "7D",
    oneMonth = "1M",
    sixMonths = "6M",
    oneYear = "1Y",
    currentYearToDate = "YTD",
}
@Component({
    selector: 'app-preset-date-filter',
    templateUrl: './preset-date-filter.component.html',
    styleUrls: ['./preset-date-filter.component.scss'],
})
export class PresetDateFilterComponent implements OnInit {
    @Input() defaultFilter: PresetDateFilterEnum;
    @Output() selectedFilterChange = new EventEmitter<PresetDateFilterEnum>();

    public selectedFilter: PresetDateFilterEnum;
    dateFilterValues = Object.values(PresetDateFilterEnum) as string[];

    constructor() {}

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.defaultFilter) {
            this.selectedFilter = changes.defaultFilter.currentValue;
        }
    }
    ngOnInit() {
        this.selectedFilter = this.defaultFilter;
    }

    onToggleChange(event: any) {
        this.selectedFilterChange.emit(event.value);
    }
}
