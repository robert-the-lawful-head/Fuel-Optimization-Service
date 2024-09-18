import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SharedService } from '../../../layouts/shared-service';
import { FboairportsService } from '../../../services/fboairports.service';
// Services
import { FbosService } from '../../../services/fbos.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { GroupFboViewModel } from 'src/app/models/groups';
import { GroupsService } from 'src/app/services/groups.service';

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
    public groupsFbosData: GroupFboViewModel;

    constructor(
        private router: Router,
        private fboService: FbosService,
        private fboAirportsService: FboairportsService,
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private groupsService: GroupsService
    ) {
        this.currentFbo = null;
        this.currentFboAirport = null;
    }

    async ngOnInit() {
        this.ngxLoader.startLoader(this.chartName);

        let fboDataPromise =  this.loadAllFbosForGroup();

        const groupId = this.sharedService.currentUser.groupId;
        let groupsFbosDataPromise = this.groupsService.groupsAndFbos(groupId).toPromise();
        
        Promise.all([fboDataPromise, groupsFbosDataPromise]).then(results => {
            this.fbosData = results[0];
            this.groupsFbosData = results[1];
            this.ngxLoader.stopLoader(this.chartName);
        });
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

    public async saveFboEditClicked() {
        this.currentFboAirport = null;
        this.currentFbo = null;
        this.fbosData = null;
        this.fbosData =  await this.loadAllFbosForGroup(); 
    }

    public cancelFboEditClicked() {
        this.currentFbo = null;
    }

    // Private Methods
    private async loadAllFbosForGroup() :Promise<any> {
        let groupId  = (!this.groupInfo) ? this.sharedService.currentUser.groupId : this.groupInfo.oid;
         return this.fboService.getForGroup(groupId).toPromise();
    } 
}
