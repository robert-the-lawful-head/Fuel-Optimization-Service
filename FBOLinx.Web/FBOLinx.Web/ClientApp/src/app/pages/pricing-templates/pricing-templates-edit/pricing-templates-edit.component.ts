import {
    Component,
    EventEmitter,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';
import { differenceBy, forOwn } from 'lodash';
import { combineLatest } from 'rxjs';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { CustomermarginsService } from '../../../services/customermargins.service';
import { EmailcontentService } from '../../../services/emailcontent.service';
import { FbofeeandtaxomitsbypricingtemplateService } from '../../../services/fbofeeandtaxomitsbypricingtemplate.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
// Components
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/pricing-templates',
        title: 'ITP Margin Templates',
    },
    {
        link: '',
        title: 'Edit Margin Template',
    },
];

@Component({
    selector: 'app-pricing-templates-edit',
    styleUrls: ['./pricing-templates-edit.component.scss'],
    templateUrl: './pricing-templates-edit.component.html',
})
export class PricingTemplatesEditComponent implements OnInit {
    @ViewChild('priceBreakdownPreview')
    private priceBreakdownPreview: PriceBreakdownComponent;
    @ViewChild('feeAndTaxGeneralBreakdown')
    private feeAndTaxGeneralBreakdown: PriceBreakdownComponent;
    @ViewChild('typeRTE') rteObj: RichTextEditorComponent;

    // Input/Output Bindings
    @Output() savePricingTemplateClicked = new EventEmitter<any>();

    pricingTemplate: any;
    id             :any;
    breadcrumb: any[] = BREADCRUMBS;
    pricingTemplateForm: FormGroup;
    pageTitle = 'Edit Margin Template';
    marginTypeDataSource: Array<any> = [
        { text: 'Cost +', value: 0 },
        { text: 'Retail -', value: 1 },
     ];


     discountTypeDataSource: Array<any> = [
        { text: 'Flat per Gallon', value: 0 },
        { text: 'Percentage', value: 1 },
     ];

