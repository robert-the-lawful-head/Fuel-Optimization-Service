import {
    AfterViewInit,
    Component,
    ElementRef,
    HostListener,
    Input,
    OnChanges,
    OnInit,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { isEqual } from 'lodash';
import { forkJoin, Observable } from 'rxjs';

import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';
import { DistributionService } from '../../../services/distribution.service';
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { EmailcontentService } from '../../../services/emailcontent.service';

// Components
import { ProceedConfirmationComponent } from '../../../shared/components/proceed-confirmation/proceed-confirmation.component';
import { DistributionWizardReviewComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component';
import { PricingExpiredNotificationComponent } from '../../../shared/components/pricing-expired-notification/pricing-expired-notification.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';

@Component({
    selector: 'app-addition-navbar',
    templateUrl: './addition-navbar.component.html',
    styleUrls: [ './addition-navbar.component.scss' ],
    host: {
        '[class.addition-navbar]': 'true',
        '[class.open]': 'open',
    },
})
export class AdditionNavbarComponent implements OnInit, AfterViewInit, OnChanges {
    @Input() templatelst: any[];
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild('marginTableContainer') table: ElementRef;
    @ViewChild('nodeInput') fileInput: ElementRef;
    @ViewChild('insideElement') insideElement;

    title: string;
    open: boolean;
    // Public Members
    displayedColumns: string[] = [ 'template', 'toggle' ];
    resultsLength: any;
    marginTemplateDataSource: MatTableDataSource<any> = null;
    pricingTemplatesData: any[];
    filteredTemplate: any;
    distributionLog: any[];
    timeLeft = 0;
    interval: any;
    selectAll = true;
    previewEmail = '';
    labelPosition = 'before';
    buttontext = 'Distribute Pricing';

    message: string;
    private priceTemplatesForSending: any[];
    private priceTemplatesEmailContent: any[];
    private pricesExpired: boolean;

    constructor(
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private proceedConfirmationDialog: MatDialog,
        private templateDialog: MatDialog,
        private distributionService: DistributionService,
        private customerContactsService: CustomercontactsService,
        private emailContentService: EmailcontentService,
        private expiredPricingDialog: MatDialog
    ) {
        this.title = 'Distribute Prices';
        this.open = false;
    }

    ngOnInit() {

    }

    ngAfterViewInit() {
        this.sharedService.currentMessage.subscribe((message) => {
            this.message = message;
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.templatelst && !isEqual(changes.templatelst.currentValue, changes.templatelst.previousValue)) {
            this.refresh();
        }
    }

    @HostListener('document:click', [ '$event' ])
    onClick(targetElement) {
        if (
            this.open && ((targetElement.target.innerText === 'Clear') ||
            (targetElement.target.nodeName !== 'svg' &&
                !(
                    targetElement.target.className === 'ng-star-inserted' ||
                    targetElement.target.offsetParent.className.lastIndexOf(
                        'addition-navbar'
                    ) > -1 ||
                    targetElement.target.textContent === '$Cost' ||
                    targetElement.target.offsetParent.className.lastIndexOf(
                        'addition-navbar'
                    ) > -1 ||
                    targetElement.target.offsetParent.className.lastIndexOf(
                        'open-navbar'
                    ) > -1 ||
                    targetElement.target.offsetParent.className.lastIndexOf(
                        'mat-slide-toggle-thumb-container'
                    ) > -1 ||
                    targetElement.target.offsetParent.tagName ===
                    'MAT-PROGRESS-BAR' ||
                    targetElement.target.nodeName === 'MAT-PROGRESS-BAR' ||
                    targetElement.target.offsetParent.className.lastIndexOf(
                        'btn-text'
                    ) > -1 ||
                    targetElement.target.offsetParent.className.lastIndexOf(
                        'ng-star-inserted'
                    ) > -1 ||
                    targetElement.target.offsetParent.className.lastIndexOf(
                        'mat-progress-bar-element'
                    ) > -1 ||
                    targetElement.target.offsetParent.innerText ===
                    'Distribute Pricing' ||
                    targetElement.target.offsetParent.offsetParent.className ===
                    'addition-navbar'
                )))
        ) {
            if (targetElement.target.innerText !== 'Sent!') {
                this.open = !this.open;
            }
        }
    }

    refresh() {
        this.distributionService
            .getDistributionLogForFbo(this.sharedService.currentUser.fboId, 50)
            .subscribe((data: any) => {
                this.distributionLog = data;
                if (!this.distributionLog) {
                    this.distributionLog = [];
                } else {
                    this.templatelst.forEach((m) => {
                        this.distributionLog.forEach((obj) => {
                            if (obj.pricingTemplateId === m.oid) {
                                if (m.text === undefined) {
                                    m.text =
                                        'Last sent ' +
                                        moment(
                                            moment.utc(obj.dateSent).toDate()
                                        ).format('MM/DD/YYYY HH:mm');
                                }
                            }
                        });
                    });
                }
            });

        this.pricingTemplatesData = this.templatelst.filter(
            (element: any, index: number, array: any[]) => (
                array.indexOf(
                    array.find(
                        (t) =>
                            t.oid === element.oid &&
                            t.text === element.text &&
                            t.name === element.name
                    )
                ) === index
            )
        );

        this.prepareDataSource();
    }

    openNavbar(event) {
        event.preventDefault();

        if (!this.open) {
            this.pricingTemplatesData = [];

            this.pricingTemplatesService
                .getByFbo(
                    this.sharedService.currentUser.fboId,
                    this.sharedService.currentUser.groupId
                )
                .subscribe((data: any) => {
                    if (data) {
                        this.pricingTemplatesData = data.map(row => ({
                            ...row,
                            toSend: true,
                        }));
                    }

                    this.marginTemplateDataSource = new MatTableDataSource(
                        this.pricingTemplatesData
                    );
                    this.resultsLength = this.pricingTemplatesData.length;
                });
        }

        this.open = !this.open;
    }

    openMarginInfo(templateId) {
        this.pricesExpired = false;
        this.filteredTemplate = this.pricingTemplatesData.find(
            ({ oid }) => oid === templateId
        );

        this.checkExpiredPrices(this.filteredTemplate);

        if (!this.pricesExpired) {
            if (this.filteredTemplate.emailContentId > 0) {
                this.emailContentService.get({ oid: this.filteredTemplate.emailContentId }).
                    subscribe((data: any) => {
                        if (data != null && data.oid > 0) {
                            this.checkCustomerContacts(templateId);
                        }
                        else {
                            const dialogRef = this.templateDialog.open(
                                NotificationComponent,
                                {
                                    data: { text: 'Please assign an Email Template to this ITP Template before proceeding.' }
                                }
                            );

                            dialogRef.afterClosed().subscribe();
                        }
                    });
            }
            else {
                this.checkCustomerContacts(templateId);
            }
        }
    }

    async delay(ms: number) {
        await new Promise<void>((resolve) =>
            setTimeout(() => resolve(), ms)
        ).then(() => console.log('fired'));
    }

    changeSentOption(item) {
        item.toSend = !item.toSend;
        if (!item.toSend) {
            this.selectAll = false;
        }
    }

    checkCustomerContacts(templateId) {
        this.customerContactsService
            .getCustomerEmailCountByGroupAndFBOAndPricing(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                templateId
            ).subscribe((data: number) => {
                if (data > 0) {
                    const dialogRef = this.templateDialog.open(
                        DistributionWizardReviewComponent,
                        {
                            data: this.previewEmail,
                            panelClass: 'wizard',
                        }
                    );

                    dialogRef.afterClosed().subscribe((result) => {
                        if (!result) {
                            return;
                        }

                        this.previewEmail = result;
                        const request = this.GetSendDistributionRequest(this.filteredTemplate);
                        this.distributionService
                            .previewDistribution(request)
                            .subscribe(() => {
                            });
                    });
                } else {
                    const dialogRef = this.templateDialog.open(
                        NotificationComponent,
                        {
                            data: { text: 'Please add a contact with an email to a customer assigned to this template before proceeding.' }
                        }
                    );

                    dialogRef.afterClosed().subscribe();
                }
            });
    }

    confirmSendEmails() {
        this.pricesExpired = false;
        for (const template of this.pricingTemplatesData) {
            if (template.toSend) {
                this.checkExpiredPrices(template);
                if (this.pricesExpired) {
                    return;
                }
            }
        }

        if (!this.pricesExpired) {
            const templatesWithEmailContent: any[] = [];
            this.getTemplateEmailContent().subscribe((responseList: any[]) => {
                for (const responseIndex in responseList) {
                    if (responseList[responseIndex] == null && this.priceTemplatesEmailContent[responseIndex].emailContentId > 0) {
                        templatesWithEmailContent.push(
                            this.priceTemplatesEmailContent[responseIndex].name
                        );
                    }
                }

                if (templatesWithEmailContent.length == 0)
                    this.sendEmails();
                else {
                    var templatesList = templatesWithEmailContent.join(', ');
                    const dialogRef = this.templateDialog.open(
                        NotificationComponent,
                        {
                            data: { text: 'Please assign an Email Template to these ITP Template(s) before proceeding: ' + templatesList }
                        }
                    );

                    dialogRef.afterClosed().subscribe();
                }
            });
        }
    }

    sendEmails() {
        const templatesWithCustomerCount: any[] = [];
        this.getTemplatesWithCustomerCount().subscribe((responseList: any[]) => {
            if (!responseList) {
                alert('No customers found for the selected distributions.');
            }
            for (const responseIndex in responseList) {
                templatesWithCustomerCount.push(
                    this.priceTemplatesForSending[responseIndex].name + ': ' + responseList[responseIndex] +
                    (responseList[responseIndex] === 1 ? ' email' : ' emails')
                );
            }

            const dialogRef = this.proceedConfirmationDialog.open(
                ProceedConfirmationComponent,
                {
                    data: {
                        description:
                            'You are about to distribute the following templates to the customers assigned to them. Are you sure?',
                        itemsList: templatesWithCustomerCount
                    },
                    autoFocus: false
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }
                this.sendMails();
            });
        });
    }

    getTemplateEmailContent(): Observable<any[]> {
        const result: any[] = [];
        this.priceTemplatesEmailContent = [];

        this.pricingTemplatesData.forEach((x) => {
            if (x.toSend) {
                this.priceTemplatesEmailContent.push(x);
                result.push(this.emailContentService.get({ oid: x.emailContentId }
                ));
            }
        });
        return forkJoin(result);
    }

    getTemplatesWithCustomerCount(): Observable<any[]> {
        const result: any[] = [];
        this.priceTemplatesForSending = [];

        this.pricingTemplatesData.forEach((x) => {
            if (x.toSend) {
                this.priceTemplatesForSending.push(x);
                result.push(this.customerContactsService
                    .getCustomerEmailCountByGroupAndFBOAndPricing(
                        this.sharedService.currentUser.groupId,
                        this.sharedService.currentUser.fboId,
                        x.oid
                    ));
            }
        });
        return forkJoin(result);
    }

    sendMails() {
        this.pricingTemplatesData.forEach((x) => {
            if (x.toSend) {
                const request = this.GetSendDistributionRequest(x);
                this.distributionService
                    .distributePricing(request)
                    .subscribe((data: any) => {
                    });
                this.interval = setInterval(() => {
                    if (this.timeLeft <= 3) {
                        this.timeLeft++;
                        x.val =
                            this.timeLeft === 3
                                ? 100
                                : 0 + 33.33 * this.timeLeft;
                        if (this.timeLeft === 3) {
                            x.sent = true;
                        }
                    }
                }, 1000);
                this.delay(8000).then(() => {
                    x.sent = false;
                });
                x.text =
                    'Last sent ' +
                    moment(new Date()).format('MM/DD/YYYY HH:mm');
                this.buttontext = 'Sent!';
            }

            setTimeout(() => {
                this.buttontext = 'Distribute Pricing';
            }, 5000);
        });
    }

    selectAllTemplates() {
        this.selectAll = !this.selectAll;
        this.pricingTemplatesData.forEach((x) => {
            x.toSend = this.selectAll;
        });
    }

    // Private Methods
    private GetSendDistributionRequest(template) {
        return {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            pricingTemplate: template,
            previewEmail: this.previewEmail
        };
    }

    private checkExpiredPrices(template) {
        if (template.intoPlanePrice <= 0 || template.intoPlanePrice === null) {
            const dialogRef = this.expiredPricingDialog.open(
                PricingExpiredNotificationComponent, {
                    data: {
                        hideRemindMeButton: true
                    },
                    autoFocus: false
                }
            );
            dialogRef.afterClosed().subscribe();
            this.pricesExpired = true;
        }
    }

    private prepareDataSource(): void {
        if (!this.pricingTemplatesData || !this.sort) {
            return;
        }
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.marginTemplateDataSource = new MatTableDataSource(
            this.pricingTemplatesData
        );
        this.marginTemplateDataSource.sort = this.sort;
        this.marginTemplateDataSource.paginator = this.paginator;
        this.resultsLength = this.pricingTemplatesData.length;
    }
}
