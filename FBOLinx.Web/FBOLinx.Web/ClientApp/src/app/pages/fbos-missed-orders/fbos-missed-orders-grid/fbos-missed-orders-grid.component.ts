import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forEach } from 'lodash';
import { NgxUiLoaderService } from 'ngx-ui-loader';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { FbomissedquoteslogService } from '../../../services/fbomissedquoteslog.service';

@Component({
    selector: 'app-fbos-missed-orders-grid',
    styleUrls: ['./fbos-missed-orders-grid.component.scss'],
    templateUrl: './fbos-missed-orders-grid.component.html',
})
export class FbosMissedOrdersGridComponent implements OnInit {
    fbosMissedOrdersDataSource: any = null;
    missedOrdersLoader = 'missed-orders-loader';
    noMissedOrders = false;

    constructor(private router: Router, private sharedService: SharedService, private fboMissedQuotesLogService: FbomissedquoteslogService, private NgxUiLoader: NgxUiLoaderService) {

    }

    ngOnInit() {
        this.refreshMissedOrdersDataSource();
    }

    goToCustomerManager(missedOrder: any): void {
        this.router.navigate(['/default-layout/fuelreqs'], {queryParams: {search: missedOrder.customerName, tab: 1}});
    }

    private refreshMissedOrdersDataSource() {
        this.NgxUiLoader.startLoader(this.missedOrdersLoader);
        this.fboMissedQuotesLogService.getRecentMissedOrders(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.fbosMissedOrdersDataSource = data;
                    if (this.fbosMissedOrdersDataSource.length == 0)
                        this.noMissedOrders = true;
                    this.NgxUiLoader.stopLoader(this.missedOrdersLoader);
                }
            );
    }
}
