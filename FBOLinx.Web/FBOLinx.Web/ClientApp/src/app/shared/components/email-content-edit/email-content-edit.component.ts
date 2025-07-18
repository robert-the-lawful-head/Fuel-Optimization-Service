import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { ImageSettingsModel } from '@syncfusion/ej2-angular-richtexteditor';

// Services
import { EmailcontentService } from '../../../services/emailcontent.service';

export interface EmailContentDialogData {
    oid: number;
    emailContentHTML: string;
    fboId: number;
    emailContentType: number;
    name: string;
}

@Component({
    selector: 'app-email-content-edit',
    styleUrls: ['./email-content-edit.component.scss'],
    templateUrl: './email-content-edit.component.html',
})
export class EmailContentEditComponent {
    public emailContentTypes: any[] = [];
    public insertImageSettings: ImageSettingsModel = { saveFormat: 'Base64' }

    constructor(
        public dialogRef: MatDialogRef<EmailContentEditComponent>,
        @Inject(MAT_DIALOG_DATA) public emailContent: EmailContentDialogData,
        private emailContentService: EmailcontentService
    ) {}

    // Public Methods
    public onSaveChangesClick(): void {
        if (this.emailContent.oid > 0) {
            this.emailContentService
                .update(this.emailContent)
                .subscribe(() => this.completeSave());
        } else {
            this.emailContentService
                .add(this.emailContent)
                .subscribe(() => this.completeSave());
        }
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    // Private Methods
    private completeSave() {
        this.dialogRef.close(this.emailContent);
    }
}
