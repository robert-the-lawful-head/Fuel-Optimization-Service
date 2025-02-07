import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { UntypedFormArray, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import {
    MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
    MatLegacyDialog as MatDialog,
    MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';
import { MatStepper } from '@angular/material/stepper';
import { ImageSettingsModel, RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';

import { SharedService } from '../../../layouts/shared-service';
import { EmailcontentService } from '../../../services/emailcontent.service';
// Services
import { FbopricesService } from '../../../services/fboprices.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { CloseConfirmationComponent } from '../../../shared/components/close-confirmation/close-confirmation.component';
import { EmailTemplatesDialogNewTemplateComponent } from '../../../shared/components/email-templates-dialog-new-template/email-templates-dialog-new-template.component';
import { ProceedConfirmationComponent } from '../../../shared/components/proceed-confirmation/proceed-confirmation.component';
import { PricingTemplateCalcService } from '../pricingTemplateCalc.service';
import { DecimalPrecisionPipe } from 'src/app/shared/pipes/decimal/decimal-precision.pipe';
import { StringHelperService } from 'src/app/helpers/strings/stringHelper.service';
import { Subscription } from 'rxjs';

export interface NewPricingTemplateMargin {
    allin: UntypedFormControl;
    amount: UntypedFormControl;
    itp: UntypedFormControl;
    max: UntypedFormControl;
    min: UntypedFormControl;
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

    form: UntypedFormGroup;

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
        { text: 'Percentage', value: 1 },
     ];

    emailTemplatesDataSource: Array<any>;
    public insertImageSettings: ImageSettingsModel = { saveFormat: 'Base64' }
    inputStepDefaultValue: string = this.stringHelperService.getNumberInputStepDefaultValue();
    valueChangeSubscription: Subscription;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        private dialogRef: MatDialogRef<PricingTemplatesDialogNewTemplateComponent>,
        private closeConfirmationDialog: MatDialog,
        private formBuilder: UntypedFormBuilder,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fboPricesService: FbopricesService,
        private sharedService: SharedService,
        private emailContentService: EmailcontentService,
        public newTemplateDialog: MatDialog,
        private marginLessThanOneDialog: MatDialog,
        private pricingTemplateCalcService: PricingTemplateCalcService,
        private decimalPrecisionPipe: DecimalPrecisionPipe,
        private stringHelperService: StringHelperService
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
        const secondStep = this.form.controls.secondStep as UntypedFormGroup;
        return secondStep.controls.customerMargins as UntypedFormArray;
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
    ngOnDestroy() {
        this.valueChangeSubscription?.unsubscribe();
    }
    initForm() {
        this.form = this.formBuilder.group({
            firstStep: this.formBuilder.group({
                templateDefault: [false],
                templateName: ['', Validators.required],

            }),
            secondStep: this.formBuilder.group({
                customerMargins: this.formBuilder.array([
                    this.formBuilder.group({
                            allin: new UntypedFormControl(0),
                            amount: new UntypedFormControl(this.decimalPrecisionPipe.transform(Number(0),true)),
                            itp: new UntypedFormControl(0),
                            max: new UntypedFormControl({value: 99999, disabled: true}, Validators.required),
                            min: new UntypedFormControl(1),
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

        const secondStep = this.form.controls.secondStep as UntypedFormGroup;
        this.valueChangeSubscription = secondStep.valueChanges.subscribe(() => {
            const updatedMargins = this.updateMargins(
                this.customerMarginsFormArray.getRawValue(),
                this.marginType ,
                this.discountType
            );
            this.customerMarginsFormArray.setValue(updatedMargins, {
                emitEvent: false,
            });
        });
    }

    marginTypeChange() {
        const secondStep = this.form.controls.secondStep as UntypedFormGroup;
        secondStep.setControl(
            'customerMargins',
            this.formBuilder.array([
                this.formBuilder.group({
                        allin: new UntypedFormControl(0),
                        amount: new UntypedFormControl(this.decimalPrecisionPipe.transform(Number(0))),
                        itp: new UntypedFormControl(0),
                        max: new UntypedFormControl({value: 99999, disabled: true}, Validators.required),
                        min: new UntypedFormControl(1),
                    },
                    {
                        updateOn: 'blur'
                    }
                ),
            ])
        );
    }
    updateCustomerMarginVolumeValues(index: number){
        this.pricingTemplateCalcService.adjustCustomerMarginPreviousValues(index,this.customerMarginsFormArray);
        this.pricingTemplateCalcService.adjustCustomerMarginNextValues(index,this.customerMarginsFormArray);
    }

    addCustomerMargin() {
        const customerMargin: NewPricingTemplateMargin = {
            allin: new UntypedFormControl(0),
            amount: new UntypedFormControl(this.decimalPrecisionPipe.transform(Number(0))),
            itp: new UntypedFormControl(0),
            max: new UntypedFormControl({value: 99999, disabled: true}, Validators.required),
            min: new UntypedFormControl(1),
        };
        if (this.customerMarginsFormArray.length > 0) {
            const lastIndex = this.customerMarginsFormArray.length - 1;

            customerMargin.min.setValue(
                Math.abs(
                    this.customerMarginsFormArray.at(lastIndex).value.min
                ) + 250);
            this.customerMarginsFormArray.at(lastIndex).patchValue({
                max: Math.abs(customerMargin.min.value) - 1,
            });
        }

        this.customerMarginsFormArray.push(
            this.formBuilder.group(customerMargin, { updateOn: 'blur' })
        );
    }

    deleteCustomerMargin(index) {
        this.pricingTemplateCalcService.adjustCustomerMarginValuesOnDelete(index,this.customerMarginsFormArray);
    }

    addTemplateClicked() {
        var isMarginLessThanZero = false;
        if (this.form.value.secondStep.marginType == 0) {
            this.customerMarginsFormArray.value.every(
                (customerMargin: any) => {
                    if (customerMargin.amount <= 0) {
                        isMarginLessThanZero = true;
                        return false;
                    }
                });
        }
        else {
            this.customerMarginsFormArray.value.every(
                (customerMargin: any) => {
                    if (customerMargin.amount <= 0) {
                        isMarginLessThanZero = true;
                        return false;
                    }
                });
        }

        if (!isMarginLessThanZero) {
            this.completeTemplateAdd();
        }
        else {
            const dialogRef = this.marginLessThanOneDialog.open(
                ProceedConfirmationComponent,
                {
                    autoFocus: false,
                    data: {
                        buttonText: 'Yes',
                        title: 'This ITP template contains a margin that is less than or equal to zero.  Please confirm you want to proceed'
                    },
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }
                this.completeTemplateAdd();
            });
        }
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

    private completeTemplateAdd() {
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
                        this.fboPricesService.handlePriceChangeCleanUp(this.sharedService.currentUser.fboId).subscribe(
                            (response:
                                any) => {
                                this.dialogRef.close(savedTemplate);
                            });

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
                margins[i].amount = this.decimalPrecisionPipe.transform(margins[i].amount);
            }
        }
        return margins;
    }
}
