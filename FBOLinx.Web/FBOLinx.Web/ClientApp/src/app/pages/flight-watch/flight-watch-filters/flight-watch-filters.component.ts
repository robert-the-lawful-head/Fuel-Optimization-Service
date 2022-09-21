import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SwimFilter } from 'src/app/models/filter';

@Component({
  selector: 'app-flight-watch-filters',
  templateUrl: './flight-watch-filters.component.html',
  styleUrls: ['./flight-watch-filters.component.scss']
})

export class FlightWatchFiltersComponent implements OnInit {
    @Input() icao: string;
    @Input() icaoList: string[];
    @Input() isSearchAirportHidden: boolean = false;

    @Output() icaoChanged = new EventEmitter<string>();
    @Output() updateDrawerButtonPosition = new EventEmitter<any>();
    @Output() filterChanged = new EventEmitter<SwimFilter>();

    constructor() { }

    ngOnInit() {
    }
    updateIcao(event: any ){
        this.updateDrawerButtonPosition.emit();
        this.icaoChanged.emit(event);
    }
    applyFilter(event: Event) {
        let filter: SwimFilter = {
            filterText: (event.target as HTMLInputElement).value,
            dataType: null
        };
        this.filterChanged.emit(filter);
    }
}
