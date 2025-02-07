import { Component, HostListener, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
    selector: 'app-request-demo-modal',
    styleUrls: ['./request-demo-modal.component.scss'],
    templateUrl: './request-demo-modal.component.html',
})
export class RequestDemoModalComponent {
    public zohoLoader = 'Zoho loader';

    constructor(
        public dialogRef: MatDialogRef<RequestDemoModalComponent>,
        private ngxLoader: NgxUiLoaderService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.ngxLoader.startLoader(this.zohoLoader);
    }

    @HostListener('window:message', ['$event'])
    messageUpdated(event: MessageEvent) {
        if (event.isTrusted && event.returnValue) {
            this.data.succeed = true;
            this.dialogRef.close(this.data);
        }
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    public zohoLoaded() {
        this.ngxLoader.stopLoader(this.zohoLoader);
    }
}
