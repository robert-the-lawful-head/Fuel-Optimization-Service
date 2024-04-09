import {
    Component,
    Input,
    OnDestroy,
    OnInit,
    Inject,
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgxUiLoaderService } from "ngx-ui-loader";
import { MatDialog } from '@angular/material/dialog';

// Services
import { AcukwikairportsService } from "../../../services/acukwikairports.service";

// Models
import { FlightWatch } from '../../../models/flight-watch';
import { AirportFboGeoFenceCluster } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-cluster';
import { AirportFboGeoFenceGridViewmodel } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-grid-viewmodel';

@Component({
    selector: 'app-fbo-geofencing-dialog-new-cluster',
    styleUrls: ['./fbo-geofencing-dialog-new-cluster.component.scss'],
    templateUrl: './fbo-geofencing-dialog-new-cluster.component.html',
})
export class FboGeofencingDialogNewClusterComponent implements OnInit {

    public dataSources: any = {};
    public icao: string = '';
    public acukwikFbo: any = null;
    public loaderName: string = 'fbo-geofence-grid-loader';

    constructor(public dialogRef: MatDialogRef<FboGeofencingDialogNewClusterComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private acukwikAirportsService: AcukwikairportsService) {
        this.icao = data.icao;
    }

    ngOnInit(): void {
        if (!this.dataSources.acukwikFbos) {
            this.loadAvailableFbos();
        }
    }

    public onStartClick(): void {
        this.dialogRef.close({ acukwikFbo: this.acukwikFbo});
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    private loadAvailableFbos(): void {
        this.acukwikAirportsService.getAcukwikFboHandlerDetailByIcao(this.icao)
            .subscribe((response: any) => {
                this.dataSources.acukwikFbos = [];
                if (!response)
                    return;
                this.dataSources.acukwikFbos.push(...response);
            });
    }
}

