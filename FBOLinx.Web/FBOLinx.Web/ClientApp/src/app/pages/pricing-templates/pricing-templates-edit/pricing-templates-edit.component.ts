import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { CustomermarginsService } from '../../../services/customermargins.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { DistributionWizardMainComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Pricing Templates',
        link: '#/default-layout/pricing-templates'
    },
    {
        title: 'Edit Pricing Template',
        link: ''
    }
];

@Component({
    selector: 'app-pricing-templates-edit',
    templateUrl: './pricing-templates-edit.component.html',
    styleUrls: ['./pricing-templates-edit.component.scss']
})
/** pricing-templates-edit component*/
export class PricingTemplatesEditComponent {

    //Private Members
    private _RequiresRouting: boolean = false;
    
    //Input/Output Bindings
    @Output() savePricingTemplateClicked = new EventEmitter<any>();
    @Output() cancelPricingTemplateEditclicked = new EventEmitter<any>();
    @Input() pricingTemplate: any;

    //Public Members
    public pageTitle: string = 'Edit Pricing Template';
    public breadcrumb: any[] = BREADCRUMBS;
    public marginTypeDataSource: Array<any> = [
        { text: 'Cost +', value: 0 },
        { text: 'Retail -', value: 1 },
        { text: 'Flat Fee', value: 2 },
        { text: 'Inactive', value: 3 }
    ];
    public isSaving: boolean = false;

    /** pricing-templates-edit ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        public distributionDialog: MatDialog    ) {

        this.sharedService.emitChange(this.pageTitle);

        //Check for passed in id
        let id = this.route.snapshot.paramMap.get('id');
        if (!id)
            return;
        else {
            this._RequiresRouting = true;
            this.pricingTemplatesService.get({oid: id}).subscribe((data: any) => {
                this.pricingTemplate = data;
                this.customerMarginsService.getCustomerMarginsByPricingTemplateId(this.pricingTemplate.oid).subscribe((data: any) => {
                    this.pricingTemplate.customerMargins = data;
                });
            });
        }
    }

    public savePricingTemplate() {
        this.isSaving = true;
        //Update the pricing template first
        this.pricingTemplatesService.update(this.pricingTemplate).subscribe((data: any) => {
            this.saveCustomerMargins();
        });
    }
    

    public cancelPricingTemplateEdit() {
        if (this._RequiresRouting)
            this.router.navigate(['/default-layout/pricing-templates/']);
        else
            this.cancelPricingTemplateEditclicked.emit();
    }

    public deleteCustomerMargin(customerMargin) {
        this.customerMarginsService.remove(customerMargin).subscribe((data: any) => {
            this.priceTiersService.remove({ oid: customerMargin.priceTierId }).subscribe((data: any) => {
                this.pricingTemplate.customerMargins.splice(
                    this.pricingTemplate.customerMargins.indexOf(customerMargin),
                    1);
            });
        });
    }

    public addCustomerMargin() {
        let customerMargin = {
            oid: 0,
            templatesId: this.pricingTemplate.oid,
            priceTierId: 0,
            min: 1,
            max: 99999,
            amount: 0
        };
        if (this.pricingTemplate.customerMargins.length > 0) {
            customerMargin.min =
                this.pricingTemplate.customerMargins[this.pricingTemplate.customerMargins.length - 1].min + 250;
        }
        this.pricingTemplate.customerMargins.push(customerMargin);
        this.fixCustomerMargins();
    }

    public fixCustomerMargins() {
        for (let i in this.pricingTemplate.customerMargins) {
            let indexNumber = Number(i);
            if (this.pricingTemplate.customerMargins.length > (indexNumber + 1) &&
                this.pricingTemplate.customerMargins[indexNumber + 1].min > 0)
                this.pricingTemplate.customerMargins[i].max = this.pricingTemplate.customerMargins[indexNumber + 1].min - 1;
            else
                this.pricingTemplate.customerMargins[i].max = 99999;
        }
    }

    public distributePricingTemplate() {
        var pricingTemplates = [this.pricingTemplate];
        const dialogRef = this.distributionDialog.open(DistributionWizardMainComponent, {
            data: {
                pricingTemplates: pricingTemplates,
                fboId: this.sharedService.currentUser.fboId,
                groupId: this.sharedService.currentUser.groupId
            }
        });

        dialogRef.afterClosed().subscribe(result => {

        });
    }

    //Private Methods
    private saveCustomerMargins() {
        this.pricingTemplate.customerMargins.forEach((customerMargin: any) => {
            customerMargin.templateId = this.pricingTemplate.oid;
        });

        this.priceTiersService.updateFromCustomerMarginsViewModel(this.pricingTemplate.customerMargins).subscribe(
            (data: any) => {
                if (this._RequiresRouting)
                    this.router.navigate(['/default-layout/pricing-templates/']);
                else
                    this.savePricingTemplateClicked.emit();
                this.isSaving = false;
            });
    }
}
