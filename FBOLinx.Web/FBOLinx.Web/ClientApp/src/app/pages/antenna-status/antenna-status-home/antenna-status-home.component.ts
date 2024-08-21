import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportWatchService } from '../../../services/airportwatch.service';

// Models
import { NgxUiLoaderService } from "ngx-ui-loader";
import { AntennaStatusGridViewmodel } from '../../../models/antenna-status/antenna-status-grid-viewmodel';


@Component({
    selector: 'app-antenna-status-home',
    styleUrls: ['./antenna-status-home.component.scss'],
    templateUrl: './antenna-status-home.component.html',
})
export class AntennaStatusHomeComponent implements OnInit {
    // Public Members
    public pageTitle = 'Antenna Status';
    public breadcrumb: any[];
    public antennaStatusData: any[];
    public antennaStatusGridData: any[];
    public antennaStatusGridItem: AntennaStatusGridViewmodel;
    public antennaStatusHomeLoaderName: string = 'antenna-status-home-loader';

    constructor(
        private sharedService: SharedService,
        private ngxLoader: NgxUiLoaderService,
        private airportWatchService: AirportWatchService
    ) {
        
    }

    ngOnInit() {
        this.loadAntennaStatus();
    }

    private loadAntennaStatus() {
        this.antennaStatusGridData = null;
        this.ngxLoader.startLoader(this.antennaStatusHomeLoaderName);
        this.airportWatchService
            .getAntennaStatusData()
            .subscribe((data: any) => {
                this.antennaStatusGridData = data;
                this.ngxLoader.stopLoader(this.antennaStatusHomeLoaderName);
            });
    }
}
