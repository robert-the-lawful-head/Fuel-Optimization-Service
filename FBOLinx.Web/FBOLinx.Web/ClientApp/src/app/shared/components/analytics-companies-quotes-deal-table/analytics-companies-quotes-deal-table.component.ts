import { Component, OnInit, Input, SimpleChanges, OnChanges } from "@angular/core";
import { FormControl } from "@angular/forms";
import * as _ from "lodash";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";
import { CustomerinfobygroupService } from "../../../services/customerinfobygroup.service";
import { Observable } from "rxjs";
import { startWith, map } from "rxjs/operators";
import { MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";

@Component({
    selector: "app-analytics-companies-quotes-deal",
    templateUrl: "./analytics-companies-quotes-deal-table.component.html",
    styleUrls: ["./analytics-companies-quotes-deal-table.component.scss"],
})
export class AnalyticsCompaniesQuotesDealTableComponent implements OnInit, OnChanges {
    @Input() startDate: Date;
    @Input() endDate: Date;

    // Public Members
    public currentCustomer: any;
    public customers: any[];
    public formControl = new FormControl();
    public filteredCustomers: Observable<string[]>;
    public displayedColumns: string[] = ["fboOrders", "fboVolume", "airportOrders", "lastPullDate"];
    public dataSource: any[];

    constructor(
        private fuelreqsService: FuelreqsService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        const user = this.sharedService.currentUser;
        this.customerInfoByGroupService
            .getCustomerNamesByGroupAndFBO(user.groupId, user.fboId)
            .subscribe((customers: any) => {
                this.customers = customers;
                if (this.customers.length > 0) {
                    this.currentCustomer = this.customers[0];
                    this.formControl.setValue(this.currentCustomer);
                    this.refreshData();
                }
            });
        this.filteredCustomers = this.formControl.valueChanges
            .pipe(
                startWith(""),
                map(value => this._filter(value))
            );
    }

    ngOnChanges(changes: SimpleChanges) {
        if (this.currentCustomer) {
            this.refreshData();
        }
    }

    public refreshData() {
        this.dataSource = [];
        this.fuelreqsService
            .getCompaniesQuotingDealStatistics(
                this.sharedService.currentUser.fboId, 
                this.startDate,
                this.endDate,
                this.currentCustomer.customerId
            )
            .subscribe((data: any) => {
                this.dataSource.push(data);
            }, (error: any) => {
            });
    }

    public changeCustomer(event: MatAutocompleteSelectedEvent) {
        this.currentCustomer = event.option.value;
        this.refreshData();
    }

    private _filter(value: any) {
        let filterValue = "";
        if (typeof value === "string") {
            filterValue = value.toLowerCase();
        } else {
            filterValue = value.company;
        }
        const result = _.filter(this.customers, (customer: any) => {
            return customer.company.toLowerCase().includes(filterValue);
        });
        return result;
    }

    public getCompanyName(value: any) {
        return value.company;
    }
}
