import { Component, OnInit } from "@angular/core";
import * as _ from "lodash";
import { MatSliderChange } from "@angular/material/slider";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-analytics-volumes-nearby-airport",
    templateUrl: "./analytics-volumes-nearby-airport-chart.component.html",
    styleUrls: ["./analytics-volumes-nearby-airport-chart.component.scss"],
})
export class AnalyticsVolumesNearbyAirportChartComponent implements OnInit {
    // Public Members
    public totalOrdersData: any[];
    public mile = 200;
    public colorScheme = {
        domain: [
            "#a8385d",
            "#7aa3e5",
            "#a27ea8",
            "#aae3f5",
            "#adcded",
            "#a95963",
            "#8796c0",
            "#7ed3ed",
            "#50abcc",
            "#ad6886",
        ],
    };

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        const startDate = this.sharedService.dashboardSettings.filterStartDate;
        const endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService
            .getVolumesNearbyAirport(this.sharedService.currentUser.fboId, startDate, endDate, this.mile)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
            });
    }

    public formatLabel(value: number) {
        return value + "mi";
    }

    public changeMile(event: MatSliderChange) {
        this.mile = event.value;
        this.refreshData();
    }
}
