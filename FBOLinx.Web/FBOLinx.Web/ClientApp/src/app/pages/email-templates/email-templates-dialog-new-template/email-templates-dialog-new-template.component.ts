import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';


@Component({
    selector: 'app-email-templates-dialog-new-template',
    templateUrl: './email-templates-dialog-new-template.component.html',
    styleUrls: ['./email-templates-dialog-new-template.component.scss'],
})
export class EmailTemplatesDialogNewTemplateComponent implements OnInit {
    @ViewChild('typeEmail') rteEmail: RichTextEditorComponent;

    public emailTemplateForm: FormGroup;

    constructor(private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: any) {

    }

    ngOnInit(): void {
        this.data.name = '';
        this.data.subject = '';
        this.data.email = '';

        this.emailTemplateForm = this.formBuilder.group({
            name: [this.data.name],
            subject: [this.data.subject],
            email: [this.data.emailContentHtml]
        });
    }
}
