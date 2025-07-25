import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-fbos-contacts',
    styleUrls: ['./fbos-contacts.component.scss'],
    templateUrl: './fbos-contacts.component.html',
})
export class FbosContactsComponent {
    @Output() recordDeleted = new EventEmitter<any>();
    @Output() newFboContactClicked = new EventEmitter<any>();
    @Output() editFboContactClicked = new EventEmitter<any>();
    @Input() fboContactsData: Array<any>;

    constructor() {}
}
