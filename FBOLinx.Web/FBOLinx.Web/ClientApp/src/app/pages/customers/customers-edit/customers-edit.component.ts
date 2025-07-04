import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacySnackBar as MatSnackBar } from '@angular/material/legacy-snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { combineLatest, EMPTY, of } from 'rxjs';
import { catchError, debounceTime, map, switchMap } from 'rxjs/operators';
import { CustomermarginsService } from 'src/app/services/customermargins.service';
import { TagsService } from 'src/app/services/tags.service';

import { SharedService } from '../../../layouts/shared-service';
import { CustomerInfoByGroupNote } from '../../../models/customer-info-by-group-note';
import { ContactinfobyfboService } from '../../../services/contactinfobyfbo.service';
import { ContactinfobygroupsService } from '../../../services/contactinfobygroups.service';
import { ContactsService } from '../../../services/contacts.service';
// Services
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
import { CustomerCompanyTypesService } from '../../../services/customer-company-types.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomersviewedbyfboService } from '../../../services/customersviewedbyfbo.service';
import { FbofeeandtaxomitsbycustomerService } from '../../../services/fbofeeandtaxomitsbycustomer.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';
import { ContactsDialogNewContactComponent } from '../../contacts/contacts-edit-modal/contacts-edit-modal.component';
// Components
import { CustomerCompanyTypeDialogComponent } from '../customer-company-type-dialog/customer-company-type-dialog.component';
import { CustomerTagDialogComponent } from '../customer-tag-dialog/customer-tag-dialog.component';
import { MarginType } from 'src/app/enums/margin-type.enum';

export interface FboFeeAndTaxOmitsByCustomer
{
    oid: number;
    fboFeeAndTaxId: number;
    customerId: number;
    isOmitted: boolean | null;
}
@Component({
    selector: 'app-customers-edit',
    styleUrls: ['./customers-edit.component.scss'],
    templateUrl: './customers-edit.component.html',
})
export class CustomersEditComponent implements OnInit {
    @Input() customerInfoByGroupId: number = 0;
    @Input() showBreadCrumb: boolean = true;
    @Input() showAircraftTab: boolean = true;
    @Input() showPricingTab: boolean = true;
    @Input() showAnalyticsTab: boolean = true;
    @Input() showHistoryTab: boolean = true;
    @Input() showClose: boolean = true;

    @ViewChild('priceBreakdownPreview')
    private priceBreakdownPreview: PriceBreakdownComponent;
    // Members
    pageTitle = 'Edit Customer';
    customerInfoByGroup: any;
    customerInfoByGroupNote: CustomerInfoByGroupNote;
    contactsData: any[];
    pricingTemplatesData: any[];
    customerAircraftsData: any[];
    selectedContactRecord: any;
    currentContactInfoByGroup: any;
    customCustomerType: any;
    certificateTypes: any[];
    selectedIndex: any  = 0;
    customerCompanyTypes: any[];
    customerForm: UntypedFormGroup;
    public customerHistory: any;
    feesAndTaxes: Array<any>;
    isEditing: boolean = false;
    public customerId: number;
    tags: any[];
    tagsSelected: any[] = [];
    tagSubsctiption: Subscription;
    loading: boolean = false;
    Historyupdate: boolean = false;
    isLoadingHistory: boolean = true;
    searchText: string = "";

    routeSubscription: Subscription;
    formValueChangesSubscription: Subscription[] = [];
    constructor(
        private formBuilder: UntypedFormBuilder,
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
        private tagsService: TagsService,
        private contactInfoByFboService: ContactinfobyfboService
    ) {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        

        this.routeSubscription = this.route.queryParams.subscribe((params) => {
            if (params.tab) {
                this.selectedIndex = parseInt(params.tab);
            }

            if (params.search) {
                this.searchText = params.search;
            }
        });
    }

