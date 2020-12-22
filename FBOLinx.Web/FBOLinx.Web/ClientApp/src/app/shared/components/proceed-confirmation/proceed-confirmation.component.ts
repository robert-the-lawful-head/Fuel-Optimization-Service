import { Component, Inject, Input } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';

export interface ProceedConfirmationData {
  description: string;
    itemsList: string[];
    buttonText: string;
    title: string;
}

@Component({
  selector: 'app-proceed-confirmation',
  templateUrl: './proceed-confirmation.component.html',
  styleUrls: ['./proceed-confirmation.component.scss'],
})
export class ProceedConfirmationComponent {
    @Input() buttonText = 'Confirm';
    @Input() title = 'Distribute?';

    constructor(
        public dialogRef: MatDialogRef<ProceedConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ProceedConfirmationData
    ) {
        if (data.buttonText) {
            this.buttonText = data.buttonText;
        }

        if (data.title) {
            this.title = data.title;
        }
    }

  onCancelClick(): void {
    this.dialogRef.close();
  }
}
