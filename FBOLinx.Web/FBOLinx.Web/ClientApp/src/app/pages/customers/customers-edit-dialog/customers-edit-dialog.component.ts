import { SharedService } from './../../../layouts/shared-service';
import {
    Component,
    EventEmitter,
    Inject,
    Input,
    OnInit,
    Output,
} from '@angular/core';
import {
    MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
    MatLegacyDialog as MatDialog,
    MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';

@Component({
    selector: 'app-customers-edit-dialog',
    templateUrl: './customers-edit-dialog.component.html'
})
export class CustomersEditDialogComponent implements OnInit {
    public customerInfoByGroupId: number;

    constructor(@Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<CustomersEditDialogComponent>,
    ) {
        this.customerInfoByGroupId = data.customerInfoByGroupId;
    }

    ngOnInit() {

    }
}
