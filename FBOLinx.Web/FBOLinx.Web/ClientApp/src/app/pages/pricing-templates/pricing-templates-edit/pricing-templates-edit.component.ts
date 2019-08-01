import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { CustomermarginsService } from '../../../services/customermargins.service';
import { FbopricesService } from '../../../services/fboprices.service';
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
        title: 'Margin Templates',
        link: '#/default-layout/pricing-templates'
    },
    {
        title: 'Edit Margin Template',
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
    public pageTitle: string = 'Edit Margin Template';
    public breadcrumb: any[] = BREADCRUMBS;
    public marginTypeDataSource: Array<any> = [
        { text: 'Cost +', value: 0 },
        { text: 'Retail -', value: 1 },
        { text: 'Flat Fee', value: 2 },
        { text: 'Inactive', value: 3 }
    ];
    public isSaving: boolean = false;
    public currentPrice: any;
    public product: any;

    /** pricing-templates-edit ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fbopricesService: FbopricesService,
        private sharedService: SharedService,
        public distributionDialog: MatDialog    ) {

        this.sharedService.emitChange(this.pageTitle);
        
        //Check for passed in id
        let id = this.route.snapshot.paramMap.get('id');
        if (!id) {
            this.loadCurrentPrice();
            return;
        } else {
            this._RequiresRouting = true;
            this.pricingTemplatesService.get({ oid: id }).subscribe((data: any) => {
                this.pricingTemplate = data;
                this.customerMarginsService.getCustomerMarginsByPricingTemplateId(this.pricingTemplate.oid).subscribe(
                    (data: any) => {
                        this.pricingTemplate.customerMargins = data;
                        this.loadCurrentPrice();
                    });
            });
        }
    }

    public savePricingTemplate() {
        this.isSaving = true;
        //Update the margin template first
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
        if (customerMargin.oid == 0) {
            this.pricingTemplate.customerMargins.splice(
                this.pricingTemplate.customerMargins.indexOf(customerMargin),
                1);
        } else {
            this.customerMarginsService.remove(customerMargin).subscribe((data: any) => {
                this.priceTiersService.remove({ oid: customerMargin.priceTierId }).subscribe((data: any) => {
                    this.pricingTemplate.customerMargins.splice(
                        this.pricingTemplate.customerMargins.indexOf(customerMargin),
                        1);
                });
            });
        }
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
            if (!this.pricingTemplate.customerMargins[indexNumber].min || this.pricingTemplate.customerMargins[indexNumber].min == '') {
                if (indexNumber == 0)
                    this.pricingTemplate.customerMargins[indexNumber].min = 1;
                else
                    this.pricingTemplate.customerMargins[indexNumber].min =
                        (this.pricingTemplate.customerMargins[indexNumber - 1].min + 1);
            }

            if (indexNumber > 0 && this.pricingTemplate.customerMargins[indexNumber].min == this.pricingTemplate.customerMargins[indexNumber - 1].min) {
                this.pricingTemplate.customerMargins[indexNumber].min =
                    (this.pricingTemplate.customerMargins[indexNumber - 1].min + 1);
            }
                
            if (this.pricingTemplate.customerMargins.length > (indexNumber + 1) &&
                this.pricingTemplate.customerMargins[indexNumber + 1].min > 0)
                this.pricingTemplate.customerMargins[i].max = this.pricingTemplate.customerMargins[indexNumber + 1].min - 1;
            else
                this.pricingTemplate.customerMargins[i].max = 99999;

            this.calculateItpForMargin(this.pricingTemplate.customerMargins[indexNumber]);
        }
    }

    public distributePricingTemplate() {
        var pricingTemplate = this.pricingTemplate;
        const dialogRef = this.distributionDialog.open(DistributionWizardMainComponent, {
            data: {
                pricingTemplate: pricingTemplate,
                fboId: this.sharedService.currentUser.fboId,
                groupId: this.sharedService.currentUser.groupId
            },
            disableClose: true
        });

        dialogRef.afterClosed().subscribe(result => {

        });
    }

    public customerMarginAmountChanged(customerMargin) {
        var indexOfMargin = this.pricingTemplate.customerMargins.indexOf(customerMargin);
        if (indexOfMargin > 0) {
            var previousTier = this.pricingTemplate.customerMargins[indexOfMargin - 1];
            if ((this.pricingTemplate.marginType == 0 || this.pricingTemplate.marginType == 2) && Math.abs(previousTier.amount) < Math.abs(customerMargin.amount))
                customerMargin.amount = previousTier.amount + .01;
            else if (this.pricingTemplate.marginType == 1 && Math.abs(previousTier.amount) > Math.abs(customerMargin.amount))
                customerMargin.amount = previousTier.amount - .01;
        }
        this.calculateItpForMargin(customerMargin);
    }

    public marginTypeChange() {
        this.loadCurrentPrice();
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

    private loadCurrentPrice() {
        this.fbopricesService.getFbopricesByFboIdAndProductCurrent(this.sharedService.currentUser.fboId, this.getCurrentProductForMarginType()).subscribe((data: any) => {
            this.currentPrice = data;
            for (let margin of this.pricingTemplate.customerMargins) {
                this.calculateItpForMargin(margin);
            }
        });
    }

    private getCurrentProductForMarginType() {
        if (this.pricingTemplate.marginType == 0)
            return 'JetA Cost';
        return 'JetA Retail';
    }

    private calculateItpForMargin(customerMargin) {
        if (this.pricingTemplate.marginType == 0)
            customerMargin.itp = this.currentPrice.price + Math.abs(customerMargin.amount);
        else if (this.pricingTemplate.marginType == 1)
            customerMargin.itp = this.currentPrice.price - Math.abs(customerMargin.amount);
        else
            customerMargin.itp = Math.abs(customerMargin.amount);
    }
}
