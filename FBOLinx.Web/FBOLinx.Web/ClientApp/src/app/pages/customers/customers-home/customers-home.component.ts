import { Component } from '@angular/core';
import { Router } from '@angular/router';

//Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Customers',
        link: '#/default-layout/customers'
    }
];

@Component({
    selector: 'app-customers-home',
    templateUrl: './customers-home.component.html',
    styleUrls: ['./customers-home.component.scss']
})
/** customers-home component*/
export class CustomersHomeComponent {

    //Public Members
    public pageTitle: string = 'Customers';
    public breadcrumb: any[] = BREADCRUMBS;
    public customersData: any[];
    public aircraftsData: any[];
    public pricingTemplatesData: any[];

    /** customers-home ctor */
    constructor(
        private router: Router,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private customerAircraftsService: CustomeraircraftsService
    ) {

        this.sharedService.emitChange(this.pageTitle);
        this.loadCustomers();
        this.loadCustomerAircrafts();
        this.loadPricingTemplates();
    }

    public editCustomerClicked(record) {
        this.router.navigate(['/default-layout/customers/' + record.customerInfoByGroupId]);
    }

    public customerDeleted() {
        this.loadCustomers();
    }

    //Private Methods
    private loadCustomers() {
        this.customersData = null;
        this.customerInfoByGroupService
            .getByGroupAndFbo(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.customersData = data;
            });
    }

    private loadCustomerAircrafts() {
        this.customerAircraftsService
            .getCustomerAircraftsByGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: any) => {
                this.aircraftsData = data;
            });
    }

    private loadPricingTemplates() {
        this.pricingTemplatesService
            .getByFbo(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.pricingTemplatesData = data
            });
    }
}
