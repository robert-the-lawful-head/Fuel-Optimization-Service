import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { isEqual } from 'lodash';

export interface CloseConfirmationData {
    description: string;
    customTitle: string;
    customText: string;
    ok: string;
    cancel: string;
}

@Component({
    selector: 'app-autocomplete-search',
    templateUrl: './autocomplete-search.component.html',
    styleUrls: [ './autocomplete-search.component.scss' ],
})
export class AutocompleteSearchComponent implements OnChanges {
    @Input() label: string;
    @Input() optionValue: string | Array<string>;
    @Input() options: Array<any>;
    @Input() displayFn: (value: any) => any;
    @Input() disabled = false;
    @Output() selectionChanged = new EventEmitter();

    filter = '';
    filteredOptions: Array<any> = [];

    constructor() {
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.options.currentValue && !isEqual(changes.options.currentValue, changes.options.previousValue)) {
            this.filteredOptions = [...this.options];
        }
    }

    onFilterChanged() {
        this.filteredOptions = this.options.filter(option => {
            if (!this.filter) {
                return true;
            }
            if (typeof this.optionValue === 'string') {
                return option[this.optionValue as string].toLowerCase().includes(this.filter.toLowerCase());
            }
            if (!this.optionValue) {
                return option.toLowerCase().includes(this.filter.toLowerCase());
            }
            if (Array.isArray(this.optionValue)) {
                return this.optionValue.map(ov => option[ov]).join(' ').toLowerCase().includes(this.filter.toLowerCase());
            }
            return false;
        });
    }

    optionSelected(event) {
        this.selectionChanged.emit(event.option.value);
    }

    renderOption(option) {
        if (typeof this.optionValue === 'string') {
            return option[this.optionValue as string];
        }
        if (!this.optionValue) {
            return option;
        }
        return this.optionValue.map(ov => option[ov]).join(' ');
    }
}
