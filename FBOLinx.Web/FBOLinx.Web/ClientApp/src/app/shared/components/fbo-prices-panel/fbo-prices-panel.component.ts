import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-fbo-prices-panel',
    styleUrls: ['./fbo-prices-panel.component.scss'],
    templateUrl: './fbo-prices-panel.component.html',
})
export class FboPricesPanelComponent {
    @Input() enableJetA: boolean;
    @Input() enableSaf: boolean;
    @Input() retailSaf: number;
    @Input() costSaf: number;
    @Input() retailJetA: number;
    @Input() costJetA: number;
    @Input() effectiveToSaf: string;
    @Input() effectiveToJetA: string;
    @Output() onClearFboPrice = new EventEmitter<string>();

    constructor() { }

    public clear(product) {
        this.onClearFboPrice.emit(product);
    }
}
