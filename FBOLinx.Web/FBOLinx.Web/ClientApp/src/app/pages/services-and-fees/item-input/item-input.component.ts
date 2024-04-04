import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface DialogData {
    name: string;
  }
@Component({
  selector: 'app-item-input',
  templateUrl: './item-input.component.html',
  styleUrls: ['./item-input.component.scss']
})
export class ItemInputComponent {

    constructor(
        public dialogRef: MatDialogRef<ItemInputComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
      ) {}

      onNoClick(): void {
        this.dialogRef.close();
      }

}
