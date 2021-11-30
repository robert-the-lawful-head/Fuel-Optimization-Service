import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportFboGeofenceClustersService } from '../../../services/airportfbogeofenceclusters.service';

// Models
import { AirportFboGeoFenceGridViewmodel as airportFboGeoFenceGridViewmodel } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-grid-viewmodel';
import { NgxUiLoaderService } from "ngx-ui-loader";

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
    public airportFboGeoFenceGridData: any[];
    public airportFboGeofenceGridItem: airportFboGeoFenceGridViewmodel;
    public fboHomeLoaderName: string = 'fbo-home-loader';

    constructor(
        private sharedService: SharedService,
        private airportFboGeofenceService: AirportFboGeofenceClustersService,
        private ngxLoader: NgxUiLoaderService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.loadFbogeofencing();
        this.loadAirportsWithAntennaData();
    }

    public onEditAirportFboGeoFence(airportFboGeoFenceClusterViewModel: airportFboGeoFenceGridViewmodel): void {
        this.airportFboGeofenceGridItem = airportFboGeoFenceClusterViewModel;
    }

    public onEditClosed(): void {
        this.airportFboGeofenceGridItem = null;
        this.loadAirportsWithAntennaData();
    }

    // PRIVATE METHODS
    private loadFbogeofencing() {
        this.airportFboGeofenceService
            .getAllClusters()
            .subscribe((data: any) => {
                this.fboGeofencingData = data;
            });
    }

    private loadAirportsWithAntennaData() {
        this.ngxLoader.startLoader(this.fboHomeLoaderName);
        this.airportFboGeofenceService
            .getAirportsWithAntennaData()
            .subscribe((data: any) => {
                this.airportFboGeoFenceGridData = data;
                this.ngxLoader.stopLoader(this.fboHomeLoaderName);
            });
    }
}
