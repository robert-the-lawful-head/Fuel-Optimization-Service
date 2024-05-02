import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { FboairportsService } from '../../../services/fboairports.service';
// Services
import { FbosService } from '../../../services/fbos.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
    selector: 'app-fbos-home',
    styleUrls: ['./fbos-home.component.scss'],
    templateUrl: './fbos-home.component.html',
})
export class FbosHomeComponent implements OnInit {
    @Input() groupInfo: any;
    @Input() embed: boolean;

    // Public Members
    public fbosData: Array<any>;
    public currentFbo: any;
    public currentFboAirport: any;
    public chartName = 'FBOs';

    constructor(
        private router: Router,
        private fboService: FbosService,
        private fboAirportsService: FboairportsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService
    ) {
        this.currentFbo = null;
        this.currentFboAirport = null;
    }

    ngOnInit() {
        this.loadAllFbosForGroup();
    }

    public editFboClicked(record) {
        if (!this.groupInfo) {
            this.router.navigate(['/default-layout/fbos/' + record.oid]);
        } else {
            this.ngxLoader.startLoader(this.chartName);

            this.fboService
                .get(record)
                .subscribe((data: any) => { this.currentFbo = data; this.ngxLoader.stopLoader(this.chartName); });
            this.fboAirportsService
                .getForFbo(record)
                .subscribe((data: any) => { this.currentFboAirport = data; this.ngxLoader.stopLoader(this.chartName); });
        }
    }

    public saveFboEditClicked() {
        this.currentFboAirport = null;
        this.currentFbo = null;
        this.fbosData = null;
        this.loadAllFbosForGroup();
    }

    public cancelFboEditClicked() {
        this.currentFbo = null;
    }

    // Private Methods
    private loadAllFbosForGroup() {
        this.ngxLoader.startLoader(this.chartName);
        if (!this.groupInfo) {
            this.fboService
                .getForGroup(this.sharedService.currentUser.groupId)
                .subscribe((data: any) => {
                    this.fbosData = data;
                    this.ngxLoader.stopLoader(this.chartName);
                });
        } else {
            this.fboService
                .getForGroup(this.groupInfo.oid)
                .subscribe((data: any) => {
                    this.fbosData = data;
                    this.ngxLoader.stopLoader(this.chartName);
                });
        }
    }
}
