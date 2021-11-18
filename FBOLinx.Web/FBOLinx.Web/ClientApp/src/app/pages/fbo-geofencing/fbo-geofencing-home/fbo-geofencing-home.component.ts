import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportFboGeofenceClustersService } from '../../../services/airportfbogeofenceclusters.service';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/fbo-geofencing',
        title: 'FBO Geofencing',
    },
];

@Component({
    selector: 'app-fbo-geofencing-home',
    styleUrls: ['./fbo-geofencing-home.component.scss'],
    templateUrl: './fbo-geofencing-home.component.html',
})
export class FboGeofencingHomeComponent implements OnInit {
    // Public Members
    public pageTitle = 'FBO Geofencing';
    public breadcrumb: any[] = BREADCRUMBS;
    public fboGeofencingData: any[];

    constructor(
        private sharedService: SharedService,
        private airportFboGeofenceService: AirportFboGeofenceClustersService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.loadFbogeofencing();
    }

    // PRIVATE METHODS
    private loadFbogeofencing() {
        this.airportFboGeofenceService
            .getAllClusters()
            .subscribe((data: any) => {
                this.fboGeofencingData = data;
            });
    }
}
