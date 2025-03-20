import { Component } from '@angular/core';
import { MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

@Component({
    selector: 'app-ramp-fees-import-informatino-dialog',
    styleUrls: ['./ramp-fees-import-information-dialog.component.scss'],
    templateUrl: './ramp-fees-import-information-dialog.component.html',
})
export class RampFeesImportInformationComponent {
    constructor(
        public dialogRef: MatDialogRef<RampFeesImportInformationComponent>
    ) {}

    public onOkClick(): void {
        this.dialogRef.close('ok');
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
