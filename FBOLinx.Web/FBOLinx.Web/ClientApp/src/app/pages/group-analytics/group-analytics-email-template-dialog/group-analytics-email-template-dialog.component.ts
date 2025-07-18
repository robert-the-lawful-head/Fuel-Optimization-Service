import { Component, Inject, OnInit } from '@angular/core';
import {
    AbstractControl,
    UntypedFormControl,
    UntypedFormGroup,
    ValidationErrors,
    Validators,
} from '@angular/forms';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { ImageSettingsModel } from '@syncfusion/ej2-angular-richtexteditor';
import { SharedService } from 'src/app/layouts/shared-service';
import { GroupsService } from 'src/app/services/groups.service';
import { validateEmail } from 'src/utils/email';

@Component({
    providers: [SharedService],
    selector: 'app-group-analytics-email-template-dialog',
    styleUrls: ['./group-analytics-email-template-dialog.component.scss'],
    templateUrl: './group-analytics-email-template-dialog.component.html',
})
export class GroupAnalyticsEmailTemplateDialogComponent implements OnInit {
    logo: File = null;
    isLogoUploading = false;
    logoUrl = '';

    form: UntypedFormGroup;

    public insertImageSettings: ImageSettingsModel = { saveFormat: 'Base64' }

    constructor(
        public dialogRef: MatDialogRef<GroupAnalyticsEmailTemplateDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private readonly sharedService: SharedService,
        private readonly groupsService: GroupsService
    ) {
        this.form = new UntypedFormGroup({
            emailContentHtml: new UntypedFormControl(
                this.data.emailContentHtml
            ),
            fromAddress: new UntypedFormControl(this.data.fromAddress, [
                this.fromAddressValidator,
            ]),
            replyTo: new UntypedFormControl(
                this.data.replyTo,
                Validators.pattern(
                    /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                )
            ),
            subject: new UntypedFormControl(this.data.subject, Validators.required),
        });
    }

    ngOnInit(): void {
        this.logoUrl = this.data.logoUrl;
    }

    onFileChange(event) {
        this.logo = null;
        if (event.target.files && event.target.files.length > 0) {
            // Set theFile property
            this.logo = event.target.files[0];
        }
    }

    uploadFile(): void {
        if (this.logo != null) {
            this.isLogoUploading = true;

            const file = {
                contentType: this.logo.type,
                fileData: null,
                // Set File Information
                fileName: this.logo.name,
                groupId: this.sharedService.currentUser.groupId,
            };

            // Use FileReader() object to get file to upload
            // NOTE: FileReader only works with newer browsers
            const reader = new FileReader();

            // Setup onload event for reader
            reader.onload = () => {
                // Store base64 encoded representation of file
                file.fileData = reader.result.toString();

                // POST to server
                this.groupsService.uploadLogo(file).subscribe((resp: any) => {
                    this.isLogoUploading = false;
                    this.logoUrl = resp.message;
                });
            };

            // Read the file
            reader.readAsDataURL(this.logo);
        }
    }

    deleteFile(): void {
        this.groupsService
            .deleteLogo(this.sharedService.currentUser.groupId)
            .subscribe(() => {
                this.logoUrl = '';
            });
    }

    update(): void {
        this.dialogRef.close(this.form.value);
    }

    fromAddressValidator(control: AbstractControl): ValidationErrors | null {
        const value = control.value;

        if (!value) {
            return null;
        }

        return validateEmail(value + '@fbolinx.com')
            ? null
            : { emailNotValid: true };
    }
}
