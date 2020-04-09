import { Component, OnInit, Input, SimpleChanges, OnChanges } from "@angular/core";
import * as _ from "lodash";

// Services
import { FuelreqsService } from "../../../services/fuelreqs.service";
import { SharedService } from "../../../layouts/shared-service";
import { MatTableDataSource } from "@angular/material/table";

@Component({
    selector: "app-analytics-companies-quotes-deal",
    templateUrl: "./analytics-companies-quotes-deal-table.component.html",
    styleUrls: ["./analytics-companies-quotes-deal-table.component.scss"],
})
export class AnalyticsCompaniesQuotesDealTableComponent implements OnInit, OnChanges {
    @Input() startDate: Date;
    @Input() endDate: Date;

    // Public Members
    public displayedColumns: string[] = ["company", "fboOrders", "fboVolume", "airportOrders", "lastPullDate"];
    public dataSource: any;

    constructor(
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService
    ) {}

    ngOnInit() {
    }

    ngOnChanges(changes: SimpleChanges) {
        this.refreshData();
    }

    public refreshData() {
        this.fuelreqsService
            .getCompaniesQuotingDealStatistics(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId,
                this.startDate,
                this.endDate
            )
            .subscribe((data: any) => {
                this.dataSource = new MatTableDataSource(data);
            }, (error: any) => {
            });
    }

    public applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }
}
