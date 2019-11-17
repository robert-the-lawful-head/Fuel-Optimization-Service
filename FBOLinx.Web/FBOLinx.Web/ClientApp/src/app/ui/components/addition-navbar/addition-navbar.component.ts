import { Component, OnInit, ViewChild, Input, ElementRef } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog } from '@angular/material';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';
import { DistributionWizardReviewComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component';

@Component({
  selector: 'addition-navbar',
  templateUrl: './addition-navbar.component.html',
  styleUrls: ['./addition-navbar.component.scss'],
  host: {
    '[class.addition-navbar]': 'true',
    '[class.open]': 'open'
  }
})
export class AdditionNavbarComponent implements OnInit {
  title: string;
    open: boolean;
   // @Input() pricingTemplatesData: Array<any>;

    //Public Members
    public displayedColumns: string[] = ['template','toggle'];
    public resultsLength: any;
    public marginTemplateDataSource: MatTableDataSource<any> = null;
    public pricingTemplatesData: any[];
    public filtered: any;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    @ViewChild('marginTableContainer') table: ElementRef;
    @Input() templatelst: any[];
    constructor(private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        public templateDialog: MatDialog) {
    this.title = 'Margins to be distributed';
        this.open = false;
        //this.marginTemplateDataSource = new MatTableDataSource(this.pricingTemplatesData);
  }

  openNavbar(event) {
    event.preventDefault();

    this.open = !this.open;
  }

    ngOnInit() {
        this.pricingTemplatesData = this.templatelst;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        //this.marginTemplateDataSource = new MatTableDataSource(this.pricingTemplatesData);
        this.marginTemplateDataSource = new MatTableDataSource(this.pricingTemplatesData.filter((element: any, index: number, array: any[]) => {
            return true;
        }));
        this.marginTemplateDataSource.sort = this.sort;
        this.marginTemplateDataSource.paginator = this.paginator;
        this.resultsLength = this.pricingTemplatesData.length;
        //this.resultsLength = 0;//this.pricingTemplatesData.length;
        //this.marginTemplateDataSource.paginator = this.paginator;
        //this.pricingTemplatesData.length;
    }

    public OpenMarginInfo(event) {
        this.filtered = this.pricingTemplatesData.find(({ oid }) => oid === event);

        const dialogRef = this.templateDialog.open(DistributionWizardReviewComponent,
            {
                data: this.filtered
            });

        dialogRef.afterClosed().subscribe(result => {

        });
    }
    async delay(ms: number) {
        await new Promise(resolve => setTimeout(() => resolve(), ms)).then(() => console.log("fired"));
    }
    public sendMails() {
        this.pricingTemplatesData.forEach(x => {
            if (x.toSend) {
                x.sent = true;
                this.delay(5000).then(any => {
                    x.sent = false;
                });
            }
        })

    }
}
