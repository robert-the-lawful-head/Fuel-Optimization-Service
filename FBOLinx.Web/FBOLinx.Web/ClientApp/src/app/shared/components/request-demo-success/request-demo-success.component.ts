import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'app-request-demo-success',
    styleUrls: [ './request-demo-success.component.scss' ],
    templateUrl: './request-demo-success.component.html',
})
export class RequestDemoSuccessComponent {
    public zohoLoader = 'Zoho loader';

    constructor(
        public dialogRef: MatDialogRef<RequestDemoSuccessComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
