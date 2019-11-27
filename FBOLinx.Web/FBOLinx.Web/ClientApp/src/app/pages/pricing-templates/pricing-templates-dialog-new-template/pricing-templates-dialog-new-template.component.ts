import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { CustomermarginsService } from '../../../services/customermargins.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';

export interface NewPricingTemplateDialogData {
    oid: number;
    name: string;
    fboId: number;
    marginType: number;
    subject: string;
    email: any;
    notes: any;
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
    allin: number;
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
    @ViewChild('typeEmail') rteEmail: RichTextEditorComponent;
    @ViewChild('typeNotes') rteObj: RichTextEditorComponent;
    public currentPrice: any[];
    public focus: any = false;
    public count: number = 0;
    public title: string;
    public isSaving: boolean;
    public total: number = 0;
    public marginTypeDataSource: Array<any> = [
        { text: 'Cost +', value: 0 },
        { text: 'Retail -', value: 1 }
        //{ text: 'Flat Fee', value: 2 }
    ];

    /** pricing-templates-dialog-new-template ctor */
    constructor(public dialogRef: MatDialogRef<PricingTemplatesDialogNewTemplateComponent>, @Inject(MAT_DIALOG_DATA) public data: NewPricingTemplateDialogData,
        private formBuilder: FormBuilder,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fbopricesService: FbopricesService) {

        this.loadCurrentPrice();
        this.title = 'New Margin Template';
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
            subject: ['', Validators.required],
            email: ['', Validators.required]
        });
        //Subscribe to necessary changes
        this.secondFormGroup.get('marginType').valueChanges.subscribe(val => {
            this.data.marginType = val;
        });


        this.secondFormGroup.patchValue({
            marginType: this.data.marginType
        });


    }

    onKey(event: KeyboardEvent) {
        //alert(this.data.notes);
        null;
    }

    public modelChanged(event) {
        alert(event);
    }

    public disableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = false;

    }

    public enableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = true;
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
            itp: 0,
            allin: 0
        };
        if (this.data.customerMargins.length > 0) {
            customerMargin.min =
                this.data.customerMargins[this.data.customerMargins.length - 1].min + 250;
        }
        this.data.customerMargins.push(customerMargin);
       // this.fixCustomerMargins();
    }

    public updateCustomerMargin(margin) {
        var jetACost = this.currentPrice.filter(item => item.product == 'JetA Cost')[0].price;
        var jetARetail = this.currentPrice.filter(item => item.product == 'JetA Retail')[0].price;

        if (this.data.marginType == 0) {
            if (margin.min && margin.itp) {
                margin.allin = jetACost + margin.itp;
            }

        }
        else if (this.data.marginType == 1) {
            if (margin.amount && margin.min) {

                console.log(jetACost.price);
                margin.allin = jetARetail - margin.amount;
                if (margin.allin) {
                    margin.itp = margin.allin - jetACost;
                }
            }
        }        
    }

    //public fixCustomerMargins() {
    //    for (let i in this.data.customerMargins) {
    //        let indexNumber = Number(i);
    //        if (!this.data.customerMargins[indexNumber].min || this.data.customerMargins[indexNumber].min == 0) {
    //            if (indexNumber == 0)
    //                this.data.customerMargins[indexNumber].min = 1;
    //            else
    //                this.data.customerMargins[indexNumber].min =
    //                    (this.data.customerMargins[indexNumber - 1].min + 1);
    //        }

    //        if (indexNumber > 0 && this.data.customerMargins[indexNumber].min == this.data.customerMargins[indexNumber - 1].min) {
    //            this.data.customerMargins[indexNumber].min =
    //                (this.data.customerMargins[indexNumber - 1].min + 1);
    //        }

    //        if (this.data.customerMargins.length > (indexNumber + 1) &&
    //            this.data.customerMargins[indexNumber + 1].min > 0)
    //            this.data.customerMargins[i].max = this.data.customerMargins[indexNumber + 1].min - 1;
    //        else
    //            this.data.customerMargins[i].max = 99999;

    //        this.calculateItpForMargin(this.data.customerMargins[indexNumber]);
    //        for (let margin of this.data.customerMargins) {
    //            this.total += margin.amount;
    //        }
    //    }
    //}

    public empty(customerMargin) {
        customerMargin.amount = 0;
    }

    public onFocus() {
        this.focus =true;
    }

    public outFocus() {
        this.focus = false;
        this.count = 0;
    }


    public onType(customerMargin, event) {
        this.count = this.count + 1;
        if (this.focus && this.count == 1) {
            customerMargin.amount = event.key/100;
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
       // this.calculateItpForMargin(customerMargin);
        for (let margin of this.data.customerMargins) {
            this.total += margin.amount;
        }
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
        //this.data.notes = this.thirdFormGroup.get('note').value;
        //alert(JSON.stringify(this.data));
        //this.data.notes = this.thirdFormGroup.get('notes').value;
        /*this.data.email = this.thirdFormGroup.value.email;
        this.data.subject = this.thirdFormGroup.value.subject;*/
       // alert((this.rteObj.contentModule.getEditPanel()).innerHTML);
        //return;
        this.data.notes = (this.rteObj.contentModule.getEditPanel()).innerHTML;
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
        /*this.fbopricesService.getFbopricesByFboIdAndProductCurrent(this.data.fboId, this.getCurrentProductForMarginType()).subscribe((data: any) => {
            this.currentPrice = data;
            for (let margin of this.data.customerMargins) {
                this.calculateItpForMargin(margin);
            }
        });*/
        this.fbopricesService.getFbopricesByFboIdCurrent(this.data.fboId).subscribe((data: any) => {
            this.currentPrice = data;
            console.log(this.data.customerMargins);
                for (let margin of this.data.customerMargins) {
                   // this.calculateItpForMargin(margin);
                }
        })
    }

    private getCurrentProductForMarginType() {
        if (this.data.marginType == 0)
            return 'JetA Cost';
        return 'JetA Retail';
    }

    private calculateItpForMargin(customerMargin) {
        for (let m of this.currentPrice) {
            if (this.data.marginType == 0 && this.getCurrentProductForMarginType() == m.product) {
                customerMargin.itp = m ? m.price : 0 + Math.abs(customerMargin.amount);
                return;
            }
            else if (this.data.marginType == 1 && this.getCurrentProductForMarginType() == m.product) {
                customerMargin.itp = m ? m.price : 0 - Math.abs(customerMargin.amount);
                return;
            }
            else if (this.data.marginType>1) {
                customerMargin.itp = Math.abs(customerMargin.amount);
                return;
                }
        }
    }

    private calculateRetailVolumeTiers() {

    }
}
