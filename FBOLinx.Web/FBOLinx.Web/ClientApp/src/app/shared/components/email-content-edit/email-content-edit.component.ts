import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
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
    templateUrl: './email-content-edit.component.html',
    styleUrls: ['./email-content-edit.component.scss']
})
/** email-content-edit component*/
export class EmailContentEditComponent {
    public emailContentTypes: any[] = [];

    /** email-content-edit ctor */
    constructor(public dialogRef: MatDialogRef<EmailContentEditComponent>, @Inject(MAT_DIALOG_DATA) public emailContent: EmailContentDialogData,
        private emailContentService: EmailcontentService) {

    }

    //Public Methods
    public onSaveChangesClick(): void {
        if (this.emailContent.oid > 0) {
            this.emailContentService.update(this.emailContent).subscribe((data: any) => this.completeSave());
        } else {
            this.emailContentService.add(this.emailContent).subscribe((data: any) => this.completeSave());
        }
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    //Private Methods
    private completeSave() {
        this.dialogRef.close(this.emailContent);
    }
}
