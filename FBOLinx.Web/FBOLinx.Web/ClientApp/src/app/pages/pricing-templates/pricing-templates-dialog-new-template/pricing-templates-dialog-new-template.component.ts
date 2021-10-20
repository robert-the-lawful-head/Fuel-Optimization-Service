import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
    MAT_DIALOG_DATA,
    MatDialog,
    MatDialogRef,
} from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';

import { SharedService } from '../../../layouts/shared-service';
import { EmailcontentService } from '../../../services/emailcontent.service';
// Services
import { FbopricesService } from '../../../services/fboprices.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { CloseConfirmationComponent } from '../../../shared/components/close-confirmation/close-confirmation.component';
import { EmailTemplatesDialogNewTemplateComponent } from '../../../shared/components/email-templates-dialog-new-template/email-templates-dialog-new-template.component';

export interface NewPricingTemplateMargin {
    min?: number;
    amount?: number;
    itp?: number;
    max?: number;
    templatesId?: number;
    allin?: number;

}

@Component({
    selector: 'app-pricing-templates-dialog-new-template',
    styleUrls: ['./pricing-templates-dialog-new-template.component.scss'],
    templateUrl: './pricing-templates-dialog-new-template.component.html',
})
export class PricingTemplatesDialogNewTemplateComponent implements OnInit {
    @ViewChild('typeEmail') rteEmail: RichTextEditorComponent;
    @ViewChild('typeNotes') rteObj: RichTextEditorComponent;
    @ViewChild('stepper') stepper: MatStepper;

    form: FormGroup;

    jetACost: number;
    jetARetail: number;
    currentPrice: any[];
    title: string;
    isSaving: boolean;
    marginTypeDataSource: Array<any> = [
        {
            text: 'Cost +',
            value: 0,
        },
        {
            text: 'Retail -',
            value: 1,
        },
    ];

    discountTypeDataSource: Array<any> = [
        { text: 'Flat per Gallon', value: 0 },
        { text: 'Precentage', value: 1 },
     ];

