import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SharedService } from 'src/app/layouts/shared-service';
import { GroupsService } from 'src/app/services/groups.service';


@Component({
    selector: 'app-group-analytics-email-template-dialog',
    templateUrl: './group-analytics-email-template-dialog.component.html',
    styleUrls: ['./group-analytics-email-template-dialog.component.scss'],
    providers: [SharedService]
})
export class GroupAnalyticsEmailTemplateDialogComponent implements OnInit {
    logo: File = null;
    isLogoUploading = false;
    logoUrl = '';

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        private readonly sharedService: SharedService,
        private readonly groupsService: GroupsService,
    ) {
    }

    ngOnInit(): void {
        this.logoUrl = this.data.logoUrl;
    }

    get isSubmitDisabled() {
        return !this.data.emailContentHtml || !this.data.subject;
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

            const file = { // Set File Information
                fileName: this.logo.name,
                contentType: this.logo.type,
                fileData: null,
                groupId: this.sharedService.currentUser.groupId
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
        this.groupsService.deleteLogo(this.sharedService.currentUser.groupId)
            .subscribe(() => {
                this.logoUrl = '';
            });
    }
}
