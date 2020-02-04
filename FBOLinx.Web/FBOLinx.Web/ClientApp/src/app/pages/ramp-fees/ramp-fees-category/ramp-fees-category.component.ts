import { Component, EventEmitter, Input, Output, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { RampfeesService } from '../../../services/rampfees.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

@Component({
    selector: 'app-ramp-fees-category',
    templateUrl: './ramp-fees-category.component.html',
    styleUrls: ['./ramp-fees-category.component.scss']
})
/** ramp-fees-category component*/
export class RampFeesCategoryComponent implements OnInit {

    @Output() rampFeeFieldChanged = new EventEmitter<any>();
    @Output() rampFeeDeleted = new EventEmitter<any>();
    @Input() rampFees: any;
    @Input() categoryTitle: string;
    @Input() categoryTypes: Array<number>;
    @Input() supportedValues: Array<number> = [];
    @Input() sortedValues: Array<number> = [];
    @Input() expirationDate: any;

    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public rampFeesForCategory: Array<any> = [];
    public tmpArray: Array<any> = [];

    /** ramp-fees-category ctor */
    constructor(private aircraftsService: AircraftsService,
        private rampFeesService: RampfeesService,
        private sharedService: SharedService,
        public deleteRampFeeDialog: MatDialog    ) {

    }

    ngOnInit() {
        this.aircraftsService.getAll().subscribe((data: any) => this.aircraftTypes = data);
        this.rampFees.forEach(fee => {
            if (this.categoryTypes.indexOf(fee.categoryType) > -1) {
                if (this.supportedValues.length == 0 || this.supportedValues.indexOf(fee.categoryMinValue) > -1) {
                    this.rampFeesForCategory.push(fee);
                }
                this.tmpArray.push(fee);
            }
        });

        this.tmpArray.forEach(fee => {
            if (fee.categoryMinValue > 0 && this.sortedValues.indexOf(fee.categoryMinValue) > -1) {
                this.rampFeesForCategory.splice(this.sortedValues.indexOf(fee.categoryMinValue), 1, fee);
            }
        });
    }

    public rampFeeRequiresUpdate(fee) {
        fee.requiresUpdate = true;
        this.rampFeeFieldChanged.emit();

    }

    public deleteRampFee(fee) {
        const dialogRef = this.deleteRampFeeDialog.open(DeleteConfirmationComponent, {
            data: { item: fee, description: 'ramp fee'}
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            this.rampFeesService.remove(result.item).subscribe((data: any) => {
                this.rampFeeDeleted.emit();
            });
        });
    }
}
