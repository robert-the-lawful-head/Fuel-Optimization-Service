import { Component, Input, OnInit } from "@angular/core";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-statistics-orders-by-location",
    templateUrl: "./statistics-orders-by-location.component.html",
    styleUrls: ["./statistics-orders-by-location.component.scss"],
})
export class StatisticsOrdersByLocationComponent implements OnInit {
    @Input() options: any;

    // Public Members
    public totalOrders: number;
    public icao: string;

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {
        if (!this.options) {
            this.options = {};
        }
    }

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        const startDate = this.sharedService.dashboardSettings.filterStartDate;
        const endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService
            .getOrdersByLocation({
                StartDateTime: startDate,
                EndDateTime: endDate,
                ICAO: "",
                FboId: this.sharedService.currentUser.fboId,
            })
            .subscribe((data: any) => {
                this.totalOrders = 0;
                if (data.totalOrdersByMonth) {
                    for (const orders of data.totalOrdersByMonth) {
                        this.totalOrders = orders.count;
                    }
                }
                this.icao = data.icao;
            });
    }
}