    async ngOnInit() {
        if (this.customerInfoByGroupId == 0)
            this.customerInfoByGroupId = parseInt(this.route.snapshot.paramMap.get('id'));
        this.customerInfoByGroup = await this.customerInfoByGroupService
            .get({ oid: this.customerInfoByGroupId })
            .toPromise();
        this.customerId = this.customerInfoByGroup.customerId;

        //Grab the appropriate note
        var notesForGroup = this.customerInfoByGroup?.notes?.filter(x => !x.fboId || x.fboId == 0);
        this.customerInfoByGroupNote = notesForGroup && notesForGroup.length > 0 ? notesForGroup[0] : null;
        if (this.customerInfoByGroupNote == null) {
            this.customerInfoByGroupNote = {
                oid: 0,
                fboId: 0,
                customerInfoByGroupId: this.customerInfoByGroup.oid,
                notes: '',
                lastUpdatedByUserId: this.sharedService.currentUser.oid
            };
            this.customerInfoByGroup.notes.push(this.customerInfoByGroupNote);
        }

        const results = await combineLatest([
            //0
            this.customerInfoByGroupService.getCertificateTypes(),
            //1
            this.pricingTemplatesService.getByFbo(
                this.sharedService.currentUser.fboId,
                this.sharedService.currentUser.groupId
            ),
            //2
            this.contactInfoByGroupsService.getCustomerContactInfoByGroup(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId
            ),
            //3
            this.customerAircraftsService.getCustomerAircraftsByGroupAndCustomerId(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId
            ),
            //4
            this.customCustomerTypesService.getForFboAndCustomer(
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId
            ),
            //5
            this.customerCompanyTypesService.getNonFuelerLinxForFbo(
                this.sharedService.currentUser.fboId
            ),
            //6
            this.customerInfoByGroupService.getCustomerLogger(this.customerInfoByGroupId,
                this.sharedService.currentUser.fboId),

            this.customersViewedByFboService.add({
                customerId: this.customerInfoByGroup.customerId,
                fboId: this.sharedService.currentUser.fboId,
                groupId: this.sharedService.currentUser.groupId,
            }),



        ]).toPromise();

        this.certificateTypes = results[0] as any[];
        this.pricingTemplatesData = results[1] as any[];
        this.contactsData = results[2] as any[];
        this.customerAircraftsData = results[3] as any[];
        this.customCustomerType = results[4];
        this.customerCompanyTypes = results[5] as any[];
        this.customerHistory = results[6] as any [];

        this.customerForm = this.formBuilder.group({
            active: [this.customerInfoByGroup.active],
            address: [this.customerInfoByGroup.address],
            certificateType: [this.customerInfoByGroup.certificateType],
            city: [this.customerInfoByGroup.city],
            company: [this.customerInfoByGroup.company],
            country: [this.customerInfoByGroup.country],
            customerCompanyType: [this.customerInfoByGroup.customerCompanyType],
            customerMarginTemplate: [this.customCustomerType.customerType],
            distribute: [this.customerInfoByGroup.distribute],
            mainPhone: [this.customerInfoByGroup.mainPhone],
            show100Ll: [this.customerInfoByGroup.show100Ll],
            showJetA: [this.customerInfoByGroup.showJetA],
            state: [this.customerInfoByGroup.state],
            website: [this.customerInfoByGroup.website],
            zipCode: [this.customerInfoByGroup.zipCode],
            customerTag: [this.customerInfoByGroup.customerTag],
            editableNote: [this.customerInfoByGroupNote.notes]
        });

        this.customerForm.valueChanges
            .pipe(
                map(() => {

                }),
                debounceTime(500),
                switchMap(async () => {
                    this.isEditing = true;
                    const customerInfoByGroup = {
                        ...this.customerInfoByGroup,
                        ...this.customerForm.value,
                    };
                    //Null the notes collection to prevent EF from saving the entire collection
                    customerInfoByGroup.notes = null;

                    this.customCustomerType.customerType =
                        this.customerForm.value.customerMarginTemplate;

                    this.saveNotes();

                    await this.customerInfoByGroupService
                        .update(customerInfoByGroup ,  this.sharedService.currentUser.oid)
                        .toPromise();
                    if (
                        !this.customCustomerType.oid ||
                        this.customCustomerType.oid === 0
                    ) {
                        await this.customCustomerTypesService
                            .add(this.customCustomerType)
                            .toPromise();
                    } else {
                        await this.customCustomerTypesService
                            .update(this.customCustomerType ,  this.sharedService.currentUser.oid)
                            .toPromise();
                    }

                    this.customerInfoByGroup = customerInfoByGroup;
                    this.isEditing = false;

                }),
                catchError((err: Error) => {
                    console.error(err);
                    this.snackBar.open(err.message, '', {
                        duration: 5000,
                        panelClass: ['blue-snackbar'],
                    });
                    this.isEditing = false;
                    return of(EMPTY);
                })
            )
            .subscribe( scusess => {this.loadCustomerHistory();});

        let customerCompanyTypeSubscription = this.customerForm.controls.customerCompanyType.valueChanges.subscribe(
            (type) => {
                if (type < 0) {
                    this.customerCompanyTypeChanged();
                    this.loadCustomerHistory();
                }
            }
        );
        this.formValueChangesSubscription.push(customerCompanyTypeSubscription);

        let customerMarginTemplateValueChangeSubscription = this.customerForm.controls.customerMarginTemplate.valueChanges.subscribe(
            (selectedValue) => {
                this.loadCustomerHistory();
                this.customCustomerType.customerType = selectedValue;
                this.recalculatePriceBreakdown();

            }
        );
        this.formValueChangesSubscription.push(customerMarginTemplateValueChangeSubscription);
        this.loadCustomerFeesAndTaxes();
        this.loadCustomerTags();
        this.checkAircraftsForCompanyPricing();
    }
    ngOnDestroy() {
        this.formValueChangesSubscription.forEach(subscription => {
            subscription?.unsubscribe();
        });  
        this.tagSubsctiption?.unsubscribe();
        this.routeSubscription?.unsubscribe();
    }
    loadCustomerHistory()
    {
       this.sharedService.updatedHistory.next(true);

    }

