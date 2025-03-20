import { Component, Inject, Input, OnInit, ViewChild } from '@angular/core';
import { UntypedFormArray, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatLegacyDialog as MatDialog, MatLegacyDialogRef as MatDialogRef, MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA } from '@angular/material/legacy-dialog';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { SharedService } from 'src/app/layouts/shared-service';
import {
    AircraftSize,
    AircraftType,
    CertificateType,
    CustomerCompanyType,
    FeesAndTaxes,
    PricingTemplate,
} from 'src/app/models';
import { AircraftsService } from 'src/app/services/aircrafts.service';
import { ContactinfobygroupsService } from 'src/app/services/contactinfobygroups.service';
import { CustomcustomertypesService } from 'src/app/services/customcustomertypes.service';
import { CustomerCompanyTypesService } from 'src/app/services/customer-company-types.service';
import { CustomeraircraftsService } from 'src/app/services/customeraircrafts.service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { CustomersService } from 'src/app/services/customers.service';
import { CustomersviewedbyfboService } from 'src/app/services/customersviewedbyfbo.service';
import { FbofeesandtaxesService } from 'src/app/services/fbofeesandtaxes.service';
import { PricingtemplatesService } from 'src/app/services/pricingtemplates.service';
import { PriceBreakdownComponent } from 'src/app/shared/components/price-breakdown/price-breakdown.component';

import { CloseConfirmationComponent } from '../../../shared/components/close-confirmation/close-confirmation.component';
import { FormValidationHelperService } from 'src/app/helpers/forms/formValidationHelper.service';
import { AircraftResult, JetNet } from '../../../models/jetnet-information';

enum WizardStep {
    COMPANY_INFO,
    CONTACT_INFO,
    ITP_MARGIN_TEMPLATE,
    AIRCRAFT,
}

@Component({
    selector: 'app-customers-dialog-new-customer',
    styleUrls: ['./customers-dialog-new-customer.component.scss'],
    templateUrl: './customers-dialog-new-customer.component.html',
})
export class CustomersDialogNewCustomerComponent implements OnInit {
    @ViewChild('priceBreakdownPreview')
    private priceBreakdownPreview: PriceBreakdownComponent;
    customerForm: UntypedFormGroup;
    companyInfoDetailOpenState = false;
    certificateTypes: CertificateType[] = [];
    customerCompanyTypes: CustomerCompanyType[] = [];
    pricingTemplates: PricingTemplate[];
    feesAndTaxes: FeesAndTaxes[];
    aircraftTypes: AircraftType[];
    aircraftSizes: AircraftSize[];
    step: WizardStep = WizardStep.COMPANY_INFO;
    submitting = false;
    aircraftType: AircraftType;

