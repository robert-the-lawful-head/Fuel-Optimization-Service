import {
    Component,
    EventEmitter,
    Input,
    Output,
    ViewChild,
    OnInit,
} from "@angular/core";
import { MatDialog, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

// Services
import { CustomermarginsService } from "../../../services/customermargins.service";
import { FbopricesService } from "../../../services/fboprices.service";
import { PricetiersService } from "../../../services/pricetiers.service";
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { SharedService } from "../../../layouts/shared-service";

// Components
import { DistributionWizardMainComponent } from "../../../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component";
import { RichTextEditorComponent } from "@syncfusion/ej2-angular-richtexteditor";

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
    // Private Members
    private requiresRouting = false;

    // Input/Output Bindings
    @Output() savePricingTemplateClicked = new EventEmitter<any>();
    @Output() cancelPricingTemplateEditclicked = new EventEmitter<any>();
    @Input() pricingTemplate: any;
    @ViewChild("typeRTE") rteObj: RichTextEditorComponent;
    @ViewChild("typeEmail") rteEmail: RichTextEditorComponent;

    // Public Members
    public focus: any = false;
    public count = 0;
    public pageTitle = "Edit Margin Template";
    public breadcrumb: any[] = BREADCRUMBS;
    public marginTypeDataSource: Array<any> = [
        { text: "Cost +", value: 0 },
        { text: "Retail -", value: 1 },
    ];
    public isSaving = false;
    public currentPrice: any;
    public product: any;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fbopricesService: FbopricesService,
        private sharedService: SharedService,
        public distributionDialog: MatDialog
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit(): void {
        // Check for passed in id
        const id = this.route.snapshot.paramMap.get("id");
        if (!id) {
            this.loadCurrentPrice();
            return;
        } else {
            this.requiresRouting = true;
            this.pricingTemplatesService
                .get({ oid: id })
                .subscribe((data: any) => {
                    this.pricingTemplate = data;
                    this.customerMarginsService
                        .getCustomerMarginsByPricingTemplateId(
                            this.pricingTemplate.oid
                        )
                        .subscribe((result: any) => {
                            this.pricingTemplate.customerMargins = result;

                            this.loadCurrentPrice();
                        });
                });
        }
    }

    public disableToolbarNote() {
        // this.rteObj.toolbarSettings.enable = false;
    }

    public enableToolbarNote() {
        this.pricingTemplate.notes = this.rteObj.getText();
        this.rteObj.toolbarSettings.enable = true;
    }

    public disableToolbarEmail() {
        // this.rteEmail.toolbarSettings.enable = false;
    }

    public enableToolbarEmail() {
        this.pricingTemplate.email = this.rteEmail.getText();
        this.rteEmail.toolbarSettings.enable = true;
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

    public savePricingTemplate() {
        this.isSaving = true;

        // Update the margin template first
        this.pricingTemplatesService
            .update(this.pricingTemplate)
            .subscribe((data: any) => {
                this.saveCustomerMargins();
                this.sharedService.NotifyPricingTemplateComponent(
                    "updatecomponent"
                );
            });
    }

    public cancelPricingTemplateEdit() {
        if (this.requiresRouting) {
            this.router.navigate(["/default-layout/pricing-templates/"]);
        } else {
            this.cancelPricingTemplateEditclicked.emit();
        }
    }

    public deleteCustomerMargin(customerMargin) {
        if (customerMargin.oid === 0) {
            this.pricingTemplate.customerMargins.splice(
                this.pricingTemplate.customerMargins.indexOf(customerMargin),
                1
            );
            this.pricingTemplate.customerMargins[this.pricingTemplate.customerMargins.length - 1].max = 99999;
        } else {
            this.customerMarginsService.remove(customerMargin).subscribe(() => {
                this.priceTiersService
                    .remove({ oid: customerMargin.priceTierId })
                    .subscribe(() => {
                        this.pricingTemplate.customerMargins.splice(
                            this.pricingTemplate.customerMargins.indexOf(
                                customerMargin
                            ),
                            1
                        );

                        this.pricingTemplate.customerMargins[this.pricingTemplate.customerMargins.length - 1].max = 99999;
                    });
            });
        }

        
    }

    public addCustomerMargin() {
        const customerMargin = {
            oid: 0,
            templatesId: this.pricingTemplate.oid,
            priceTierId: 0,
            min: 1,
            max: 99999,
            amount: 0,
            itp: 0,
            allin: 0,
        };
        if (this.pricingTemplate.customerMargins.length > 0) {
            customerMargin.min =
                Math.abs(this.pricingTemplate.customerMargins[
                    this.pricingTemplate.customerMargins.length - 1
                ].min) + 250;
            this.pricingTemplate.customerMargins[
                this.pricingTemplate.customerMargins.length - 1
            ].max = Math.abs(customerMargin.min) - 1;
        }
        this.pricingTemplate.customerMargins.push(customerMargin);

        // this.fixCustomerMargins();
    }

    public fixCustomerMargins() {
        for (const i of Object.keys(this.pricingTemplate.customerMargins)) {
            const indexNumber = Number(i);
            if (
                !this.pricingTemplate.customerMargins[indexNumber].min ||
                this.pricingTemplate.customerMargins[indexNumber].min === ""
            ) {
                if (indexNumber === 0) {
                    this.pricingTemplate.customerMargins[indexNumber].min = 1;
                } else {
                    this.pricingTemplate.customerMargins[indexNumber].min =
                        this.pricingTemplate.customerMargins[indexNumber - 1]
                            .min + 1;
                }
            }

            if (
                indexNumber > 0 &&
                this.pricingTemplate.customerMargins[indexNumber].min ===
                    this.pricingTemplate.customerMargins[indexNumber - 1].min
            ) {
                this.pricingTemplate.customerMargins[indexNumber].min =
                    this.pricingTemplate.customerMargins[indexNumber - 1].min +
                    1;
            }

            if (
                this.pricingTemplate.customerMargins.length > indexNumber + 1 &&
                this.pricingTemplate.customerMargins[indexNumber + 1].min > 0
            ) {
                this.pricingTemplate.customerMargins[i].max =
                    Math.abs(this.pricingTemplate.customerMargins[indexNumber + 1].min) -
                    1;
            } else {
                this.pricingTemplate.customerMargins[i].max = 99999;
            }

            this.calculateItpForMargin(
                this.pricingTemplate.customerMargins[indexNumber]
            );
        }
    }

    public updateCustomerMargin(margin, index) {
        if (index) {
            if (index !== 0) {
                const previousPrice = this.pricingTemplate.customerMargins[
                    index - 1
                ];

                if (previousPrice) {
                    previousPrice.max = Math.abs(margin.min - 1);
                }
            }
        }
        const jetACost = this.currentPrice.filter(
            (item) => item.product === "JetA Cost"
        )[0].price;
        const jetARetail = this.currentPrice.filter(
            (item) => item.product === "JetA Retail"
        )[0].price;

        if (this.pricingTemplate.marginType === 0) {
            if (margin.min !== null && margin.amount !== null) {
                margin.allin = jetACost + margin.amount;
            }
        } else if (this.pricingTemplate.marginType === 1) {
            if (margin.amount !== null && margin.min !== null) {
                margin.allin = jetARetail - margin.amount;
                if (margin.allin) {
                    margin.itp = margin.allin - jetACost;
                }
            }
        }
    }

    public distributePricingTemplate() {
        const pricingTemplate = this.pricingTemplate;
        const dialogRef = this.distributionDialog.open(
            DistributionWizardMainComponent,
            {
                data: {
                    pricingTemplate,
                    fboId: this.sharedService.currentUser.fboId,
                    groupId: this.sharedService.currentUser.groupId,
                },
                disableClose: true,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {});
    }

    public customerMarginAmountChanged(customerMargin) {
        const indexOfMargin = this.pricingTemplate.customerMargins.indexOf(
            customerMargin
        );
        if (indexOfMargin > 0) {
            const previousTier = this.pricingTemplate.customerMargins[
                indexOfMargin - 1
            ];
            if (
                (this.pricingTemplate.marginType === 0 ||
                    this.pricingTemplate.marginType === 2) &&
                Math.abs(previousTier.amount) < Math.abs(customerMargin.amount)
            ) {
                customerMargin.amount = previousTier.amount + 0.01;
            } else if (
                this.pricingTemplate.marginType === 1 &&
                Math.abs(previousTier.amount) > Math.abs(customerMargin.amount)
            ) {
                customerMargin.amount = previousTier.amount - 0.01;
            }
        }
        this.calculateItpForMargin(customerMargin);
        this.calculateAllInForMargin(customerMargin);
    }

    public marginTypeChange() {
        this.loadCurrentPrice();
    }

    // Private Methods
    private saveCustomerMargins() {
        this.pricingTemplate.customerMargins.forEach((customerMargin: any) => {
            customerMargin.templateId = this.pricingTemplate.oid;
        });

        this.priceTiersService
            .updateFromCustomerMarginsViewModel(
                this.pricingTemplate.customerMargins
            )
            .subscribe((data: any) => {
                if (this.requiresRouting) {
                    this.router.navigate([
                        "/default-layout/pricing-templates/",
                    ]);
                } else {
                    this.savePricingTemplateClicked.emit();
                }
                this.isSaving = false;
            });
    }

    private loadCurrentPrice() {
        this.fbopricesService
            .getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.currentPrice = data;
                for (const margin of this.pricingTemplate.customerMargins) {
                    this.updateCustomerMargin(margin, 0);
                }
            });
    }

    private getCurrentProductForMarginType() {
        if (this.pricingTemplate.marginType === 0) {
            return "JetA Cost";
        }
        return "JetA Retail";
    }

    private calculateItpForMargin(customerMargin) {
        for (const m of this.currentPrice) {
            if (
                this.pricingTemplate.marginType === 0 &&
                this.getCurrentProductForMarginType() === m.product
            ) {
                customerMargin.itp = m
                    ? m.price
                    : 0 + Math.abs(customerMargin.amount);
                return;
            } else if (
                this.pricingTemplate.marginType === 1 &&
                this.getCurrentProductForMarginType() === m.product
            ) {
                customerMargin.itp = m
                    ? m.price
                    : 0 - Math.abs(customerMargin.amount);
                return;
            } else if (this.pricingTemplate.marginType > 1) {
                customerMargin.itp = Math.abs(customerMargin.amount);
                return;
            }
        }
    }

    private calculateAllInForMargin(customerMargin) {
        const jetACost = this.currentPrice.filter(
            (item) => item.product === "JetA Cost"
        )[0].price;
        const jetARetail = this.currentPrice.filter(
            (item) => item.product === "JetA Retail"
        )[0].price;

        if (this.pricingTemplate.marginType === 0) {
            if (customerMargin.min !== null && customerMargin.amount !== null) {
                customerMargin.allin = jetACost + customerMargin.amount;
            }
        } else if (this.pricingTemplate.marginType === 1) {
            if (customerMargin.amount !== null && customerMargin.min !== null) {
                customerMargin.allin = jetARetail - customerMargin.amount;
                if (customerMargin.allin) {
                    customerMargin.itp = customerMargin.allin - jetACost;
                }
            }
        }
    }
}
