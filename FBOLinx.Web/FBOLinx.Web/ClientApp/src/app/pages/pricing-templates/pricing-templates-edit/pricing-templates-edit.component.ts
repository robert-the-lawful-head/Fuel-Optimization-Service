import {
    Component,
    EventEmitter,
    OnInit,
    OnDestroy,
    Output,
    ViewChild} from '@angular/core';
import { UntypedFormArray, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';
import { differenceBy, forOwn } from 'lodash';
import { combineLatest, Subscription } from 'rxjs';
import {
    MatLegacyDialog as MatDialog,
} from '@angular/material/legacy-dialog';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { CustomermarginsService } from '../../../services/customermargins.service';
import { EmailcontentService } from '../../../services/emailcontent.service';
import { FbofeeandtaxomitsbypricingtemplateService } from '../../../services/fbofeeandtaxomitsbypricingtemplate.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
// Components
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';
import { ProceedConfirmationComponent } from '../../../shared/components/proceed-confirmation/proceed-confirmation.component';
import { PricingTemplateCalcService } from '../pricingTemplateCalc.service';
import { PricingTemplatesDialogDeleteWarningComponent } from '../pricing-template-dialog-delete-warning-template/pricing-template-dialog-delete-warning.component';
import { ToolbarSettingsModel } from '@syncfusion/ej2-angular-dropdowns';
import { DecimalPrecisionPipe } from 'src/app/shared/pipes/decimal/decimal-precision.pipe';
import { StringHelperService } from 'src/app/helpers/strings/stringHelper.service';
import { MarginType } from 'src/app/enums/margin-type.enum';
export interface DefaultTemplateUpdate {
    currenttemplate: number;
    newtemplate: number;
    fboid: number;
    isDeleting: boolean;
}
export interface FboFeeAndTaxOmitsByPricingTemplate
{
    oid: number;
    fboFeeAndTaxId: number;
    pricingTemplateId: number ;
    isOmitted: boolean | null;
}
@Component({
    selector: 'app-pricing-templates-edit',
    styleUrls: ['./pricing-templates-edit.component.scss'],
    templateUrl: './pricing-templates-edit.component.html',
})
export class PricingTemplatesEditComponent implements OnInit, OnDestroy {
    @ViewChild('priceBreakdownPreview')
    private priceBreakdownPreview: PriceBreakdownComponent;
    @ViewChild('feeAndTaxGeneralBreakdown')
    private feeAndTaxGeneralBreakdown: PriceBreakdownComponent;
    @ViewChild('typeRTE') rteObj: RichTextEditorComponent;
    @ViewChild('fileUpload') fileUploadName;

    // Input/Output Bindings
    @Output() savePricingTemplateClicked = new EventEmitter<any>();

    pricingTemplate: any;
    id             :any;
    pricingTemplateForm: UntypedFormGroup;
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
    isDefaultChanged = false;

    theFile: any;
    isUploadingFile: boolean;
    fileName: any = "";
    public tools: ToolbarSettingsModel = { items: ['Bold', 'Italic', 'Underline', '|', 'Formats', 'Alignments', 'OrderedList', 'UnorderedList', '|', 'CreateLink', '|', 'SourceCode', 'Undo', 'Redo'] };

    public updateModel: DefaultTemplateUpdate = {
        currenttemplate: 0,
        fboid: 0,
        newtemplate: 0,
        isDeleting: false
    };
    pricingTemplates: Array<any>;
    inputStepDefaultValue: string = this.stringHelperService.getNumberInputStepDefaultValue();
    combineLatestSubscription: Subscription[] = [];
    valueChangesSubscription: Subscription[] = [];
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private formBuilder: UntypedFormBuilder,
        private customerMarginsService: CustomermarginsService,
        private priceTiersService: PricetiersService,
        private pricingTemplatesService: PricingtemplatesService,
        private fboPricesService: FbopricesService,
        private fboFeesAndTaxesService: FbofeesandtaxesService,
        private fboFeeAndTaxOmitsbyPricingTemplateService: FbofeeandtaxomitsbypricingtemplateService,
        private sharedService: SharedService,
        private emailContentService: EmailcontentService,
        private marginLessThanOneDialog: MatDialog,
        private pricingTemplateCalcService: PricingTemplateCalcService,
        public deleteTemplateWarningDialog: MatDialog,
        public customCustomerService: CustomcustomertypesService,
        private decimalPrecisionPipe: DecimalPrecisionPipe,
        private stringHelperService: StringHelperService
    ) {


    }

    get customerMarginsFormArray() {
        return this.pricingTemplateForm.controls.customerMargins as UntypedFormArray;
    }

    get isMember() {
        return this.sharedService.currentUser.role === 4;
    }

    ngOnDestroy() {
        this.valueChangesSubscription.forEach((subscription) => {
            subscription?.unsubscribe();
        });
        this.combineLatestSubscription.forEach((subscription) => {  subscription?.unsubscribe(); });
        if (!this.hasSaved)
            return;
        this.fboPricesService.handlePriceChangeCleanUp(this.sharedService.currentUser.fboId).subscribe((response: any) => {
            //Do nothing
            });
    }

    onActionBegin (args: any): void {
    if (args.type === 'drop' || args.type === 'dragstart') {
        args.cancel = true;
    }
}

    async ngOnInit() {
        // Check for passed in id
        this.id = this.route.snapshot.paramMap.get('id');
        this.pricingTemplate = await this.pricingTemplatesService
            .get({ oid: this.id })
            .toPromise();

        this.pricingTemplates = await this.pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId, this.sharedService.currentUser.groupId).toPromise();

        let combineLatestSubscription = combineLatest([
            this.customerMarginsService.getCustomerMarginsByPricingTemplateId(
                this.id
            ),
            this.fboPricesService.getFbopricesByFboIdCurrent(
                this.sharedService.currentUser.fboId
            ),
            this.pricingTemplatesService.getFileAttachmentName(
                this.id
            ),
        ]).subscribe(([customerMarginsData, fboPricesData, fileAttachmentName]) => {
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
                this.jetARetail = jetARetailRecords[0].price;

            }

            this.pricingTemplate.customerMargins = this.updateMargins(
                customerMarginsData,
                this.pricingTemplate.marginType ,
                this.pricingTemplate.discountType

            );

            this.fileName = fileAttachmentName;

            let customerMargins: UntypedFormArray = this.formBuilder.array([]);
            if (this.pricingTemplate.customerMargins) {
                customerMargins = this.formBuilder.array(
                    this.pricingTemplate.customerMargins.map(
                        (customerMargin) => {
                            if (
                                customerMargin.amount !== undefined &&
                                customerMargin.amount !== null
                            ) {
                                customerMargin.amount = this.decimalPrecisionPipe.transform(Number(
                                    customerMargin.amount
                                ));


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

            let pricingTemplateValueChangeSubscription = this.pricingTemplateForm.valueChanges.subscribe(() => {
                this.canSave = true;
                if (!this.isDefaultChanged)
                    this.savePricingTemplate();
            });
            this.valueChangesSubscription.push(pricingTemplateValueChangeSubscription);
            // Margin type change event
            let pricingMarginTypeSubscription = this.pricingTemplateForm.controls.marginType.valueChanges.subscribe(
                (type) => {
                    const updatedMargins = this.decimalPrecisionPipe.transform(this.updateMargins(
                        this.pricingTemplateForm.value.customerMargins,
                        type ,
                        this.pricingTemplateForm.value.discountType,
                    ));

                    this.pricingTemplateForm.controls.customerMargins.setValue(
                        updatedMargins,
                        {
                            emitEvent: false,
                        }
                    );
                    this.savePricingTemplate();
                }
            );
            this.valueChangesSubscription.push(pricingMarginTypeSubscription);
            let pricingCustomerMarginSubscription = this.pricingTemplateForm.controls.customerMargins.valueChanges.subscribe(
                (margins) => {
                    const updatedMargins = this.decimalPrecisionPipe.transform(this.updateMargins(
                        margins,
                        this.pricingTemplateForm.value.marginType ,
                        this.pricingTemplateForm.value.discountType
                    ));
                    this.pricingTemplateForm.controls.customerMargins.setValue(
                        updatedMargins,
                        {
                            emitEvent: false,
                        }
                    );
                    this.savePricingTemplate();
                }
            );

            this.valueChangesSubscription.push(pricingCustomerMarginSubscription);
        //When Discount Type Change event
        let pricingDiscountVAlueChangeSubscription = this.pricingTemplateForm.controls.discountType.valueChanges.subscribe(
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
            this.valueChangesSubscription.push(pricingDiscountVAlueChangeSubscription);
            // Default toggle event
            let pricingValueChangeSubscription  = this.pricingTemplateForm.controls.default.valueChanges.subscribe(
                (type) => {
                    if (!type) {
                        this.isDefaultChanged = true;
                        const dialogRef = this.deleteTemplateWarningDialog.open(
                            PricingTemplatesDialogDeleteWarningComponent,
                            {
                                autoFocus: false,
                                data: { data: this.pricingTemplates, action: "removing" },
                                width: '600px',
                                disableClose:false
                            }
                        );

                        dialogRef.afterClosed().subscribe((response) => {
                            if (response !== 'cancel') {
                                this.updateModel.currenttemplate = this.pricingTemplate.oid;
                                this.updateModel.fboid = this.pricingTemplate.fboid;
                                this.updateModel.newtemplate = response;
                                this.updateModel.isDeleting = false;

                                this.customCustomerService
                                    .updateDefaultTemplate(this.updateModel)
                                    .subscribe((result) => {
                                        if (result) {

                                            if (this.updateModel.newtemplate == this.updateModel.currenttemplate) {
                                                this.isSaving = false;
                                                this.hasSaved = true;
                                                this.pricingTemplateForm.controls.default.setValue(true);
                                            }
                                            else {
                                                this.canSave = true;
                                                this.savePricingTemplate();
                                            }
                                        }
                                    });
                            }
                            else {
                                this.pricingTemplateForm.controls.default.setValue(true);
                            }
                        });
                    }

                }
            );
            this.valueChangesSubscription.push(pricingValueChangeSubscription);
        });
        this.combineLatestSubscription.push(combineLatestSubscription);
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

        var isMarginLessThanZero = false;
        isMarginLessThanZero = this.checkMarginLessThanZero();
        if (isMarginLessThanZero) {
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
                    this.isSaving = false;
                    return;
                }
                this.continueSavingTemplate();
            });
        }
        else {
            this.continueSavingTemplate();
        }
    }

    continueSavingTemplate() {
        this.hasSaved = false;
        const removedCustomerMargins = differenceBy(
            this.pricingTemplate.customerMargins,
            this.pricingTemplateForm.value.customerMargins,
            'oid'
        );

        let combineLatestSubscription  = combineLatest([
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
        this.combineLatestSubscription.push(combineLatestSubscription);
    }

    onFileChange(event) {
        this.theFile = null;
        if (event.target.files && event.target.files.length > 0) {
            // Set theFile property
            this.theFile = event.target.files[0];
        }
    }

    uploadFile(): void {
        if (this.theFile != null) {
            this.isUploadingFile = true;
            this.readAndUploadFile(this.theFile);
        }
    }

    deleteFile(): void {
        this.pricingTemplatesService
            .deleteFileAttachment(this.id)
            .subscribe((data: any) => {
                this.fileName = '';
            });
    }

    downloadFile(): void {
        this.pricingTemplatesService.downloadFileAttachment(this.id).subscribe((data: any) => {
            const source = data;
            const link = document.createElement("a");
            link.href = source;
            link.download = this.fileName;
            link.click();
        });
    }

    checkMarginLessThanZero(): boolean {
        var isMarginLessThanZero = false;
        if (this.pricingTemplateForm.value
            .marginType == 0) {
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
        return isMarginLessThanZero;
    }

    cancelPricingTemplateEdit() {
            this.router
                .navigate(['/default-layout/pricing-templates/'])
    }

    deleteCustomerMargin(index: number) {
        this.pricingTemplateCalcService.adjustCustomerMarginValuesOnDelete(index, this.customerMarginsFormArray);
    }

    addCustomerMargin() {
        const customerMargin = {
            allin: 0,
            amount: this.decimalPrecisionPipe.transform(Number(0)),
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
    updateCustomerMarginVolumeValues(index: number){
        this.pricingTemplateCalcService.adjustCustomerMarginPreviousValues(index,this.customerMarginsFormArray);
        this.pricingTemplateCalcService.adjustCustomerMarginNextValues(index,this.customerMarginsFormArray);
    }

    async omitFeeAndTaxCheckChanged(feeAndTax: any): Promise<void> {
        feeAndTax.omitsByPricingTemplate = feeAndTax.omitsByPricingTemplate ?? [];

        let omitRecord: FboFeeAndTaxOmitsByPricingTemplate = feeAndTax.omitsByPricingTemplate.find(o => o.fboFeeAndTaxId == feeAndTax.oid);

        if(this.pricingTemplate.marginType == MarginType.RetailMinus){
            if(omitRecord){
                omitRecord.isOmitted = feeAndTax.isOmitted;
                await this.fboFeeAndTaxOmitsbyPricingTemplateService.update(omitRecord).toPromise();
            }else{
                omitRecord = {
                    fboFeeAndTaxId: feeAndTax.oid,
                    oid: 0,
                    pricingTemplateId: this.id,
                    isOmitted: feeAndTax.isOmitted,
                };    
                await this.fboFeeAndTaxOmitsbyPricingTemplateService.add(omitRecord).toPromise();
            }
            this.recalculatePriceBreakdown();
            return;
        }
        
        if (feeAndTax.isOmitted) {
            omitRecord = {
                fboFeeAndTaxId: feeAndTax.oid,
                oid: 0,
                pricingTemplateId: this.id,
                isOmitted: feeAndTax.isOmitted,
            };    

            this.fboFeeAndTaxOmitsbyPricingTemplateService
                .add(omitRecord)
                .subscribe((response: any) => {
                    omitRecord.oid = response.oid;
                    feeAndTax.omitsByPricingTemplate.push(omitRecord);
                    this.recalculatePriceBreakdown();
                });
        }else {
            this.fboFeeAndTaxOmitsbyPricingTemplateService
                .remove(omitRecord)
                .subscribe(() => {
                    feeAndTax.omitsByPricingTemplate.splice(feeAndTax.omitsByPricingTemplate.indexOf(omitRecord), 1);
                    this.recalculatePriceBreakdown();
                });
        }
    }

    private readAndUploadFile(theFile: any) {
        const file = {
            ContentType: theFile.type,

            PricingTemplateId: this.id,

            FileData: null,
            // Set File Information
            FileName: theFile.name,
        };

        var printEventType = function (event) {
            var error = event;
        };

        // Use FileReader() object to get file to upload
        // NOTE: FileReader only works with newer browsers
        const reader = new FileReader();

        // Setup onload event for reader
        reader.onload = () => {
            // Store base64 encoded representation of file
            file.FileData = reader.result.toString();

            // POST to server
            this.pricingTemplatesService.uploadFileAttachment(file).subscribe((data: any) => {
                if (data.indexOf("Message:") < 1) {
                    this.isUploadingFile = false;
                    this.fileUploadName.nativeElement.value = '';
                    this.fileName = theFile.name;
                }
            });
        }

        // Read the file
        reader.readAsDataURL(theFile);
        reader.onerror = printEventType;
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

                      if(discountType == 1)
                      {
                          margins[i].allin =
                              this.jetARetail * (1 - (Number(margins[i].amount / 100)));
                       }
                    else{
                          margins[i].allin =
                              this.jetARetail - Number(margins[i].amount);
                    }

                    margins[i].itp = this.jetARetail;

                    if (margins[i].allin) {
                        margins[i].itp = margins[i].allin - this.jetACost;
                    }
                }
            }
            if (margins[i].amount !== null || margins[i].amount !== '') {
                margins[i].amount = this.decimalPrecisionPipe.transform(margins[i].amount);
                   this.pricingTemplate.discountType = discountType;
            }
        }
        return margins;
    }
}



