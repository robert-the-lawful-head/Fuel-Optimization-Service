import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-contacts-edit',
    styleUrls: [ './contacts-edit.component.scss' ],
    templateUrl: './contacts-edit.component.html',
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
