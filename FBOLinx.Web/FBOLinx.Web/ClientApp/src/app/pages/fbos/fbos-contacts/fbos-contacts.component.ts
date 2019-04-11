import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

@Component({
    selector: 'app-fbos-contacts',
    templateUrl: './fbos-contacts.component.html',
    styleUrls: ['./fbos-contacts.component.scss']
})
/** fbos-contacts component*/
export class FbosContactsComponent {

    @Output() recordDeleted = new EventEmitter<any>();
    @Output() newFboContactClicked = new EventEmitter<any>();
    @Output() editFboContactClicked = new EventEmitter<any>();
    @Input() fboContactsData: Array<any>;

    /** fbos-contacts ctor */
    constructor() {

    }
}
