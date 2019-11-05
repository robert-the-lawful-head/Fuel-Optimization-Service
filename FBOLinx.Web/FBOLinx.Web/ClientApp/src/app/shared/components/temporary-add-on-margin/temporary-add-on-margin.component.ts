import { Component, Inject, EventEmitter, Output, Optional, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
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
    public stringButton: any;
    public help: boolean;
    public helpOne: boolean;
    public pom: any;
    public counter: number = 0;
    public brojac: number = 0;
    @Output() idChanged1: EventEmitter<any> = new EventEmitter();
    @Output() jetChanged: EventEmitter<any> = new EventEmitter();
    @ViewChild("prm") btn: ElementRef;

    constructor(private router: Router,
        public dialogRef: MatDialogRef<TemporaryAddOnMarginComponent>, @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
        private temporaryAddOnMargin: TemporaryAddOnMarginService,
        private sharedService: SharedService) {
        this.stringButton = this.data.update ? 'Update' : 'Save & Append';
        this.data.EffectiveFrom = new Date(this.data.EffectiveFrom);
        this.data.EffectiveTo = new Date(this.data.EffectiveTo);
    }

    public onType(data, event) {
        this.brojac += 1;
        if (event.key === data.MarginJet * 100) {
            this.counter = 0;
        }
        if (this.helpOne) {
            //data.MarginJet = (data.MarginJet * 100 - event.key * 0.01);
            data.MarginJet = (data.MarginJet * 100 - event.key)/ 1000 + event.key*0.01;
            this.helpOne = false;
        }
        if (this.help) {
            data.MarginJet = data.MarginJet * 10;
            this.help = false;
            this.helpOne = true;
        }
        if (event.key === '.' && this.counter === 0 && this.brojac<3) {
            this.help = true;
            this.counter += 1;
        }
    }

    public setSelectionRange(input, selectionStart, selectionEnd) {
        if (input.setSelectionRange) {
            input.focus();
            input.setSelectionRange(selectionStart, selectionEnd);
        } else if (input.createTextRange) {
            var range = input.createTextRange();
            range.collapse(true);
            range.moveEnd('character', selectionEnd);
            range.moveStart('character', selectionStart);
            range.select();
        }
    }

    public add() {
        if (this.data.update) {

            this.data.EffectiveFrom = moment(this.data.EffectiveFrom).format("YYYY-MM-DDT00:00:00.000") + "Z";
            this.data.EffectiveTo = moment(this.data.EffectiveTo).format("YYYY-MM-DDT00:00:00.000") + "Z";
            this.temporaryAddOnMargin.update(this.data).subscribe((savedTemplate: temporaryAddOnMargin) => {
                this.jetChanged.emit(savedTemplate);
                this.dialogRef.close();
            });
        }
        else {
            this.data.EffectiveFrom = moment(this.data.EffectiveFrom).format("YYYY-MM-DDT00:00:00.000") + "Z";
            this.data.EffectiveTo = moment(this.data.EffectiveTo).format("YYYY-MM-DDT00:00:00.000") + "Z";

            this.data.fboId = this.sharedService.currentUser.fboId;
            this.temporaryAddOnMargin.add(this.data).subscribe((savedTemplate: temporaryAddOnMargin) => {
                this.idChanged1.emit({ id: savedTemplate.id, EffectiveFrom: this.data.EffectiveFrom, EffectiveTo: this.data.EffectiveTo, MarginJet: this.data.MarginJet });
                this.dialogRef.close();
            });
        }
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
