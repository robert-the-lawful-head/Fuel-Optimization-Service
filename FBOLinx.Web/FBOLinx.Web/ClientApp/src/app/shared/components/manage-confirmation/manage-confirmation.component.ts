import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';
import { FbosService } from 'src/app/services/fbos.service';

@Component({
    selector: 'app-manage-confirmation',
    templateUrl: './manage-confirmation.component.html',
    styleUrls: [ './manage-confirmation.component.scss' ],
})
export class ManageConfirmationComponent {
    loading = false;

    constructor(
        public dialogRef: MatDialogRef<ManageConfirmationComponent>,
        private fboService: FbosService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
    }

    onConfirm(): void {
        if (this.data.group) {
            this.dialogRef.close(true);
        } else {
            this.loading = true;
            this.fboService.manageFbo(this.data.fboId)
                .subscribe(() => {
                    this.loading = false;
                    this.dialogRef.close(true);
                });
        }
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
