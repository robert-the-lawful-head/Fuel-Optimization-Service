import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { CustomermarginsService } from '../../../services/customermargins.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';

export interface NewPricingTemplateDialogData {
    oid: number;
    name: string;
    fboId: number;
    marginType: number;
    notes: string;
    customerMargins: NewPricingTemplateMargin[];
    default: boolean;
}

export interface NewPricingTemplateMargin {
    oid: number;
    min: number;
    amount: number;
    itp: number;
    priceTierId: number;
    max: number;
    templatesId: number;
}

@Component({
    selector: 'app-pricing-templates-dialog-new-template',
    templateUrl: './pricing-templates-dialog-new-template.component.html',
    styleUrls: ['./pricing-templates-dialog-new-template.component.scss']
})
/** pricing-templates-dialog-new-template component*/
export class PricingTemplatesDialogNewTemplateComponent implements OnInit {

    public firstFormGroup: FormGroup;
    public secondFormGroup: FormGroup;
    public thirdFormGroup: FormGroup;
    public currentPrice: any;
    public isSaving: boolean;
    public marginTypeDataSource: Array<any> = [
        { text: 'Cost +', value: 0 },
        { text: 'Retail -', value: 1 },
        { text: 'Flat Fee', value: 2 },
        { text: 'Inactive', value: 3 }
    ];

    /** pricing-templates-dialog-new-template ctor */
    constructor(public dialogRef: MatDialogRef<PricingTemplatesDialogNewTemplateComponent>, @Inject(MAT_DIALOG_DATA) public data: NewPricingTemplateDialogData,
        private formBuilder: FormBuilder,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fbopricesService: FbopricesService) {

        this.loadCurrentPrice();
    }

    ngOnInit() {

        this.firstFormGroup = this.formBuilder.group({
            templateName: ['', Validators.required],
            templateDefault: ['']
        });
        this.secondFormGroup = this.formBuilder.group({
            marginType: ['', Validators.required]
        });
        this.thirdFormGroup = this.formBuilder.group({
            emailContentSignature: ['', Validators.required],
            emailContentSignatureName: ['', Validators.required]
        });

        //Subscribe to necessary changes
        this.secondFormGroup.get('marginType').valueChanges.subscribe(val => {
            this.data.marginType = val;
        });

        this.secondFormGroup.patchValue({
            marginType: this.data.marginType
        });
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public marginTypeChange() {
        this.loadCurrentPrice();
    }

    public addCustomerMargin() {
        let customerMargin = {
            oid: 0,
            templatesId: this.data.oid,
            priceTierId: 0,
            min: 1,
            max: 99999,
            amount: 0,
            itp: 0
        };
        if (this.data.customerMargins.length > 0) {
            customerMargin.min =
                this.data.customerMargins[this.data.customerMargins.length - 1].min + 250;
        }
        this.data.customerMargins.push(customerMargin);
        this.fixCustomerMargins();
    }

    public fixCustomerMargins() {
        for (let i in this.data.customerMargins) {
            let indexNumber = Number(i);
            if (!this.data.customerMargins[indexNumber].min || this.data.customerMargins[indexNumber].min == '') {
                if (indexNumber == 0)
                    this.data.customerMargins[indexNumber].min = 1;
                else
                    this.data.customerMargins[indexNumber].min =
                        (this.data.customerMargins[indexNumber - 1].min + 1);
            }

            if (indexNumber > 0 && this.data.customerMargins[indexNumber].min == this.data.customerMargins[indexNumber - 1].min) {
                this.data.customerMargins[indexNumber].min =
                    (this.data.customerMargins[indexNumber - 1].min + 1);
            }

            if (this.data.customerMargins.length > (indexNumber + 1) &&
                this.data.customerMargins[indexNumber + 1].min > 0)
                this.data.customerMargins[i].max = this.data.customerMargins[indexNumber + 1].min - 1;
            else
                this.data.customerMargins[i].max = 99999;

            this.calculateItpForMargin(this.data.customerMargins[indexNumber]);
        }
    }

    public customerMarginAmountChanged(customerMargin) {
        var indexOfMargin = this.data.customerMargins.indexOf(customerMargin);
        if (indexOfMargin > 0) {
            var previousTier = this.data.customerMargins[indexOfMargin - 1];
            if ((this.data.marginType == 0 || this.data.marginType == 2) && Math.abs(previousTier.amount) < Math.abs(customerMargin.amount))
                customerMargin.amount = previousTier.amount + .01;
            else if (this.data.marginType == 1 && Math.abs(previousTier.amount) > Math.abs(customerMargin.amount))
                customerMargin.amount = previousTier.amount - .01;
        }
        this.calculateItpForMargin(customerMargin);
    }

    public deleteCustomerMargin(customerMargin) {
        if (customerMargin.oid == 0) {
            this.data.customerMargins.splice(
                this.data.customerMargins.indexOf(customerMargin),
                1);
        } else {
            this.customerMarginsService.remove(customerMargin).subscribe((data: any) => {
                this.priceTiersService.remove({ oid: customerMargin.priceTierId }).subscribe((data: any) => {
                    this.data.customerMargins.splice(
                        this.data.customerMargins.indexOf(customerMargin),
                        1);
                });
            });
        }
    }

    public addTemplateClicked() {
        this.isSaving = true;

        this.data.name = this.firstFormGroup.get('templateName').value;
        this.data.marginType = Number(this.secondFormGroup.get('marginType').value);
        this.data.default = (this.firstFormGroup.get('templateDefault').value == true);

        this.pricingTemplatesService.add(this.data).subscribe((savedTemplate: any) => {
            this.data.customerMargins.forEach((customerMargin: any) => {
                customerMargin.templateId = savedTemplate.oid;
            });

            this.priceTiersService.updateFromCustomerMarginsViewModel(this.data.customerMargins).subscribe(
                (savedMargins: any) => {
                    this.isSaving = false;
                    this.dialogRef.close(this.data);
                });
        });
    }

    //Private Methods
    private loadCurrentPrice() {
        this.fbopricesService.getFbopricesByFboIdAndProductCurrent(this.data.fboId, this.getCurrentProductForMarginType()).subscribe((data: any) => {
            this.currentPrice = data;
            for (let margin of this.data.customerMargins) {
                this.calculateItpForMargin(margin);
            }
        });
    }

    private getCurrentProductForMarginType() {
        if (this.data.marginType == 0)
            return 'JetA Cost';
        return 'JetA Retail';
    }

    private calculateItpForMargin(customerMargin) {
        if (this.data.marginType == 0)
            customerMargin.itp = this.currentPrice.price + Math.abs(customerMargin.amount);
        else if (this.data.marginType == 1)
            customerMargin.itp = this.currentPrice.price - Math.abs(customerMargin.amount);
        else
            customerMargin.itp = Math.abs(customerMargin.amount);
    }
}