    get isMember() {
        return this.sharedService.currentUser.role === 4;
    }

    // Methods
    cancelCustomerEdit() {
        this.router.navigate(['/default-layout/customers/']).then();
    }

    contactDeleted(contact) {
        this.contactInfoByGroupsService
            .remove(contact.contactInfoByGroupId, this.sharedService.currentUser.oid)
            .subscribe(() => {
                this.customerContactsService
                    .remove(contact.customerContactId, this.sharedService.currentUser.oid)
                    .subscribe((data) => {

                        this.contactsService.update(data).subscribe();
                        const index = this.contactsData.findIndex(
                            (d) =>
                            d.customerContactId ===
                            contact.customerContactId
                        ); // find index in your array
                        this.contactsData.splice(index, 1); // remove element from array
                    });

                    this.loadCustomerHistory();
            });
    }

    newContactClicked() {
        this.selectedContactRecord = null;
        this.currentContactInfoByGroup = {
            contactId: 0,
            copyAlerts: true,
            email: '',
            groupId: this.sharedService.currentUser.groupId,
            oid: 0,
        };

        const dialogRef = this.newContactDialog.open(
            ContactsDialogNewContactComponent,
            {
                data: this.currentContactInfoByGroup,
                disableClose: true
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
                            this.loadCustomerHistory();
                        });
                } else {
                    this.saveContactInfoByGroup();
                    this.loadCustomerHistory();

                }
            } else {
                this.loadCustomerContacts();
                this.loadCustomerHistory();

            }
        });
    }

    updateCustomerPricingTemplate(pricingTemplateId: number) {
        if (this.customCustomerType) {
            this.customCustomerType.customerType = pricingTemplateId;
            this.loadCustomerHistory();
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
                    this.loadCustomerHistory();
                });
        });
    }

    addCustomerTag(tag) {
        this.loading = true;
        tag['groupId'] = this.sharedService.currentUser.groupId;
        tag['customerId'] = this.customerInfoByGroup.customerId;
        tag['oid'] = 0;
        this.tagsService.add(tag).subscribe(response => {
            this.loading = false;
            this.tagSubsctiption.unsubscribe();
            this.loadCustomerTags();
        });
    }

    removeCustomerTag(tag) {
        this.loading = true;
        this.tagsSelected = this.tagsSelected.filter(obj => obj.oid !== tag.oid);
        this.tagsService.remove(tag).subscribe(response => {
            this.loading = false;
            this.tagSubsctiption.unsubscribe();
            this.loadCustomerTags();
        })
    }

    newCustomTag() {
        const data = {
            customerId: this.customerInfoByGroup.customerId,
            groupId: this.sharedService.currentUser.groupId,
        };
        const dialogRef = this.dialog.open(CustomerTagDialogComponent, {
            data,
        });

        dialogRef.afterClosed().subscribe((response) => {
            if (!response) {
                return;
            }
            this.tagsService
                .add(response)
                .subscribe((result: any) => {
                    this.loadCustomerTags();
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

    async omitFeeAndTaxCheckChanged(feeAndTax: any): Promise<void> {
        feeAndTax.omitsByCustomer = feeAndTax.omitsByCustomer ?? [];

        let omitRecord: FboFeeAndTaxOmitsByCustomer = feeAndTax.omitsByCustomer.find(x => x.customerId == this.customerInfoByGroup.customerId &&
            x.fboFeeAndTaxId == feeAndTax.oid);

        let pricingTemplate = this.pricingTemplatesData.find(x => x.oid == this.customCustomerType.customerType);

        if(pricingTemplate.marginType == MarginType.RetailMinus){
            if(omitRecord){
                omitRecord.isOmitted = feeAndTax.isOmitted;
                await this.fboFeeAndTaxOmitsbyCustomerService.update(omitRecord).toPromise();
            }else{
                omitRecord = {
                    customerId: this.customerInfoByGroup.customerId,
                    fboFeeAndTaxId: feeAndTax.oid,
                    oid: 0,
                    isOmitted: feeAndTax.isOmitted
                };    
                await this.fboFeeAndTaxOmitsbyCustomerService.add(omitRecord).toPromise();
            }
            this.recalculatePriceBreakdown();
            return;
        }

        if (feeAndTax.isOmitted) {
            omitRecord = {
                customerId: this.customerInfoByGroup.customerId,
                fboFeeAndTaxId: feeAndTax.oid,
                oid: 0,
                isOmitted: feeAndTax.isOmitted
            };   
            this.fboFeeAndTaxOmitsbyCustomerService
                .add(omitRecord)
                .subscribe((response: any) => {
                    omitRecord.oid = response.oid;
                    feeAndTax.omitsByCustomer.push(omitRecord);
                    this.recalculatePriceBreakdown();
                });   
        }else {
            this.fboFeeAndTaxOmitsbyCustomerService
                .remove(omitRecord)
                .subscribe(() => {
                    feeAndTax.omitsByCustomer.splice(feeAndTax.omitsByCustomer.indexOf(omitRecord), 1);
                    this.recalculatePriceBreakdown();
                });
        }
    }

    public priceBreakdownCalculationsCompleted(
        calculationResults: any[]
    ): void {
        if (!calculationResults || !calculationResults.length) {
            return;
        }

        try {
            calculationResults[0].pricingList[0].feesAndTaxes.forEach(calculatedTax => {
                var matchingTaxes = this.feesAndTaxes.filter(feeAndTax => feeAndTax.oid == calculatedTax.oid);
                if (matchingTaxes && matchingTaxes.length > 0)
                    matchingTaxes[0].omittedFor = calculatedTax.omittedFor;
            });
        } catch (e) {

        }
    }

    tabClick(selectedTab) {
        const tab = selectedTab.tab.textLabel;
        if (tab === "History") {
            this.isLoadingHistory = true;
            this.sharedService.updatedHistory.subscribe(
                update => {
                    if (update == true) {
                        this.customerInfoByGroupService.getCustomerLogger(this.customerInfoByGroupId, this.sharedService.currentUser.fboId).subscribe(
                            data => {
                                this.customerHistory = data;
                                this.isLoadingHistory = false;
                            }
                        )
                    }
                }
            )
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
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId,
            )
            .subscribe((data: any) => {
                this.contactsData = data;
                this.currentContactInfoByGroup = null;
                if (!this.contactsData) {
                    return;
                }
            });

            this.loadCustomerHistory();
    }
    isOptionDisabled(opt: any): boolean {
        return this.tagsSelected.length >= 10 && !this.tagsSelected.find(el => el.oid == opt)
    }

    private loadCustomerTags() {
        this.tagsService.getTags({
            groupId: this.sharedService.currentUser.groupId,
            customerId: this.customerInfoByGroup.customerId,
            isFuelerLinx: this.customerInfoByGroup.customer.fuelerlinxId > 0 ? true : false
        })
            .subscribe((data: any) => {
                this.tags = data;
                this.tagsSelected = this.tags.filter(x => x.customerId == this.customerInfoByGroup.customerId);
                this.customerForm.controls.customerTag.setValue(this.tagsSelected.map(x => x.oid));

                this.tagSubsctiption = this.customerForm.controls.customerTag.valueChanges.subscribe(
                    (selectedValue) => {
                        if (selectedValue.includes(-1)) {
                            this.newCustomTag();
                            this.customerForm.controls.customerTag
                            .setValue(this.customerForm.controls.customerTag.value.filter(function(item) {
                                return item !== -1
                            }));
                        } else {
                            let addTags = this.tags.filter(x => selectedValue.includes(x.oid) && x.customerId != this.customerInfoByGroup.customerId);
                            let removeTags = this.tags.filter(x => !selectedValue.includes(x.oid) && x.customerId == this.customerInfoByGroup.customerId);
                            for (const tag of addTags) {
                                this.addCustomerTag(tag);
                            }

                            for (const tag of removeTags) {
                                this.removeCustomerTag(tag);
                            }
                        }
                    }
                );
                if (!this.tags) {
                    return;
                }
            });
    }

    private saveContactInfoByGroup() {
        if (this.currentContactInfoByGroup.oid === 0) {
            var currentContactInfoByGroup = this.currentContactInfoByGroup;
            if (currentContactInfoByGroup.copyAlerts)
                currentContactInfoByGroup.CopyAlerts = false;

            this.contactInfoByGroupsService
                .add(this.currentContactInfoByGroup, this.sharedService.currentUser.oid, this.customerInfoByGroup.customerId)
                .subscribe((data: any) => {
                    this.currentContactInfoByGroup.oid = data.oid;
                    this.saveCustomerContact();

                });
        }
        this.loadCustomerHistory();
    }

    private saveCustomerContact() {
        if (!this.selectedContactRecord) {
            this.customerContactsService
                .add({
                    contactId: this.currentContactInfoByGroup.contactId,
                    customerId: this.customerInfoByGroup.customerId,
                } , this.sharedService.currentUser.oid)
                .subscribe(() => {
                    this.UpdateCopyAlerts(this.currentContactInfoByGroup);
                });
        } else {
            this.UpdateCopyAlerts(this.currentContactInfoByGroup);

        }
    }

    private UpdateCopyAlerts(contact) {
        this.addCustomerInfoByFbo(contact);
    }

    private addCustomerInfoByFbo(contact) {
        var contactInfoByFbo = {
            ContactId: contact.contactId,
            FboId: this.sharedService.currentUser.fboId,
            CopyAlerts: contact.copyAlerts
        };

        this.contactInfoByFboService
            .add(contactInfoByFbo)
            .subscribe((data: any) => {
                this.loadCustomerContacts();
            });
    }

    private loadCustomerFeesAndTaxes(): void {
        this.fboFeesAndTaxesService
            .getByFboAndCustomer(
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId,
                this.customCustomerType.customerType
            )
            .subscribe((response: any[]) => {
                this.feesAndTaxes = response;
            });
    }

    private checkAircraftsForCompanyPricing(): void {
        this.customerAircraftsData.forEach((result) => {
            if (result.isCompanyPricing) {
                result.pricingTemplateId = null;
            }
        });
    }

    private saveNotes(): void {
        if (this.customerInfoByGroupNote == null)
            return;
        this.customerInfoByGroupNote.lastUpdatedByUserId = this.sharedService.currentUser.oid;
        if (this.customerInfoByGroupNote.oid > 0) {
            this.customerInfoByGroupService.updateCustomerInfoByGroupNote(this.customerInfoByGroupNote).subscribe((data: any) => { });
        } else {
            this.customerInfoByGroupService.addCustomerInfoByGroupNote(this.customerInfoByGroupNote).subscribe((data: any) => {
                this.customerInfoByGroupNote.oid = data.oid;
            });
        }
    }
}
