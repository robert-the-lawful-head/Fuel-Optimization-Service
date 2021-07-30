import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EmailTemplate } from 'src/app/models/email-template';
import { GroupsService } from 'src/app/services/groups.service';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { EmailcontentService } from '../../../services/emailcontent.service';
import { FbosService } from '../../../services/fbos.service';
import { GroupAnalyticsEmailTemplateDialogComponent } from '../group-analytics-email-template-dialog/group-analytics-email-template-dialog.component';
import {
    GroupAnalyticsGenerateDialogComponent,
    GroupAnalyticsGenerateDialogData,
} from '../group-analytics-generate-dialog/group-analytics-generate-dialog.component';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '',
        title: 'Group Analytics',
    },
];

@Component({
    selector: 'app-group-analytics-home',
    styleUrls: ['./group-analytics-home.component.scss'],
    templateUrl: './group-analytics-home.component.html',
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
        private emailContentService: EmailcontentService
    ) {}

    ngOnInit() {
        this.loadCustomers();
        this.loadFbos();
        this.loadEmailTemplate();
        this.loadLogo();
    }

    onGenerate() {
        const dialogRef = this.reportDialog.open<
            GroupAnalyticsGenerateDialogComponent,
            GroupAnalyticsGenerateDialogData
        >(GroupAnalyticsGenerateDialogComponent, {
            autoFocus: false,
            data: {
                customers: this.customers,
                emailTemplate: this.emailTemplate,
            },
            panelClass: 'group-analytics-dialog',
            width: '500px',
        });

        dialogRef.afterClosed().subscribe((result) => {
            this.loadEmailTemplate();
        });
    }

    onEditEmail() {
        const dialogRef = this.editEmailDialog.open(
            GroupAnalyticsEmailTemplateDialogComponent,
            {
                data: {
                    emailContentHtml: this.emailTemplate?.emailContentHtml,
                    fromAddress: this.emailTemplate?.fromAddress,
                    logoUrl: this.logoUrl,
                    replyTo: this.emailTemplate?.replyTo,
                    subject: this.emailTemplate?.subject,
                },
                height: '600px',
                width: '500px',
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            const emailTemplate: EmailTemplate = {
                emailContentHtml: result.emailContentHtml,
                fromAddress: result.fromAddress,
                groupId: this.sharedService.currentUser.groupId,
                oid: this.emailTemplate?.oid,
                replyTo: result.replyTo,
                subject: result.subject,
            };

            if (!this.emailTemplate || !this.emailTemplate.oid) {
                this.emailContentService
                    .add(emailTemplate)
                    .subscribe((data: EmailTemplate) => {
                        this.emailTemplate = data;
                    });
            } else {
                this.emailContentService
                    .update(emailTemplate)
                    .subscribe((data: EmailTemplate) => {
                        this.emailTemplate = emailTemplate;
                    });
            }
        });
    }

    private loadCustomers() {
        this.customerInfoByGroupService
            .getCustomersWithContactsByGroup(
                this.sharedService.currentUser.groupId
            )
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
        this.emailContentService
            .getForGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: EmailTemplate) => {
                this.emailTemplate = data;

                if (!this.emailTemplate) {
                    this.emailTemplate = {
                        emailContentHtml: '',
                        fromAddress: 'donotreply',
                        groupId: this.sharedService.currentUser.groupId,
                        subject: '',
                    };
                }
                if (!this.emailTemplate.fromAddress) {
                    this.emailTemplate.fromAddress = 'donotreply';
                }
            });
    }

    private loadLogo() {
        this.groupsService
            .getLogo(this.sharedService.currentUser.groupId)
            .subscribe((logoData: any) => {
                this.logoUrl = logoData.message;
            });
    }
}
