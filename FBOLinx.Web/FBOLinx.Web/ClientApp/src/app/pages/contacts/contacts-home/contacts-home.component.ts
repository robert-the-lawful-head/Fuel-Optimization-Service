import { Component, Input, } from '@angular/core';

@Component({
    selector: 'app-contacts-home',
    styleUrls: [ './contacts-home.component.scss' ],
    templateUrl: './contacts-home.component.html',
})
export class ContactsHomeComponent {
    @Input() contactsData: Array<any>;

    constructor() {
    }
}
