import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

// Services
import { CustomerinfobygroupService } from "../../../services/customerinfobygroup.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-statistics-total-customers",
    templateUrl: "./statistics-total-customers.component.html",
    styleUrls: ["./statistics-total-customers.component.scss"],
})
// statisticsTotalCustomers component
export class StatisticsTotalCustomersComponent implements OnInit {
    @Input() options: any;

    // Public Members
    public totalCustomers: number;

    constructor(
        private router: Router,
        private customerinfobygroupService: CustomerinfobygroupService,
        private sharedService: SharedService
    ) {
        if (!this.options) {
            this.options = {};
        }
    }

    ngOnInit() {
        this.refreshData();
    }

    public redirectClicked() {
        this.router.navigate(["/default-layout/customers"]);
    }

    public refreshData() {
        this.customerinfobygroupService
            .getCustomerCountByGroupAndFBO(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((data: any) => {
                this.totalCustomers = data;
            });
    }
}
