import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TagsService } from 'src/app/services/tags.service';

import { SharedService } from '../../../layouts/shared-service';
import { locationChangedEvent } from '../../../models/sharedEvents';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
// Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { customerGridSet } from '../../../store/actions';
import { State } from '../../../store/reducers';
import { CustomerGridState } from '../../../store/reducers/customer';
import { getCustomerGridState } from '../../../store/selectors';
import { MatTableDataSource } from '@angular/material/table';
import { AircraftsGridComponent } from '../../aircrafts/aircrafts-grid/aircrafts-grid.component';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/customers',
        title: 'Customers',
    },
];

@Component({
    selector: 'app-customers-home',
    styleUrls: ['./customers-home.component.scss'],
    templateUrl: './customers-home.component.html',
})
export class CustomersHomeComponent implements OnInit, OnDestroy {
    @ViewChild(AircraftsGridComponent) aircraftsGridComponent:AircraftsGridComponent;

    // Public Members
    pageTitle = 'Customers';
    breadcrumb: any[] = BREADCRUMBS;
    customersData: any[];
    aircraftData: any[];
    pricingTemplatesData: any[];
    locationChangedSubscription: any;
    customerGridState: CustomerGridState;
    fuelVendors: any[];
    tags : any[];

    public displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'airportOrders', 'lastPullDate', 'pricingFormula'];
    public dataSource:       MatTableDataSource<any[]>;
    public icao:             string;
    public fbo:              string;
    public id:               string;

    constructor(
        private store: Store<State>,
        private router: Router,

        private customerInfoByGroupService: CustomerinfobygroupService,
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private customerAircraftService: CustomeraircraftsService,
        private tagService : TagsService

    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.loadCustomers();
        this.loadPricingTemplates();
        this.loadCustomerAircraft();
        this.loadFuelVendors();
        this.loadTags();
    }

    ngOnInit(): void {
        this.store.select(getCustomerGridState).subscribe((state) => {
            this.customerGridState = state;
        });
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === locationChangedEvent) {
                    this.loadCustomers();
                    this.loadPricingTemplates();
                }
            });
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    editCustomerClicked(event) {
        this.store.dispatch(
            customerGridSet({
                filter: event.filter,
                filterType: event.filterType,
                order: event.order,
                orderBy: event.orderBy,
                page: event.page,
            })
        );

        this.router
            .navigate([
                '/default-layout/customers/' + event.customerInfoByGroupId,
            ])
            .then();
    }

    customerPriceClicked(event) {
        this.store.dispatch(
            customerGridSet({
                filter: event.filter,
                filterType: event.filterType,
                order: event.order,
                orderBy: event.orderBy,
                page: event.page,
            })
        );

        this.router
            .navigate([
                '/default-layout/pricing-templates/' + event.pricingTemplateId,
            ])
            .then();
    }
    exportAircraftClick($event){
        this.aircraftsGridComponent.exportCsv();
    }
    customerDeleted() {
        this.loadCustomers();
    }
    refreshAircrafts() {
        this.loadCustomerAircraft();
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
                this.customersData = data.map((c) => ({
                    ...c,
                    fuelVendors: c.fuelVendors.map((fv) => ({
                        label: fv,
                        value: fv,
                    })),
                }));
            });
    }

    private loadPricingTemplates() {
        this.pricingTemplatesData = null;
        this.pricingTemplatesService
            .getByFbo(
                this.sharedService.currentUser.fboId,
                this.sharedService.currentUser.groupId
            )
            .subscribe((data: any) => {

                this.pricingTemplatesData = data;
            });
    }
    private loadCustomerAircraft() {
        this.aircraftData = null;
        this.customerAircraftService
            .getCustomerAircraftsByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((data: any) => {
                this.aircraftData = data;
                this.aircraftData.forEach((result) => {
                    if (result.isCompanyPricing) {
                        result.pricingTemplateId = null;
                    }
                });
            });
    }

    private loadFuelVendors() {
        this.customerInfoByGroupService
            .getFuelVendors()
            .subscribe((data: any) => {

                this.fuelVendors = data.map((fv) => ({
                    label: fv,
                    value: fv,
                }));
            });
    }

    private loadTags ()
    {
        this.tagService.getGroupTags(
            this.sharedService.currentUser.groupId
        )
        .subscribe((data:any) =>
        {

           this.tags = data.map((tg) => ({
            label: tg,
            value: tg,
        }));
        });

    }

}
