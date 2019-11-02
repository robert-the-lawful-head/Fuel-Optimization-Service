import { Component, Input, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: '[notification]',
  templateUrl: './notification.component.html',
    styleUrls: ['./notification.component.scss']
})
export class NotificationComponent {
  @Input() block: boolean = false;
  @Input() gradient: boolean = false;
  @Input() disabled: boolean = false;
  @Input() outline: boolean = false;
  @Input() lineStyle: string;
  @Input() align: string = 'center';
  @Input() size: string = 'default';
  @Input() view: string = 'default';
  @Input() padding: string = 'default';
  @Input() shape: number | string;
  @Input() beforeIcon: string;
  @Input() afterIcon: string;
  @Input() iconAnimation: boolean = false;

    constructor(public dialogRef: MatDialogRef<NotificationComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {

    }
}
