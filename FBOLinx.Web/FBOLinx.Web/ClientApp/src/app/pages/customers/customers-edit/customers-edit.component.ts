import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { ContactinfobygroupsService } from '../../../services/contactinfobygroups.service';
import { ContactsService } from '../../../services/contacts.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomerCompanyTypesService } from '../../../services/customer-company-types.service';
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { CustomersService } from '../../../services/customers.service';
import { CustomersviewedbyfboService } from '../../../services/customersviewedbyfbo.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { DistributionWizardMainComponent } from '../../../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component';
import { CustomerCompanyTypeDialogComponent } from '../customer-company-type-dialog/customer-company-type-dialog.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Customers',
        link: '#/default-layout/customers'
    },
    {
        title: 'Edit Customer',
        link: ''
    }
];

@Component({
    selector: 'app-customers-edit',
    templateUrl: './customers-edit.component.html',
    styleUrls: ['./customers-edit.component.scss']
})
/** customers-edit component*/
export class CustomersEditComponent {

    //Private Members
    private _RequiresRouting: boolean = false;

    //Public Members
    public pageTitle: string = 'Edit Customer';
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
    public isSaving: boolean = false;
    public certificateTypes: any[];
    public customerCompanyTypes: any[];

    //Input/Output Bindings
    @Output() saveCustomerClicked = new EventEmitter<any>();
    @Output() cancelCustomerEditclicked = new EventEmitter<any>();
    @Input() customerInfoByGroup: any;

