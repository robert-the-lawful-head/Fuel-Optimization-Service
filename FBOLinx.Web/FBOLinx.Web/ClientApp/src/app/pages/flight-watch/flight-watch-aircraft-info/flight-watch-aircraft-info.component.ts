import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

import { FlightWatch } from '../../../models/flight-watch';

@Component({
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
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-aircraft-info',
    styleUrls: [ './flight-watch-aircraft-info.component.scss'],
    templateUrl: './flight-watch-aircraft-info.component.html',
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
