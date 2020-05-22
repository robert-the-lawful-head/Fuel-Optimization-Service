import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    Inject,
} from "@angular/core";

// Services
import { AircraftsService } from "../../../services/aircrafts.service";
import { CustomeraircraftsService } from "../../../services/customeraircrafts.service";
import {
    MAT_DIALOG_DATA,
    MatDialogRef,
    MatDialog,
} from "@angular/material/dialog";
import { DialogConfirmAircraftDeleteComponent } from "../customer-aircrafts-confirm-delete-modal/customer-aircrafts-confirm-delete-modal.component";
import { NotificationComponent } from '../../../shared/components/notification/notification.component';

@Component({
    selector: "app-customer-aircrafts-edit",
    templateUrl: "./customer-aircrafts-edit.component.html",
    styleUrls: ["./customer-aircrafts-edit.component.scss"],
})
export class CustomerAircraftsEditComponent implements OnInit {
    @Output() saveEditClicked = new EventEmitter<any>();
    @Output() cancelEditClicked = new EventEmitter<any>();
    @Input() customerAircraftInfo: any;

    // Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public pricingTemplates: Array<any>;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        private dialogRef: MatDialogRef<CustomerAircraftsEditComponent>,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private dialogAircraftDeleteRef: MatDialog,
        private notification: MatDialog,
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
        this.customerAircraftsService
            .update(this.customerAircraftInfo)
            .subscribe((data: any) => {
                this.dialogRef.close(data);
            });
    }

    public cancelEdit() {
        this.data = null;
        this.dialogRef.close();
    }

    public ConfirmDelete(data: any) {
        if (this.data.disableDelete) {
            this.notification.open(
                NotificationComponent,
                {
                    data: {
                        title: 'Fuelerlinx Customer Aircraft',
                        text: 'You can\'t delete fuelerlinx account tails.'
                    }
                }
            )
        } else {
            const dialogRef = this.dialogAircraftDeleteRef.open(
                DialogConfirmAircraftDeleteComponent,
                {
                    data,
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (result === "cancel") {
                } else if (result.oid) {
                    result.toDelete = true;
                    this.dialogRef.close(result);
                }
            });
        }
    }
}
