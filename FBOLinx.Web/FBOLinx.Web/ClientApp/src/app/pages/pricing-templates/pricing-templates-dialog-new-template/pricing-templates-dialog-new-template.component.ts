import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

// Services
import { CustomermarginsService } from "../../../services/customermargins.service";
import { FbopricesService } from "../../../services/fboprices.service";
import { PricetiersService } from "../../../services/pricetiers.service";
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { RichTextEditorComponent } from "@syncfusion/ej2-angular-richtexteditor";
import { CloseConfirmationComponent } from "../../../shared/components/close-confirmation/close-confirmation.component";

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
    selector: "app-pricing-templates-dialog-new-template",
    templateUrl: "./pricing-templates-dialog-new-template.component.html",
    styleUrls: ["./pricing-templates-dialog-new-template.component.scss"],
})
export class PricingTemplatesDialogNewTemplateComponent implements OnInit {
    public firstFormGroup = this.formBuilder.group({
        templateName: ["", Validators.required],
        templateDefault: [""],
    });
    public secondFormGroup = this.formBuilder.group({
        marginType: ["", Validators.required],
    });
    public thirdFormGroup = this.formBuilder.group({
        subject: ["", Validators.required],
        email: ["", Validators.required],
    });
    @ViewChild("typeEmail") rteEmail: RichTextEditorComponent;
    @ViewChild("typeNotes") rteObj: RichTextEditorComponent;
    public currentPrice: any[];
    public focus: any = false;
    public count = 0;
    public title: string;
    public isSaving: boolean;
    public total = 0;
    public marginTypeDataSource: Array<any> = [
        { text: "Cost +", value: 0 },
        { text: "Retail -", value: 1 },
    ];

    constructor(
        public dialogRef: MatDialogRef<PricingTemplatesDialogNewTemplateComponent>,
        public closeConfirmationDialog: MatDialog,
        @Inject(MAT_DIALOG_DATA) public data: NewPricingTemplateDialogData,
        private formBuilder: FormBuilder,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fbopricesService: FbopricesService
    ) {
        this.loadCurrentPrice();
        this.title = "New Margin Template";

        // Prevent modal close on outside click
        dialogRef.disableClose = true;
        dialogRef.backdropClick().subscribe(() => {
            const marginName = this.firstFormGroup.get("templateName").value;
            if (!marginName) {
                dialogRef.close();
            } else {
                const closeDialogRef = this.closeConfirmationDialog.open(
                    CloseConfirmationComponent,
                    {
                        data: {
                            customTitle: "Discard Changes?",
                            customText:
                                "You have unsaved changes. Are you sure?",
                            ok: "Discard",
                            cancel: "Cancel",
                        },
                        autoFocus: false,
                    }
                );
                closeDialogRef.afterClosed().subscribe((result) => {
                    if (result === true) {
                        dialogRef.close();
                    }
                });
            }
        });
    }

    ngOnInit() {
        // Subscribe to necessary changes
        this.secondFormGroup.get("marginType").valueChanges.subscribe((val) => {
            this.data.marginType = val;
        });

        this.secondFormGroup.patchValue({
            marginType: this.data.marginType,
        });
    }

    onKey(event: KeyboardEvent) {}

    public modelChanged(event) {
        alert(event);
    }

