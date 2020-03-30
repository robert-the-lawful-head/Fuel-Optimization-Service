import { Component, Inject, EventEmitter, Output } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

// Services
import { AcukwikairportsService } from "../../../services/acukwikairports.service";

import { AirportAutocompleteComponent } from "../../../shared/components/airport-autocomplete/airport-autocomplete.component";

// Interfaces
export interface NewFBODialogData {
    oid: number;
    fbo: string;
    icao: string;
    acukwikFboHandlerId: number;
    acukwikFbo: any;
}

@Component({
    selector: "app-fbos-dialog-new-fbo",
    templateUrl: "./fbos-dialog-new-fbo.component.html",
    styleUrls: ["./fbos-dialog-new-fbo.component.scss"],
})
export class FbosDialogNewFboComponent {
    @Output() contactAdded = new EventEmitter<any>();

    // Public Members
    public dataSources: any = {};

    constructor(
        public dialogRef: MatDialogRef<FbosDialogNewFboComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewFBODialogData,
        private acukwikairportsService: AcukwikairportsService
    ) {}

    public airportValueChanged(airport) {
        this.data.icao = airport.icao;
        this.acukwikairportsService
            .getAcukwikFboHandlerDetailByIcao(this.data.icao)
            .subscribe((result: any) => {
                this.dataSources.acukwikFbos = [];
                if (!result) {
                    return;
                }

                result.forEach((fbo) => {
                    this.dataSources.acukwikFbos.push(fbo);
                });
            });
    }

    public fboSelectionChange() {
        if (!this.data.acukwikFbo) {
            this.data.fbo = "";
            this.data.acukwikFboHandlerId = 0;
        } else {
            this.data.fbo = this.data.acukwikFbo.handlerLongName;
            this.data.acukwikFboHandlerId = this.data.acukwikFbo.handlerId;
        }
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    // public addFbo() {
    //    this.contactAdded.emit(this.data);
    // }
}
