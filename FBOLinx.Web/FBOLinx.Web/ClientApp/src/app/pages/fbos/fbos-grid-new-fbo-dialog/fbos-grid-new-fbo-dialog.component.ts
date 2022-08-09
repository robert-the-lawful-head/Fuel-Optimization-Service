import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

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
    styleUrls: ['./fbos-grid-new-fbo-dialog.component.scss'],
    templateUrl: './fbos-grid-new-fbo-dialog.component.html',
})
export class FbosGridNewFboDialogComponent {
    @Output() contactAdded = new EventEmitter<any>();
    public errorHappened = false;
    public errorMessage: string = '';
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
        this.fboService.getByAcukwikHandlerId(this.data.acukwikFbo.handlerId).subscribe((result: any) => {
            //No pre-existing record exists for that FBO in another group - allow adding
            if (!result || result.oid == 0) {
                this.errorMessage = '';
                this.data.fbo = this.data.acukwikFbo.handlerLongName;
                this.data.acukwikFboHandlerId = this.data.acukwikFbo.handlerId;
                this.data.group = `${this.data.fbo} - ${this.data.icao}`;
            } else {
                this.errorMessage = 'That FBO is already part of a group.';
            }

        });
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public onSaveClick(data): void {
        this.errorHappened = false;

        this.fboService.addSingleFbo(data).subscribe(
            (newFbo: any) => {
                this.dialogRef.close(newFbo);
            },
            (err) => {
                this.errorHappened = true;
            }
        );
    }
}
