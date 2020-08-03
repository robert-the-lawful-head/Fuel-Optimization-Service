import { Component, OnInit, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";



export interface CustomerMatchDialogComponent {
    currentCustomerId: number;
    matchCustomerId: number;
    currentCustomerName: string;
    matchCustomerName: string;
    isAircraftMatch: boolean;
    isNameMatch: boolean;
    isContactMatch: boolean;
    matchNameCustomerId: number;
    matchNameCustomerOId: number;
    matchNameCustomer: string;
    matchContactCustomerOid: number;
    matchNameCustomerOid: number;
    matchCustomerOid: number;
    aircraftTails: [];
    result: string;
}

@Component({
  selector: "customer-match-dialog",
  templateUrl: "./customer-match-dialog.component.html",
  styleUrls: ["./customer-match-dialog.component.scss"],
})
export class CustomerMatchDialogComponent implements OnInit {

    constructor(
        public dialogRef: MatDialogRef<CustomerMatchDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CustomerMatchDialogComponent
    ) { }

    ngOnInit(): void {
        console.log("in modal");
        console.log(this.data);
  }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public keepSeparate(): void {
        this.data.result = "KeepSeparate";
        this.dialogRef.close("KeepSeparate");
    }

    public mergeCustomers(): void {
        this.data.result = "Merge";
        this.dialogRef.close(this.data);
    }
}
