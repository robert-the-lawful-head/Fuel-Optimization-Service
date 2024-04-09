import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SelectedEventArgs, FileInfo } from '@syncfusion/ej2-angular-inputs';
import { ImageDropEventArgs, ImageSettingsModel } from '@syncfusion/ej2-angular-richtexteditor';
import { FileHelper } from 'src/app/helpers/files/file.helper';
import { EditorBase } from 'src/app/services/text-editor/editorBase';

@Component({
    selector: 'app-email-templates-dialog-new-template',
    styleUrls: ['./email-templates-dialog-new-template.component.scss'],
    templateUrl: './email-templates-dialog-new-template.component.html',
})
export class EmailTemplatesDialogNewTemplateComponent extends EditorBase implements OnInit {
    public insertImageSettings: ImageSettingsModel = { saveFormat: 'Base64' }

    constructor(@Inject(MAT_DIALOG_DATA) public data: any, fileHelper: FileHelper) {
        super(fileHelper);
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
        return (
            (!this.data.hideName && !this.data.name) ||
            !this.data.emailContentHtml ||
            !this.data.subject
        );
    }

    get submitButtonTitle() {
        return this.data.isUpdate
            ? 'Update Email Template'
            : 'Add Email Template';
    }
}
