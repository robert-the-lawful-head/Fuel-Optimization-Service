import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
    selector: 'app-email-templates-dialog-new-template',
    templateUrl: './email-templates-dialog-new-template.component.html',
    styleUrls: ['./email-templates-dialog-new-template.component.scss'],
})
export class EmailTemplatesDialogNewTemplateComponent implements OnInit {
    constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    }

    ngOnInit(): void {
        if (!this.data.hideName && !this.data.name) {
            this.data.name = '';
        }
        if (!this.data.subject) {
            this.data.subject = '';
        }
        if (!this.data.emailContentHtml) {
            this.data.emailContentHtml = '';
        }
    }

    get isSubmitDisabled() {
        return (!this.data.hideName && !this.data.name) || !this.data.emailContentHtml || !this.data.subject;
    }

    get submitButtonTitle() {
        return this.data.isUpdate ? 'Update Email Template' : 'Add Email Template';
    }
}
