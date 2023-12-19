import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

// Services
import { RampfeesService } from '../../../services/rampfees.service';
// Components
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { SnackBarService } from 'src/app/services/utils/snackBar.service';
import { rampfeeCategoryType } from 'src/app/enums/ramp-fee.enum';

@Component({
    selector: 'app-ramp-fees-category',
    styleUrls: ['./ramp-fees-category.component.scss'],
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
        private snackBarService: SnackBarService
    ) {}

    ngOnInit() {
        this.refreshData();
    }

    public refreshData() {
        this.rampFeesForCategory = [];
        this.rampFees.forEach((fee) => {
            if (this.categoryTypes.indexOf(fee.categoryType) > -1) {
                fee.isWaivedNegative = false;
                fee.isCategoryValueNegative = false;
                fee.isFeeNegative = false;
                fee.isCategoryMinValueGrater = false;
                fee.requiresUpdate = false;

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
        fee.isWaivedNegative = false;
        fee.isCategoryValueNegative = false;
        fee.isFeeNegative = false;
        fee.isCategoryMinValueGrater = false;
        fee.requiresUpdate = true;

        if (fee.waived < 0)
            fee.isWaivedNegative = true;

        if (fee.price < 0)
            fee.isFeeNegative = true;

        if (fee.categoryMinValue >= fee.categoryMaxValue)
            fee.isCategoryMinValueGrater = true;

        if (fee.categoryMinValue < 0 || fee.categoryMaxValue < 0)
            fee.isCategoryValueNegative = true;

        fee.requiresUpdate = !fee.isWaivedNegative && !fee.isCategoryValueNegative && !fee.isFeeNegative && !fee.isCategoryMinValueGrater;
        this.rampFeeFieldChanged.emit();
    }

    public deleteRampFee(fee) {
        const dialogRef = this.deleteRampFeeDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'ramp fee', item: fee, includeThis: true },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.rampFeesService.remove(result.item).subscribe(() => {
                this.rampFeeDeleted.emit();
                this.snackBarService.showSuccessSnackBar(`Custom ramp fee deleted`);
            });
        });
    }
    getLabel(fee: any): string{

        if(this.categoryTypes.includes(1))
            return fee.sizeDescription;

        if(fee.categoryType == rampfeeCategoryType.aicraft)
            return ` ${fee.aircraftMake} ${fee.aircraftModel}`;

        let label = fee.categoryDescription;

        if(fee.categoryType == rampfeeCategoryType.weightRange)
            label += ` ${fee.categoryMinValue} - ${fee.categoryMaxValue}`;

        if(fee.categoryType == rampfeeCategoryType.wingspan)
            label += ` ${fee.categoryMinValue} - ${fee.categoryMaxValue}`;

        if(fee.categoryType == rampfeeCategoryType.tailnumber)
            label += ` ${fee.categoryStringValue}`;

        return label;
    }
}
