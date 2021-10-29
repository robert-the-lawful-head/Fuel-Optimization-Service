import { CustomerHistoryComponent } from '../customer-history/customer-history.component';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-customer-history-details',
  templateUrl: './customer-history-details.component.html',
  styleUrls: ['./customer-history-details.component.scss']
})
export class CustomerHistoryDetailsComponent implements OnInit {

  constructor(  public dialogRef: MatDialogRef<CustomerHistoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
      console.log(this.data)
  }

}
