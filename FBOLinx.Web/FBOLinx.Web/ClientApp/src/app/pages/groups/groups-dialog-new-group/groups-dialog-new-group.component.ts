import { Component, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

// Interfaces
export interface NewGroupDialogData {
    oid: number;
    groupName: string;
    domain: string;
    active: boolean;
}

@Component({
    selector: "app-groups-dialog-new-group",
    templateUrl: "./groups-dialog-new-group.component.html",
    styleUrls: ["./groups-dialog-new-group.component.scss"],
})
export class GroupsDialogNewGroupComponent {
    constructor(
        public dialogRef: MatDialogRef<GroupsDialogNewGroupComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewGroupDialogData
    ) {
        data.active = true;
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
