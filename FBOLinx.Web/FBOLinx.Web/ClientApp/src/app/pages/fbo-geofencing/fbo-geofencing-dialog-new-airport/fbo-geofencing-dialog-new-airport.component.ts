import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

//Services
import { AcukwikairportsService } from '../../../services/acukwikairports.service';

// Models
import { AirportFboGeoFenceGridViewmodel } from '../../../models/fbo-geofencing/airport-fbo-geo-fence-grid-viewmodel';

@Component({
    selector: 'app-fbo-geofencing-dialog-new-airport',
    styleUrls: ['./fbo-geofencing-dialog-new-airport.component.scss'],
    templateUrl: './fbo-geofencing-dialog-new-airport.component.html',
})
export class FboGeofencingDialogNewAirportComponent {

    constructor(public dialogRef: MatDialogRef<FboGeofencingDialogNewAirportComponent>,
        @Inject(MAT_DIALOG_DATA) public data: AirportFboGeoFenceGridViewmodel,
        private acukwikairportsService: AcukwikairportsService) {

    }

    airportValueChanged(airport: any) {
        this.data.icao = airport.icao;
        this.data.acukwikAirportId = airport.airportId;
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onAddAirportClick(): void {
        this.dialogRef.close(this.data);
        
    }
}
