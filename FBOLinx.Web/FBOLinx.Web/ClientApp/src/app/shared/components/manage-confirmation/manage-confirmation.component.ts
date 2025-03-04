import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { FbosService } from 'src/app/services/fbos.service';

@Component({
    selector: 'app-manage-confirmation',
    styleUrls: ['./manage-confirmation.component.scss'],
    templateUrl: './manage-confirmation.component.html',
})
export class ManageConfirmationComponent {
    loading = false;

    constructor(
        public dialogRef: MatDialogRef<ManageConfirmationComponent>,
        private fboService: FbosService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    onConfirm(): void {
        if (this.data.group) {
            this.dialogRef.close(true);
        } else {
            this.loading = true;
            this.fboService.manageFbo(this.data.fboId).subscribe(() => {
                this.loading = false;
                this.dialogRef.close(true);
            });
        }
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
