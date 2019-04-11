import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';

//Services
import { ContactsService } from '../../../services/contacts.service';

@Component({
    selector: 'app-contacts-home',
    templateUrl: './contacts-home.component.html',
    styleUrls: ['./contacts-home.component.scss']
})
/** contacts-home component*/
export class ContactsHomeComponent {
    @Input() contactsData: Array<any>;

    /** contacts-home ctor */
    constructor(private contactsService: ContactsService) {

    }
}
