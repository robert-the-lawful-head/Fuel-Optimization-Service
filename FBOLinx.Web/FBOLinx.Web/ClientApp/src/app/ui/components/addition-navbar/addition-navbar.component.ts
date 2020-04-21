import {
    Component,
    OnInit,
    ViewChild,
    Input,
    ElementRef,
    HostListener,
    AfterViewInit,
} from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { SharedService } from "../../../layouts/shared-service";
import { DistributionWizardReviewComponent } from "../../../shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component";
import { DistributionService } from "../../../services/distribution.service";
import * as moment from "moment";

@Component({
    selector: "addition-navbar",
    templateUrl: "./addition-navbar.component.html",
    styleUrls: ["./addition-navbar.component.scss"],
    host: {
        "[class.addition-navbar]": "true",
        "[class.open]": "open",
    },
})
export class AdditionNavbarComponent implements OnInit, AfterViewInit {
    title: string;
    open: boolean;
    // Public Members
    public displayedColumns: string[] = ["template", "toggle"];
    public resultsLength: any;
    public marginTemplateDataSource: MatTableDataSource<any> = null;
    public pricingTemplatesData: any[];
    public filtered: any;
    public distributionLog: any[];
    public timeLeft = 0;
    public interval: any;
    public selectAll: boolean;
    labelPosition = "before";
    buttontext = "Distribute Pricing";
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild("marginTableContainer") table: ElementRef;
    @ViewChild("nodeInput") fileInput: ElementRef;
    @ViewChild("insideElement") insideElement;
    @Input() templatelst: any[];

    message: string;

    constructor(
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        public templateDialog: MatDialog,
        public distributionService: DistributionService
    ) {
        this.title = "Distribute Prices";
        this.open = false;
    }

    ngOnInit() {
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
                                        "Last sent " +
                                        moment(
                                            moment.utc(obj.dateSent).toDate()
                                        ).format("MM/DD/YYYY HH:mm");
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
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.marginTemplateDataSource = new MatTableDataSource(
            this.pricingTemplatesData
        );
        this.marginTemplateDataSource.sort = this.sort;
        this.marginTemplateDataSource.paginator = this.paginator;
        this.resultsLength = this.pricingTemplatesData.length;
    }

    ngAfterViewInit() {
        this.sharedService.currentMessage.subscribe((message) => {
            this.message = message;
            this.pricingTemplatesService
                .getByFbo(
                    this.sharedService.currentUser.fboId,
                    this.sharedService.currentUser.groupId
                )
                .subscribe((data: any) => {
                    this.pricingTemplatesData = [];
                    if (data) {
                        this.pricingTemplatesData = data;
                    }
                    this.marginTemplateDataSource = new MatTableDataSource(
                        this.pricingTemplatesData
                    );
                    this.resultsLength = this.pricingTemplatesData.length;
                });
        });
    }

    openNavbar(event) {
        event.preventDefault();

        this.open = !this.open;
    }

    public OpenMarginInfo(event) {
        this.filtered = this.pricingTemplatesData.find(
            ({ oid }) => oid === event
        );

        const dialogRef = this.templateDialog.open(
            DistributionWizardReviewComponent,
            {
                data: this.filtered,
                panelClass: "wizard",
            }
        );
        dialogRef.componentInstance.idChanged1.subscribe((result) => {
            this.fileInput.nativeElement.click();
            // this.openNavbar(event);
        });
        dialogRef.afterClosed().subscribe((result) => {});
    }
    async delay(ms: number) {
        await new Promise((resolve) =>
            setTimeout(() => resolve(), ms)
        ).then(() => console.log("fired"));
    }
    public changeSentOption(item) {
        item.toSend = !item.toSend;
        if (!item.toSend) {
            this.selectAll = false;
        }
        //  item.val = item.toSend ? item.val === undefined ? 0 + 33.33 : item.val + 33.33 : item.val - 33.33;
    }

    @HostListener("document:click", ["$event"])
    public onClick(targetElement) {
        /* console.log(targetElement);
        const clickedInside = this.insideElement.nativeElement.contains(targetElement);
        console.log("test");
        if (!clickedInside) {
            console.log('outside clicked');
        }*/
        // console.log(targetElement.path.toString());
        const term = "addition-navbar";
        // targetElement.path.forEach(element => element[1].nodeName);
        if (
            this.open &&
            targetElement.target.nodeName !== "svg" &&
            !(
                targetElement.target.className === "ng-star-inserted" ||
                targetElement.target.offsetParent.className.lastIndexOf(
                    "addition-navbar"
                ) > -1 ||
                targetElement.target.textContent === "$Cost" ||
                targetElement.target.offsetParent.className.lastIndexOf(
                    "addition-navbar"
                ) > -1 ||
                targetElement.target.offsetParent.className.lastIndexOf(
                    "open-navbar"
                ) > -1 ||
                targetElement.target.offsetParent.className.lastIndexOf(
                    "mat-slide-toggle-thumb-container"
                ) > -1 ||
                targetElement.target.offsetParent.tagName ===
                    "MAT-PROGRESS-BAR" ||
                targetElement.target.nodeName === "MAT-PROGRESS-BAR" ||
                targetElement.target.offsetParent.className.lastIndexOf(
                    "btn-text"
                ) > -1 ||
                targetElement.target.offsetParent.className.lastIndexOf(
                    "ng-star-inserted"
                ) > -1 ||
                targetElement.target.offsetParent.className.lastIndexOf(
                    "mat-progress-bar-element"
                ) > -1 ||
                targetElement.target.offsetParent.innerText ===
                    "Distribute Pricing" ||
                targetElement.target.offsetParent.offsetParent.className ===
                    "addition-navbar"
            )
        ) {
            if (targetElement.target.innerText !== "Sent!") {
                this.open = !this.open;
            }
        }
    }
    public sendMails() {
        this.pricingTemplatesData.forEach((x) => {
            if (x.toSend) {
                const request = {
                    fboId: this.sharedService.currentUser.fboId,
                    groupId: this.sharedService.currentUser.groupId,
                    pricingTemplate: x,
                };
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
                x.text =
                    "Last sent " +
                    moment(new Date()).format("MM/DD/YYYY HH:mm");
                this.buttontext = "Sent!";
            }

            setTimeout(() => {
                this.buttontext = "Distribute Pricing";
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
        alert("refresh results");
    }
}
