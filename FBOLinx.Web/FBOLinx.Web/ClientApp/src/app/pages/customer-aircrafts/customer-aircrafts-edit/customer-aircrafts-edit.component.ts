import { SharedService } from 'src/app/layouts/shared-service';

import {
    Component,
    EventEmitter,
    Inject,
    Input,
    OnInit,
    Output,
} from '@angular/core';
import {
    MAT_DIALOG_DATA,
    MatDialog,
    MatDialogRef,
} from '@angular/material/dialog';

// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { DialogConfirmAircraftDeleteComponent } from '../customer-aircrafts-confirm-delete-modal/customer-aircrafts-confirm-delete-modal.component';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-customer-aircrafts-edit',
    styleUrls: ['./customer-aircrafts-edit.component.scss'],
    templateUrl: './customer-aircrafts-edit.component.html',
})
export class CustomerAircraftsEditComponent implements OnInit {
    @Output() saveEditClicked = new EventEmitter<any>();
    @Output() cancelEditClicked = new EventEmitter<any>();
    @Input() customerAircraftInfo: any;

    // Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public pricingTemplates: Array<any>;
    public customerInfoByGroupId : any ;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        private dialogRef: MatDialogRef<CustomerAircraftsEditComponent>,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private dialogAircraftDeleteRef: MatDialog ,
        private sharedService : SharedService


    ) {}

    ngOnInit() {
        if (this.data) {
            this.aircraftsService.get(this.data).subscribe((data: any) => {

                this.customerAircraftInfo = data;

            });
        }

        this.aircraftsService
            .getAll()
            .subscribe((data: any) => (this.aircraftTypes = data));
        this.aircraftsService
            .getAircraftSizes()
            .subscribe((data: any) => (this.aircraftSizes = data));

        }

    public saveEdit() {
        console.log(this.data.customerGroupId)
        this.customerAircraftsService
            .update(this.customerAircraftInfo , this.sharedService.currentUser.oid , this.data.customerGroupId )
            .subscribe((data: any) => {
                this.dialogRef.close(data);
            });
    }

    public cancelEdit() {
        this.data = null;
        this.dialogRef.close();
    }

    public ConfirmDelete(data: any) {
        const dialogRef = this.dialogAircraftDeleteRef.open(
            DialogConfirmAircraftDeleteComponent,
            {
                data,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result === 'cancel') {
            } else if (result && result.oid) {
                result.toDelete = true;
                this.dialogRef.close(result);
            }
        });
    }
}
