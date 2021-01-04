import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';

// Services


@Component({
    selector: 'app-customer-aircraft-select-model-dialog',
    templateUrl: './customer-aircrafts-select-model-dialog.component.html',
    styleUrls: [ './customer-aircrafts-select-model-dialog.component.scss' ],
})
export class CustomerAircraftSelectModelComponent implements OnInit {
    // Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;

    public customerAircraftsDataSource: MatTableDataSource<any> = null;

    displayedColumns: string[] = [ 'tailNumber', 'model', 'otherOptions' ];

    constructor(
        public dialogRef: MatDialogRef<CustomerAircraftSelectModelComponent>,
        @Inject(MAT_DIALOG_DATA) public data,
        private customerAircraftsService: CustomeraircraftsService
    ) {
    }

    ngOnInit() {
        console.log(this.data);
        this.customerAircraftsDataSource = new MatTableDataSource(
            this.data.aircrafts
        );
    }


    public onCancelClick(): void {

        this.dialogRef.close();
    }

    public onSaveClick(): void {
        console.log(this.data.aircrafts);
        this.data.aircrafts.forEach((result) => {
            if (result.selectedModel) {
                result.model = result.selectedModel;
            }
        });
        this.customerAircraftsService.import(this.data.aircrafts).subscribe(() => {
            this.dialogRef.close();
        });

    }
}
