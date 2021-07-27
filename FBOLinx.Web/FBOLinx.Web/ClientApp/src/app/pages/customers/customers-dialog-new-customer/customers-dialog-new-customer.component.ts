import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, } from '@angular/material/dialog';
import { combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { SharedService } from 'src/app/layouts/shared-service';
import { AircraftSize, AircraftType, CertificateType, CustomerCompanyType, FeesAndTaxes, PricingTemplate } from 'src/app/models';
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

enum WizardStep {
    COMPANY_INFO,
    CONTACT_INFO,
    ITP_MARGIN_TEMPLATE,
    AIRCRAFT
}

@Component({
    selector: 'app-customers-dialog-new-customer',
    templateUrl: './customers-dialog-new-customer.component.html',
    styleUrls: [ './customers-dialog-new-customer.component.scss' ],
})
export class CustomersDialogNewCustomerComponent implements OnInit {
    @ViewChild('priceBreakdownPreview')
    private priceBreakdownPreview: PriceBreakdownComponent;

    customerForm: FormGroup;
    companyInfoDetailOpenState = false;
    certificateTypes: CertificateType[] = [];
    customerCompanyTypes: CustomerCompanyType[] = [];
    pricingTemplates: PricingTemplate[];
    feesAndTaxes: FeesAndTaxes[];
    aircraftTypes: AircraftType[];
    aircraftSizes: AircraftSize[];
    step: WizardStep = WizardStep.COMPANY_INFO;
    submitting = false;

    constructor(
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
        private sharedService: SharedService,
    ) {
        this.customerForm = new FormGroup({
            company: new FormGroup({
                company: new FormControl('', Validators.required),
                certificateType: new FormControl(),
                mainPhone: new FormControl(),
                address: new FormControl(),
                city: new FormControl(),
                state: new FormControl(),
                zipCode: new FormControl(),
                country: new FormControl(),
                website: new FormControl(),
            }),
            contact: new FormArray([]),
            template: new FormGroup({
                customerMarginTemplate: new FormControl(undefined, Validators.required),
            }),
            aircraft: new FormArray([]),
        });
        this.addNewContact();
        this.addNewAircraft();

        this.templateFormGroup.valueChanges.pipe(
            switchMap(() => this.fboFeesAndTaxesService.getByFboAndPricingTemplate(this.sharedService.currentUser.fboId, this.pricingTemplateId))
        ).subscribe((feesAndTaxes) => {
            this.feesAndTaxes = feesAndTaxes;
            this.recalculatePriceBreakdown();
        });
    }

    get companyFormGroup() {
        return this.customerForm.controls.company as FormGroup;
    }

    get contactFormArray() {
        return this.customerForm.controls.contact as FormArray;
    }

    get templateFormGroup() {
        return this.customerForm.controls.template as FormGroup;
    }

    get aircraftFormArray() {
        return this.customerForm.controls.aircraft as FormArray;
    }

    get backDisabled() {
        return this.step === WizardStep.COMPANY_INFO;
    }

    get nextDisabled() {
        return (this.step === WizardStep.COMPANY_INFO && this.companyFormGroup.invalid) ||
            (this.step === WizardStep.CONTACT_INFO && this.contactFormArray.invalid) ||
            (this.step === WizardStep.ITP_MARGIN_TEMPLATE && this.templateFormGroup.invalid) ||
            (this.step === WizardStep.AIRCRAFT && this.aircraftFormArray.invalid);
    }

    get submitDisabled() {
        return this.customerForm.invalid;
    }

    get pricingTemplateId() {
        return this.templateFormGroup.get('customerMarginTemplate').value;
    }

    ngOnInit(): void {
        combineLatest([
            this.customerInfoByGroupService.getCertificateTypes(),
            this.customerCompanyTypesService.getNonFuelerLinxForFbo(this.sharedService.currentUser.fboId),
            this.pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId, this.sharedService.currentUser.groupId),
            this.aircraftsService.getAll(),
            this.aircraftsService.getAircraftSizes(),

        ]).subscribe(([certificateTypes, customerCompanyTypes, pricingTemplates, aircraftTypes, aircraftSizes]) => {
            this.certificateTypes = certificateTypes;
            this.customerCompanyTypes = customerCompanyTypes;
            this.pricingTemplates = pricingTemplates;
            this.aircraftTypes = aircraftTypes;
            this.aircraftSizes = aircraftSizes;
        });
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
                data: {
                    customTitle: 'Reset Changes?',
                    customText:
                        'Are you sure to reset changes?',
                    ok: 'Reset',
                    cancel: 'Cancel',
                },
                autoFocus: false,
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
                data: {
                    customTitle: 'Discard Changes?',
                    customText:
                        'You have unsaved changes. Are you sure?',
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

    addNewContact() {
        this.contactFormArray.push(new FormGroup({
            firstName: new FormControl(),
            lastName: new FormControl(),
            title: new FormControl(),
            email: new FormControl('', [Validators.required, Validators.email]),
            phone: new FormControl(),
            copyAlerts: new FormControl(true),
            extension: new FormControl(),
            mobile: new FormControl(),
            fax: new FormControl(),
            address: new FormControl(),
            city: new FormControl(),
            state: new FormControl(),
            country: new FormControl(),
            primary: new FormControl(),
        }));
    }

    addNewAircraft() {
        this.aircraftFormArray.push(new FormGroup({
            tailNumber: new FormControl(undefined, Validators.required),
            aircraft: new FormControl(undefined, Validators.required),
            aircraftPricingTemplate: new FormControl(),
        }));
    }

    async submit() {
        this.submitting = true;
        const customer = await this.customersService.add(this.companyFormGroup.value).toPromise();
        await this.customersViewedByFboService.add({
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
            customerId: customer.oid,
        }).toPromise();

        const cig = await this.customerInfoByGroupService
            .add({
                ...this.companyFormGroup.value,
                customerId: customer.oid,
                groupId: this.sharedService.currentUser.groupId,
            }).toPromise() as any;

        await this.contactInfoByGroupsService.addMultiple(
            this.sharedService.currentUser.groupId,
            customer.oid,
            this.contactFormArray.value
        ).toPromise();

        await this.customCustomerTypesService.add({
            customerId: customer.oid,
            customerType: this.pricingTemplateId,
            fboid: this.sharedService.currentUser.fboId,
        }).toPromise();

        await this.customerAircraftsService.addMultipleWithTemplate(
            this.sharedService.currentUser.groupId,
            this.sharedService.currentUser.fboId,
            customer.oid,
            this.aircraftFormArray.value.map(v => ({
                pricingTemplateId: v.aircraftPricingTemplate,
                tailNumber: v.tailNumber,
                aircraftId: v.aircraft.aircraftId,
                size: v.aircraft.size,
            })),
        ).toPromise();

        this.submitting = false;

        this.dialogRef.close(cig.oid);
    }

    stepDisabled(step: WizardStep) {
        return (step === WizardStep.CONTACT_INFO && this.companyFormGroup.invalid) ||
            (step === WizardStep.ITP_MARGIN_TEMPLATE && (this.companyFormGroup.invalid || this.contactFormArray.invalid)) ||
            (step === WizardStep.AIRCRAFT && (this.companyFormGroup.invalid || this.contactFormArray.invalid || this.templateFormGroup.invalid));
    }

    public displayAircraft(aircraft: AircraftType) {
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
