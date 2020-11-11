import { Component, Inject } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';

export interface ProceedConfirmationData {
  description: string;
  itemsList: string[];
}

@Component({
  selector: 'app-proceed-confirmation',
  templateUrl: './proceed-confirmation.component.html',
  styleUrls: ['./proceed-confirmation.component.scss'],
})
export class ProceedConfirmationComponent {
  constructor(
    public dialogRef: MatDialogRef<ProceedConfirmationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProceedConfirmationData
  ) { }

  onCancelClick(): void {
    this.dialogRef.close();
  }
}
