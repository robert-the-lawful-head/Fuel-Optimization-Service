import { Component, OnInit, Inject, Input, Output } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { AcukwikairportsService } from '../../../services/acukwikairports.service';
import { FbosService } from '../../../services/fbos.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { FbosDialogNewFboComponent } from '../fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';

@Component({
    selector: 'app-fbos-home',
    templateUrl: './fbos-home.component.html',
    styleUrls: ['./fbos-home.component.scss']
})
/** fbos-home component*/
export class FbosHomeComponent {

    @Input() groupInfo: any;

    //Public Members
    public fbosData: Array<any>;
    public currentFbo: any;
    public currentFboAirport: any;
    public airportData: Array<any>;

    /** fbos-home ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private fboService: FbosService,
        private fboAirportsService: FboairportsService,
        public newFboDialog: MatDialog,
        private acukwikAirportsService: AcukwikairportsService,
        private sharedService: SharedService) {
        
        this.currentFbo = null;
        this.currentFboAirport = null;
    }

    ngOnInit() {
        this.loadInitialData();

        //this.currentFbo = this.route.paramMap.pipe(
        //    switchMap((params: ParamMap) =>
        //        this.fboService.get({ oid: params.get('id') }))
        //);
    }

    /** Public Methods */
    public editFboClicked = function (record) {
        if (!this.groupInfo)
            this.router.navigate(['/default-layout/fbos/' + record.oid]);
        else {
            this.fboService.get(record).subscribe((data: any) => this.currentFbo = data);
            this.fboAirportsService.getForFbo(record).subscribe((data: any) => this.currentFboAirport = data);
        }
    }

    public deleteFboClicked = function (record) {
        
    };

    public saveFboEditClicked = function () {
        this.currentFboAirport = null;
        this.currentFbo = null;
    }

    public cancelFboEditClicked() {
        this.currentFbo = null;
    }

    //Private Methods
    private loadInitialData() {
        if (!this.groupInfo)
            this.loadAllFbos();
        else
            this.loadAllFbosForGroup();
    }

    private loadAllFbos() {
        this.fboService.getAllFbos().subscribe((data: any) => this.fbosData = data);
    }

    private loadAllFbosForGroup() {
        this.fboService.getForGroup(this.groupInfo.oid).subscribe((data: any) => this.fbosData = data);
    }
}
