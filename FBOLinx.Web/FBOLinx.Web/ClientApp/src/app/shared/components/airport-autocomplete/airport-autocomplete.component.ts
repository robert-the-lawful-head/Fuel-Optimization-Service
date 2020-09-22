import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { FormControl } from '@angular/forms';
import {
    startWith,
    map,
    debounceTime,
    switchMap,
    catchError,
} from 'rxjs/operators';
import { Observable } from 'rxjs';
import { of } from 'rxjs';

// Services
import { AcukwikairportsService } from '../../../services/acukwikairports.service';

export interface AirportAutoCompleteData {
    icao: string;
    iata: string;
    fullAirportName: string;
}

export declare type AirportAutoCompleteDataSource = AirportAutoCompleteData[];

@Component({
    selector: 'app-airport-autocomplete',
    templateUrl: './airport-autocomplete.component.html',
    styleUrls: ['./airport-autocomplete.component.scss'],
})
export class AirportAutocompleteComponent implements OnInit {
    @Input() airportContainerModel: any;
    @Output() valueChange = new EventEmitter();

    searchControl: FormControl = new FormControl();
    filteredAirports: Observable<AirportAutoCompleteDataSource>;
    airports: AirportAutoCompleteData[] = null;
    isLoading = false;
    airportModel = null;

    constructor(private acukwikAirportsService: AcukwikairportsService) {
    }

    ngOnInit() {
        this.searchControl.setValue(this.airportModel ? this.airportModel : '');
        this.filteredAirports = this.searchControl.valueChanges.pipe(
            startWith(''),
            // delay emits
            debounceTime(300),
            // use switch map so as to cancel previous subscribed events, before creating new once
            switchMap((value) => {
                if (value !== '') {
                    // lookup from github
                    return this.lookup(value);
                } else {
                    // if no value is pressent, return null
                    return of(null);
                }
            })
        );
    }

    lookup(value: any): Observable<AirportAutoCompleteDataSource> {
        if (typeof value === 'object') {
            return of(null);
        }
        return this.acukwikAirportsService.search(value).pipe(
            // map the item property of the github results as our return object
            map((results) => results),
            // catch errors
            catchError((_) => {
                return of(null);
            })
        );
    }

    public displayFn(airport?: AirportAutoCompleteData): string | undefined {
        return airport
            ? airport.icao + ' - ' + airport.fullAirportName
            : undefined;
    }

    public selected(airport: MatAutocompleteSelectedEvent) {
        this.airportModel = airport.option.value;
        // send to parent or do whatever you want to do
        this.valueChange.emit(airport.option.value);
    }
}
