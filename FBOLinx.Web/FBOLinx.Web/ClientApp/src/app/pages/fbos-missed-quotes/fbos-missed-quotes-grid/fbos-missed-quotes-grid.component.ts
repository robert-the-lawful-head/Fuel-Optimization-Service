import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forEach } from 'lodash';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';

import { PriceCheckerDialogComponent } from '../../fbo-prices/price-checker-dialog/price-checker-dialog.component';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { FbomissedquoteslogService } from '../../../services/fbomissedquoteslog.service';


@Component({
    selector: 'app-fbos-missed-quotes-grid',
    styleUrls: ['./fbos-missed-quotes-grid.component.scss'],
    templateUrl: './fbos-missed-quotes-grid.component.html',
})
export class FbosMissedQuotesGridComponent implements OnInit {
    fbosMissedQuotesDataSource: any = null;
    missedQuotesLoader = 'missed-quotes-loader';
    noMissedQuotes = false;
    getScreenWidth: any;
    getScreenHeight: any;

    constructor(private router: Router,
        private sharedService: SharedService,
        private fboMissedQuotesLogService: FbomissedquoteslogService,
        private NgxUiLoader: NgxUiLoaderService,
        private priceCheckerDialog: MatDialog    ) {

    }

    ngOnInit() {
        this.refreshMissedQuotesDataSource();
    }


    openPriceChecker(): void {
        this.getScreenWidth = window.innerWidth;
        this.getScreenHeight = window.innerHeight;

        var dialogWidth = "650px";
        var dialogHeight = "600px";

        if (this.getScreenWidth <= 543) {
            dialogWidth = "100%";
            dialogHeight = "80%";
        }

        const dialogRef = this.priceCheckerDialog.open(
            PriceCheckerDialogComponent
            ,
            {
                width: dialogWidth,
                height: dialogHeight
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
         
            if (!result) {
                return;
            }
        });
    }

    private refreshMissedQuotesDataSource() {
        this.NgxUiLoader.startLoader(this.missedQuotesLoader);
        this.fboMissedQuotesLogService.getRecentMissedQuotes(this.sharedService.currentUser.fboId)
            .subscribe(
                (data: any) => {
                    this.fbosMissedQuotesDataSource = data;
                    if (this.fbosMissedQuotesDataSource.length == 0)
                        this.noMissedQuotes = true;
                    this.NgxUiLoader.stopLoader(this.missedQuotesLoader);
                }
        );
    }
}
