import { Component, Inject, EventEmitter, Output } from '@angular/core';
import { Popover } from '../popover';

@Component({
    selector: 'app-tooltip-modal',
    templateUrl: './tooltip-modal.component.html',
    styleUrls: ['./tooltip-modal.component.scss']
})
/** close-confirmation component*/
export class TooltipModalComponent {
    public data: any;
    @Output() closeEmitter: EventEmitter<void> = new EventEmitter();
    /** close-confirmation ctor */
    constructor(public popover: Popover) {}

    close() {
        this.popover.close();
    }

    setProperty(data: any) {
        this.data = data;
    }

    ngOnDestroy() {
        this.closeEmitter.emit();
    }
}
