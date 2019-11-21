import { OnInit, Component, Inject, ViewChild, EventEmitter, Output } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { PricingtemplatesService } from "../../../../services/pricingtemplates.service";
import { RichTextEditorComponent } from "@syncfusion/ej2-angular-richtexteditor";
import { CustomermarginsService } from "../../../../services/customermargins.service";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
    selector: 'app-distribution-wizard-review',
    templateUrl: './distribution-wizard-review.component.html',
    styleUrls: ['./distribution-wizard-review.component.scss']
})
/** distribution-wizard-rewiew component*/
export class DistributionWizardReviewComponent implements OnInit {
    public edit: boolean = false;
    public customerMargins: any[];
   
    @ViewChild('typeEmail') rteEmail: RichTextEditorComponent;
    @Output() idChanged1: EventEmitter<any> = new EventEmitter();

    constructor(public dialogRef: MatDialogRef<DistributionWizardReviewComponent>, @Inject(MAT_DIALOG_DATA) public data: any,
        private pricingTemplatesService: PricingtemplatesService,
        private customerMarginsService: CustomermarginsService,
        private router: Router,
        private route: ActivatedRoute) { }

    ngOnInit() {
        this.customerMarginsService.getCustomerMarginsByPricingTemplateId(this.data.oid).subscribe(
            (data: any) => {
                this.customerMargins = data;
                this.data.customerMargins = data;
            });

        this.route.params.subscribe(
            params => {
                const id = +params['id'];
            }
        );
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
                console.log("Navigation is successful!");
            } else {
                this.router.navigateByUrl('./' + pricingTemplate.oid);
            }
        });
    }
}
