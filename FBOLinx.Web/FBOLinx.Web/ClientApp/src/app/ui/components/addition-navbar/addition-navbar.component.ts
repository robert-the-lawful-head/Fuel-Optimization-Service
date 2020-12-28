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

// Components
import { ProceedConfirmationComponent } from '../../../shared/components/proceed-confirmation/proceed-confirmation.component';
import { DistributionWizardReviewComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component';
import { PricingExpiredNotificationComponent } from '../../../shared/components/pricing-expired-notification/pricing-expired-notification.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';

@Component({
    selector: 'addition-navbar',
    templateUrl: './addition-navbar.component.html',
    styleUrls: ['./addition-navbar.component.scss'],
    host: {
        '[class.addition-navbar]': 'true',
        '[class.open]': 'open',
    },
})
export class AdditionNavbarComponent implements OnInit, AfterViewInit, OnChanges {
    title: string;
    open: boolean;
    // Public Members
    public displayedColumns: string[] = ['template', 'toggle'];
    public resultsLength: any;
    public marginTemplateDataSource: MatTableDataSource<any> = null;
    public pricingTemplatesData: any[];
    public filteredTemplate: any;
    public distributionLog: any[];
    public timeLeft = 0;
    public interval: any;
    public selectAll: boolean;
    public previewEmail = '';
    labelPosition = 'before';
    buttontext = 'Distribute Pricing';
    @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
    @ViewChild(MatSort, {static: true}) sort: MatSort;
    @ViewChild('marginTableContainer') table: ElementRef;
    @ViewChild('nodeInput') fileInput: ElementRef;
    @ViewChild('insideElement') insideElement;
    @Input() templatelst: any[];
    message: string;
    private priceTemplatesForSending: any[];
    private pricesExpired: boolean;

    constructor(
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private proceedConfirmationDialog: MatDialog,
        public templateDialog: MatDialog,
        public distributionService: DistributionService,
        private customerContactsService: CustomercontactsService,
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

    public refresh() {
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
            (element: any, index: number, array: any[]) => {
                return (
                    array.indexOf(
                        array.find(
                            (t) =>
                                t.oid === element.oid &&
                                t.text === element.text &&
                                t.name === element.name
                        )
                    ) === index
                );
            }
        );

        this.selectAll = false;
        this.prepareDataSource();
    }

    public openNavbar(event) {
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
                        this.pricingTemplatesData = data;
                    }
                    this.marginTemplateDataSource = new MatTableDataSource(
                        this.pricingTemplatesData
                    );
                    this.resultsLength = this.pricingTemplatesData.length;
                });
        }

        this.open = !this.open;
    }

    public OpenMarginInfo(templateId) {
        this.pricesExpired = false;
        this.filteredTemplate = this.pricingTemplatesData.find(
            ({oid}) => oid === templateId
        );

        this.checkExpiredPrices(this.filteredTemplate);

        if (!this.pricesExpired) {
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
                            data: {text: 'Please assign the template to a customer before proceeding.'}
                        }
                    );

                    dialogRef.afterClosed().subscribe();
                }
            });
        }
    }

    async delay(ms: number) {
        await new Promise((resolve) =>
            setTimeout(() => resolve(), ms)
        ).then(() => console.log('fired'));
    }

    public changeSentOption(item) {
        item.toSend = !item.toSend;
        if (!item.toSend) {
            this.selectAll = false;
        }
        //  item.val = item.toSend ? item.val === undefined ? 0 + 33.33 : item.val + 33.33 : item.val - 33.33;
    }

    @HostListener('document:click', ['$event'])
    public onClick(targetElement) {
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

    public confirmSendEmails() {
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
                            description: 'You are about to distribute the following templates to the customers assigned to them. Are you sure?',
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
    }

    public getTemplatesWithCustomerCount(): Observable<any[]> {
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

    public sendMails() {
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

    public SelectAllTemplates(event) {
        this.selectAll = !this.selectAll;
        this.pricingTemplatesData.forEach((x) => {
            x.toSend = this.selectAll;
        });
    }

    public ReloadResults() {
        alert('refresh results');
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
        if (template.intoPlanePrice === 0 || template.intoPlanePrice === null) {
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
