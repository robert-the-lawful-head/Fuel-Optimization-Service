import { Component, Input } from '@angular/core';

import { Item } from './item';

@Component({
    selector: 'ni-breadcrumb',
    styleUrls: ['./ni-breadcrumb.component.scss'],
    templateUrl: './ni-breadcrumb.component.html',
})
export class NiBreadcrumbComponent {
    @Input() menu: Item[] = [];
    @Input() separator = '/';

    constructor() {}
}
