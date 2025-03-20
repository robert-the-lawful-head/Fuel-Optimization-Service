import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TagsService } from 'src/app/services/tags.service';

import { SharedService } from '../../../layouts/shared-service';
import { locationChangedEvent } from '../../../constants/sharedEvents';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
// Services
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { customerGridSet } from '../../../store/actions';
import { State } from '../../../store/reducers';
import { CustomerGridState } from '../../../store/reducers/customer';
import { getCustomerGridState } from '../../../store/selectors';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import { AircraftsGridComponent } from '../../aircrafts/aircrafts-grid/aircrafts-grid.component';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Subscription } from 'rxjs';
import { PricingTemplate } from '../../../models';

@Component({
    selector: 'app-customers-home',
    styleUrls: ['./customers-home.component.scss'],
    templateUrl: './customers-home.component.html',
})
export class CustomersHomeComponent implements OnInit, OnDestroy {
    @ViewChild(AircraftsGridComponent) aircraftsGridComponent:AircraftsGridComponent;

    // Public Members
    pageTitle = 'Customers';
    customersData: any[];
    aircraftData: any[];
    fullAicraftDataCpy: any[];
    pricingTemplatesData: any[];
    locationChangedSubscription: any;
    customerGridState: CustomerGridState;
    fuelVendors: any[];
    tags : any[];
    customersCount = 0;

    public displayedColumns: string[] = ['company', 'directOrders', 'companyQuotesTotal', 'conversionRate', 'totalOrders', 'airportOrders', 'lastPullDate', 'pricingFormula'];
    public dataSource:       MatTableDataSource<any[]>;
    public icao:             string;
    public fbo:              string;
    public id:               string;
    selectedTabIndex: number = 0;

    charNameCustomer = 'CustomersCustomer';
    charNameAircraft = 'CustomersAicraft';
    charNamePricingTemplate = 'pricingTempates';
    routeQueryParamSubscription: Subscription;
    constructor(
        private store: Store<State>,
        private router: Router,

        private customerInfoByGroupService: CustomerinfobygroupService,
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private customerAircraftService: CustomeraircraftsService,
        private tagService: TagsService,
        private route: ActivatedRoute,
        private ngxLoader: NgxUiLoaderService,
        private cdr: ChangeDetectorRef
    ) {
        
        this.loadCustomers();
        this.loadPricingTemplates();
        this.loadCustomerAircraft();
        this.loadFuelVendors();
        this.loadTags();

        this.routeQueryParamSubscription = this.route.queryParams.subscribe((params) => {
            if (params.tab && params.tab) {
                this.selectedTabIndex = parseInt(params.tab);
            }
        });
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
        this.routeQueryParamSubscription?.unsubscribe();
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
    onCompanyFilterApplied(filter: string[]) {
        this.customersCount = filter.length;
        if(this.customersData == null || this.customersData.length == 0) return;
        
        this.aircraftData = null;

        if (filter.length == 0 || filter.length == this.customersData.length) {
            this.aircraftData = [...this.fullAicraftDataCpy];
            return;
        }
        this.aircraftData = this.fullAicraftDataCpy.filter((a) => filter.includes(a.company.toLowerCase()));

    }
    // Private Methods
    private loadCustomers() {
        this.ngxLoader.startLoader(this.charNameCustomer);
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
                this.cdr.detectChanges();
                this.customersCount = this.customersData.length;
                this.ngxLoader.stopLoader(this.charNameCustomer);
            });
    }

    private loadPricingTemplates() {
        this.ngxLoader.startLoader(this.charNamePricingTemplate);

        this.pricingTemplatesData = null;
        this.pricingTemplatesService
            .getByFbo(
                this.sharedService.currentUser.fboId,
                this.sharedService.currentUser.groupId
            )
            .subscribe((data: any) => {
                let blankDefault: PricingTemplate = {
                    name: '* Missing Template *',
                    customerId: 0,
                    customersAssigned: 0,
                    default: false,
                    email: '',
                    emailContentId: 0,
                    fboid: 0,
                    intoPlanePrice: 0,
                    isInvalid: false,
                    isPricingExpired: false,
                    margin: 0,
                    marginType: 0,
                    marginTypeDescription: '',
                    notes: '',
                    oid: 0,
                    subject: '',
                    type: 0,
                    yourMargin: 0
                };

                this.pricingTemplatesData = data;
                this.pricingTemplatesData.unshift(blankDefault);
                this.ngxLoader.stopLoader(this.charNamePricingTemplate);
            });
    }
    private loadCustomerAircraft() {
        this.ngxLoader.startLoader(this.charNameAircraft);

        this.aircraftData = null;
        this.customerAircraftService
            .getCustomerAircraftsByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((data: any) => {
                data.forEach((result) => {
                    if (result.isCompanyPricing) {
                        result.pricingTemplateId = null;
                    }
                });
                this.aircraftData = data;
                this.fullAicraftDataCpy = data;
                this.ngxLoader.stopLoader(this.charNameAircraft);
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
