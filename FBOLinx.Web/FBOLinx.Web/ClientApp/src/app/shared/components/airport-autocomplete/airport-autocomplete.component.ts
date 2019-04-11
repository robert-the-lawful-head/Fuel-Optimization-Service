import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import {
    startWith,
    map,
    debounceTime,
    mergeMapTo,
    mergeMap,
    switchMap,
    catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { of } from 'rxjs';

//Services
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
    styleUrls: ['./airport-autocomplete.component.scss']
})

/** airport-autocomplete component*/
export class AirportAutocompleteComponent implements OnInit {
    @Input() airportContainerModel: any;
    @Output() valueChange = new EventEmitter();

    searchControl: FormControl = new FormControl();
    filteredAirports: Observable<AirportAutoCompleteDataSource>;
    airports: AirportAutoCompleteData[] = null;
    isLoading = false;
    airportModel = null;

    constructor(private acukwikAirportsService: AcukwikairportsService) {
        //acukwikAirportsService.getAllAirports().subscribe((data: AirportAutoCompleteData[]) => {
        //    this.airports = data;
        //    this.registerFilter();
        //});

        //this.filteredAirports = this.searchControl.valueChanges.pipe(
        //    startWith<string | AirportAutoCompleteData>(''),
        //    map(val => {
        //        return this.filter(val || '');
        //    })
        //);
            //.startWith(null)
            //.debounceTime(200)
            //.distinctUntilChanged()
            //.switchMap(val => {
            //    return this.filter(val || '');
            //});
    }

    ngOnInit() {
        this.searchControl.setValue(this.airportModel ? this.airportModel : '');
        this.filteredAirports = this.searchControl.valueChanges.pipe(
            startWith(''),
            // delay emits
            debounceTime(300),
            // use switch map so as to cancel previous subscribed events, before creating new once
            switchMap(value => {
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

    lookup(value: string): Observable<AirportAutoCompleteDataSource> {
        return this.acukwikAirportsService.search(value).pipe(
            // map the item property of the github results as our return object
            map(results => results),
            // catch errors
            catchError(_ => {
                return of(null);
            })
        );
    }

    public displayFn(airport?: AirportAutoCompleteData): string | undefined {
        return airport ? airport.icao + ' - ' + airport.fullAirportName : undefined;
    }

    selected(airport) {
        console.log(airport);
        this.airportModel = airport;
        //send to parent or do whatever you want to do
        this.valueChange.emit(airport);
    }
}
