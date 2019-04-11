import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';

//Services
import { ContactsService } from '../../../services/contacts.service';

@Component({
    selector: 'app-contacts-edit',
    templateUrl: './contacts-edit.component.html',
    styleUrls: ['./contacts-edit.component.scss']
})
/** contacts-edit component*/
export class ContactsEditComponent {

    @Output() saveEditClicked = new EventEmitter<any>();
    @Output() cancelEditClicked = new EventEmitter<any>();
    @Input() contactInfo: any;

    //Masks
    phoneMask: any[] = ['+', '1', ' ', '(', /[1-9]/, /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];

    /** contacts-edit ctor */
    constructor(private contactsService: ContactsService) {

    }

    public saveEdit() {
        this.saveEditClicked.emit();
    }

    public cancelEdit() {
        this.cancelEditClicked.emit();
    }
}
