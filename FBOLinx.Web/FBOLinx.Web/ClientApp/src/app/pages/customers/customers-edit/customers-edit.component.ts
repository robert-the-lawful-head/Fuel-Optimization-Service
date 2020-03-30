import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
} from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

// Services
import { CustomcustomertypesService } from "../../../services/customcustomertypes.service";
import { CustomeraircraftsService } from "../../../services/customeraircrafts.service";
import { ContactinfobygroupsService } from "../../../services/contactinfobygroups.service";
import { ContactsService } from "../../../services/contacts.service";
import { CustomerinfobygroupService } from "../../../services/customerinfobygroup.service";
import { CustomerCompanyTypesService } from "../../../services/customer-company-types.service";
import { CustomercontactsService } from "../../../services/customercontacts.service";
import { CustomersviewedbyfboService } from "../../../services/customersviewedbyfbo.service";
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { SharedService } from "../../../layouts/shared-service";

// Components
import { DistributionWizardMainComponent } from "../../../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component";
import { CustomerCompanyTypeDialogComponent } from "../customer-company-type-dialog/customer-company-type-dialog.component";
import { DeleteConfirmationComponent } from "../../../shared/components/delete-confirmation/delete-confirmation.component";
import { ContactsDialogNewContactComponent } from "../../contacts/contacts-edit-modal/contacts-edit-modal.component";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "#/default-layout",
    },
    {
        title: "Customers",
        link: "#/default-layout/customers",
    },
    {
        title: "Edit Customer",
        link: "",
    },
];

@Component({
    selector: "app-customers-edit",
    templateUrl: "./customers-edit.component.html",
    styleUrls: ["./customers-edit.component.scss"],
})
export class CustomersEditComponent {
    // Private Members
    private requiresRouting = false;

    // Public Members
    public pageTitle = "Edit Customer";
    public breadcrumb: any[] = BREADCRUMBS;
    public pricingTemplatesDataSource: Array<any>;
    public contactsData: Array<any>;
    public pricingTemplatesData: Array<any>;
    public customerAircraftsData: Array<any>;
    public selectedContactRecord: any;
    public selectedCustomerAircraftRecord: any;
    public currentContactInfoByGroup: any;
    public currentCustomerAircraft: any;
    public customCustomerTypes: any[];
    public isSaving = false;
    public certificateTypes: any[];
    public customerCompanyTypes: any[];
    public isLoading = false;
    public hasContactForPriceDistribution = false;

    // Input/Output Bindings
    @Output() saveCustomerClicked = new EventEmitter<any>();
    @Output() cancelCustomerEditclicked = new EventEmitter<any>();
    @Input() customerInfoByGroup: any;

    constructor(
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
        public deleteCustomerDialog: MatDialog,
        public dialog: MatDialog,
        public newContactDialog: MatDialog
    ) {
        this.sharedService.emitChange(this.pageTitle);
        this.customerInfoByGroupService
            .getCertificateTypes()
            .subscribe((data: any) => (this.certificateTypes = data));

        // Check for passed in id
        const id = this.route.snapshot.paramMap.get("id");
        if (!id) {
            this.loadCustomerContacts();
            this.loadPricingTemplates();
            this.loadCustomerAircrafts();
            this.loadCustomCustomerType();
            this.loadCustomerCompanyTypes();
            return;
        } else {
            this.requiresRouting = true;
            this.customerInfoByGroupService
                .get({ oid: id })
                .subscribe((data: any) => {
                    this.customerInfoByGroup = data;
                    this.loadCustomerContacts();
                    this.loadPricingTemplates();
                    this.loadCustomerAircrafts();
                    this.loadCustomCustomerType();
                    this.loadCustomerCompanyTypes();
                    this.markCustomerAsViewedByFbo();
                });
        }
    }

    // Public Methods
    public saveCustomerEdit() {
        this.isSaving = true;
        // Update customer information
        this.customerInfoByGroupService
            .update(this.customerInfoByGroup)
            .subscribe(() => {
                for (const customCustomerType of this.customCustomerTypes) {
                    this.saveCustomCustomerType(customCustomerType);
                }
                // Update other aspects
                if (this.requiresRouting) {
                    sessionStorage.setItem("isCustomerEdit", "1");
                    this.router.navigate(["/default-layout/customers/"]);
                } else {
                    this.saveCustomerClicked.emit();
                }
            });
    }