    /** customers-edit ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private customCustomerTypesService: CustomcustomertypesService,
        private contactInfoByGroupsService: ContactinfobygroupsService,
        private contactsService: ContactsService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerContactsService: CustomercontactsService,
        private customersService: CustomersService,
        private pricingTemplatesService: PricingtemplatesService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService,
        private customerCompanyTypesService: CustomerCompanyTypesService,
        private customersViewedByFboService: CustomersviewedbyfboService,
        public deleteCustomerDialog: MatDialog,
        public dialog: MatDialog    ) {

        this.sharedService.emitChange(this.pageTitle);
        this.customerInfoByGroupService.getCertificateTypes().subscribe((data: any) => this.certificateTypes = data);

        //Check for passed in id
        let id = this.route.snapshot.paramMap.get('id');
        if (!id) {
            this.loadCustomerContacts();
            this.loadPricingTemplates();
            this.loadCustomerAircrafts();
            this.loadCustomCustomerType();
            this.loadCustomerCompanyTypes();
            return;
        } else {
            this._RequiresRouting = true;
            this.customerInfoByGroupService.get({ oid: id }).subscribe((data: any) => {
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

    //Public Methods
    public saveCustomerEdit() {
        this.isSaving = true;

        //Update customer information
        this.customerInfoByGroupService.update(this.customerInfoByGroup).subscribe((data: any) => {
            for (let customCustomerType of this.customCustomerTypes) {
                this.saveCustomCustomerType(customCustomerType);
            }
            //Update other aspects
            if (this._RequiresRouting)
                this.router.navigate(['/default-layout/customers/']);
            else
                this.saveCustomerClicked.emit();
        });
    }

    public cancelCustomerEdit() {
        if (this._RequiresRouting)
            this.router.navigate(['/default-layout/customers/']);
        else
            this.cancelCustomerEditclicked.emit();
    }

    public contactDeleted(contact) {
        this.customerContactsService.remove({ oid: contact.CustomerContactId }).subscribe((data: any) => {
            this.contactInfoByGroupsService.remove({ oid: contact.ContactInfoByGroupId }).subscribe((data: any) => {
                delete this.contactsData[contact];
            });
        });
    }

    public newContactClicked() {
        this.selectedContactRecord = null;
        this.currentContactInfoByGroup = {
            oid: 0,
            contactId: 0,
            groupId: this.sharedService.currentUser.groupId
        };
    }

    public editContactClicked(contact) {
        this.selectedContactRecord = contact;
        this.contactInfoByGroupsService.get({ oid: contact.contactInfoByGroupId }).subscribe((data: any) => this.currentContactInfoByGroup = data);
    }

    public saveEditContactClicked() {
        if (this.currentContactInfoByGroup.contactId == 0) {
            this.contactsService.add({ oid: 0 }).subscribe((data: any) => {
                this.currentContactInfoByGroup.contactId = data.oid;
                this.saveContactInfoByGroup();
            });
        } else {
            this.saveContactInfoByGroup();
        }
    }

    public cancelEditContactClicked() {
        this.currentContactInfoByGroup = null;
    }

    public newCustomerAircraftAdded() {
        this.loadCustomerAircrafts();
    }

    public editCustomerAircraftClicked(customerAircraft) {
        this.selectedCustomerAircraftRecord = customerAircraft;
        this.customerAircraftsService.get({ oid: customerAircraft.oid })
            .subscribe((data: any) => this.currentCustomerAircraft = data);
    }

    public saveEditCustomerAircraftClicked() {
        this.currentCustomerAircraft = null;
        this.loadCustomerAircrafts();
    }

    public cancelEditCustomerAircraftClicked() {
        this.currentCustomerAircraft = null;
    }

    public distributePricing() {
        var customer = this.customerInfoByGroup;
        var data = {
            customer: customer,
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId
        };
        const dialogRef = this.dialog.open(DistributionWizardMainComponent, {
            data: data
        });

        dialogRef.afterClosed().subscribe(result => {

        });
    }

    public customerCompanyTypeChanged() {
        if (this.customerInfoByGroup.customerCompanyType > -1)
            return;
        var customers = [this.customerInfoByGroup];
        var data = {
            fboId: this.sharedService.currentUser.fboId,
            groupId: this.sharedService.currentUser.groupId
        };
        const dialogRef = this.dialog.open(CustomerCompanyTypeDialogComponent, {
            data: data
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            this.customerCompanyTypesService.add(result).subscribe((data: any) => {
                this.customerInfoByGroup.customerCompanyType = data.oid;
                this.loadCustomerCompanyTypes();
            });
        });
    }

    public newCustomerPricingTemplate() {
        this.customCustomerTypes.push({
            oid: 0,
            fboId: this.sharedService.currentUser.fboId,
            customerId: this.customerInfoByGroup.customerId
        });
    }

    public deleteCustomerPricingTemplate(customCustomerType) {
        const dialogRef = this.deleteCustomerDialog.open(DeleteConfirmationComponent, {
            data: { item: customCustomerType, description: 'customer\'s pricing template' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            if (result.item.oid > 0) {
                this.customCustomerTypesService.remove({ oid: result.item.oid }).subscribe(
                    (data: any) => {
                        this.customCustomerTypes.splice(this.customCustomerTypes.indexOf(customCustomerType), 1);
                    });
            } else {
                this.customCustomerTypes.splice(this.customCustomerTypes.indexOf(customCustomerType), 1);
            }
        });
    }

    //Private Methods
    private loadCustomerContacts() {
        this.contactInfoByGroupsService.getCustomerContactInfoByGroup(this.sharedService.currentUser.groupId, this.customerInfoByGroup.customerId).subscribe(
            (data:
                any) => {
                this.contactsData = data;
                this.currentContactInfoByGroup = null;
            });
    }

    private loadPricingTemplates() {
        this.pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.pricingTemplatesData = data;
        });
    }

    private loadCustomerAircrafts() {
        this.customerAircraftsService
            .getCustomerAircraftsByGroupAndCustomerId(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId,
                this.customerInfoByGroup.customerId).subscribe((data:
                any) => {
                this.customerAircraftsData = data;
            });
    }

    private loadCustomCustomerType() {
        this.customCustomerTypesService
            .getForFboAndCustomer(this.sharedService.currentUser.fboId, this.customerInfoByGroup.customerId).subscribe(
                (data:
                    any) => {
                    //if (data)
                        this.customCustomerTypes = data;
                    //else
                    //    this.customCustomerTypes = {
                    //        fboId: this.sharedService.currentUser.fboId,
                    //        customerId: this.customerInfoByGroup.customerId
                    //    }
                });
    }

    private loadCustomerCompanyTypes() {
        this.customerCompanyTypesService.getForFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.customerCompanyTypes = data;
            this.customerCompanyTypes.push({ oid: -1, name: '<Add Custom>' });
        });
    }

    private saveContactInfoByGroup() {
        if (this.currentContactInfoByGroup.oid == 0) {
            this.contactInfoByGroupsService.add(this.currentContactInfoByGroup).subscribe((data: any) => {
                this.currentContactInfoByGroup.oid = data.oid;
                this.saveCustomerContact();
            });
        } else {
            this.contactInfoByGroupsService.update(this.currentContactInfoByGroup).subscribe((data: any) => {
                this.saveCustomerContact();
            });
        }
    }

    private saveCustomerContact() {
        if (!this.selectedContactRecord) {
            this.customerContactsService.add({ customerId: this.customerInfoByGroup.customerId, contactId: this.currentContactInfoByGroup.contactId }).subscribe((data:
                any) => {
                this.loadCustomerContacts();
            })
        } else {
            this.loadCustomerContacts();
        }
    }

    private saveCustomCustomerType(customCustomerType) {
        customCustomerType.customerId = this.customerInfoByGroup.customerId;
        if (!customCustomerType.oid || customCustomerType.oid == 0) {
            this.customCustomerTypesService.add(customCustomerType).subscribe((data: any) => {

            });
        } else {
            this.customCustomerTypesService.update(customCustomerType).subscribe((data: any) => {

            });
        }
    }

    private markCustomerAsViewedByFbo() {
        this.customersViewedByFboService.add({ fboId: this.sharedService.currentUser.fboId, groupId: this.sharedService.currentUser.groupId, customerId: this.customerInfoByGroup.customerId }).subscribe((data:
            any) => {
            
        });
    }
}
