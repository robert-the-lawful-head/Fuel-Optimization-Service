import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
} from '@angular/core';

export enum PresetDateFilterEnum {
    sevenDays = '7D',
    oneMonth = '1M',
    sixMonths = '6M',
    currentYearToDate = 'YTD',
    oneYear = '1Y',
}
export interface SelectedDateFilter {
    selectedFilter: PresetDateFilterEnum | null;
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
    @Output() selectedFilterChange = new EventEmitter<SelectedDateFilter>();

    public selectedFilter: PresetDateFilterEnum;
    dateFilterValues = Object.values(PresetDateFilterEnum) as string[];

    currentDate: Date = new Date();
    startDate: Date = new Date();

    constructor() {}

    // ngOnChanges(changes: SimpleChanges): void {
    //     if (changes.defaultFilter) {
    //         console.log("🚀 ~ PresetDateFilterComponent ~ ngOnChanges ~ changes.defaultFilter:", changes.defaultFilter)
    //         this.defaultFilter = changes.defaultFilter.currentValue;
    //     }
    // }
    ngOnInit() {
        this.selectedFilter = this.defaultFilter;
    }

    onToggleChange(event: any) {
        let filter = this.getPresetDatesFilters(event.value);
        this.selectedFilterChange.emit(filter);
    }
    private getPresetDatesFilters(value: PresetDateFilterEnum): SelectedDateFilter {
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
