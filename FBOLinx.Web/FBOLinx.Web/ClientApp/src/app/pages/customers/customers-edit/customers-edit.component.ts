import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

import { combineLatest, EMPTY, of } from 'rxjs';
import { find } from 'lodash';

// Services
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { ContactinfobygroupsService } from '../../../services/contactinfobygroups.service';
import { ContactsService } from '../../../services/contacts.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomerCompanyTypesService } from '../../../services/customer-company-types.service';
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { CustomersviewedbyfboService } from '../../../services/customersviewedbyfbo.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { FbofeeandtaxomitsbycustomerService } from '../../../services/fbofeeandtaxomitsbycustomer.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';

// Components
import { CustomerCompanyTypeDialogComponent } from '../customer-company-type-dialog/customer-company-type-dialog.component';
import { ContactsDialogNewContactComponent } from '../../contacts/contacts-edit-modal/contacts-edit-modal.component';
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';
import { catchError, debounceTime, switchMap } from 'rxjs/operators';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'Customers',
        link: '/default-layout/customers',
    },
    {
        title: 'Edit Customer',
        link: '',
    },
];

@Component({
    selector: 'app-customers-edit',
    templateUrl: './customers-edit.component.html',
    styleUrls: [ './customers-edit.component.scss' ],
})
export class CustomersEditComponent implements OnInit {
    // Members
    pageTitle = 'Edit Customer';
    breadcrumb = BREADCRUMBS;
    customerInfoByGroup: any;
    contactsData: any[];
    pricingTemplatesData: any[];
    customerAircraftsData: any[];
    selectedContactRecord: any;
    currentContactInfoByGroup: any;
    customCustomerType: any;
    certificateTypes: any[];
    customerCompanyTypes: any[];
    hasContactForPriceDistribution = false;
    customerForm: FormGroup;
    feesAndTaxes: Array<any>;
    @ViewChild('priceBreakdownPreview')
    private priceBreakdownPreview: PriceBreakdownComponent;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private customCustomerTypesService: CustomcustomertypesService,
        private contactInfoByGroupsService: ContactinfobygroupsService,
        private contactsService: ContactsService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerContactsService: CustomercontactsService,
        private pricingTemplatesService: PricingtemplatesService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService,
        private customerCompanyTypesService: CustomerCompanyTypesService,
        private customersViewedByFboService: CustomersviewedbyfboService,
        private dialog: MatDialog,
        private newContactDialog: MatDialog,
        private fboFeesAndTaxesService: FbofeesandtaxesService,
        private fboFeeAndTaxOmitsbyCustomerService: FbofeeandtaxomitsbycustomerService,
        private snackBar: MatSnackBar,
    ) {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.sharedService.titleChange(this.pageTitle);
    }

    async ngOnInit() {
        const id = this.route.snapshot.paramMap.get('id');
        this.customerInfoByGroup = await this.customerInfoByGroupService.get({ oid: id }).toPromise();
        const results = await combineLatest([
            this.customerInfoByGroupService.getCertificateTypes(),
            this.pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId, this.sharedService.currentUser.groupId),
            this.contactInfoByGroupsService.getCustomerContactInfoByGroup(
                this.sharedService.currentUser.groupId,
                this.customerInfoByGroup.customerId
            ),
            this.customerAircraftsService.getCustomerAircraftsByGroupAndCustomerId(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId
            ),
            this.customCustomerTypesService.getForFboAndCustomer(
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId
            ),
            this.customerCompanyTypesService.getNonFuelerLinxForFbo(this.sharedService.currentUser.fboId),
            this.customersViewedByFboService.add({
                fboId: this.sharedService.currentUser.fboId,
                groupId: this.sharedService.currentUser.groupId,
                customerId: this.customerInfoByGroup.customerId,
            }),
        ]).toPromise();

        this.certificateTypes = results[0] as any[];
        this.pricingTemplatesData = results[1] as any[];
        this.contactsData = results[2] as any[];
        this.customerAircraftsData = results[3] as any[];
        this.customCustomerType = results[4];
        this.customerCompanyTypes = results[5] as any[];
        if (find(this.contactsData, c => c.copyAlerts)) {
            this.hasContactForPriceDistribution = true;
        }

        this.customerForm = this.formBuilder.group({
            active: [ this.customerInfoByGroup.active ],
            company: [ this.customerInfoByGroup.company ],
            customerCompanyType: [ this.customerInfoByGroup.customerCompanyType ],
            certificateType: [ this.customerInfoByGroup.certificateType ],
            mainPhone: [ this.customerInfoByGroup.mainPhone ],
            address: [ this.customerInfoByGroup.address ],
            city: [ this.customerInfoByGroup.city ],
            state: [ this.customerInfoByGroup.state ],
            zipCode: [ this.customerInfoByGroup.zipCode ],
            country: [ this.customerInfoByGroup.country ],
            website: [ this.customerInfoByGroup.website ],
            distribute: [ this.customerInfoByGroup.distribute ],
            showJetA: [ this.customerInfoByGroup.showJetA ],
            show100Ll: [ this.customerInfoByGroup.show100Ll ],
            customerMarginTemplate: [ this.customCustomerType.customerType ],
        });
        this.customerForm.valueChanges.pipe(
            debounceTime(1000),
            switchMap(async () => {
                const customerInfoByGroup = {
                    ...this.customerInfoByGroup,
                    ...this.customerForm.value,
                };
                this.customCustomerType.customerType = this.customerForm.value.customerMarginTemplate;

                await this.customerInfoByGroupService.update(customerInfoByGroup).toPromise();
                if (!this.customCustomerType.oid || this.customCustomerType.oid === 0) {
                    await this.customCustomerTypesService.add(this.customCustomerType).toPromise();
                } else {
                    await this.customCustomerTypesService.update(this.customCustomerType).toPromise();
                }
            }),
            catchError((err: Error) => {
                console.error(err);
                this.snackBar.open(err.message, '', {
                    duration: 5000,
                    panelClass: [ 'blue-snackbar' ],
                });
                return of(EMPTY);
            })
        ).subscribe();
        this.customerForm.controls.customerCompanyType.valueChanges.subscribe(type => {
            if (type < 0) {
                this.customerCompanyTypeChanged();
            }
        });
        this.customerForm.controls.customerMarginTemplate.valueChanges.subscribe((selectedValue) => {
            this.customCustomerType.customerType = selectedValue;
            this.recalculatePriceBreakdown();
        });

        this.loadCustomerFeesAndTaxes();
    }

    // Methods
    cancelCustomerEdit() {
        this.router.navigate([ '/default-layout/customers/' ]).then();
    }

    contactDeleted(contact) {
        this.customerContactsService
            .remove(contact.customerContactId)
            .subscribe(() => {
                this.contactInfoByGroupsService
                    .remove(contact.contactInfoByGroupId)
                    .subscribe(() => {
                        const index = this.contactsData.findIndex(
                            (d) =>
                                d.customerContactId ===
                                contact.customerContactId
                        ); // find index in your array
                        this.contactsData.splice(index, 1); // remove element from array
                    });
            });
    }

    newContactClicked() {
        this.selectedContactRecord = null;
        this.currentContactInfoByGroup = {
            oid: 0,
            contactId: 0,
            groupId: this.sharedService.currentUser.groupId,
        };

        const dialogRef = this.newContactDialog.open(
            ContactsDialogNewContactComponent,
            {
                data: this.currentContactInfoByGroup,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result !== 'cancel') {
                if (this.currentContactInfoByGroup.contactId === 0) {
                    this.contactsService
                        .add({ oid: 0 })
                        .subscribe((data: any) => {
                            this.currentContactInfoByGroup.contactId = data.oid;
                            this.saveContactInfoByGroup();
                        });
                } else {
                    this.saveContactInfoByGroup();
                }
            } else {
                this.loadCustomerContacts();
            }
        });
    }

    updateCustomerPricingTemplate(pricingTemplateId: number) {
        if (this.customCustomerType) {
            this.customCustomerType.customerType = pricingTemplateId;
        }
    }

    customerCompanyTypeChanged() {
        const data = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
        };
        const dialogRef = this.dialog.open(CustomerCompanyTypeDialogComponent, {
            data,
        });

        dialogRef.afterClosed().subscribe((response) => {
            if (!response) {
                return;
            }
            this.customerCompanyTypesService
                .add(response)
                .subscribe((result: any) => {
                    this.customerCompanyTypes.push(result);
                    this.customerForm.patchValue({
                        customerCompanyType: result.oid,
                    });
                });
        });
    }

    toggleChange($event) {
        if ($event.checked) {
            this.customerInfoByGroup.showJetA = true;
            this.customerInfoByGroup.show100Ll = true;
            this.customerInfoByGroup.distribute = true;
        } else {
            this.customerInfoByGroup.showJetA = false;
            this.customerInfoByGroup.show100Ll = false;
            this.customerInfoByGroup.distribute = false;
        }
    }

    omitFeeAndTaxCheckChanged(feeAndTax: any): void {
        if (!feeAndTax.omitsByCustomer) {
            feeAndTax.omitsByCustomer = [];
        }
        let omitRecord: any = {
            oid: 0,
            fboFeeAndTaxId: feeAndTax.oid,
            customerId: this.customerInfoByGroup.customerId
        };
        if (feeAndTax.omitsByCustomer.length > 0) {
            omitRecord = feeAndTax.omitsByCustomer[0];
        } else {
            feeAndTax.omitsByCustomer.push(omitRecord);
        }
        omitRecord.fboFeeAndTaxId = feeAndTax.oid;
        if (feeAndTax.isOmitted) {
            this.fboFeeAndTaxOmitsbyCustomerService.add(omitRecord).subscribe((response: any) => {
                omitRecord.oid = response.oid;
                this.recalculatePriceBreakdown();
            });
        } else {
            this.fboFeeAndTaxOmitsbyCustomerService.remove(omitRecord).subscribe(() => {
                feeAndTax.omitsByCustomer = [];
                this.recalculatePriceBreakdown();
            });
        }
    }

    // Private Methods
    private recalculatePriceBreakdown(): void {
        // Set a timeout so the child component is aware of model changes
        const self = this;
        setTimeout(() => {
            self.priceBreakdownPreview.performRecalculation();
        });
    }

    private loadCustomerContacts() {
        this.contactInfoByGroupsService
            .getCustomerContactInfoByGroup(
                this.sharedService.currentUser.groupId,
                this.customerInfoByGroup.customerId
            )
            .subscribe((data: any) => {
                this.contactsData = data;
                this.currentContactInfoByGroup = null;
                this.hasContactForPriceDistribution = false;
                if (!this.contactsData) {
                    return;
                }
                for (const contact of this.contactsData) {
                    if (contact.copyAlerts) {
                        this.hasContactForPriceDistribution = true;
                    }
                }
            });
    }

    private saveContactInfoByGroup() {
        if (this.currentContactInfoByGroup.oid === 0) {
            this.contactInfoByGroupsService
                .add(this.currentContactInfoByGroup)
                .subscribe((data: any) => {
                    this.currentContactInfoByGroup.oid = data.oid;
                    this.saveCustomerContact();
                });
        } else {
            this.contactInfoByGroupsService
                .update(this.currentContactInfoByGroup)
                .subscribe(() => {
                    this.saveCustomerContact();
                });
        }
    }

    private saveCustomerContact() {
        if (!this.selectedContactRecord) {
            this.customerContactsService
                .add({
                    customerId: this.customerInfoByGroup.customerId,
                    contactId: this.currentContactInfoByGroup.contactId,
                })
                .subscribe(() => {
                    this.loadCustomerContacts();
                });
        } else {
            this.loadCustomerContacts();
        }
    }

    private loadCustomerFeesAndTaxes(): void {
        this.fboFeesAndTaxesService
            .getByFboAndCustomer(this.sharedService.currentUser.fboId, this.customerInfoByGroup.customerId).subscribe(
            (response: any[]) => {
                this.feesAndTaxes = response;
            });
    }
}