    emailTemplatesDataSource: Array<any>;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        private dialogRef: MatDialogRef<PricingTemplatesDialogNewTemplateComponent>,
        private closeConfirmationDialog: MatDialog,
        private formBuilder: FormBuilder,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fboPricesService: FbopricesService,
        private sharedService: SharedService,
        private emailContentService: EmailcontentService,
        public newTemplateDialog: MatDialog
    ) {
        this.loadCurrentPrice();
        this.title = 'New Margin Template';

        // Prevent modal close on outside click
        this.dialogRef.disableClose = true;
        this.dialogRef.backdropClick().subscribe(() => {
            const marginName = this.form.value.firstStep.templateName;
            if (!marginName) {
                this.dialogRef.close();
            } else {
                const closeDialogRef = this.closeConfirmationDialog.open(
                    CloseConfirmationComponent,
                    {
                        autoFocus: false,
                        data: {
                            cancel: 'Cancel',
                            customText:
                                'You have unsaved changes. Are you sure?',
                            customTitle: 'Discard Changes?',
                            ok: 'Discard',
                        },
                    }
                );
                closeDialogRef.afterClosed().subscribe((result) => {
                    if (result === true) {
                        this.dialogRef.close();
                    }
                });
            }
        });
    }

    get customerMarginsFormArray() {
        const secondStep = this.form.controls.secondStep as FormGroup;
        return secondStep.controls.customerMargins as FormArray;
    }

    get marginType() {
        return this.form.value.secondStep.marginType;
    }

    get discountType() {

        return this.form.value.secondStep.discountType;
    }


    ngOnInit() {
        this.initForm();
        this.loadEmailContentTemplate();
    }

    initForm() {
        this.form = this.formBuilder.group({
            firstStep: this.formBuilder.group({
                templateDefault: [false],
                templateName: ['', Validators.required],

            }),
            secondStep: this.formBuilder.group({
                customerMargins: this.formBuilder.array([
                    this.formBuilder.group(
                        {
                            allin: [0],
                            amount: [Number(0).toFixed(4)],
                            itp: [0],
                            max: [99999],
                            min: [1],
                        },
                        {
                            updateOn: 'blur',
                        }
                    ),
                ]),
                marginType: [1, Validators.required],
                discountType :[0 , Validators.required]
            }),
            thirdStep: this.formBuilder.group({
                emailContentId: [''],
                notes: [''],
            }),
        });

        const secondStep = this.form.controls.secondStep as FormGroup;
        secondStep.valueChanges.subscribe(() => {
            const updatedMargins = this.updateMargins(
                this.customerMarginsFormArray.value,
                this.marginType ,
                this.discountType
            );
            this.customerMarginsFormArray.setValue(updatedMargins, {
                emitEvent: false,
            });
        });
    }

    marginTypeChange() {
        const secondStep = this.form.controls.secondStep as FormGroup;
        secondStep.setControl(
            'customerMargins',
            this.formBuilder.array([
                this.formBuilder.group(
                    {
                        allin: [0],
                        amount: [Number(0).toFixed(4)],
                        itp: [0],
                        max: [99999],
                        min: [1],
                    },
                    {
                        updateOn: 'blur',
                    }
                ),
            ])
        );
    }

    addCustomerMargin() {
        const customerMargin = {
            allin: 0,
            amount: Number(0).toFixed(4),
            itp: 0,
            max: 99999,
            min: 1,
        };
        if (this.customerMarginsFormArray.length > 0) {
            const lastIndex = this.customerMarginsFormArray.length - 1;
            customerMargin.min =
                Math.abs(
                    this.customerMarginsFormArray.at(lastIndex).value.min
                ) + 250;
            this.customerMarginsFormArray.at(lastIndex).patchValue({
                max: Math.abs(customerMargin.min) - 1,
            });
        }

        this.customerMarginsFormArray.push(
            this.formBuilder.group(customerMargin, { updateOn: 'blur' })
        );
    }

    deleteCustomerMargin(index) {
        this.customerMarginsFormArray.removeAt(index);
    }

    addTemplateClicked() {


        this.isSaving = true;

        const templatePayload = {
            default: this.form.value.firstStep.templateDefault,
            emailContentId: this.form.value.thirdStep.emailContentId,
            fboId: this.data.fboId,
            marginType: this.form.value.secondStep.marginType,
            discountType: this.discountType,
            name: this.form.value.firstStep.templateName,
            notes: this.form.value.thirdStep.notes,
        };


        this.pricingTemplatesService
            .add(templatePayload)
            .subscribe((savedTemplate: any) => {
                const customerMargins = [];
                this.customerMarginsFormArray.value.forEach(
                    (customerMargin: any) => {
                        customerMargins.push({
                            ...customerMargin,
                            templateId: savedTemplate.oid,
                        });
                    }
                );

                this.priceTiersService
                    .updateFromCustomerMarginsViewModel(customerMargins)
                    .subscribe(() => {
                        this.isSaving = false;
                        this.dialogRef.close(savedTemplate);
                    });
            });
    }

    loadCurrentPrice() {
        this.fboPricesService
            .getFbopricesByFboIdCurrent(this.data.fboId)
            .subscribe((data: any) => {
                this.jetACost = data.find(
                    (item) => item.product === 'JetA Cost'
                ).price;
                this.jetARetail = data.find(
                    (item) => item.product === 'JetA Retail'
                ).price;
            });
    }

    resetWizard() {
        this.initForm();
        this.stepper.reset();
    }

    public onEmailConentTemplateChanged(event): void {
        if (this.form.value.thirdStep.emailContentId > -1)
            return;

        const dialogRef = this.newTemplateDialog.open(
            EmailTemplatesDialogNewTemplateComponent,
            {
                data: {
                    fboId: this.sharedService.currentUser.fboId,
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.emailContentService
                .add(result)
                .subscribe((response: any) => {
                    this.form.get(['thirdStep', 'emailContentId']).setValue(response.oid);
                    this.loadEmailContentTemplate();
                });
        });
    }

    private loadEmailContentTemplate(): void {
        this.emailContentService
            .getForFbo(this.sharedService.currentUser.fboId)
            .subscribe((response: any) => {
                this.emailTemplatesDataSource = response;
                this.emailTemplatesDataSource.push({
                    oid: -1,
                    name: '--Add New Email Template--',
                    subject: ''
                });
            });
    }

    private updateMargins(oldMargins, marginType , discountType) {
        const margins = [...oldMargins];
        for (let i = 0; i < margins?.length; i++) {
            if (i > 0) {
                margins[i - 1].max = Math.abs(margins[i].min - 1);
            }

            if (marginType == 0) {
                if (margins[i].min !== null && margins[i].amount !== null) {
                       if(discountType == 0)
                       {
                        margins[i].allin = Number(margins[i].amount);
                       }
                       else
                       {
                        margins[i].allin =
                        this.jetACost * Number(margins[i].amount)/100;
                       }

                       margins[i].itp = this.jetACost;
                       if (margins[i].allin !== null) {
                           margins[i].itp = margins[i].allin ;
                       }

                }
            } else {
                if (margins[i].amount !== null && margins[i].min !== null) {

                    if(discountType == 0)
                    {
                        margins[i].allin =
                         this.jetARetail - Number(margins[i].amount);
                    }
                    else
                    {
                        margins[i].allin =
                        this.jetARetail * (1 - (Number(margins[i].amount) /100));
                    }


                    if (margins[i].allin !== null) {

                        margins[i].itp = margins[i].allin -  this.jetACost ;
                    }
                    else
                    {
                        margins[i].itp = this.jetARetail;
                    }
                }
            }
            if (margins[i].amount !== null || margins[i].amount !== '') {
                margins[i].amount = Number(margins[i].amount).toFixed(4);
            }
        }
        return margins;
    }
}
