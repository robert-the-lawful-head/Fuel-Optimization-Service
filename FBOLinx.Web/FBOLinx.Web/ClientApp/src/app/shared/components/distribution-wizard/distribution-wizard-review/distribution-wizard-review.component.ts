import { OnInit, Component, Inject, ViewChild, EventEmitter, Output, ElementRef } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { PricingtemplatesService } from "../../../../services/pricingtemplates.service";
import { RichTextEditorComponent } from "@syncfusion/ej2-angular-richtexteditor";
import { CustomermarginsService } from "../../../../services/customermargins.service";
import { Router, ActivatedRoute, NavigationEnd } from "@angular/router";
import { FbopricesService } from "../../../../services/fboprices.service";
import { SharedService } from "../../../../layouts/shared-service";

@Component({
    selector: 'app-distribution-wizard-review',
    templateUrl: './distribution-wizard-review.component.html',
    styleUrls: ['./distribution-wizard-review.component.scss'],
    providers: [SharedService]
})
/** distribution-wizard-rewiew component*/
export class DistributionWizardReviewComponent implements OnInit {
    public edit: boolean = false;
    public customerMargins: any[];
    public currentPrice: any[];
    public navigationSubscription: any;

    @ViewChild('typeEmail') rteEmail: RichTextEditorComponent;
    @Output() idChanged1: EventEmitter<any> = new EventEmitter();
    @ViewChild("custMargin") divView: ElementRef;

    constructor(public dialogRef: MatDialogRef<DistributionWizardReviewComponent>, @Inject(MAT_DIALOG_DATA) public data: any,
        private pricingTemplatesService: PricingtemplatesService,
        private customerMarginsService: CustomermarginsService,
        private router: Router,
        private fbopricesService: FbopricesService,
        private sharedService: SharedService,
        private route: ActivatedRoute) {
        this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
        }

        this.router.events.subscribe((evt) => {
            if (evt instanceof NavigationEnd) {
                // trick the Router into believing it's last link wasn't previously loaded
                this.router.navigated = false;
                // if you need to scroll back to top, here is the right place
                window.scrollTo(0, 0);
            }
        });
    }

    ngOnInit() {
        this.customerMarginsService.getCustomerMarginsByPricingTemplateId(this.data.oid).subscribe(
            (data: any) => {
                this.customerMargins = data;
                this.data.customerMargins = data;
                this.fbopricesService.getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId).subscribe((dates: any) => {
                    this.currentPrice = dates;
                    for (let margin of this.data.customerMargins) {
                        //this.calculateItpForMargin(margin);
                        this.updateCustomerMargin(margin);
                    }
                });
            });

        this.route.params.subscribe(
            params => {
                const id = +params['id'];
            }
        );
    }
    public updateCustomerMargin(margin) {
        var jetACost = this.currentPrice.filter(item => item.product == 'JetA Cost')[0].price;
        var jetARetail = this.currentPrice.filter(item => item.product == 'JetA Retail')[0].price;
        margin.allin = 0;
        if (this.data.marginType == 0) {
            if (margin.min && margin.itp) {
                margin.allin = jetACost + margin.itp;
            }

        }
        else if (this.data.marginType == 1) {
            if (margin.amount && margin.min) {
                margin.allin = jetARetail - margin.amount;
                if (margin.allin) {
                    margin.itp = margin.allin - jetACost;
                }
            }
        }
    }

    public changeStatus() {
        this.edit = true;
    }

    public update() {
        this.pricingTemplatesService.update(this.data).subscribe((data: any) => {

            this.edit = false;
        });
    }

    public disableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = false;

    }

    public enableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = true;
    }

    public closeDialog() {
        this.dialogRef.close();
    }

    public editPricingTemplate(pricingTemplate) {
        this.dialogRef.close();
        this.idChanged1.emit(null);
        //const clonedRecord = Object.assign({}, pricingTemplate);
        //this.router.navigate(['/default-layout/pricing-templates/' + pricingTemplate.oid]);
        this.router.navigateByUrl('/default-layout/pricing-templates/' + pricingTemplate.oid).then(e => {
            if (e) {
                this.router.navigateByUrl(this.router.url);
            
            } else {
                this.router.navigateByUrl('./' + pricingTemplate.oid);
            }
        });

    }
}

