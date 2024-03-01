import { Component, HostListener, Input, OnInit, SimpleChanges } from '@angular/core';
import { SharedService } from 'src/app/layouts/shared-service';
import { CustomersListType } from 'src/app/models';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { AnatylticsReports } from '../analytics-activity-reports/analytics-activity-reports.component';
import { trigger, state, style, transition, animate } from '@angular/animations';

export enum AnaliticsReportType {
    CustomerStatistics,
    ArrivalsDepartures,
    FBONetworkArrivalDepartures,
    LostToCompetition,
    FuelerLinxCustomerCaptureRate
}

@Component({
  selector: 'app-analytics-report-popup',
  templateUrl: './analytics-report-popup.component.html',
  styleUrls: ['./analytics-report-popup.component.scss'],
  animations: [
    trigger('fadeInOut', [
      state('in', style({ opacity: 1 })),
      transition(':enter', [
        style({ opacity: 0 }),
        animate(300)
      ]),
      transition(':leave', [
        animate(300, style({ opacity: 0 }))
      ])
    ])
  ]
})
export class AnalyticsReportPopupComponent implements OnInit {
  @Input() report: AnatylticsReports;
  analiticsReportTypes = AnaliticsReportType;

  public isPopUpOpen: boolean = false;
  public isReportVisible : Record<AnaliticsReportType, boolean>;
  public customers: CustomersListType[] = [];

  constructor(
    private customerInfoByGroupService: CustomerinfobygroupService,
    private sharedService: SharedService
    ) { }

  ngOnInit() {
    this.getCustomersList();
    this.isReportVisible = {
      [AnaliticsReportType.CustomerStatistics]: false,
      [AnaliticsReportType.ArrivalsDepartures]: false,
      [AnaliticsReportType.FBONetworkArrivalDepartures]: false,
      [AnaliticsReportType.LostToCompetition]: false,
      [AnaliticsReportType.FuelerLinxCustomerCaptureRate]: false
    };
  }
    ngOnChanges(changes: SimpleChanges): void {
    for (const key in this.isReportVisible) {
          if(key == changes.report.currentValue.type){
            this.isPopUpOpen = true;
            this.isReportVisible[key] = true;
          }
          else
            this.isReportVisible[key] = false;
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
    console.log('Exporting report');
  }
  closePopUp() {
    this.isPopUpOpen = false;
}
}
