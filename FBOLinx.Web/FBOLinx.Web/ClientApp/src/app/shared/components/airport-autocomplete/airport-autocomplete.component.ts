import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { MatLegacyAutocompleteSelectedEvent as MatAutocompleteSelectedEvent } from '@angular/material/legacy-autocomplete';
import { Observable, of } from 'rxjs';
import {
    catchError,
    debounceTime,
    map,
    startWith,
    switchMap,
} from 'rxjs/operators';

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
    styleUrls: ['./airport-autocomplete.component.scss'],
    templateUrl: './airport-autocomplete.component.html',
})
export class AirportAutocompleteComponent implements OnInit {
    @Input() airportContainerModel: any;
    @Output() valueChange = new EventEmitter();

    searchControl: UntypedFormControl = new UntypedFormControl();
    filteredAirports: Observable<AirportAutoCompleteDataSource>;
    airports: AirportAutoCompleteData[] = null;
    isLoading = false;
    airportModel = null;

    constructor(private acukwikAirportsService: AcukwikairportsService) {}

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
            map((results: AirportAutoCompleteDataSource) => results),
            // catch errors
            catchError((_) => of(null))
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
