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
import { isEqual } from 'lodash';
import * as moment from 'moment';
import { DistributeEmailsConfirmationComponent } from 'src/app/shared/components/distribute-emails-confirmation/distribute-emails-confirmation.component';

import { SharedService } from '../../../layouts/shared-service';
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { DistributionService } from '../../../services/distribution.service';
import { EmailcontentService } from '../../../services/emailcontent.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { DistributionWizardReviewComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';

@Component({
    host: {
        '[class.addition-navbar]': 'true',
        '[class.open]': 'open',
    },
    selector: 'app-addition-navbar',
    styleUrls: ['./addition-navbar.component.scss'],
    templateUrl: './addition-navbar.component.html',
})
export class AdditionNavbarComponent
    implements OnInit, AfterViewInit, OnChanges
{
    @Input() templatelst: any[];
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild('marginTableContainer') table: ElementRef;
    @ViewChild('nodeInput') fileInput: ElementRef;
    @ViewChild('insideElement') insideElement;

    title: string;
    open: boolean;
    // Public Members
    displayedColumns: string[] = ['template', 'toggle'];
    resultsLength: any;
    marginTemplateDataSource: MatTableDataSource<any> = null;
    pricingTemplatesData: any[];
    distributionLog: any[];
    timeLeft = 0;
    interval: any;
    selectAll = true;
    previewEmail = '';
    labelPosition = 'before';
    buttontext = 'Distribute Pricing';

    message: string;
    private pricesExpired: boolean;
    private retailPrice: number;
    private costPrice: number;

    constructor(
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private distributeConfirmationDialog: MatDialog,
        private templateDialog: MatDialog,
        private distributionService: DistributionService,
        private fboPricesService: FbopricesService,
        private expiredPricingDialog: MatDialog
    ) {
        this.title = 'Distribute Prices';
        this.open = false;
    }

    get validTemplates() {
        return this.pricingTemplatesData.filter(
            (t) => t.customerEmails.length > 0
        );
    }

    ngOnInit() {}

    ngAfterViewInit() {
        this.sharedService.currentMessage.subscribe((message) => {
            this.message = message;
        });

    }

    ngOnChanges(changes: SimpleChanges): void {
        if (
            changes.templatelst &&
            !isEqual(
                changes.templatelst.currentValue,
                changes.templatelst.previousValue
            )
        ) {
            this.refresh();
        }
    }

    @HostListener('document:click', ['$event'])
    onClick(targetElement) {
        if (
            this.open &&
            targetElement.target &&
            (targetElement.target.innerText === 'Clear' ||
                (targetElement.target.nodeName !== 'svg' &&
                    targetElement.target.offsetParent &&
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
                        targetElement.target.offsetParent.offsetParent
                            .className === 'addition-navbar'
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
            (element: any, index: number, array: any[]) =>
                array.indexOf(
                    array.find(
                        (t) =>
                            t.oid === element.oid &&
                            t.text === element.text &&
                            t.name === element.name
                    )
                ) === index
        );

        this.prepareDataSource();
    }

    openNavbar(event) {
        event.preventDefault();

        if (!this.open) {
            this.pricingTemplatesData = [];

            this.pricingTemplatesService
                .getWithEmailContentByFbo(
                    this.sharedService.currentUser.fboId,
                    this.sharedService.currentUser.groupId
                )
                .subscribe((data: any) => {
                    if (data) {
                        this.pricingTemplatesData = data
                            .map((row) => ({
                                ...row,
                                toSend: row.customersAssigned > 0 && row.customerEmails.length > 0 ? true : false,
                                sending: false
                            }));
                    }

                    this.marginTemplateDataSource = new MatTableDataSource(
                        this.pricingTemplatesData
                    );
                    this.resultsLength = this.pricingTemplatesData.length;
                    this.selectAll = true;
                });
        }

        this.open = !this.open;
    }

    async openMarginInfo(templateId) {
        this.pricesExpired = false;
        const filteredTemplate = this.pricingTemplatesData.find(
            ({ oid }) => oid === templateId
        );

        await this.checkExpiredPrices();

        if (!this.pricesExpired) {
            this.checkCustomerContacts(filteredTemplate);

            if (!filteredTemplate.emailContent) {
                const dialogRef = this.templateDialog.open(
                    NotificationComponent,
                    {
                        data: {
                            text: 'Please assign an Email Template to this ITP Template before proceeding.',
                        },
                    }
                );

                dialogRef.afterClosed().subscribe();
            }
        }
    }

    async delay(ms: number) {
        await new Promise<void>((resolve) => setTimeout(() => resolve(), ms));
    }

    changeSentOption(item) {
        item.toSend = !item.toSend;
        if (!item.toSend) {
            this.selectAll = false;
        }

        if (
            this.validTemplates.length ===
            this.validTemplates.filter((t) => t.toSend).length
        ) {
            this.selectAll = true;
        }
    }

    checkCustomerContacts(template) {
        if (template.customerEmails.length) {
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
                const request = this.GetSendDistributionRequest(template);
                this.distributionService
                    .previewDistribution(request)
                    .subscribe(() => {});
            });
        } else {
            const dialogRef = this.templateDialog.open(NotificationComponent, {
                data: {
                    text: 'Please add a contact with an email to a customer assigned to this template before proceeding.',
                },
            });

            dialogRef.afterClosed().subscribe();
        }
    }

    async confirmSendEmails() {
        this.pricesExpired = false;
        await this.checkExpiredPrices();

        if (!this.pricesExpired) {
            this.sendEmails();
        }
    }

    sendEmails() {
        const dialogRef = this.distributeConfirmationDialog.open(
            DistributeEmailsConfirmationComponent,
            {
                autoFocus: false,
                data: this.validTemplates.filter((t) => t.toSend),
                width: '700px',
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.sendMails();
        });
    }

    sendMails() {
        this.pricingTemplatesData.forEach((x) => {
            if (x.toSend && !x.sending) {
                x.sending = true;
                const request = this.GetSendDistributionRequest(x);
                this.distributionService
                    .distributePricing(request)
                    .subscribe((data: any) => {});
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
                x.lastSent =
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
            previewEmail: this.previewEmail,
            pricingTemplate: template,
        };
    }

    private async checkExpiredPrices() {
        const data: any = await this.fboPricesService
            .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .toPromise();

        this.retailPrice = data.filter(
            (r) => r.product === 'JetA Retail'
        )?.[0].price;
        this.costPrice = data.filter(
            (r) => r.product === 'JetA Cost'
        )?.[0].price;

        if (this.sharedService.currentUser.role != 6 && !this.retailPrice && !this.costPrice) {
            const dialogRef = this.templateDialog.open(NotificationComponent, {
                data: {
                    text: 'Your fuel pricing has expired. Please update your cost/retail values.',
                    title: 'Pricing Expired',
                },
            });

            dialogRef.afterClosed().subscribe();
        }
        if (this.retailPrice && !this.costPrice) {
            const dialogRef = this.templateDialog.open(NotificationComponent, {
                data: {
                    text: 'You need to add a posted cost price to distribute',
                    title: 'Cost price is expired',
                },
            });

            dialogRef.afterClosed().subscribe();
        }
        if (!this.retailPrice && this.costPrice) {
            const dialogRef = this.templateDialog.open(NotificationComponent, {
                data: {
                    text: 'You need to add a posted retail price to distribute',
                    title: 'Retail price is expired',
                },
            });

            dialogRef.afterClosed().subscribe();
        }

        this.pricesExpired = !this.retailPrice || !this.costPrice;
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
