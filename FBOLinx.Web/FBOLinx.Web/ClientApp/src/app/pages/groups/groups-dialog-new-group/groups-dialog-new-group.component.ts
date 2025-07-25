import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

import { AcukwikairportsService } from '../../../services/acukwikairports.service';
import { FbosService } from '../../../services/fbos.service';
import { GroupsService } from '../../../services/groups.service';

@Component({
    selector: 'app-groups-dialog-new-group',
    styleUrls: ['./groups-dialog-new-group.component.scss'],
    templateUrl: './groups-dialog-new-group.component.html',
})
export class GroupsDialogNewGroupComponent {
    step = 1;
    type: string;
    dataSources: any = {};
    public fboAlreadyExists = false;

    constructor(
        public dialogRef: MatDialogRef<GroupsDialogNewGroupComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private acukwikairportsService: AcukwikairportsService,
        private groupsService: GroupsService,
        private fboService: FbosService
    ) {
        data.active = true;
    }

    airportValueChanged(airport: any) {
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

        this.fboService
            .getByAcukwikHandlerId(this.data.acukwikFboHandlerId)
            .subscribe((response: any) => {
                if (!response) {
                    this.fboAlreadyExists = false;
                } else {
                    this.fboAlreadyExists = true;
                }
            });
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onSaveClick(): void {
        if (this.type === 'group') {
            this.data.active = true;
            this.groupsService.add(this.data).subscribe((data: any) => {
                this.dialogRef.close({
                    data,
                    type: this.type,
                });
            });
        }
        if (this.type === 'fbo') {
            this.fboService.addSingleFbo(this.data).subscribe((data: any) => {
                this.dialogRef.close({
                    data,
                    type: this.type,
                });
            });
        }
    }
}
