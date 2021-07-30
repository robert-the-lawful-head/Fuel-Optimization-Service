import { Component, EventEmitter, Input, OnInit, Output, } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

// Services
import { RampfeesService } from '../../../services/rampfees.service';
// Components
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

@Component({
    selector: 'app-ramp-fees-category',
    styleUrls: [ './ramp-fees-category.component.scss' ],
    templateUrl: './ramp-fees-category.component.html',
})
export class RampFeesCategoryComponent implements OnInit {
    @Output() rampFeeFieldChanged = new EventEmitter<any>();
    @Output() rampFeeDeleted = new EventEmitter<any>();
    @Input() aircraftTypes: any[];
    @Input() rampFees: any;
    @Input() categoryTitle: string;
    @Input() categoryTypes: Array<number>;
    @Input() supportedValues: Array<number> = [];
    @Input() sortedValues: Array<number> = [];
    @Input() expirationDate: any;

    public aircraftSizes: Array<any>;
    public rampFeesForCategory: Array<any> = [];
    public tmpArray: Array<any> = [];

    constructor(
        private rampFeesService: RampfeesService,
        private deleteRampFeeDialog: MatDialog,
        private snackBar: MatSnackBar
    ) {
    }

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.rampFeesForCategory = [];
        this.rampFees.forEach((fee) => {
            if (this.categoryTypes.indexOf(fee.categoryType) > -1) {
                if (
                    this.supportedValues.length === 0 ||
                    this.supportedValues.indexOf(fee.categoryMinValue) > -1
                ) {
                    this.rampFeesForCategory.push(fee);
                }
                this.tmpArray.push(fee);
            }
        });

        this.tmpArray.forEach((fee) => {
            if (
                fee.categoryMinValue > 0 &&
                this.sortedValues.indexOf(fee.categoryMinValue) > -1
            ) {
                this.rampFeesForCategory.splice(
                    this.sortedValues.indexOf(fee.categoryMinValue),
                    1,
                    fee
                );
            }
        });
    }

    public rampFeeRequiresUpdate(fee) {
        fee.requiresUpdate = true;
        this.rampFeeFieldChanged.emit();
    }

    public deleteRampFee(fee) {
        const dialogRef = this.deleteRampFeeDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'ramp fee', item: fee },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.rampFeesService.remove(result.item).subscribe(() => {
                this.rampFeeDeleted.emit();
                this.snackBar.open(`${ result.item.aircraftMake } ${ result.item.aircraftModel } is deleted`, '', {
                    duration: 2000,
                    panelClass: [ 'blue-snackbar' ],
                });
            });
        });
    }
}
