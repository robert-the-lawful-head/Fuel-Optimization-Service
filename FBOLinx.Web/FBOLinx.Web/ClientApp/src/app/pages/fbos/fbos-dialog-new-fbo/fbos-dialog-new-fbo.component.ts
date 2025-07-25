import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { FbosService } from 'src/app/services/fbos.service';

// Services
import { AcukwikairportsService } from '../../../services/acukwikairports.service';

// Interfaces
export interface NewFBODialogData {
    oid: number;
    fbo: string;
    icao: string;
    iata: string;
    acukwikFboHandlerId: number;
    acukwikFbo: any;
    groupId: number;
}

@Component({
    selector: 'app-fbos-dialog-new-fbo',
    styleUrls: ['./fbos-dialog-new-fbo.component.scss'],
    templateUrl: './fbos-dialog-new-fbo.component.html',
})
export class FbosDialogNewFboComponent {
    @Output() contactAdded = new EventEmitter<any>();

    // Public Members
    public dataSources: any = {};
    public errorMessage: string = '';

    constructor(
        public dialogRef: MatDialogRef<FbosDialogNewFboComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewFBODialogData,
        private acukwikairportsService: AcukwikairportsService,
        private fbosService: FbosService
    ) {}

    public airportValueChanged(airport: any) {
        this.data.icao = airport.icao;
        this.data.iata = airport.iata;
        this.acukwikairportsService
            .getAcukwikFboHandlerDetailByIcao(this.data.icao)
            .subscribe((result: any) => {
                this.dataSources.acukwikFbos = [];
                if (!result) {
                    return;
                }
                this.dataSources.acukwikFbos.push(...result);
            });
    }

    public fboSelectionChange() {
        this.fbosService.getByAcukwikHandlerId(this.data.acukwikFbo.handlerId).subscribe((result: any) => {
            //No pre-existing record exists for that FBO in another group - allow adding
            if (!result || result.oid == 0) {
                this.errorMessage = '';
                this.data.fbo = this.data.acukwikFbo.handlerLongName;
                this.data.acukwikFboHandlerId = this.data.acukwikFbo.handlerId;
            } else {
                this.errorMessage = 'That FBO is already part of a group.';
            }

        });
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public onSaveChanges(): void {
        this.fbosService.add(this.data).subscribe((newFbo: any) => {
            this.dialogRef.close(newFbo);
        });
    }
}
