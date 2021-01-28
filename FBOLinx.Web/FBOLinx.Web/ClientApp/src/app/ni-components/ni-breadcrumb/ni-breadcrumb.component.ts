import { Component, Input } from '@angular/core';
import { Item } from './item';

@Component({
    selector: 'ni-breadcrumb',
    templateUrl: './ni-breadcrumb.component.html',
    styleUrls: ['./ni-breadcrumb.component.scss'],
})
export class NiBreadcrumbComponent {
    @Input() menu: Item[] = [];
    @Input() separator = '/';

    constructor() {}
}
