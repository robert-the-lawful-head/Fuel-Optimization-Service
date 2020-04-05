import { Component, OnInit, AfterViewInit, OnDestroy } from "@angular/core";
import * as _ from "lodash";
import * as moment from "moment";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";
import * as SharedEvent from "../../../models/sharedEvents";

@Component({
    selector: "app-analytics-market-share-airport",
    templateUrl: "./analytics-market-share-airport-chart.component.html",
    styleUrls: ["./analytics-market-share-airport-chart.component.scss"],
})
export class AnalyticsMarketShareAirportChartComponent implements OnInit, AfterViewInit, OnDestroy {
    // Public Members
    public totalOrdersData: any[];
    public icao: string;
    public icaoChangedSubscription: any;
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
    ) {
        this.icao = this.sharedService.currentUser.icao;
    }

    ngOnInit() {
        this.refreshData();
    }

    ngAfterViewInit() {
        this.icaoChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvent.icaoChangedEvent) {
                    this.icao = this.sharedService.currentUser.icao;
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.icaoChangedSubscription) {
            this.icaoChangedSubscription.unsubscribe();
        }
    }

    public refreshData() {
        const startDate = new Date(
            moment().add(-30, "days").format("MM/DD/YYYY")
        );
        const endDate = new Date(
            moment().format("MM/DD/YYYY")
        );
        this.fuelreqsService
            .getMarketShareAirport(this.sharedService.currentUser.fboId, startDate, endDate)
            .subscribe((data: any) => {
                this.totalOrdersData = data;
            }, (error: any) => {
            });
    }
}
