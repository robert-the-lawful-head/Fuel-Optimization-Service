import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { TemporaryAddOnMarginService } from '../../../services/temporaryaddonmargin.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';
import { Validators } from '@angular/forms';

export interface temporaryAddOnMargin {
    id: number;
    fboId: number;
    EffectiveFrom: any;
    EffectiveTo: any;
    MarginJet: string;
}
@Component({
    selector: 'app-temporary-add-on-margin',
    templateUrl: './temporary-add-on-margin.component.html',
    styleUrls: ['./temporary-add-on-margin.component.scss'],
    providers: [SharedService]
})

export class TemporaryAddOnMarginComponent {


    constructor(private router: Router,
        public dialogRef: MatDialogRef<TemporaryAddOnMarginComponent>, @Inject(MAT_DIALOG_DATA) public data: temporaryAddOnMargin,
        private temporaryAddOnMargin: TemporaryAddOnMarginService,
    private sharedService: SharedService) {

    }


    public add() {
        this.data.fboId = this.sharedService.currentUser.fboId;
        this.temporaryAddOnMargin.add(this.data).subscribe((savedTemplate: any) => {
            this.dialogRef.close(); 
        });
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
