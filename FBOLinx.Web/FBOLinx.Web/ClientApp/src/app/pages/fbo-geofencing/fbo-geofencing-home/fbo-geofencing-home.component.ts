import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { MatDialog } from '@angular/material/dialog';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { AirportFboGeofenceClustersService } from '../../../services/airportfbogeofenceclusters.service';

// Models
import { AirportFboGeoFenceGridViewmodel } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-grid-viewmodel';

import { NgxUiLoaderService } from "ngx-ui-loader";
import { FboGeofencingDialogNewAirportComponent } from
    "../fbo-geofencing-dialog-new-airport/fbo-geofencing-dialog-new-airport.component";

@Component({
    selector: 'app-fbo-geofencing-home',
    styleUrls: ['./fbo-geofencing-home.component.scss'],
    templateUrl: './fbo-geofencing-home.component.html',
})
export class FboGeofencingHomeComponent implements OnInit {
    // Public Members
    public pageTitle = 'FBO Geofencing';
    public breadcrumb: any[];
    public fboGeofencingData: any[];
    public airportFboGeoFenceGridData: any[];
    public airportFboGeofenceGridItem: AirportFboGeoFenceGridViewmodel;
    public fboHomeLoaderName: string = 'fbo-home-loader';

    constructor(
        private sharedService: SharedService,
        private airportFboGeofenceService: AirportFboGeofenceClustersService,
        private ngxLoader: NgxUiLoaderService,
        public newAirportDialog: MatDialog
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.loadFbogeofencing();
        this.loadAirportsWithAntennaData();
        this.setupBreadcrumb();
    }

    public onEditAirportFboGeoFence(airportFboGeoFenceClusterViewModel: AirportFboGeoFenceGridViewmodel): void {
        this.airportFboGeofenceGridItem = airportFboGeoFenceClusterViewModel;
        this.setupBreadcrumb();
    }

    public onEditClosed(event: AirportFboGeoFenceGridViewmodel): void {
        this.airportFboGeofenceGridItem = null;
        this.loadAirportsWithAntennaData();
        this.setupBreadcrumb();
    }

    public onAddAirportFboGeoFence(): void {
        const dialogRef = this.newAirportDialog.open(FboGeofencingDialogNewAirportComponent,
            {
                data: { icao: '' },
                width: '450px'
            });

        dialogRef.afterClosed().subscribe((result: AirportFboGeoFenceGridViewmodel) => {
            if (!result)
                return;

            this.airportFboGeofenceService.getAirportForGeoFencingByAcukwikAirportId(result.acukwikAirportId).subscribe(
                (response:
                    AirportFboGeoFenceGridViewmodel) => {
                    this.onEditAirportFboGeoFence(response);
                });
        });
    }

    // PRIVATE METHODS
    private setupBreadcrumb() {
        if (this.airportFboGeofenceGridItem) {
            this.breadcrumb = [
                {
                    link: '/default-layout',
                    title: 'Main',
                },
                {
                    link: '/default-layout/fbo-geofencing',
                    title: 'FBO Geofencing',
                },
                {
                    link: '',
                    title: 'Edit Geofencing',
                }
            ];
        } else {
            this.breadcrumb = [
                {
                    link: '/default-layout',
                    title: 'Main',
                },
                {
                    link: '',
                    title: 'FBO Geofencing',
                }
            ];
        }
    }

    private loadFbogeofencing() {
        this.airportFboGeofenceService
            .getAllClusters()
            .subscribe((data: any) => {
                this.fboGeofencingData = data;
            });
    }

    private loadAirportsWithAntennaData() {
        this.airportFboGeoFenceGridData = null;
        this.ngxLoader.startLoader(this.fboHomeLoaderName);
        this.airportFboGeofenceService
            .getAirportsWithAntennaData()
            .subscribe((data: any) => {
                this.airportFboGeoFenceGridData = data;
                this.ngxLoader.stopLoader(this.fboHomeLoaderName);
            });
    }
}
