import { ChangeDetectionStrategy, OnInit } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { Swim } from 'src/app/models/swim';
import { FlightWatch } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from '../flight-watch-map/aircraft-images';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-settings',
    styleUrls: ['./flight-watch-settings.component.scss'],
    templateUrl: './flight-watch-settings.component.html',
})
export class FlightWatchSettingsComponent implements OnInit {
    @Input() tableData: Observable<FlightWatch[]>;
    @Input() swimArrivals: Swim[];
    @Input() swimDepartures: Swim[];
    @Input() icao: string;
    @Input() icaoList: string[];

    @Input() filteredTypes: string[];
    @Output() typesFilterChanged = new EventEmitter<string[]>();
    @Output() filterChanged = new EventEmitter<string>();
    @Output() icaoChanged = new EventEmitter<string>();
   
    searchIcaoTxt: string;

    constructor() {   }

    ngOnInit() {}

    get aircraftTypes() {
        return AIRCRAFT_IMAGES.filter((type) => type.label !== 'Other')
            .map((type) => ({
                aircraftType: type.id,
                color: type.fillColor,
                description: type.description,
                label: type.label,
            }))
            .concat({
                aircraftType: 'default',
                color: '#5fb4e6',
                description: '',
                label: 'Other',
            });
    }

    applyFilter(event: Event) {
        this.filterChanged.emit((event.target as HTMLInputElement).value);
    }

    toggleType(type: string) {
        if (this.filteredTypes.includes(type)) {
            if (type === 'default') {
                this.typesFilterChanged.emit(
                    this.filteredTypes.filter(
                        (ft) => !['B0', 'B3', 'default'].includes(ft)
                    )
                );
            } else {
                this.typesFilterChanged.emit(
                    this.filteredTypes.filter((ft) => ft !== type)
                );
            }
        } else {
            if (type === 'default') {
                this.typesFilterChanged.emit([
                    ...this.filteredTypes,
                    'B0',
                    'B3',
                    'default',
                ]);
            } else {
                this.typesFilterChanged.emit([...this.filteredTypes, type]);
            }
        }
    }
    updateIcao(event: any ){
        this.icaoChanged.emit(event);
    }
}
