import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';

import { GroupsService } from '../../../services/groups.service';

@Component({
    selector: 'app-groups-merge-dialog',
    styleUrls: [ './groups-merge-dialog.component.scss' ],
    templateUrl: './groups-merge-dialog.component.html',
})
export class GroupsMergeDialogComponent {
    baseGroup: number;
    loading: boolean;
    failed: boolean;

    constructor(
        public dialogRef: MatDialogRef<GroupsMergeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private groupsService: GroupsService,
    ) {
        this.dialogRef.disableClose = true;
        this.baseGroup = this.data.groups[0].oid;
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onSaveClick(): void {
        this.loading = true;
        const payload = {
            baseGroupId: this.baseGroup,
            groups: this.data.groups
        };

        this.groupsService.mergeGroups(payload).subscribe(() => {
            this.loading = false;
            this.failed = false;
            this.dialogRef.close(payload);
        }, (err) => {
            console.error(err);
            this.loading = false;
            this.failed = true;
        });
    }
}
