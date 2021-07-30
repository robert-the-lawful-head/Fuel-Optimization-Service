import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: '[notification]',
    styleUrls: [ './notification.component.scss' ],
    templateUrl: './notification.component.html',
})
export class NotificationComponent {
    @Input() block = false;
    @Input() gradient = false;
    @Input() disabled = false;
    @Input() outline = false;
    @Input() lineStyle: string;
    @Input() align = 'center';
    @Input() size = 'default';
    @Input() view = 'default';
    @Input() padding = 'default';
    @Input() shape: number | string;
    @Input() beforeIcon: string;
    @Input() afterIcon: string;
    @Input() iconAnimation = false;

    constructor(
        public dialogRef: MatDialogRef<NotificationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
    }

    onCancelClick() {
        this.dialogRef.close();
    }
}
