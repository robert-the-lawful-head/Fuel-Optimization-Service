import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forEach } from 'lodash';
import { NgxUiLoaderService } from 'ngx-ui-loader';

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

    constructor(private router: Router, private sharedService: SharedService, private fboMissedQuotesLogService: FbomissedquoteslogService, private NgxUiLoader: NgxUiLoaderService) {

    }

    ngOnInit() {
        this.refreshMissedQuotesDataSource();
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
