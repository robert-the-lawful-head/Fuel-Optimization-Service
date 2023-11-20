import { Component, ElementRef, EventEmitter, Input, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-flight-watch-filters',
  templateUrl: './flight-watch-filters.component.html',
  styleUrls: ['./flight-watch-filters.component.scss'],
})

export class FlightWatchFiltersComponent implements OnInit {
    @Input() icao: string;
    @Input() icaoList: string[];
    @Input() isSearchAirportHidden: boolean = false;

    @Output() icaoChanged = new EventEmitter<string>();
    @Output() updateDrawerButtonPosition = new EventEmitter<any>();
    @Output() textFilterChanged = new EventEmitter<string>();

    @ViewChild('input') input: ElementRef<HTMLInputElement>;

    myControl = new FormControl({value: '', disabled: true});
    filteredOptions: string[];

    constructor() {
    }

    ngOnInit() {
    }
    ngOnChanges(changes: SimpleChanges): void {
        if(changes.icaoList?.currentValue){
            this.filteredOptions = changes.icaoList.currentValue.slice();
            this.myControl.enable();
        }
        if(changes.icao?.currentValue)
            this.myControl.patchValue(changes.icao.currentValue);
    }

    updateIcao(icao: string ): void {
        this.updateDrawerButtonPosition.emit();
        this.icaoChanged.emit(icao);
    }
    applyFilter(event: Event) {
        let filterText: string = (event.target as HTMLInputElement).value;
        this.textFilterChanged.emit(filterText);
    }
    filter(): void {
        const filterValue = this.myControl.value.toLowerCase();
        this.filteredOptions = this.icaoList.filter(o => o.toLowerCase().includes(filterValue));
    }
    validateTypedValue(): void {
        const filterValue = this.myControl.value.toUpperCase();
        if(!this.icaoList.includes(filterValue))
            this.myControl.patchValue(this.icao);
        else
            this.updateIcao(filterValue);
    }
    loadAllAirports(): void {
        this.filteredOptions = this.icaoList;
    }
    onOptionSelected(event: MatAutocompleteSelectedEvent): void {
        const selectedValue = event.option.viewValue;
        this.updateIcao(selectedValue);
      }
}
