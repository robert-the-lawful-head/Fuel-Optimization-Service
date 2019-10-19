import { Component, Inject, EventEmitter, Output, Optional } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { TemporaryAddOnMarginService } from '../../../services/temporaryaddonmargin.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';
import { Validators } from '@angular/forms';

export interface temporaryAddOnMargin {
    id: any;
    fboId: any;
    EffectiveFrom: any;
    EffectiveTo: any;
    MarginJet: any;
}
@Component({
    selector: 'app-temporary-add-on-margin',
    templateUrl: './temporary-add-on-margin.component.html',
    styleUrls: ['./temporary-add-on-margin.component.scss'],
    providers: [SharedService]
})

export class TemporaryAddOnMarginComponent {
    public id: any;
    public jet: any;
    public effectivefrom: any;
    public effectiveto: any;
    @Output() idChanged1: EventEmitter<any> = new EventEmitter();

    constructor(private router: Router,
        public dialogRef: MatDialogRef<TemporaryAddOnMarginComponent>, @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
        private temporaryAddOnMargin: TemporaryAddOnMarginService,
    private sharedService: SharedService) {

    }


    public add() {
        this.data.fboId = this.sharedService.currentUser.fboId;
        this.temporaryAddOnMargin.add(this.data).subscribe((savedTemplate: temporaryAddOnMargin) => {
            this.idChanged1.emit({ id: savedTemplate.id, EffectiveFrom: this.data.EffectiveFrom, EffectiveTo: this.data.EffectiveTo, MarginJet: this.data.MarginJet});
            this.dialogRef.close(); 
        });
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
