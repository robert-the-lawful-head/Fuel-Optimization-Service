import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { AcukwikairportsService } from '../../../services/acukwikairports.service';

import { AirportAutocompleteComponent } from '../../../shared/components/airport-autocomplete/airport-autocomplete.component';

//Interfaces
export interface NewFBODialogData {
    oid: number;
    fbo: string;
    icao: string;
}

@Component({
    selector: 'app-fbos-dialog-new-fbo',
    templateUrl: './fbos-dialog-new-fbo.component.html',
    styleUrls: ['./fbos-dialog-new-fbo.component.scss']
})
/** fbos-dialog-new-fbo component*/
export class FbosDialogNewFboComponent {

    /** fbos-dialog-new-fbo ctor */
    constructor(public dialogRef: MatDialogRef<FbosDialogNewFboComponent>, @Inject(MAT_DIALOG_DATA) public data: NewFBODialogData) {
    }

    public airportValueChanged(value) {
        console.log('Airport Change: ', value);
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
