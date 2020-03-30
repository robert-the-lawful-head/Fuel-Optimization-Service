import {
    Component,
    Inject,
    EventEmitter,
    Output,
    Optional,
    ViewChild,
    ElementRef,
} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

import { TemporaryAddOnMarginService } from "../../../services/temporaryaddonmargin.service";
import { SharedService } from "../../../layouts/shared-service";

export interface TemporaryAddOnMargin {
    id: any;
    fboId: any;
    effectiveFrom: any;
    effectiveTo: any;
    marginJet: any;
}

@Component({
    selector: "app-temporary-add-on-margin",
    templateUrl: "./temporary-add-on-margin.component.html",
    styleUrls: ["./temporary-add-on-margin.component.scss"],
    providers: [SharedService],
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
    public counter = 0;
    public brojac = 0;
    @Output() idChanged1: EventEmitter<any> = new EventEmitter();
    @Output() jetChanged: EventEmitter<any> = new EventEmitter();
    @ViewChild("prm") btn: ElementRef;

    constructor(
        public dialogRef: MatDialogRef<TemporaryAddOnMarginComponent>,
        @Optional()
        @Inject(MAT_DIALOG_DATA)
        public data: any,
        private temporaryAddOnMargin: TemporaryAddOnMarginService,
        private sharedService: SharedService
    ) {
        this.stringButton = this.data.update ? "Update Margin" : "Add Margin";
        if (this.data.EffectiveFrom) {
            this.data.EffectiveFrom = new Date(this.data.EffectiveFrom);
        }
        if (this.data.EffectiveTo) {
            this.data.EffectiveTo = new Date(this.data.EffectiveTo);
        }
    }

    public onType(data, event) {
        this.brojac += 1;
        if (event.key === data.MarginJet * 10000) {
            this.counter = 0;
        }
        if (this.helpOne) {
            data.MarginJet =
                (data.MarginJet * 10000 - event.key) / 100000 +
                event.key * 0.0001;
            this.helpOne = false;
        }
        if (this.help) {
            data.MarginJet = data.MarginJet * 1000;
            this.help = false;
            this.helpOne = true;
        }
    }

    public setSelectionRange(input, selectionStart, selectionEnd) {
        if (input.setSelectionRange) {
            input.focus();
            input.setSelectionRange(selectionStart, selectionEnd);
        } else if (input.createTextRange) {
            const range = input.createTextRange();
            range.collapse(true);
            range.moveEnd("character", selectionEnd);
            range.moveStart("character", selectionStart);
            range.select();
        }
    }

    public add() {
        if (this.data.update) {
            this.temporaryAddOnMargin
                .update(this.data)
                .subscribe((savedTemplate: TemporaryAddOnMargin) => {
                    this.jetChanged.emit(savedTemplate);
                    this.dialogRef.close();
                });
        } else {
            this.data.fboId = this.sharedService.currentUser.fboId;
            this.temporaryAddOnMargin
                .add(this.data)
                .subscribe((savedTemplate: TemporaryAddOnMargin) => {
                    this.idChanged1.emit({
                        id: savedTemplate.id,
                        EffectiveFrom: savedTemplate.effectiveFrom,
                        EffectiveTo: savedTemplate.effectiveTo,
                        MarginJet: savedTemplate.marginJet,
                    });
                    this.dialogRef.close();
                });
        }
    }

    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
