import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { AuthenticationService } from '../../../services/authentication.service'
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomersService } from '../../../services/customers.service';
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/'
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
    public customersData: Array<any>;

    /** customers-home ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customersService: CustomersService,
        private sharedService: SharedService) {

        this.sharedService.emitChange(this.pageTitle);
        this.loadCustomers();
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
}
