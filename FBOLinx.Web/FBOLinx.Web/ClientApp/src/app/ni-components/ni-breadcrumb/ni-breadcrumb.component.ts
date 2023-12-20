import { Component, Input, SimpleChanges } from '@angular/core';

import { Item } from './item';

@Component({
    selector: 'ni-breadcrumb',
    styleUrls: ['./ni-breadcrumb.component.scss'],
    templateUrl: './ni-breadcrumb.component.html',
})
export class NiBreadcrumbComponent {
    @Input() menu: Item[] = [];
    @Input() separator = '/';

    private _sectionTitle: Item  = { title: 'None', icon: '', link:'' };
    constructor() {}

    ngOnChanges(changes: SimpleChanges): void {
        if(changes.menu) {
            this._sectionTitle = changes.menu.currentValue.pop();
        }
    }

    get sectionTitle(): Item {
        return this._sectionTitle;

    }
    set sectionTitle(title: Item) {
        this._sectionTitle = title;
    }
}
