import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-fbo-prices-panel',
    templateUrl: './fbo-prices-panel.component.html',
    styleUrls: [ './fbo-prices-panel.component.scss' ],
})
export class FboPricesPanelComponent {
    @Input() retail: number;
    @Input() cost: number;

    constructor() {
    }
}
