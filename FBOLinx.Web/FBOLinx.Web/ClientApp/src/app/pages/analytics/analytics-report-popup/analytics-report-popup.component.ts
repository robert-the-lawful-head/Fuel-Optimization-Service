import {
    Component,
    ElementRef,
    Input,
    OnInit,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { SharedService } from 'src/app/layouts/shared-service';
import { CustomersListType } from 'src/app/models';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { AnatylticsReports } from '../analytics-activity-reports/analytics-activity-reports.component';
import {
    trigger,
    state,
    style,
    transition,
    animate,
} from '@angular/animations';
import { MissedOrdersGridComponent } from '../../missed-orders/missedorders-grid/missedorders-grid.component';
import { GroupAnalyticsIntraNetworkVisitsReportComponent } from '../../group-analytics/group-analytics-intra-network-visits-report/group-analytics-intra-network-visits-report.component';
import { AnalyticsAirportArrivalsDepaturesComponent } from '../analytics-airport-arrivals-depatures/analytics-airport-arrivals-depatures.component';
import { AnalyticsCompaniesQuotesDealTableComponent } from '../analytics-companies-quotes-deal-table/analytics-companies-quotes-deal-table.component';

export enum AnaliticsReportType {
    CustomerStatistics,
    ArrivalsDepartures,
    FBONetworkArrivalDepartures,
    LostToCompetition,
    FuelerLinxCustomerCaptureRate,
}

@Component({
    selector: 'app-analytics-report-popup',
    templateUrl: './analytics-report-popup.component.html',
    styleUrls: ['./analytics-report-popup.component.scss'],
    animations: [
        trigger('fadeInOut', [
            state('in', style({ opacity: 1 })),
            transition(':enter', [style({ opacity: 0 }), animate(300)]),
            transition(':leave', [animate(300, style({ opacity: 0 }))]),
        ]),
    ],
})
export class AnalyticsReportPopupComponent implements OnInit {
    @Input() report: AnatylticsReports;

    @ViewChild('customerStatitics', { static: false })
    customerStatitics: AnalyticsCompaniesQuotesDealTableComponent;
    @ViewChild('airportArrivalsDepartures', { static: false })
    airportArrivalsDepartures: AnalyticsAirportArrivalsDepaturesComponent;
    @ViewChild('fboNetworkArrivalsDepartures', { static: false })
    fboNetworkArrivalsDepartures: GroupAnalyticsIntraNetworkVisitsReportComponent;
    @ViewChild('missedOrders', { static: false })
    missedOrders: MissedOrdersGridComponent;

    analiticsReportTypes = AnaliticsReportType;

    public isPopUpOpen: boolean = false;
    public isReportVisible: Record<AnaliticsReportType, boolean>;
    public customers: CustomersListType[] = [];

    constructor(
        private customerInfoByGroupService: CustomerinfobygroupService,
        private sharedService: SharedService
    ) {
        this.isReportVisible = {
            [AnaliticsReportType.CustomerStatistics]: true,
            [AnaliticsReportType.ArrivalsDepartures]: true,
            [AnaliticsReportType.FBONetworkArrivalDepartures]: true,
            [AnaliticsReportType.LostToCompetition]: true,
            [AnaliticsReportType.FuelerLinxCustomerCaptureRate]: true,
        };
    }

    ngOnInit() {
        this.getCustomersList();
    }
    ngOnChanges(changes: SimpleChanges): void {
        if(!changes.report?.currentValue) {
            this.isPopUpOpen = false;
            return;
        }
        for (const key in this.isReportVisible) {
            if (key == changes.report?.currentValue?.type) {
                this.isPopUpOpen = true;
                this.isReportVisible[key] = true;
            } else this.isReportVisible[key] = false;
        }
    }
    getCustomersList() {
        this.customerInfoByGroupService
            .getCustomersListByGroupAndFbo(
                this.sharedService.currentUser.groupId,
                this.sharedService.currentUser.fboId
            )
            .subscribe((customers: any[]) => {
                this.customers = customers;
            });
    }
    exportReport() {
        let reportType = this.report.type;
        switch (reportType) {
            case AnaliticsReportType.CustomerStatistics:
                this.customerStatitics.exportCsv();
                break;
            case AnaliticsReportType.ArrivalsDepartures:
                this.airportArrivalsDepartures.exportCsv();
                break;
            case AnaliticsReportType.FBONetworkArrivalDepartures:
                this.fboNetworkArrivalsDepartures.exportCsv();
                break;
            case AnaliticsReportType.LostToCompetition:
                this.missedOrders.exportCsv();
                break;
            case AnaliticsReportType.FuelerLinxCustomerCaptureRate:
                break;
            default:
                break;
        }
    }
    closePopUp() {
        this.isPopUpOpen = false;
    }
}
