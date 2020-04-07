import { Component, AfterViewInit, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";

import * as _ from "lodash";

// Services
import { CustomerinfobygroupService } from "../../../services/customerinfobygroup.service";
import { SharedService } from "../../../layouts/shared-service";
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { CustomeraircraftsService } from "../../../services/customeraircrafts.service";

import * as SharedEvents from "../../../models/sharedEvents";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "#/default-layout",
    },
    {
        title: "Customers",
        link: "#/default-layout/customers",
    },
];

@Component({
    selector: "app-customers-home",
    templateUrl: "./customers-home.component.html",
    styleUrls: ["./customers-home.component.scss"],
})
export class CustomersHomeComponent implements AfterViewInit, OnDestroy {
    // Public Members
    public pageTitle = "Customers";
    public breadcrumb: any[] = BREADCRUMBS;
    public customersData: any[];
    public aircraftsData: any[];
    public pricingTemplatesData: any[];
    public locationChangedSubscription: any;

    constructor(
        private router: Router,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private customerAircraftsService: CustomeraircraftsService
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.loadCustomers();
        this.loadPricingTemplates();
        this.loadCustomerAircrafts();
    }

    ngAfterViewInit() {
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.locationChangedEvent) {
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

    public editCustomerClicked(record) {
        this.router.navigate([
            "/default-layout/customers/" + record.customerInfoByGroupId,
        ]);
    }

    public customerDeleted() {
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

    private loadCustomerAircrafts() {
        this.aircraftsData = null;
        this.customerAircraftsService
            .getCustomerAircraftsByGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: any) => {
                this.aircraftsData = data;
            });
    }
}
