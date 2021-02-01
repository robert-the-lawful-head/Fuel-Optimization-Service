import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FlightWatch } from '../../../models/flight-watch';

@Component({
    selector: 'app-flight-watch-aircraft-info',
    templateUrl: './flight-watch-aircraft-info.component.html',
    styleUrls: [ './flight-watch-aircraft-info.component.scss'],
    animations: [
        trigger('openClose', [
            state('closed', style({
                width: '0px',
            })),
            state('open', style({
                width: '200px',
            })),
            transition('* => closed', [
                animate('0.2s')
            ]),
            transition('* => open', [
                animate('0.2s')
            ]),
        ]),
      ],
})
export class FlightWatchAircraftInfoComponent {
    @Input() flightWatch: FlightWatch;
    @Output() closed = new EventEmitter<void>();

    constructor() {
    }

    onClose() {
        this.closed.emit();
    }
}
