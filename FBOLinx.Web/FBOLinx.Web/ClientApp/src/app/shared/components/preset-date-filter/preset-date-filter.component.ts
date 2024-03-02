import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    SimpleChanges,
} from '@angular/core';

export enum PresetDateFilterEnum {
    sevenDays = '7D',
    oneMonth = '1M',
    sixMonths = '6M',
    oneYear = '1Y',
    currentYearToDate = 'YTD',
}
export interface SelectedPresetDateFilter {
    selectedFilter: PresetDateFilterEnum;
    offsetDate: Date;
    limitDate: Date;
}
@Component({
    selector: 'app-preset-date-filter',
    templateUrl: './preset-date-filter.component.html',
    styleUrls: ['./preset-date-filter.component.scss'],
})
export class PresetDateFilterComponent implements OnInit {
    @Input() defaultFilter: PresetDateFilterEnum;
    @Output() selectedFilterChange = new EventEmitter<SelectedPresetDateFilter>();

    public selectedFilter: PresetDateFilterEnum;
    dateFilterValues = Object.values(PresetDateFilterEnum) as string[];

    currentDate: Date = new Date();
    startDate: Date = new Date();

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
        let filter = this.getPresetDatesFilters(event.value);
        this.selectedFilterChange.emit(filter);
    }
    private getPresetDatesFilters(value: PresetDateFilterEnum): SelectedPresetDateFilter {
        this.startDate = new Date(this.currentDate);
        switch (value) {
            case PresetDateFilterEnum.sevenDays:
                this.calculateDateMinusDays(7);
                break;
            case PresetDateFilterEnum.oneMonth:
                this.calculateDateMinusMonth(1);
                break;
            case PresetDateFilterEnum.sixMonths:
                this.calculateDateMinusMonth(6);
                break;
            case PresetDateFilterEnum.oneYear:
                this.calculateDateMinusYears(1);
                break;
            case PresetDateFilterEnum.currentYearToDate:
                this.calculateFirstDayOfCurrentYear();
                break;
        }

        return {
            selectedFilter: value,
            offsetDate: this.startDate,
            limitDate: this.currentDate,
        }
    }
    private calculateDateMinusDays(days: number) {
        this.startDate.setDate(this.currentDate.getDate() - days);
    }

    private calculateDateMinusMonth(months: number) {
        this.startDate.setMonth(this.currentDate.getMonth() - months);
    }

    private calculateDateMinusYears(years: number) {
        this.startDate.setFullYear(this.currentDate.getFullYear() - years);
    }
    private calculateFirstDayOfCurrentYear() {
        this.startDate = new Date(this.currentDate.getFullYear(), 0, 1);
    }
}
