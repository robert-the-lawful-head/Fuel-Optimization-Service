import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FbosService } from '../../../services/fbos.service';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';

import { GroupAnalyticsGenerateDialogComponent, GroupAnalyticsGenerateDialogData } from '../group-analytics-generate-dialog/group-analytics-generate-dialog.component';
import { EmailTemplatesDialogNewTemplateComponent } from '../../../shared/components/email-templates-dialog-new-template/email-templates-dialog-new-template.component';
import { EmailcontentService } from '../../../services/emailcontent.service';
import { EmailTemplate } from 'src/app/models/email-template';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'Group Analytics',
        link: '',
    },
];

@Component({
    selector: 'app-group-analytics-home',
    templateUrl: './group-analytics-home.component.html',
    styleUrls: [ './group-analytics-home.component.scss' ],
})
export class GroupAnalyticsHomeComponent implements OnInit {
    pageTitle = 'Group Analytics';
    breadcrumb: any[] = BREADCRUMBS;

    customers: any[];
    fbos: any[];
    emailTemplate: EmailTemplate = null;

    constructor(
        private reportDialog: MatDialog,
        private editEmailDialog: MatDialog,
        private sharedService: SharedService,
        private fbosService: FbosService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private emailContentService: EmailcontentService,
    ) {
    }

    ngOnInit() {
        this.loadCustomers();
        this.loadFbos();
        this.loadEmailTemplate();
    }

    private loadCustomers() {
        this.customerInfoByGroupService.getByGroup(this.sharedService.currentUser.groupId)
            .subscribe((customers: any[]) => {
                this.customers = customers;
            });
    }

    private loadFbos() {
        this.fbosService
            .getForGroup(this.sharedService.currentUser.groupId)
            .subscribe((fbos: any[]) => {
                this.fbos = fbos;
            });
    }

    private loadEmailTemplate() {
        this.emailContentService.getForGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: EmailTemplate) => {
                this.emailTemplate = data;
            });
    }

    onGenerate() {
        this.reportDialog.open<GroupAnalyticsGenerateDialogComponent, GroupAnalyticsGenerateDialogData>(
            GroupAnalyticsGenerateDialogComponent,
            {
                data: {
                    customers: this.customers,
                    emailTemplate: this.emailTemplate,
                },
                width: '500px',
                autoFocus: false,
                panelClass: 'group-analytics-dialog'
            },
        );
    }

    onEditEmail() {
        const dialogRef = this.editEmailDialog.open(EmailTemplatesDialogNewTemplateComponent, {
            data: {
                hideName: true,
                subject: this.emailTemplate?.subject,
                emailContentHtml: this.emailTemplate?.emailContentHtml,
                isUpdate: true,
            },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            const emailTemplate: EmailTemplate = {
                oid: this.emailTemplate?.oid,
                groupId: this.sharedService.currentUser.groupId,
                subject: result.subject,
                emailContentHtml: result.emailContentHtml,
            };

            if (!this.emailTemplate || !this.emailTemplate.oid) {
                this.emailContentService.add(emailTemplate).subscribe((data: EmailTemplate) => {
                    this.emailTemplate = data;
                });
            } else {
                this.emailContentService.update(emailTemplate).subscribe((data: EmailTemplate) => {
                    this.emailTemplate = emailTemplate;
                });
            }
        });
    }
}
