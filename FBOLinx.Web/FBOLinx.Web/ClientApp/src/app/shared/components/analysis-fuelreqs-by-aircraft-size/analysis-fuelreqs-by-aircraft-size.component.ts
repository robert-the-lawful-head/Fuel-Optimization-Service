import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { AmChartsService } from '@amcharts/amcharts3-angular';
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';

@Component({
    selector: 'app-analysis-fuelreqs-by-aircraft-size',
    templateUrl: './analysis-fuelreqs-by-aircraft-size.component.html',
    styleUrls: ['./analysis-fuelreqs-by-aircraft-size.component.scss']
})
/** analysis-fuelreqs-by-aircraft-size component*/
export class AnalysisFuelreqsByAircraftSizeComponent implements OnInit {
    //Public Members
    public ordersBySizeData: Array<any>;

    //Private Members
    private chart: any;

    /** analysis-price-orders-chart ctor */
    constructor(private router: Router,
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private AmCharts: AmChartsService) {

    }

    ngOnInit() {
        this.refreshData();
    }

    public redirectClicked() {
        this.router.navigate(['/default-layout/fuelreqs']);
    }

    public refreshData() {
        let startDate = this.sharedService.dashboardSettings.filterStartDate;
        let endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService.totalOrdersByAircraftSizeForFbo(this.sharedService.currentUser.fboId, { startDateTime: startDate, endDateTime: endDate }).subscribe((data: any) => {
            this.ordersBySizeData = data;
            this.loadChart();
        });
    }

    // Private Methods /////////////
    private loadChart() {
        this.chart = this.AmCharts.makeChart('analysis-fuelreqs-by-aircraft-size-1', {
            'type': 'pie',
            'theme': 'light',
            'dataProvider': this.ordersBySizeData,
            'pullOutRadius': 0,
            'labelRadius': -40,
            'valueField': 'totalOrders',
            'titleField': 'aircraftSizeDescription',
            'labelText': '[[totalOrders]]',
            'balloon': {
                'fixedPosition': true
            }
        });
    }
}
