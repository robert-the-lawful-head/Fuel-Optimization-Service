import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

//Services
import { AmChartsService } from '@amcharts/amcharts3-angular';
import { FbopricesService } from '../../../services/fboprices.service';
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';

@Component({
    selector: 'app-analysis-price-orders-chart',
    templateUrl: './analysis-price-orders-chart.component.html',
    styleUrls: ['./analysis-price-orders-chart.component.scss']
})
/** analysis-price-orders-chart component*/
export class AnalysisPriceOrdersChartComponent implements OnInit {

    //Public Members
    public totalOrdersData: Array<any>;

    //Private Members
    private chart: any;

    /** analysis-price-orders-chart ctor */
    constructor(private router: Router,
        private fboPricesService: FbopricesService,
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService,
        private AmCharts: AmChartsService    ) {

    }

    ngOnInit() {
        this.refreshData();
    }

    public redirectClicked() {
        this.router.navigate(['/default-layout/fbo-prices']);
    }

    public refreshData() {
        let startDate = this.sharedService.dashboardSettings.filterStartDate;
        let endDate = this.sharedService.dashboardSettings.filterEndDate;
        this.fuelreqsService.totalOrdersByMonthForFbo(this.sharedService.currentUser.fboId, { startDateTime: startDate, endDateTime: endDate }).subscribe((data: any) => {
            this.totalOrdersData = data;
            this.loadChart();
        });
    }

    // Private Methods /////////////
    private loadChart() {
        this.chart = this.AmCharts.makeChart('amchart-1', {
            'type': 'serial',
            'theme': 'light',
            'dataDateFormat': 'YYYY-MM-DD',
            'dataProvider': this.totalOrdersData,
            'addClassNames': true,
            'startDuration': 1,
            'categoryField': 'monthName',
            'categoryAxis': {
                'parseDates': false,
                'minPeriod': 'DD',
                'autoGridCount': false,
                'gridCount': 50,
                'gridAlpha': 0.1,
                'gridColor': '#FFFFFF',
                'axisColor': '#555555',
                'dateFormats': [{
                    'period': 'DD',
                    'format': 'DD'
                }, {
                    'period': 'WW',
                    'format': 'MMM DD'
                }, {
                    'period': 'MM',
                    'format': 'MMM'
                }, {
                    'period': 'YYYY',
                    'format': 'YYYY'
                }]
            },
            'valueAxes': [{
                'id': 'a1',
                'title': 'Number of Fuel Orders',
                'gridAlpha': 0,
                'axisAlpha': 0
            }, {
                'id': 'a2',
                'position': 'right',
                'gridAlpha': 0,
                'axisAlpha': 0,
                'labelsEnabled': false
            }],
            'graphs': [{
                'id': 'g1',
                'valueField': 'totalOrders',
                'title': 'Number of Fuel Orders',
                'type': 'column',
                'fillAlphas': 0.9,
                'valueAxis': 'a1',
                'balloonText': '[[value]] orders',
                'legendValueText': '[[value]]',
                'legendPeriodValueText': '[[value.sum]]',
                'lineColor': '#B3E5FC',
                'alphaField': 'alpha'
            }
                , {
                'id': 'g2',
                    'valueField': 'averageJetACost',
                'classNameField': 'bulletClass',
                'title': 'Avg. JetA Cost',
                'type': 'line',
                'valueAxis': 'a2',
                'lineColor': '#d50000',
                    'lineThickness': 1,
                    'legendValueText': '[[description]]',
                    'descriptionField': 'averageJetACostFormatted',
                'bullet': 'round',
                'bulletSizeField': 'townSize',
                'bulletBorderColor': '#ca0000',
                'bulletBorderAlpha': 1,
                'bulletBorderThickness': 2,
                'bulletColor': '#f2b3b3',
                'labelPosition': 'right',
                    'balloonText': '[[averageJetACostFormatted]]',
                'showBalloon': true,
                'animationPlayed': true
                }
                ,{
                'id': 'g3',
                'title': 'Avg. JetA Retail',
                    'valueField': 'averageJetARetail',
                'descriptionField': 'averageJetARetailFormatted',
                'type': 'line',
                'valueAxis': 'a2',
                'lineColor': '#64B5F6',
                    'balloonText': '[[averageJetARetailFormatted]]',
                'lineThickness': 1,
                    'legendValueText': '[[description]]',
                    'bullet': 'square',
                'bulletBorderColor': '#64B5F6',
                'bulletBorderThickness': 1,
                'bulletBorderAlpha': 1,
                    'dashLengthField': 'dashLength',
                'showBalloon': true,
                'animationPlayed': true
                }
            ],
            'chartCursor': {
                'zoomable': false,
                'categoryBalloonDateFormat': 'DD',
                'cursorAlpha': 0,
                'valueBalloonsEnabled': false
            },
            'legend': {
                'bulletType': 'round',
                'equalWidths': true,
                'valueWidth': 80,
                'useGraphSettings': true
            }
        });
    }
}
