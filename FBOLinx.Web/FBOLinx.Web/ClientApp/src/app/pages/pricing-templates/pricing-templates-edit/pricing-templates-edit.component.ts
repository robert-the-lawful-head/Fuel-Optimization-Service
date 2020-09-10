import {
    Component,
    EventEmitter,
    Input,
    Output,
    ViewChild,
    OnInit,
} from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder, FormGroup, FormArray } from "@angular/forms";

import { combineLatest } from "rxjs";
import { forOwn, differenceBy } from "lodash";

import { RichTextEditorComponent } from "@syncfusion/ej2-angular-richtexteditor";

// Services
import { CustomermarginsService } from "../../../services/customermargins.service";
import { FbopricesService } from "../../../services/fboprices.service";
import { PricetiersService } from "../../../services/pricetiers.service";
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { SharedService } from "../../../layouts/shared-service";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "/default-layout",
    },
    {
        title: "ITP Margin Templates",
        link: "/default-layout/pricing-templates",
    },
    {
        title: "Edit Margin Template",
        link: "",
    },
];

@Component({
    selector: "app-pricing-templates-edit",
    templateUrl: "./pricing-templates-edit.component.html",
    styleUrls: ["./pricing-templates-edit.component.scss"],
})
export class PricingTemplatesEditComponent implements OnInit {
    // Input/Output Bindings
    @Output() savePricingTemplateClicked = new EventEmitter<any>();
    @Input() pricingTemplate: any;
    @ViewChild("typeRTE") rteObj: RichTextEditorComponent;
    @ViewChild("typeEmail") rteEmail: RichTextEditorComponent;

    pricingTemplateForm: FormGroup;

    // Members
    pageTitle = "Edit Margin Template";
    breadcrumb: any[] = BREADCRUMBS;
    marginTypeDataSource: Array<any> = [
        { text: "Cost +", value: 0 },
        { text: "Retail -", value: 1 },
    ];
    canSave: boolean;
    jetACost: number;
    jetARetail: number;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private formBuilder: FormBuilder,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fboPricesService: FbopricesService,
        private sharedService: SharedService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    get customerMarginsFormArray() {
        return this.pricingTemplateForm.controls.customerMargins as FormArray;
    }

    ngOnInit(): void {
        // Check for passed in id
        const id = this.route.snapshot.paramMap.get("id");

        combineLatest([
            this.pricingTemplatesService.get({ oid: id }),
            this.customerMarginsService.getCustomerMarginsByPricingTemplateId(id),
            this.fboPricesService.getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId),
        ]).subscribe(([pricingTemplateData, customerMarginsData, fboPricesData]) => {
            this.jetACost = (fboPricesData as any).filter(item => item.product === "JetA Cost")[0].price;
            this.jetARetail = (fboPricesData as any).filter(item => item.product === "JetA Retail")[0].price;

            this.pricingTemplate = pricingTemplateData;
            this.pricingTemplate.customerMargins = this.updateMargins(customerMarginsData, this.pricingTemplate.marginType);

            this.pricingTemplateForm = this.formBuilder.group({
                name: [this.pricingTemplate.name],
                default: [this.pricingTemplate.default],
                marginType: [this.pricingTemplate.marginType],
                customerMargins: this.formBuilder.array(
                    this.pricingTemplate.customerMargins?.map(customerMargin => {
                        const group = {
                            itp: undefined,
                        };
                        forOwn(customerMargin, (value, key) => {
                            group[key] = [value];
                        });
                        return this.formBuilder.group(group);
                    })
                ),
                notes: [this.pricingTemplate.notes],
                subject: [this.pricingTemplate.subject],
                email: [this.pricingTemplate.email],
            });

            this.pricingTemplateForm.valueChanges.subscribe(() => {
                this.canSave = true;
            });

            this.pricingTemplateForm.controls.marginType.valueChanges.subscribe(type => {
                const updatedMargins =
                    this.updateMargins(this.pricingTemplateForm.value.customerMargins, type);
                this.pricingTemplateForm.controls.customerMargins.setValue(updatedMargins, {
                    emitEvent: false,
                });
            });
            this.pricingTemplateForm.controls.customerMargins.valueChanges.subscribe(margins => {
                const updatedMargins = this.updateMargins(margins, this.pricingTemplateForm.value.marginType);
                this.pricingTemplateForm.controls.customerMargins.setValue(updatedMargins, {
                    emitEvent: false,
                });
            });
        });
    }

    savePricingTemplate() {
        const removedCustomerMargins =
            differenceBy(this.pricingTemplate.customerMargins, this.pricingTemplateForm.value.customerMargins, "oid");

        combineLatest([
            this.customerMarginsService.bulkRemove(removedCustomerMargins),
            this.priceTiersService.updateFromCustomerMarginsViewModel(this.pricingTemplateForm.value.customerMargins),
            this.pricingTemplatesService.update({
                ...this.pricingTemplate,
                ...this.pricingTemplateForm.value,
            }),
        ]).subscribe(() => {
            this.canSave = false;

            this.sharedService.NotifyPricingTemplateComponent("updateComponent");
        });
    }

    cancelPricingTemplateEdit() {
        this.router.navigate(["/default-layout/pricing-templates/"]).then(() => {});
    }

    deleteCustomerMargin(index: number) {
        this.customerMarginsFormArray.removeAt(index);
        if (this.customerMarginsFormArray.length) {
            this.customerMarginsFormArray.at(this.customerMarginsFormArray.length - 1).patchValue({
                max: 99999,
            });
        }
    }

    addCustomerMargin() {
        const customerMargin = {
            oid: 0,
            templateId: this.pricingTemplate.oid,
            priceTierId: 0,
            min: 1,
            max: 99999,
            amount: 0,
            itp: 0,
            allin: 0,
        };
        if (this.customerMarginsFormArray.length > 0) {
            const lastIndex = this.customerMarginsFormArray.length - 1;
            customerMargin.min = Math.abs(this.customerMarginsFormArray.at(lastIndex).value.min) + 250;
            this.customerMarginsFormArray.at(lastIndex).patchValue({
                max: Math.abs(customerMargin.min) - 1,
            }, {
                emitEvent: false,
            });
        }
        const group = {};
        forOwn(customerMargin, (value, key) => {
            group[key] = [value];
        });
        this.customerMarginsFormArray.push(this.formBuilder.group(group));
    }

    private updateMargins(oldMargins, marginType) {
        const margins = [...oldMargins];
        for(let i = 0; i < margins?.length; i++) {
            if ( i > 0) {
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
