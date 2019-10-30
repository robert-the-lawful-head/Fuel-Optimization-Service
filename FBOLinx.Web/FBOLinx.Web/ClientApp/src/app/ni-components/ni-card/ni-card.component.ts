import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { TemporaryAddOnMarginComponent } from '../../shared/components/temporary-add-on-margin/temporary-add-on-margin.component';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs';

@Component({
    selector: 'ni-card',
    templateUrl: './ni-card.component.html',
    styleUrls: ['./ni-card.component.scss'],
    host: {
        '[class.ni-card]': 'true'
    }
})


export class NiCardComponent implements OnInit {
    vId: any;
    vEffectiveFrom: any;
    vEffectiveTo: any;
    vJet: any;
    @Output() jetChanged: EventEmitter<any> = new EventEmitter();
    @Input() title: string = '';
    @Input() tempId: string = '';
    @Input() visible: string = '';
    @Input() bgColor: string = '';
    @Input() customBgColor: string = '';
    @Input() color: string = '';
    @Input() customColor: string = '';
    @Input() bgImage: string = '';
    @Input() outline: boolean = false;
    @Input() indents: any = '';
    @Input() align: string = 'left';
    @Input() headerBgColor: string = '';
    @Input() headerColor: string = '';
    @Input() theme: string = '';

    constructor(public tempAddOnMargin: MatDialog, public deleteFBODialog: MatDialog) { }

    ngOnInit() { }

    openDialog(): Observable<any> {
        const dialogRef = this.tempAddOnMargin.open(TemporaryAddOnMarginComponent, {
            data: { EffectiveFrom: this.vEffectiveFrom, EffectiveTo: this.vEffectiveTo, MarginJet: this.vJet, Id: this.vId, update: false }
        });
        dialogRef.componentInstance.idChanged1.subscribe((result) => {
            console.log(result);
            this.jetChanged.emit(result);

        });
        return dialogRef.afterClosed();
    }
    private openAddOnMargin() {

        this.openDialog();


    }
}
