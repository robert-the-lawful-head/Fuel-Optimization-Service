import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../constants/sharedEvents';
// Services
import { EmailcontentService } from '../../../services/emailcontent.service';
// Components
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/email-templates',
        title: 'Email Templates',
    },
];

@Component({
    selector: 'app-email-templates-home',
    styleUrls: ['./email-templates-home.component.scss'],
    templateUrl: './email-templates-home.component.html',
})
export class EmailTemplatesHomeComponent
    implements AfterViewInit, OnDestroy, OnInit
{
    public pageTitle = 'Email Templates';
    public breadcrumb: any[] = BREADCRUMBS;
    public emailTemplates: any[];
    public locationChangedSubscription: any;

    public constructor(
        private router: Router,
        private emailContentService: EmailcontentService,
        private sharedService: SharedService,
        private deleteEmailContentDialog: MatDialog
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit(): void {
        this.loadEmailTemplatesData();
    }

    ngAfterViewInit(): void {
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.loadEmailTemplatesData();
                }
            });
    }

    ngOnDestroy(): void {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    public editEmailTemplateClicked($event) {
        this.router
            .navigate([
                '/default-layout/email-templates/' + $event.emailTemplateId,
            ])
            .then(() => {});
    }

    public deleteEmailTemplateClicked(emailTemplate): void {
        const dialogRef = this.deleteEmailContentDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'email template', item: emailTemplate },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.emailTemplates = null;
            this.emailContentService
                .remove({
                    fboId: this.sharedService.currentUser.fboId,
                    oid: emailTemplate.oid,
                })
                .subscribe(() => {
                    this.loadEmailTemplatesData();
                });
        });
    }

    public newEmailTemplateAdded(emailTemplate) {
        this.emailContentService
            .add(emailTemplate)
            .subscribe((response: any) => {
                this.loadEmailTemplatesData();
            });
    }

    public copyEmailTemplateClicked(emailTemplate) {
        this.emailContentService
            .add(emailTemplate)
            .subscribe((response: any) => {
                this.loadEmailTemplatesData();
            });
    }

    // Private Methods
    private loadEmailTemplatesData() {
        this.emailTemplates = null;
        this.emailContentService
            .getForFbo(this.sharedService.currentUser.fboId)
            .subscribe((data: any[]) => {
                this.emailTemplates = data;
            });
    }
}
