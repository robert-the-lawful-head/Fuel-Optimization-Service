import { Component, Inject, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { ImageSettingsModel } from '@syncfusion/ej2-angular-richtexteditor';

import { SharedService } from '../../../../layouts/shared-service';
import { CustomerCompanyTypesService } from '../../../../services/customer-company-types.service';
// Services
import { CustomerinfobygroupService } from '../../../../services/customerinfobygroup.service';
import { DistributionService } from '../../../../services/distribution.service';
import { EmailcontentService } from '../../../../services/emailcontent.service';
import { PricingtemplatesService } from '../../../../services/pricingtemplates.service';
import { Subscription } from 'rxjs';

export interface DistributionDialogData {
    pricingTemplate: any;
    customer: any;
    customerCompanyType: number;
    emailContentGreeting: any;
    emailContentSignature: any;
    fboId: number;
    groupId: number;
}

@Component({
    selector: 'app-distribution-wizard-main',
    styleUrls: ['./distribution-wizard-main.component.scss'],
    templateUrl: './distribution-wizard-main.component.html',
})
export class DistributionWizardMainComponent implements OnInit {
    public emailContentGreetings: any[] = [];
    public emailContentBodies: any[] = [];
    public emailContentSignatures: any[] = [];
    public isDistributionComplete = false;
    public firstFormGroup: UntypedFormGroup;
    public secondFormGroup: UntypedFormGroup;
    public thirdFormGroup: UntypedFormGroup;
    public fourthFormGroup: UntypedFormGroup;
    public fifthFormGroup: UntypedFormGroup;
    public isLoadingCustomers = true;
    public isLoadingPricingTemplates = true;
    public isLoadingEmailContent = true;
    public isLoadingPreview = true;
    public availablePricingTemplates: any[];
    public customerCompanyTypes: any[];
    public availableCustomers: any[];
    public distributionPreview: string;
    public validityMessage: string;
    public isForSingleCustomer = false;
    public isForSinglePricingTemplate = false;
    public insertImageSettings: ImageSettingsModel = { saveFormat: 'Base64' }

    // Added services
    public sharedservice: SharedService;

    pricingTemplateSubscription: Subscription;
    constructor(
        public dialogRef: MatDialogRef<DistributionWizardMainComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DistributionDialogData,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerCompanyTypesService: CustomerCompanyTypesService,
        private distributionService: DistributionService,
        private emailContentService: EmailcontentService,
        private pricingtemplatesService: PricingtemplatesService,
        private formBuilder: UntypedFormBuilder
    ) {
        if (this.data.customer !== null) {
            this.isForSingleCustomer = true;
        } else if (this.data.pricingTemplate !== null) {
            this.isForSinglePricingTemplate = true;
        }
    }

    ngOnInit() {
        this.loadAvailableEmailContent();
        this.loadAvailablePricingTemplates();
        this.loadAvailableCustomerCompanyTypes();
        this.loadValidityMessage();

        this.firstFormGroup = this.formBuilder.group({
            customer: [''],
            customerCompanyType: [''],
            pricingTemplate: [''],
        });
        this.secondFormGroup = this.formBuilder.group({
            emailContentGreeting: ['', Validators.required],
            emailContentGreetingName: ['', Validators.required],
            emailContentSubject: ['', Validators.required],
        });
        this.thirdFormGroup = this.formBuilder.group({
            emailContentSignature: ['', Validators.required],
            emailContentSignatureName: ['', Validators.required],
        });

        this.fourthFormGroup = this.formBuilder.group({});

        this.fifthFormGroup = this.formBuilder.group({});

        // Subscribe to necessary changes
        this.pricingTemplateSubscription = this.firstFormGroup
            .get('pricingTemplate')
            .valueChanges.subscribe((val) => {
                this.data.pricingTemplate = val;
                // Only reload customers if they've already been loaded
                if (
                    this.availableCustomers &&
                    this.availableCustomers.length > 0
                ) {
                    this.loadAvailableCustomers();
                }
            });
        // ***Removing customer type selection for now***
        // this.firstFormGroup.get('customerCompanyType').valueChanges.subscribe(val => {
        //    this.data.customerCompanyType = val;
        //    //Only reload customers if they've already been loaded
        //    if (this.availableCustomers && this.availableCustomers.length > 0)
        //        this.loadAvailableCustomers();
        // });
    }
    ngOnDestroy() {
        this.pricingTemplateSubscription?.unsubscribe();
    }

    public pricingTemplateChanged() {
        this.data.pricingTemplate =
            this.firstFormGroup.get('pricingTemplate').value;
        this.loadAvailableCustomers();
    }

    public customerCompanyTypeChanged() {
        this.data.customerCompanyType = this.firstFormGroup.get(
            'customerCompanyType'
        ).value;
        this.loadAvailableCustomers();
    }

    public customerChanged() {
        this.data.customer = this.firstFormGroup.get('customer').value;
    }

    public emailContentGreetingChange() {
        this.secondFormGroup.patchValue({
            emailContentGreetingName: this.secondFormGroup.get(
                'emailContentGreeting'
            ).value.name,
        });
    }

    public emailContentSubjectChange() {
        this.secondFormGroup.patchValue({
            emailContentSubject: this.secondFormGroup.get('emailContentSubject')
                .value.name,
        });
    }

    public emailContentSignatureChange() {
        this.thirdFormGroup.patchValue({
            emailContentSignatureName: this.secondFormGroup.get(
                'emailContentSignature'
            ).value.name,
        });
    }

    public generatePreview() {
        this.isLoadingPreview = true;
        this.distributionService
            .previewDistribution(this.data)
            .subscribe((data: any) => {
                this.isLoadingPreview = false;
                this.distributionPreview = data.preview;
            });
    }

    public distributePricingClicked() {
        this.data.emailContentGreeting = this.secondFormGroup.get(
            'emailContentGreeting'
        ).value;
        this.data.emailContentGreeting.name = this.secondFormGroup.get(
            'emailContentGreetingName'
        ).value;
        this.data.emailContentGreeting.subject = this.secondFormGroup.get(
            'emailContentSubject'
        ).value;
        this.data.emailContentSignature = this.thirdFormGroup.get(
            'emailContentSignature'
        ).value;
        this.data.emailContentSignature.name = this.thirdFormGroup.get(
            'emailContentSignatureName'
        ).value;

        this.distributionService
            .distributePricing(this.data)
            .subscribe((data: any) => {});
        this.isDistributionComplete = true;
    }

    // Private Methods
    private loadAvailablePricingTemplates() {
        this.pricingtemplatesService
            .getByFbo(this.data.fboId)
            .subscribe((data: any) => {
                this.availablePricingTemplates = data;
                this.availablePricingTemplates.splice(0, 0, {
                    name: '--All Margin Templates--',
                    oid: 0,
                });
                if (
                    !this.data.pricingTemplate ||
                    this.data.pricingTemplate.oid === 0
                ) {
                    this.data.pricingTemplate =
                        this.availablePricingTemplates[0];
                } else {
                    for (const pricingTemplate of this
                        .availablePricingTemplates) {
                        if (
                            this.data.pricingTemplate.oid ===
                            pricingTemplate.oid
                        ) {
                            this.data.pricingTemplate = pricingTemplate;
                        }
                    }
                }
                this.firstFormGroup.patchValue({
                    pricingTemplate: this.data.pricingTemplate,
                });
                this.loadAvailableCustomers();
                this.isLoadingPricingTemplates = false;
            });
    }

    private loadAvailableCustomers() {
        this.isLoadingCustomers = true;
        this.customerInfoByGroupService
            .getCustomersByGroupAndFBOAndPricing(
                this.data.groupId,
                this.data.fboId,
                this.data.pricingTemplate.oid
            )
            .subscribe((data: any) => {
                this.availableCustomers = [];
                for (const customer of data) {
                    if (
                        customer.distribute &&
                        (!this.data.customerCompanyType ||
                            this.data.customerCompanyType === 0 ||
                            this.data.customerCompanyType ===
                                customer.customerCompanyType)
                    ) {
                        this.availableCustomers.push(customer);
                    }
                }
                this.availableCustomers.splice(0, 0, {
                    company: '--All Applicable Customers--',
                    oid: 0,
                });
                if (!this.data.customer || this.data.customer.oid === 0) {
                    this.data.customer = this.availableCustomers[0];
                } else {
                    for (const customer of this.availableCustomers) {
                        if (this.data.customer.oid === customer.oid) {
                            this.data.customer = customer;
                        }
                    }
                }
                this.firstFormGroup.patchValue({
                    customer: this.data.customer,
                });
                this.isLoadingCustomers = false;
            });
    }

    private loadAvailableCustomerCompanyTypes() {
        this.customerCompanyTypesService
            .getForFbo(this.data.fboId)
            .subscribe((data: any) => {
                this.customerCompanyTypes = data;
                this.customerCompanyTypes.splice(0, 0, {
                    name: '--All Types--',
                    oid: 0,
                });
                if (!this.data.customerCompanyType) {
                    this.data.customerCompanyType = 0;
                }
                this.firstFormGroup.patchValue({
                    customerCompanyType: this.data.customerCompanyType,
                });
            });
    }

    private loadAvailableEmailContent() {
        this.emailContentService
            .getForFbo(this.data.fboId)
            .subscribe((data: any) => {
                for (const template of data) {
                    if (template.emailContentType === 1) {
                        this.emailContentGreetings.push(template);
                    } else if (template.emailContentType === 2) {
                        this.emailContentBodies.push(template);
                    } else if (template.emailContentType === 3) {
                        this.emailContentSignatures.push(template);
                    }
                }
                this.emailContentGreetings.splice(0, 0, {
                    emailContentHtml: '',
                    emailContentType: 1,
                    name: 'New Greeting',
                    oid: 0,
                });
                this.emailContentSignatures.splice(0, 0, {
                    emailContentHtml: '',
                    emailContentType: 3,
                    name: 'New Signature',
                    oid: 0,
                });
                this.data.emailContentGreeting =
                    this.emailContentGreetings[
                        this.emailContentGreetings.length - 1
                    ];
                this.data.emailContentSignature =
                    this.emailContentSignatures[
                        this.emailContentSignatures.length - 1
                    ];
                this.isLoadingEmailContent = false;
            });
    }

    private loadValidityMessage() {
        this.distributionService
            .getDistributionValidityForFbo(this.data.fboId)
            .subscribe((data: any) => {
                this.validityMessage = data.message;
            });
    }
}
