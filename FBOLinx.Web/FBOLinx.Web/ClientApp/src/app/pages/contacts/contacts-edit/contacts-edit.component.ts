import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-contacts-edit',
    templateUrl: './contacts-edit.component.html',
    styleUrls: [ './contacts-edit.component.scss' ],
})
export class ContactsEditComponent {
    @Output() saveEditClicked = new EventEmitter<any>();
    @Output() cancelEditClicked = new EventEmitter<any>();
    @Input() contactInfo: any;

    // Masks
    phoneMask: any[] = [
        '+',
        '1',
        ' ',
        '(',
        /[1-9]/,
        /\d/,
        /\d/,
        ')',
        ' ',
        /\d/,
        /\d/,
        /\d/,
        '-',
        /\d/,
        /\d/,
        /\d/,
        /\d/,
    ];

    constructor() {
    }

    public saveEdit() {
        this.saveEditClicked.emit();
    }

    public cancelEdit() {
        this.cancelEditClicked.emit();
    }
}
