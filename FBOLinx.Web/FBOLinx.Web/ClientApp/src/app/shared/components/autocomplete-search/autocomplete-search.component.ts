import {
    Component,
    EventEmitter,
    Input,
    OnChanges,
    Output,
    SimpleChanges,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { isEqual } from 'lodash';

export interface CloseConfirmationData {
    description: string;
    customTitle: string;
    customText: string;
    ok: string;
    cancel: string;
}

@Component({
    providers: [
        {
            multi: true,
            provide: NG_VALUE_ACCESSOR,
            useExisting: AutocompleteSearchComponent,
        },
    ],
    selector: 'app-autocomplete-search',
    styleUrls: ['./autocomplete-search.component.scss'],
    templateUrl: './autocomplete-search.component.html',
})
export class AutocompleteSearchComponent
    implements OnChanges, ControlValueAccessor
{
    @Input() label: string;
    @Input() optionValue: string | Array<string>;
    @Input() options: Array<any>;
    @Input() displayFn: (value: any) => any;
    @Input() disabled = false;
    @Input() required = false;
    @Output() selectionChanged = new EventEmitter();
    @Output() filterChanged = new EventEmitter();

    filter = '';
    filteredOptions: Array<any> = [];
    option: any;

    onChange = (val: any) => {};
    onTouched = () => {};

    constructor() {}

    writeValue(obj: any): void {
        this.option = obj;
    }
    registerOnChange(fn: any): void {
        this.onChange = fn;
    }
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }
    setDisabledState?(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (
            changes.options.currentValue &&
            !isEqual(
                changes.options.currentValue,
                changes.options.previousValue
            )
        ) {
            this.filteredOptions = [...this.options];
        }
    }

    onFilterChanged() {
        this.filteredOptions = this.options.filter((option) => {
            if (!this.filter) {
                return true;
            }
            if (typeof this.optionValue === 'string') {
                return option[this.optionValue as string]
                    .toLowerCase()
                    .includes(this.filter.toLowerCase());
            }
            if (!this.optionValue) {
                return option.toLowerCase().includes(this.filter.toLowerCase());
            }
            if (Array.isArray(this.optionValue)) {
                return this.optionValue
                    .map((ov) => option[ov])
                    .join(' ')
                    .toLowerCase()
                    .includes(this.filter.toLowerCase());
            }
            return false;
        });
        this.filterChanged.emit(this.filter);
    }

    optionSelected(event) {
        this.option = event.option.value;
        this.onChange(event.option.value);
        this.selectionChanged.emit(event.option.value);
    }

    renderOption(option) {
        if (typeof this.optionValue === 'string') {
            return option[this.optionValue as string];
        }
        if (!this.optionValue) {
            return option;
        }
        return this.optionValue.map((ov) => option[ov]).join(' ');
    }
}
