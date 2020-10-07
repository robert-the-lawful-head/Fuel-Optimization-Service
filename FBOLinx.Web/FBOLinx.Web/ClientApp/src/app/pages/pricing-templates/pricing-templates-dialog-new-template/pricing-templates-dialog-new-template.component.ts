import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';

// Services
import { FbopricesService } from '../../../services/fboprices.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';

import { CloseConfirmationComponent } from '../../../shared/components/close-confirmation/close-confirmation.component';

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
  templateUrl: './pricing-templates-dialog-new-template.component.html',
  styleUrls: ['./pricing-templates-dialog-new-template.component.scss'],
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
      value: 0
    },
    {
      text: 'Retail -',
      value: 1
    },
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef < PricingTemplatesDialogNewTemplateComponent > ,
    private closeConfirmationDialog: MatDialog,
    private formBuilder: FormBuilder,
    private priceTiersService: PricetiersService,
    private pricingTemplatesService: PricingtemplatesService,
    private fboPricesService: FbopricesService
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
          CloseConfirmationComponent, {
            data: {
              customTitle: 'Discard Changes?',
              customText: 'You have unsaved changes. Are you sure?',
              ok: 'Discard',
              cancel: 'Cancel',
            },
            autoFocus: false,
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

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.form = this.formBuilder.group({
      firstStep: this.formBuilder.group({
        templateName: ['', Validators.required],
        templateDefault: [false],
      }),
      secondStep: this.formBuilder.group({
        marginType: [1, Validators.required],
        customerMargins: this.formBuilder.array([
          this.formBuilder.group({
            min: [1],
            max: [99999],
            amount: [Number(0).toFixed(4)],
            itp: [0],
            allin: [0],
          }, {
            updateOn: 'blur'
          }),
        ]),
      }),
      thirdStep: this.formBuilder.group({
        subject: ['', Validators.required],
        email: ['', Validators.required],
        notes: ['', Validators.required],
      }),
    });

    const secondStep = this.form.controls.secondStep as FormGroup;
    secondStep.valueChanges.subscribe(() => {
      const updatedMargins = this.updateMargins(this.customerMarginsFormArray.value, this.marginType);
      this.customerMarginsFormArray.setValue(updatedMargins, {
        emitEvent: false,
      });
    });
  }

  get customerMarginsFormArray() {
    const secondStep = this.form.controls.secondStep as FormGroup;
    return secondStep.controls.customerMargins as FormArray;
  }

  get marginType() {
    return this.form.value.secondStep.marginType;
  }

  marginTypeChange() {
    const secondStep = this.form.controls.secondStep as FormGroup;
    secondStep.setControl('customerMargins', this.formBuilder.array([
      this.formBuilder.group({
        min: [1],
        max: [99999],
        amount: [0],
        itp: [0],
        allin: [0],
      }),
    ]));
  }

  addCustomerMargin() {
    const customerMargin = {
      min: 1,
      max: 99999,
      amount: Number(0).toFixed(4),
      itp: 0,
      allin: 0,
    };
    if (this.customerMarginsFormArray.length > 0) {
      const lastIndex = this.customerMarginsFormArray.length - 1;
      customerMargin.min = Math.abs(this.customerMarginsFormArray.at(lastIndex).value.min) + 250;
      this.customerMarginsFormArray.at(lastIndex).patchValue({
        max: Math.abs(customerMargin.min) - 1,
      });
    }

    this.customerMarginsFormArray.push(this.formBuilder.group(customerMargin, { updateOn: 'blur' }));
  }

  deleteCustomerMargin(index) {
    this.customerMarginsFormArray.removeAt(index);
  }

  addTemplateClicked() {
    this.isSaving = true;

    const templatePayload = {
      fboId: this.data.fboId,
      name: this.form.value.firstStep.templateName,
      default: this.form.value.firstStep.default,
      marginType: this.form.value.secondStep.marginType,
      subject: this.form.value.thirdStep.subject,
      email: this.form.value.thirdStep.email,
      notes: this.form.value.thirdStep.notes,
    };

    this.pricingTemplatesService
      .add(templatePayload)
      .subscribe((savedTemplate: any) => {
        const customerMargins = [];
        this.customerMarginsFormArray.value.forEach((customerMargin: any) => {
          customerMargins.push({
            ...customerMargin,
            templateId: savedTemplate.oid,
          });
        });

        this.priceTiersService
          .updateFromCustomerMarginsViewModel(
            customerMargins
          )
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
        this.jetACost = data.find(item => item.product === 'JetA Cost').price;
        this.jetARetail = data.find(item => item.product === 'JetA Retail').price;
      });
  }

  resetWizard() {
    this.initForm();
    this.stepper.reset();
  }

  private updateMargins(oldMargins, marginType) {
    const margins = [...oldMargins];
    for (let i = 0; i < margins?.length; i++) {
      if(margins[i].amount !== null || margins[i].amount !== '') {
        margins[i].amount = Number(margins[i].amount).toFixed(4);
      }
      if (i > 0) {
        margins[i - 1].max = Math.abs(margins[i].min - 1);
      }
      if (marginType !== 1) {
        if (margins[i].min !== null && margins[i].amount !== null) {
          margins[i].allin = this.jetACost + margins[i].amount;
        }
      } else {
        if (margins[i].amount !== null && margins[i].min !== null) {
          margins[i].allin = this.jetARetail - margins[i].amount;
          margins[i].itp = this.jetARetail;
          if (margins[i].allin) {
            margins[i].itp = margins[i].allin - this.jetACost;
          }
        }
      }
    }
    return margins;
  }
}
