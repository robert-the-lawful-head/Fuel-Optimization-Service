import { Component, Inject, EventEmitter, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

// Services
import { AcukwikairportsService } from '../../../services/acukwikairports.service';
import { FbosService } from '../../../services/fbos.service';

// Interfaces
export interface NewFboModel {
    oid: number;
    fbo: string;
    icao: string;
    iata: string;
    acukwikFboHandlerId: number;
    acukwikFbo: any;
    group: string;
}

@Component({
    selector: 'app-fbos-grid-new-fbo-dialog',
    templateUrl: './fbos-grid-new-fbo-dialog.component.html',
    styleUrls: ['./fbos-grid-new-fbo-dialog.component.scss'],
})
export class FbosGridNewFboDialogComponent {
    @Output() contactAdded = new EventEmitter<any>();
    public errorHappened = false;
    // Public Members
    public dataSources: any = {};

    constructor(
        public dialogRef: MatDialogRef<FbosGridNewFboDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewFboModel,
        private acukwikairportsService: AcukwikairportsService,
        private fboService: FbosService
    ) {}

    public airportValueChanged(airport: any) {
        this.data.icao = airport.icao;
        this.data.iata = airport.iata;
        this.dataSources.acukwikFbos = [];
        this.data.acukwikFbo = null;
        this.acukwikairportsService
            .getAcukwikFboHandlerDetailByIcao(this.data.icao)
            .subscribe((result: any) => {
                if (!result) {
                    return;
                }

                this.dataSources.acukwikFbos.push(...result);
            });
    }

    public fboSelectionChange() {
        this.data.fbo = this.data.acukwikFbo.handlerLongName;
        this.data.acukwikFboHandlerId = this.data.acukwikFbo.handlerId;
        this.data.group = `${this.data.fbo} - ${this.data.icao}`;
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public onSaveClick(data): void {
        this.errorHappened = false;

        this.fboService.addSingleFbo(data).subscribe((newFbo: any) =>
        { this.dialogRef.close(newFbo); },
            err => {
                this.errorHappened = true;
            }
        );
    }
}
