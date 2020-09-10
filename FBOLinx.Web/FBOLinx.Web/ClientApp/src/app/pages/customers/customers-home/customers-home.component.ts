import {Component, OnDestroy, OnInit} from "@angular/core";
import {Router} from "@angular/router";
import {Store} from "@ngrx/store";

// Services
import {CustomerinfobygroupService} from "../../../services/customerinfobygroup.service";
import {SharedService} from "../../../layouts/shared-service";
import {PricingtemplatesService} from "../../../services/pricingtemplates.service";
import {CustomeraircraftsService} from "../../../services/customeraircrafts.service";

import {locationChangedEvent} from "../../../models/sharedEvents";

import {getCustomerGridState} from "../../../store/selectors/customer";
import {State} from "../../../store/reducers";
import {customerGridSet} from "../../../store/actions/customer";
import {CustomerGridState} from "../../../store/reducers/customer";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "/default-layout",
    },
    {
        title: "Customers",
        link: "/default-layout/customers",
    },
];

@Component({
    selector: "app-customers-home",
    templateUrl: "./customers-home.component.html",
    styleUrls: ["./customers-home.component.scss"],
})
export class CustomersHomeComponent implements OnInit, OnDestroy {
    // Public Members
    pageTitle = "Customers";
    breadcrumb: any[] = BREADCRUMBS;
    customersData: any[];
    aircraftData: any[];
    pricingTemplatesData: any[];
    locationChangedSubscription: any;
    customerGridState: CustomerGridState;

    constructor(
        private store: Store<State>,
        private router: Router,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private customerAircraftService: CustomeraircraftsService
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.loadCustomers();
        this.loadPricingTemplates();
        this.loadCustomerAircraft();
    }

    ngOnInit(): void {
        this.store.select(getCustomerGridState).subscribe(state => {
            this.customerGridState = state;
        });
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === locationChangedEvent) {
                    this.loadCustomers();
                    this.loadPricingTemplates();
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    editCustomerClicked(event) {
        this.store.dispatch(customerGridSet({
            filter: event.filter,
            page: event.page,
            order: event.order,
            orderBy: event.orderBy,
            filterType: event.filterType,
        }));

        this.router.navigate([
            "/default-layout/customers/" + event.customerInfoByGroupId,
        ]).then();
    }

    customerDeleted() {
        this.loadCustomers();
    }

    // Private Methods
    private loadCustomers() {
        this.customersData = null;
        this.customerInfoByGroupService
            .getByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((data: any) => {
                this.customersData = data;
            });
    }

    private loadPricingTemplates() {
        this.pricingTemplatesData = null;
        this.pricingTemplatesService
            .getByFbo(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.pricingTemplatesData = data;
            });
    }

    private loadCustomerAircraft() {
        this.aircraftData = null;
        this.customerAircraftService
            .getCustomerAircraftsByGroupAndFbo(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.aircraftData = data;
            });
    }
}
