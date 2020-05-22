import { Component, Input, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";

@Component({
    selector: "[notification]",
    templateUrl: "./notification.component.html",
    styleUrls: ["./notification.component.scss"],
})
export class NotificationComponent {
    @Input() block = false;
    @Input() gradient = false;
    @Input() disabled = false;
    @Input() outline = false;
    @Input() lineStyle: string;
    @Input() align = "center";
    @Input() size = "default";
    @Input() view = "default";
    @Input() padding = "default";
    @Input() shape: number | string;
    @Input() beforeIcon: string;
    @Input() afterIcon: string;
    @Input() iconAnimation = false;

    constructor(
        public dialogRef: MatDialogRef<NotificationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    onCancelClick() {
        this.dialogRef.close();
    }
}