    formValueChangesSubscription: Subscription;
    combineLatestSubscription: Subscription;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: AircraftResult,
        public dialogRef: MatDialogRef<CustomersDialogNewCustomerComponent>,
        public closeConfirmationDialog: MatDialog,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerCompanyTypesService: CustomerCompanyTypesService,
        private pricingTemplatesService: PricingtemplatesService,
        private fboFeesAndTaxesService: FbofeesandtaxesService,
        private aircraftsService: AircraftsService,
        private customersService: CustomersService,
        private customersViewedByFboService: CustomersviewedbyfboService,
        private contactInfoByGroupsService: ContactinfobygroupsService,
        private customCustomerTypesService: CustomcustomertypesService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService ,
        private route : ActivatedRoute,
        private formValidationHelperService: FormValidationHelperService
    ) {
        this.customerForm = new UntypedFormGroup({
            aircraft: new UntypedFormArray([]),
            company: new UntypedFormGroup({
                address: new UntypedFormControl(),
                certificateType: new UntypedFormControl(),
                city: new UntypedFormControl(),
                company: new UntypedFormControl('', [Validators.required, this.formValidationHelperService.noWhitespaceValidator]),
                country: new UntypedFormControl(),
                mainPhone: new UntypedFormControl(),
                state: new UntypedFormControl(),
                website: new UntypedFormControl(),
                zipCode: new UntypedFormControl(),
            }),
            contact: new UntypedFormArray([]),
            template: new UntypedFormGroup({
                customerMarginTemplate: new UntypedFormControl(
                    undefined,
                    Validators.required
                ),
            }),
        });
        this.addNewContact();
        this.addNewAircraft();

        this.formValueChangesSubscription = this.templateFormGroup.valueChanges
            .pipe(
                switchMap(() =>
                    this.fboFeesAndTaxesService.getByFboAndPricingTemplate(
                        this.sharedService.currentUser.fboId,
                        this.pricingTemplateId
                    )
                )
            )
            .subscribe((feesAndTaxes) => {
                this.feesAndTaxes = feesAndTaxes;
                this.recalculatePriceBreakdown();
            });

        if (this.data != null) {
            this.data.companies.forEach((company) => {
                if (company.companyDetailOpenState) {
                    this.companyFormGroup.get('company').setValue(company.company);
                    this.companyFormGroup.get('address').setValue(company.companyrelationships[0].companyaddress1);
                    this.companyFormGroup.get('city').setValue(company.companyrelationships[0].companycity);
                    this.companyFormGroup.get('country').setValue(company.companyrelationships[0].companycountry);
                    this.companyFormGroup.get('state').setValue(company.companyrelationships[0].companystateabbr);
                    this.companyFormGroup.get('zipCode').setValue(company.companyrelationships[0].companypostcode);

                    this.aircraftFormArray.controls[0].get("tailNumber").setValue(this.data.regnbr);
                    this.aircraftFormArray.controls[0].get("aircraft").setValue(this.data.make.toUpperCase() + " " + this.data.model.toUpperCase());

                    if (company.companyrelationships.length > 0) {
                        var contactNumber = 1;

                        for (var i = 0; i <= company.companyrelationships.length - 1; i++) {
                            if (company.companyrelationships[i].add && company.companyrelationships[i].contactfirstname != null) {
                                if (contactNumber == 1) {
                                    this.contactFormArray.controls[contactNumber - 1].get("firstName").setValue(company.companyrelationships[i].contactfirstname);
                                    this.contactFormArray.controls[contactNumber - 1].get("lastName").setValue(company.companyrelationships[i].contactlastname);
                                    this.contactFormArray.controls[contactNumber - 1].get("email").setValue(company.companyrelationships[i].contactemail);
                                    this.contactFormArray.controls[contactNumber - 1].get("mobile").setValue(company.companyrelationships[i].contactmobilephone);
                                    this.contactFormArray.controls[contactNumber - 1].get("phone").setValue(company.companyrelationships[i].contactbestphone);
                                    this.contactFormArray.controls[contactNumber - 1].get("title").setValue(company.companyrelationships[i].contacttitle);

                                    contactNumber++;
                                }
                                else {
                                    this.contactFormArray.push(
                                        new UntypedFormGroup({
                                            address: new UntypedFormControl(),
                                            city: new UntypedFormControl(),
                                            copyAlerts: new UntypedFormControl(true),
                                            country: new UntypedFormControl(),
                                            email: new UntypedFormControl('', [
                                                Validators.required,
                                                Validators.email,
                                            ]),
                                            extension: new UntypedFormControl(),
                                            fax: new UntypedFormControl(),
                                            firstName: new UntypedFormControl(),
                                            lastName: new UntypedFormControl(),
                                            mobile: new UntypedFormControl(),
                                            phone: new UntypedFormControl(),
                                            primary: new UntypedFormControl(),
                                            state: new UntypedFormControl(),
                                            title: new UntypedFormControl(),
                                        })
                                    );

                                    this.contactFormArray.controls[contactNumber - 1].get("firstName").setValue(company.companyrelationships[i].contactfirstname);
                                    this.contactFormArray.controls[contactNumber - 1].get("lastName").setValue(company.companyrelationships[i].contactlastname);
                                    this.contactFormArray.controls[contactNumber - 1].get("email").setValue(company.companyrelationships[i].contactemail);
                                    this.contactFormArray.controls[contactNumber - 1].get("mobile").setValue(company.companyrelationships[i].contactmobilephone);
                                    this.contactFormArray.controls[contactNumber - 1].get("phone").setValue(company.companyrelationships[i].contactbestphone);
                                    this.contactFormArray.controls[contactNumber - 1].get("title").setValue(company.companyrelationships[i].contacttitle);

                                    contactNumber++;
                                }
                            }
                        }
                    }
                }
            });
        }
    }
    get companyFormGroup() {
        return this.customerForm.controls.company as UntypedFormGroup;
    }

    get contactFormArray() {
        return this.customerForm.controls.contact as UntypedFormArray;
    }

    get templateFormGroup() {
        return this.customerForm.controls.template as UntypedFormGroup;
    }

    get aircraftFormArray() {
        return this.customerForm.controls.aircraft as UntypedFormArray;
    }

    get backDisabled() {
        return this.step === WizardStep.COMPANY_INFO;
    }

    get nextDisabled() {
        return (
            (this.step === WizardStep.COMPANY_INFO &&
                this.companyFormGroup.invalid) ||
            (this.step === WizardStep.CONTACT_INFO &&
                this.contactFormArray.invalid) ||
            (this.step === WizardStep.ITP_MARGIN_TEMPLATE &&
                this.templateFormGroup.invalid) ||
            (this.step === WizardStep.AIRCRAFT &&
                this.aircraftFormArray.invalid)
        );
    }

    get submitDisabled() {
        return this.customerForm.invalid;
    }

    get pricingTemplateId() {
        return this.templateFormGroup.get('customerMarginTemplate').value;
    }

    ngOnInit(): void {
        this.combineLatestSubscription = combineLatest([
            this.customerInfoByGroupService.getCertificateTypes(),
            this.customerCompanyTypesService.getNonFuelerLinxForFbo(
                this.sharedService.currentUser.fboId
            ),
            this.pricingTemplatesService.getByFbo(
                this.sharedService.currentUser.fboId,
                this.sharedService.currentUser.groupId
            ),
            this.aircraftsService.getAll(),
            this.aircraftsService.getAircraftSizes(),
        ]).subscribe(
            ([
                certificateTypes,
                customerCompanyTypes,
                pricingTemplates,
                aircraftTypes,
                aircraftSizes,
            ]) => {
                this.certificateTypes = certificateTypes;
                this.customerCompanyTypes = customerCompanyTypes;
                this.pricingTemplates = pricingTemplates;
                this.aircraftTypes = aircraftTypes;
                this.aircraftSizes = aircraftSizes;

                this.aircraftType = this.aircraftTypes.find(a => a.make.trim() == this.data.make.toUpperCase().trim() && a.model.trim() == this.data.model.toUpperCase().trim());
            }
        );
    }
    ngOnDestroy() {
        this.combineLatestSubscription?.unsubscribe();
        this.formValueChangesSubscription?.unsubscribe();
    }
    next() {
        if (this.step !== WizardStep.AIRCRAFT) {
            this.step++;
        }
    }

    back() {
        if (this.step !== WizardStep.COMPANY_INFO) {
            this.step--;
        }
    }

    reset() {
        const closeDialogRef = this.closeConfirmationDialog.open(
            CloseConfirmationComponent,
            {
                autoFocus: false,
                data: {
                    cancel: 'Cancel',
                    customText: 'Are you sure to reset changes?',
                    customTitle: 'Reset Changes?',
                    ok: 'Reset',
                },
            }
        );
        closeDialogRef.afterClosed().subscribe((result) => {
            if (result === true) {
                this.customerForm.reset();
                this.step = WizardStep.COMPANY_INFO;
            }
        });
    }

    cancel() {
        const closeDialogRef = this.closeConfirmationDialog.open(
            CloseConfirmationComponent,
            {
                autoFocus: false,
                data: {
                    cancel: 'Cancel',
                    customText: 'You have unsaved changes. Are you sure?',
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

    addNewContact() {
        this.contactFormArray.push(
            new UntypedFormGroup({
                address: new UntypedFormControl(),
                city: new UntypedFormControl(),
                copyAlerts: new UntypedFormControl(true),
                country: new UntypedFormControl(),
                email: new UntypedFormControl('', [
                    Validators.required,
                    Validators.email,
                ]),
                extension: new UntypedFormControl(),
                fax: new UntypedFormControl(),
                firstName: new UntypedFormControl(),
                lastName: new UntypedFormControl(),
                mobile: new UntypedFormControl(),
                phone: new UntypedFormControl(),
                primary: new UntypedFormControl(),
                state: new UntypedFormControl(),
                title: new UntypedFormControl(),
            })
        );
    }

    addNewAircraft() {
        this.aircraftFormArray.push(
            new UntypedFormGroup({
                aircraft: new UntypedFormControl(undefined, Validators.required),
                aircraftPricingTemplate: new UntypedFormControl(),
                tailNumber: new UntypedFormControl(undefined, Validators.required),
            })
        );
    }

    async submit() {
        this.submitting = true;
        const customer = await this.customersService
            .add(this.companyFormGroup.value)
            .toPromise();
        await this.customersViewedByFboService
            .add({
                customerId: customer.oid,
                fboId: this.sharedService.currentUser.fboId,
                groupId: this.sharedService.currentUser.groupId,
            })
            .toPromise();
       const id = this.route.snapshot.paramMap.get('id');
        const cig = (await this.customerInfoByGroupService
            .add({
                ...this.companyFormGroup.value,
                customerId: customer.oid,
                groupId: this.sharedService.currentUser.groupId,
            } , this.sharedService.currentUser.oid)
            .toPromise()) as any;

        await this.contactInfoByGroupsService
            .addMultiple(
                this.sharedService.currentUser.groupId,
                customer.oid,
                this.contactFormArray.value
            )
            .toPromise();

        await this.customCustomerTypesService
            .add({
                customerId: customer.oid,
                customerType: this.pricingTemplateId,
                fboid: this.sharedService.currentUser.fboId,
            })
            .toPromise();

        if (this.aircraftFormArray.value[0].tailNumber != null) {
            await this.customerAircraftsService
                .addMultipleWithTemplate(
                    this.sharedService.currentUser.groupId,
                    this.sharedService.currentUser.fboId,
                    customer.oid,
                    this.aircraftFormArray.value.map((v) => ({
                        aircraftId: v.aircraft?.aircraftId ?? this.aircraftType.aircraftId,
                        pricingTemplateId: v.aircraftPricingTemplate,
                        size: v.aircraft?.size ?? 0,
                        tailNumber: v.tailNumber,
                    }))
                )
                .toPromise();
        }

        this.submitting = false;

        this.dialogRef.close(cig.oid);
    }

    stepDisabled(step: WizardStep) {
        return (
            (step === WizardStep.CONTACT_INFO &&
                this.companyFormGroup.invalid) ||
            (step === WizardStep.ITP_MARGIN_TEMPLATE &&
                (this.companyFormGroup.invalid ||
                    this.contactFormArray.invalid))
            ||
            (step === WizardStep.AIRCRAFT &&
                (this.companyFormGroup.invalid ||
                    this.contactFormArray.invalid ||
                    this.templateFormGroup.invalid))
        );
    }

    displayAircraft(aircraft: AircraftType) {
        return aircraft ? `${aircraft.make} ${aircraft.model}` : null;
    }

    private recalculatePriceBreakdown(): void {
        // Set a timeout so the child component is aware of model changes
        const self = this;
        setTimeout(() => {
            self.priceBreakdownPreview.performRecalculation();
        });
    }
}