    public disableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = false;
    }

    public enableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = true;
    }

    public marginTypeChange() {
        this.data.customerMargins.forEach((customerMargin: any) => {
            customerMargin.amount = 0;
            customerMargin.itp = 0;
            customerMargin.allin = 0;
        });
        this.loadCurrentPrice();
    }

    public addCustomerMargin() {
        const customerMargin = {
            oid: 0,
            templatesId: this.data.oid,
            priceTierId: 0,
            min: 1,
            max: 99999,
            amount: 0,
            itp: 0,
            allin: 0,
        };
        if (this.data.customerMargins.length > 0) {
            customerMargin.min =
                Math.abs(this.data.customerMargins[this.data.customerMargins.length - 1]
                    .min) + 250;
        }
        this.data.customerMargins[this.data.customerMargins.length - 1]
            .max = Math.abs(customerMargin.min) - 1;

        this.data.customerMargins.push(customerMargin);
    }

    public updateCustomerMargin(margin) {

        const index = this.data.customerMargins.findIndex(x => x.min === margin.min);

        if (index) {
            this.data.customerMargins[index - 1].max = margin.min - 1;
        }

        const jetACost = this.currentPrice.filter(
            (item) => item.product === "JetA Cost"
        )[0].price;
        const jetARetail = this.currentPrice.filter(
            (item) => item.product === "JetA Retail"
        )[0].price;

        if (this.data.marginType === 0) {
            if (margin.min && margin.amount) {
                margin.allin = jetACost + margin.amount;
            }
        } else if (this.data.marginType === 1) {
            if (margin.amount && margin.min) {
                margin.allin = jetARetail - margin.amount;
                if (margin.allin) {
                    margin.itp = margin.allin - jetACost;
                }
            }
        }
    }

    public empty(customerMargin) {
        customerMargin.amount = 0;
    }

    public onFocus() {
        this.focus = true;
    }

    public outFocus() {
        this.focus = false;
        this.count = 0;
    }

    public onType(customerMargin, event) {
        this.count = this.count + 1;
        if (this.focus && this.count === 1) {
            customerMargin.amount = event.key / 100;
        }
    }

    public customerMarginAmountChanged(customerMargin) {
        const indexOfMargin = this.data.customerMargins.indexOf(customerMargin);
        if (indexOfMargin > 0) {
            const previousTier = this.data.customerMargins[indexOfMargin - 1];
            if (
                (this.data.marginType === 0 || this.data.marginType === 2) &&
                Math.abs(previousTier.amount) < Math.abs(customerMargin.amount)
            ) {
                customerMargin.amount = previousTier.amount + 0.01;
            } else if (
                this.data.marginType === 1 &&
                Math.abs(previousTier.amount) > Math.abs(customerMargin.amount)
            ) {
                customerMargin.amount = previousTier.amount - 0.01;
            }
        }

        for (const margin of this.data.customerMargins) {
            this.total += margin.amount;
        }
    }

    public deleteCustomerMargin(customerMargin) {
        if (customerMargin.oid === 0) {
            this.data.customerMargins.splice(
                this.data.customerMargins.indexOf(customerMargin),
                1
            );
        } else {
            this.customerMarginsService.remove(customerMargin).subscribe(() => {
                this.priceTiersService
                    .remove({ oid: customerMargin.priceTierId })
                    .subscribe(() => {
                        this.data.customerMargins.splice(
                            this.data.customerMargins.indexOf(customerMargin),
                            1
                        );
                    });
            });
        }
    }

    public addTemplateClicked() {
        this.isSaving = true;

        this.data.name = this.firstFormGroup.get("templateName").value;
        this.data.marginType = Number(
            this.secondFormGroup.get("marginType").value
        );
        this.data.default =
            this.firstFormGroup.get("templateDefault").value === true;
        // tslint:disable-next-line deprecation
        this.data.notes = this.rteObj.contentModule.getEditPanel().innerHTML;

        this.pricingTemplatesService
            .add(this.data)
            .subscribe((savedTemplate: any) => {
                this.data.customerMargins.forEach((customerMargin: any) => {
                    customerMargin.templateId = savedTemplate.oid;
                });

                this.priceTiersService
                    .updateFromCustomerMarginsViewModel(
                        this.data.customerMargins
                    )
                    .subscribe((savedMargins: any) => {
                        this.isSaving = false;
                        this.dialogRef.close(this.data);
                    });
            });
    }

    public loadCurrentPrice() {
        this.fbopricesService
            .getFbopricesByFboIdCurrent(this.data.fboId)
            .subscribe((data: any) => {
                this.currentPrice = data;
            });
    }
}
