import { COMMA, ENTER } from '@angular/cdk/keycodes';
import {
    Component,
    ElementRef,
    forwardRef,
    Input,
    ViewChild,
} from '@angular/core';
import { FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

export interface MultiSelectElement {
    label: string;
    value: any;
}
@Component({
    selector: 'app-multiselect-autocomplete',
    templateUrl: './multiselect-autocomplete.component.html',
    styleUrls: ['./multiselect-autocomplete.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => MultiselectAutocompleteComponent),
            multi: true,
        },
    ],
})
export class MultiselectAutocompleteComponent {
    @Input() label: string = 'Select filters';
    @Input() selectable: boolean = true;
    @Input() removable: boolean = true;
    @Input() selectedOptions: MultiSelectElement[] = [];
    @Input() dataSource: MultiSelectElement[] = [];
    @Input() enableAddNew: boolean = false;

    @ViewChild('optionInput') optionInput: ElementRef<HTMLInputElement>;

    // ControlValueAccessor methods
    private onChange: (value: any) => void = () => {};
    private onTouched: () => void = () => {};

    separatorKeysCodes: number[] = [ENTER, COMMA];
    optionCtrl = new FormControl();
    filteredOptions: Observable<MultiSelectElement[]>;
    inputPlaceholder = this.enableAddNew
        ? 'Add new Option...'
        : 'Search for an option...';

    constructor(private elementRef: ElementRef) {
        this.filteredOptions = this.optionCtrl.valueChanges.pipe(
            startWith(null),
            map((option: string | null) =>
                option ? this._filter(option) : this.dataSource.slice()
            )
        );
    }
    add(event: MatChipInputEvent): void {
        if (!this.enableAddNew) return;

        const inputValue = (event.value || '').trim();

        // Find the item in allFruits by label
        const item = this.dataSource.find(
            (option) => option.label.toLowerCase() === inputValue.toLowerCase()
        );

        if (item) {
            this.selectedOptions.push(item);
            this.onChange(this.selectedOptions.map((x) => x.value)); // Notify Angular Forms
        }

        // Clear the input value
        event.chipInput!.clear();

        this.optionCtrl.setValue(null);
    }

    remove(fruit: MultiSelectElement): void {
        const index = this.selectedOptions.indexOf(fruit);

        if (index >= 0) {
            this.selectedOptions.splice(index, 1);
            this.onChange(this.selectedOptions.map((x) => x.value)); // Notify Angular Forms
        }
    }

    selected(event: any): void {
        const item = this.dataSource.find(
            (option) => option.label === event.option.viewValue
        );

        if (item && !this.selectedOptions.includes(item)) {
            this.selectedOptions.push(item);
            this.onChange(this.selectedOptions.map((x) => x.value)); // Notify Angular Forms
        }

        this.optionCtrl.setValue(null);

        this.blurChipInput();
    }
    blurChipInput(): void {
        const inputElement =
            this.elementRef.nativeElement.querySelector('input');
        inputElement.blur();
    }

    // Angular Forms ControlValueAccessor methods
    writeValue(value: any): void {
        if (value) {
            this.selectedOptions = value;
        }
    }
    registerOnChange(fn: any): void {
        this.onChange = fn;
    }
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    private _filter(value: string): MultiSelectElement[] {
        const filterValue = value.toLowerCase();

        return this.dataSource.filter((option) =>
            option.label.toLowerCase().includes(filterValue)
        );
    }
}