    emailTemplatesDataSource: Array<any>;
    canSave: boolean;
    jetACost: number;
    jetARetail: number;
    isSaving = false;
    hasSaved = false;
    isSaveQueued = false;
    feesAndTaxes: Array<any>;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private formBuilder: FormBuilder,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fboPricesService: FbopricesService,
        private fboFeesAndTaxesService: FbofeesandtaxesService,
        private fboFeeAndTaxOmitsbyPricingTemplateService: FbofeeandtaxomitsbypricingtemplateService,
        private sharedService: SharedService,
        private emailContentService: EmailcontentService
    ) {
        this.sharedService.titleChange(this.pageTitle);


    }

    get customerMarginsFormArray() {
        return this.pricingTemplateForm.controls.customerMargins as FormArray;
    }

    async ngOnInit() {
        // Check for passed in id
        this.id = this.route.snapshot.paramMap.get('id');
        this.pricingTemplate = await this.pricingTemplatesService
            .get({ oid: this.id })
            .toPromise();

        combineLatest([
            this.customerMarginsService.getCustomerMarginsByPricingTemplateId(
                this.id
            ),
            this.fboPricesService.getFbopricesByFboIdCurrent(
                this.sharedService.currentUser.fboId
            ),
        ]).subscribe(([customerMarginsData, fboPricesData]) => {
            const jetACostRecords = (fboPricesData as any).filter(
                (item) => item.product === 'JetA Cost'
            );
            const jetARetailRecords = (fboPricesData as any).filter(
                (item) => item.product === 'JetA Retail'
            );
            if (jetACostRecords && jetACostRecords.length > 0) {
                this.jetACost = jetACostRecords[0].price;
            }
            if (jetARetailRecords && jetARetailRecords.length > 0) {
                console.log(jetACostRecords);
                this.jetARetail = jetARetailRecords[0].price;

            }

            this.pricingTemplate.customerMargins = this.updateMargins(
                customerMarginsData,
                this.pricingTemplate.marginType ,
                this.pricingTemplate.discountType

            );

            let customerMargins: FormArray = this.formBuilder.array([]);
            if (this.pricingTemplate.customerMargins) {
                customerMargins = this.formBuilder.array(
                    this.pricingTemplate.customerMargins.map(
                        (customerMargin) => {
                            if (
                                customerMargin.amount !== undefined &&
                                customerMargin.amount !== null
                            ) {
                                customerMargin.amount = Number(
                                    customerMargin.amount
                                ).toFixed(4);


                            }
                            const group = {
                                itp: undefined,
                            };
                            forOwn(customerMargin, (value, key) => {
                                group[key] = [value];
                            });
                            return this.formBuilder.group(group, {
                                updateOn: 'blur',
                            });
                        }
                    )
                );
            }

            this.pricingTemplateForm = this.formBuilder.group({
                customerMargins,
                default: [this.pricingTemplate.default],
                emailContentId: [this.pricingTemplate.emailContentId],
                marginType: [this.pricingTemplate.marginType],
                discountType:[this.pricingTemplate.discountType],
                name: [this.pricingTemplate.name],
                notes: [this.pricingTemplate.notes],
            });

            this.pricingTemplateForm.valueChanges.subscribe(() => {
                this.canSave = true;
                this.savePricingTemplate();

            });

            // Margin type change event
            this.pricingTemplateForm.controls.marginType.valueChanges.subscribe(
                (type) => {
                    const updatedMargins = this.updateMargins(
                        this.pricingTemplateForm.value.customerMargins,
                        type ,
                        this.pricingTemplateForm.value.discountType,
                    );
                    this.pricingTemplateForm.controls.customerMargins.setValue(
                        updatedMargins,
                        {
                            emitEvent: false,
                        }
                    );
                    this.savePricingTemplate();
                }
            );

            this.pricingTemplateForm.controls.customerMargins.valueChanges.subscribe(
                (margins) => {
                    const updatedMargins = this.updateMargins(
                        margins,
                        this.pricingTemplateForm.value.marginType ,
                        this.pricingTemplateForm.value.discountType
                    );
                    this.pricingTemplateForm.controls.customerMargins.setValue(
                        updatedMargins,
                        {
                            emitEvent: false,
                        }
                    );
                    this.savePricingTemplate();
                }
            );


        //When Discount Type Change event
        this.pricingTemplateForm.controls.discountType.valueChanges.subscribe(
            (type) => {
                 const updatedMargins = this.updateMargins(
                    this.pricingTemplateForm.value.customerMargins,
                    this.pricingTemplateForm.value.marginType,
                    type ,

                );
                this.pricingTemplateForm.controls.customerMargins.setValue(
                    updatedMargins,
                    {
                        emitEvent: false,
                    }
                );
                this.savePricingTemplate();
            }
        );


        });



        this.loadPricingTemplateFeesAndTaxes();
        this.loadEmailContentTemplate();
    }

    savePricingTemplate(): void {
        const self = this;
        if (this.isSaving) {
            // Save already in queue - no need to double-up the queue
            if (this.isSaveQueued) {
                return;
            }
            this.isSaveQueued = true;
            setTimeout(() => {
                self.savePricingTemplate();
            }, 250);
            return;
        }

        this.isSaveQueued = false;
        this.isSaving = true;
        this.hasSaved = false;
        const removedCustomerMargins = differenceBy(
            this.pricingTemplate.customerMargins,
            this.pricingTemplateForm.value.customerMargins,
            'oid'
        );

        combineLatest([
            this.customerMarginsService.bulkRemove(removedCustomerMargins),
            this.priceTiersService.updateFromCustomerMarginsViewModel(
                this.pricingTemplateForm.value.customerMargins
            ),
            this.pricingTemplatesService.update({
                ...this.pricingTemplate,
                ...this.pricingTemplateForm.value,
            }),
        ]).subscribe(
            ([
                bulkRemoveResponse,
                customerMarginsUpdateResponse,
                priceTemplatesResponse,
            ]) => {
                // this.router.navigate(['/default-layout/pricing-templates/']).then(() => {});
                this.pricingTemplate.customerMargins =
                    customerMarginsUpdateResponse;
                for (
                    let i = 0;
                    i < this.pricingTemplateForm.value.customerMargins.length;
                    i++
                ) {
                    this.pricingTemplateForm.value.customerMargins[i].oid =
                        this.pricingTemplate.customerMargins[i].oid;
                }
                this.isSaving = false;
                this.hasSaved = true;
                this.priceBreakdownPreview.performRecalculation();
                this.feeAndTaxGeneralBreakdown.performRecalculation();

                this.sharedService.NotifyPricingTemplateComponent(
                    'updateComponent'
                );
            }
        );
    }

    cancelPricingTemplateEdit() {
        this.router
            .navigate(['/default-layout/pricing-templates/'])
            .then(() => {});
    }

    deleteCustomerMargin(index: number) {
        this.customerMarginsFormArray.removeAt(index);
        if (this.customerMarginsFormArray.length) {
            this.customerMarginsFormArray
                .at(this.customerMarginsFormArray.length - 1)
                .patchValue({
                    max: 99999,
                });
        }
    }

    addCustomerMargin() {
        const customerMargin = {
            allin: 0,
            amount: Number(0).toFixed(4),
            itp: 0,
            max: 99999,
            min: 1,
            oid: 0,
            priceTierId: 0,
            discountType: 0,
            templateId: this.id,
        };
        if (this.customerMarginsFormArray.length > 0) {
            const lastIndex = this.customerMarginsFormArray.length - 1;
            customerMargin.min =
                Math.abs(
                    this.customerMarginsFormArray.at(lastIndex).value.min
                ) + 250;
            this.customerMarginsFormArray.at(lastIndex).patchValue(
                {
                    max: Math.abs(customerMargin.min) - 1,
                },
                {
                    emitEvent: false,
                }
            );
        }
        const group = {};
        forOwn(customerMargin, (value, key) => {
            group[key] = [value];
        });
        this.customerMarginsFormArray.push(
            this.formBuilder.group(group, { updateOn: 'blur' })
        );
    }

    omitFeeAndTaxCheckChanged(feeAndTax: any): void {
        if (!feeAndTax.omitsByPricingTemplate) {
            feeAndTax.omitsByPricingTemplate = [];
        }
        let omitRecord: any = {
            fboFeeAndTaxId: feeAndTax.oid,
            oid: 0,
            pricingTemplateId: this.id,
        };
        if (feeAndTax.omitsByPricingTemplate.length > 0) {
            omitRecord = feeAndTax.omitsByPricingTemplate[0];
        } else {
            feeAndTax.omitsByPricingTemplate.push(omitRecord);
        }
        omitRecord.fboFeeAndTaxId = feeAndTax.oid;
        if (feeAndTax.isOmitted) {
            omitRecord.oid = 0;
            this.fboFeeAndTaxOmitsbyPricingTemplateService
                .add(omitRecord)
                .subscribe((response: any) => {
                    omitRecord.oid = response.oid;
                    this.recalculatePriceBreakdown();
                });
        } else {
            this.fboFeeAndTaxOmitsbyPricingTemplateService
                .remove(omitRecord)
                .subscribe(() => {
                    feeAndTax.omitsByPricingTemplate = [];
                    this.recalculatePriceBreakdown();
                });
        }
    }

    private loadPricingTemplateFeesAndTaxes(): void {
        this.fboFeesAndTaxesService
            .getByFboAndPricingTemplate(
                this.sharedService.currentUser.fboId,
                this.id
            )
            .subscribe((response: any[]) => {
                this.feesAndTaxes = response;
            });
    }

    private loadEmailContentTemplate(): void {
        this.emailContentService
            .getForFbo(this.sharedService.currentUser.fboId)
            .subscribe((response: any) => {
                this.emailTemplatesDataSource = response;
            });
    }

    private recalculatePriceBreakdown(): void {
        // Set a timeout so the child component is aware of model changes
        const self = this;
        setTimeout(() => {
            self.priceBreakdownPreview.performRecalculation();
            self.feeAndTaxGeneralBreakdown.performRecalculation();
        });
    }

    private updateMargins(oldMargins, marginType , discountType ) {

        const margins = [...oldMargins];

       for (let i = 0; i < margins?.length; i++) {

            if (i > 0) {
                margins[i - 1].max = Math.abs(margins[i].min - 1);
            }
            //in Cost Mode
            if (marginType !== 1) {
                if (margins[i].min !== null && margins[i].amount !== null) {
                       if(discountType == 0)
                       {
                        margins[i].allin = Number(margins[i].amount);
                        margins[i].itp  = Number(margins[i].amount);
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
                }

          //in Retail Mode
            else {
                if (margins[i].amount !== null && margins[i].min !== null) {

                      if(discountType == 0)
                      {
                        margins[i].allin =
                        this.jetARetail - Number(margins[i].amount);

                       }
                    else{

                        margins[i].allin =
                        this.jetARetail * (1 - (Number(margins[i].amount /100)));

                    }

                    margins[i].itp = this.jetARetail;

                    if (margins[i].allin) {
                        margins[i].itp = margins[i].allin - this.jetACost;
                    }

                    console.log(i + "   " + margins[i].itp + '   ' + this.jetARetail)

                }
            }
            if (margins[i].amount !== null || margins[i].amount !== '') {
                margins[i].amount = Number(margins[i].amount).toFixed(4);
                this.pricingTemplate.discountType = discountType;
            }
        }
        return margins;
    }
}



