import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-fbo-prices-panel',
    styleUrls: [ './fbo-prices-panel.component.scss' ],
    templateUrl: './fbo-prices-panel.component.html',
})
export class FboPricesPanelComponent {
    @Input() retail: number;
    @Input() cost: number;

    constructor() {
    }
}
