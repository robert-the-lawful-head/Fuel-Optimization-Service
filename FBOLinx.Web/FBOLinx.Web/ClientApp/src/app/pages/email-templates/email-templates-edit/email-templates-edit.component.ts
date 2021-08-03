import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

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
    public breadcrumb: any[] = BREADCRUMBS;
    public pageTitle = 'Edit Email Template';
    public emailTemplateForm: FormGroup;
    public emailTemplate: any;
    public canSave: boolean;
    public isSaving = false;
    public hasSaved = false;
    public isSaveQueued = false;

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
        const id = this.route.snapshot.paramMap.get('id');
        this.emailTemplate = await this.emailContentService
            .get({ oid: id })
            .toPromise();
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
}
