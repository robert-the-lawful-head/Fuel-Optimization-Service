import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FbosService } from '../../../services/fbos.service';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';

import { GroupAnalyticsGenerateDialogComponent, GroupAnalyticsGenerateDialogData } from '../group-analytics-generate-dialog/group-analytics-generate-dialog.component';
import { EmailcontentService } from '../../../services/emailcontent.service';
import { EmailTemplate } from 'src/app/models/email-template';
import { GroupAnalyticsEmailTemplateDialogComponent } from '../group-analytics-email-template-dialog/group-analytics-email-template-dialog.component';
import { GroupsService } from 'src/app/services/groups.service';

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

    logoUrl = '';

    constructor(
        private reportDialog: MatDialog,
        private editEmailDialog: MatDialog,
        private sharedService: SharedService,
        private fbosService: FbosService,
        private groupsService: GroupsService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private emailContentService: EmailcontentService,
    ) {
    }

    ngOnInit() {
        this.loadCustomers();
        this.loadFbos();
        this.loadEmailTemplate();
        this.loadLogo();
    }

    onGenerate() {
        const dialogRef = this.reportDialog.open<GroupAnalyticsGenerateDialogComponent, GroupAnalyticsGenerateDialogData>(
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

        dialogRef.afterClosed().subscribe((result) => {
            this.loadEmailTemplate();
        });
    }

    onEditEmail() {
        const dialogRef = this.editEmailDialog.open(GroupAnalyticsEmailTemplateDialogComponent, {
            data: {
                subject: this.emailTemplate?.subject,
                emailContentHtml: this.emailTemplate?.emailContentHtml,
                fromAddress: this.emailTemplate?.fromAddress,
                logoUrl: this.logoUrl,
            },
            height: '600px',
            width: '500px'
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
                fromAddress: result.fromAddress,
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

    private loadLogo() {
        this.groupsService.getLogo(this.sharedService.currentUser.groupId)
            .subscribe((logoData: any) => {
                this.logoUrl = logoData.message;
            });
    }
}
