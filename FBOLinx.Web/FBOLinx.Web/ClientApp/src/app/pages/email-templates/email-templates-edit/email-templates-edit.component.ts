import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ImageSettingsModel } from '@syncfusion/ej2-angular-richtexteditor';

import { SharedService } from '../../../layouts/shared-service';
import { EmailcontentService } from '../../../services/emailcontent.service';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/email-templates',
        title: 'Email Templates',
    },
    {
        link: '',
        title: 'Edit Email Template',
    },
];

@Component({
    selector: 'app-email-templates-edit',
    styleUrls: ['./email-templates-edit.component.scss'],
    templateUrl: './email-templates-edit.component.html',
})
export class EmailTemplatesEditComponent implements OnInit {
    @ViewChild('fileUpload') fileUploadName;

    private id: any;

    public breadcrumb: any[] = BREADCRUMBS;
    public pageTitle = 'Edit Email Template';
    public emailTemplateForm: FormGroup;
    public emailTemplate: any;
    public canSave: boolean;
    public isSaving = false;
    public hasSaved = false;
    public isSaveQueued = false;

    public insertImageSettings: ImageSettingsModel = { saveFormat: 'Base64', maxWidth: 800, maxHeight: 600 }
    theFile: any;
    isUploadingFile: boolean;
    fileName: any = "";

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private sharedService: SharedService,
        private emailContentService: EmailcontentService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    async ngOnInit() {
        // Check for passed in id
        this.id = this.route.snapshot.paramMap.get('id');
        this.emailTemplate = await this.emailContentService
            .get({ oid: this.id })
            .toPromise();
        this.emailContentService.getFileAttachmentName(
            this.id).subscribe((data: any) => {
                this.fileName = data;
            });
    }

    public cancelEmailTemplateEdit(): void {
        this.router
            .navigate(['/default-layout/email-templates/'])
            .then(() => {});
    }

    public formChanged(event): void {
        this.canSave = true;
        this.saveEmailTemplate();
    }

    onFileChange(event) {
        this.theFile = null;
        if (event.target.files && event.target.files.length > 0) {
            // Set theFile property
            this.theFile = event.target.files[0];
        }
    }

    uploadFile(): void {
        if (this.theFile != null) {
            this.isUploadingFile = true;
            this.readAndUploadFile(this.theFile);
        }
    }

    deleteFile(): void {
        this.emailContentService
            .deleteFileAttachment(this.id)
            .subscribe((data: any) => {
                this.fileName = '';
            });
    }

    downloadFile(): void {
        this.emailContentService.downloadFileAttachment(this.id).subscribe((data: any) => {
            const source = data;
            const link = document.createElement("a");
            link.href = source;
            link.download = this.fileName;
            link.click();
        });
    }

    // Private Methods
    private saveEmailTemplate() {
        const self = this;
        if (this.isSaving) {
            // Save already in queue - no need to double-up the queue
            if (this.isSaveQueued) {
                return;
            }
            this.isSaveQueued = true;
            setTimeout(() => {
                self.saveEmailTemplate();
            }, 250);
            return;
        }

        this.isSaveQueued = false;
        this.isSaving = true;
        this.hasSaved = false;

        this.emailContentService
            .update(this.emailTemplate)
            .subscribe((response: any) => {
                this.isSaving = false;
                this.hasSaved = true;
            });
    }

    private readAndUploadFile(theFile: any) {
        const file = {
            ContentType: theFile.type,

            EmailContentId: this.id,

            FileData: null,
            // Set File Information
            FileName: theFile.name,
        };

        var printEventType = function (event) {
            var error = event;
        };

        // Use FileReader() object to get file to upload
        // NOTE: FileReader only works with newer browsers
        const reader = new FileReader();

        // Setup onload event for reader
        reader.onload = () => {
            // Store base64 encoded representation of file
            file.FileData = reader.result.toString();

            // POST to server
            this.emailContentService.uploadFileAttachment(file).subscribe((data: any) => {
                if (data.indexOf("Message:") < 1) {
                    this.isUploadingFile = false;
                    this.fileUploadName.nativeElement.value = '';
                    this.fileName = theFile.name;
                }
            });
        }

        // Read the file
        reader.readAsDataURL(theFile);
        reader.onerror = printEventType;
    }
}
