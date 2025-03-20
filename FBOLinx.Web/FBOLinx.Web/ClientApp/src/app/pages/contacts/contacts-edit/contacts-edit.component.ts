import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-contacts-edit',
    styleUrls: ['./contacts-edit.component.scss'],
    templateUrl: './contacts-edit.component.html',
})
export class ContactsEditComponent {
    @Output() saveEditClicked = new EventEmitter<any>();
    @Output() cancelEditClicked = new EventEmitter<any>();
    @Input() contactInfo: any;

    // Masks
    phoneMask = '(000) 000-0000';
    prefix = '+1 ';

    constructor() {}

    public saveEdit() {
        this.saveEditClicked.emit();
    }

    public cancelEdit() {
        this.cancelEditClicked.emit();
    }
}
function provideNgxMask(): import("@angular/core").Provider {
    throw new Error('Function not implemented.');
}