    public saveDirectCustomerEdit() {
        this.isSaving = true;
        this.requiresRouting = true;
        // Update customer information
        this.customerInfoByGroupService
            .update(this.customerInfoByGroup)
            .subscribe((data: any) => {
                for (const customCustomerType of this.customCustomerTypes) {
                    this.saveCustomCustomerType(customCustomerType);
                }
                // Update other aspects
                if (this.requiresRouting) {
                    sessionStorage.setItem("isCustomerEdit", "1");
                    this.router.navigate(["/default-layout/customers/"]);
                } else {
                    this.saveCustomerClicked.emit();
                }
            });
    }

    public cancelCustomerEdit() {
        if (this.requiresRouting) {
            sessionStorage.setItem("isCustomerEdit", "1");
            this.router.navigate(["/default-layout/customers/"]);
        } else {
            this.cancelCustomerEditclicked.emit();
        }
    }

    public contactDeleted(contact) {
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

    public newContactClicked() {
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
            if (result !== "cancel") {
                this.requiresRouting = false;
                this.saveCustomerEdit();
                if (this.currentContactInfoByGroup.contactId === 0) {
                    this.contactsService
                        .add({ oid: 0 })
                        .subscribe((data: any) => {
                            this.currentContactInfoByGroup.contactId = data.oid;
                            this.saveContactInfoByGroup();
                        });
                } else {
                    this.saveCustomerEdit();
                    this.saveContactInfoByGroup();
                }
            } else {
                this.loadCustomerContacts();
            }
        });
    }

    public editContactClicked(contact) {
        const dialogRef = this.newContactDialog.open(
            ContactsDialogNewContactComponent,
            {
                data: contact,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result !== "cancel") {
                if (result.toDelete) {
                    this.customerContactsService
                        .remove(result.customerContactId)
                        .subscribe(() => {
                            this.contactInfoByGroupsService
                                .remove(contact.contactInfoByGroupId)
                                .subscribe(() => {
                                    this.loadCustomerContacts();
                                });
                        });
                } else {
                    this.selectedContactRecord = contact;
                    this.contactInfoByGroupsService
                        .get({ oid: contact.contactInfoByGroupId })
                        .subscribe((data: any) => {
                            if (data) {
                                this.currentContactInfoByGroup = data;
                                if (this.currentContactInfoByGroup.oid) {
                                    data.email = contact.email;
                                    data.firstName = contact.firstName;
                                    data.lastName = contact.lastName;
                                    data.title = contact.title;
                                    data.phone = contact.phone;
                                    data.extension = contact.extension;
                                    data.mobile = contact.mobile;
                                    data.fax = contact.fax;
                                    data.address = contact.address;
                                    data.city = contact.city;
                                    data.state = contact.state;
                                    data.country = contact.country;
                                    data.primary = contact.primary;
                                    data.copyAlerts = contact.copyAlerts;
                                    this.saveContactInfoByGroup();
                                }
                            }
                        });
                }
            } else {
                this.loadCustomerContacts();
            }
        });
    }

    public saveEditContactClicked() {
        this.saveCustomerEdit();
        if (this.currentContactInfoByGroup.contactId === 0) {
            this.contactsService.add({ oid: 0 }).subscribe((data: any) => {
                this.currentContactInfoByGroup.contactId = data.oid;
                this.saveContactInfoByGroup();
            });
        } else {
            this.saveCustomerEdit();
            this.saveContactInfoByGroup();
        }
    }

    public cancelEditContactClicked() {
        this.currentContactInfoByGroup = null;
    }

    public newCustomerAircraftAdded() {
        this.saveCustomerEdit();
        this.loadCustomerAircrafts();
    }

    public editCustomerAircraftClicked(customerAircraft) {
        this.currentCustomerAircraft = null;
        this.requiresRouting = false;
        this.saveCustomerEdit();
        this.loadCustomerAircrafts();
        this.selectedCustomerAircraftRecord = customerAircraft;
    }

    public saveEditCustomerAircraftClicked() {
        this.currentCustomerAircraft = null;

        this.loadCustomerAircrafts();
    }

    public cancelEditCustomerAircraftClicked() {
        this.currentCustomerAircraft = null;
    }

    public distributePricing() {
        this.isLoading = true;
        const customer = this.customerInfoByGroup;
        const data = {
            customer,
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId,
        };

        // Update customer information
        this.customerInfoByGroupService
            .update(this.customerInfoByGroup)
            .subscribe(() => {
                for (const customCustomerType of this.customCustomerTypes) {
                    customCustomerType.customerId = this.customerInfoByGroup.customerId;
                }
                const pricingTemplatesToUpdate = [];

                for (const customCustomerType of this.customCustomerTypes) {
                    if (customCustomerType.requiresUpdate) {
                        pricingTemplatesToUpdate.push(customCustomerType);
                    }
                }

                this.customCustomerTypesService
                    .updateCollection(pricingTemplatesToUpdate)
                    .subscribe(() => {
                        this.customCustomerTypesService
                            .getForFboAndCustomer(
                                this.sharedService.currentUser.fboId,
                                this.customerInfoByGroup.customerId
                            )
                            .subscribe((result: any) => {
                                this.customCustomerTypes = result;
                                this.isLoading = false;

                                // Show dispatch dialog
                                const dialogRef = this.dialog.open(
                                    DistributionWizardMainComponent,
                                    {
                                        data,
                                        disableClose: true,
                                    }
                                );

                                dialogRef.afterClosed().subscribe(() => {});
                            });
                    });
            });
    }

    public customerCompanyTypeChanged() {
        if (this.customerInfoByGroup.customerCompanyType > -1) {
            return;
        }
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
                    this.customerInfoByGroup.customerCompanyType = result.oid;
                    this.loadCustomerCompanyTypes();
                });
        });
    }

    public newCustomerPricingTemplate() {
        this.customCustomerTypes.push({
            oid: 0,
            fboId: this.sharedService.currentUser.fboId,
            customerId: this.customerInfoByGroup.customerId,
        });
    }

    public deleteCustomerPricingTemplate(customCustomerType) {
        const dialogRef = this.deleteCustomerDialog.open(
            DeleteConfirmationComponent,
            {
                data: {
                    item: customCustomerType,
                    description: "customer's margin template",
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            if (result.item.oid > 0) {
                this.customCustomerTypesService
                    .remove({ oid: result.item.oid })
                    .subscribe(() => {
                        this.customCustomerTypes.splice(
                            this.customCustomerTypes.indexOf(
                                customCustomerType
                            ),
                            1
                        );
                    });
            } else {
                this.customCustomerTypes.splice(
                    this.customCustomerTypes.indexOf(customCustomerType),
                    1
                );
            }
        });
    }

    public pricingTemplateSelectionChanged(customCustomerType) {
        customCustomerType.requiresUpdate = true;
    }

    // Private Methods
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

    public toggleChange($event) {
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

    private loadPricingTemplates() {
        this.pricingTemplatesService
            .getByFbo(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.pricingTemplatesData = data;
            });
    }

    private loadCustomerAircrafts() {
        this.customerAircraftsService
            .getCustomerAircraftsByGroupAndCustomerId(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId
            )
            .subscribe((data: any) => {
                this.customerAircraftsData = data;
            });
    }

    private loadCustomCustomerType() {
        this.customCustomerTypesService
            .getForFboAndCustomer(
                this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId
            )
            .subscribe((data: any) => {
                this.customCustomerTypes = data;
            });
    }

    private loadCustomerCompanyTypes() {
        this.customerCompanyTypesService
            .getForFbo(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.customerCompanyTypes = data;
                this.customerCompanyTypes.push({
                    oid: -1,
                    name: "<Add Custom>",
                });
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
                .subscribe((data: any) => {
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

    private saveCustomCustomerType(customCustomerType) {
        customCustomerType.customerId = this.customerInfoByGroup.customerId;
        if (!customCustomerType.oid || customCustomerType.oid === 0) {
            this.customCustomerTypesService
                .add(customCustomerType)
                .subscribe(() => {});
        } else {
            this.customCustomerTypesService
                .update(customCustomerType)
                .subscribe(() => {});
        }
    }

    private markCustomerAsViewedByFbo() {
        this.customersViewedByFboService
            .add({
                fboId: this.sharedService.currentUser.fboId,
                groupId: this.sharedService.currentUser.groupId,
                customerId: this.customerInfoByGroup.customerId,
            })
            .subscribe(() => {});
    }
}
