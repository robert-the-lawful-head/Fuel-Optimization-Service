import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

import { AssociationsService } from '../../../services/associations.service';

@Component({
    selector: 'app-associations-dialog-new-association',
    styleUrls: ['./associations-dialog-new-association.component.scss'],
    templateUrl: './associations-dialog-new-association.component.html',
})
export class AssociationsDialogNewAssociationComponent {
    type: string;
    dataSources: any = {};
    data: any = {};

    constructor(
        public dialogRef: MatDialogRef<AssociationsDialogNewAssociationComponent>,
        private associationsService: AssociationsService,
    ) {

    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onSaveClick(): void {
        this.data.oid = 0;
        this.associationsService.add(this.data).subscribe((data: any) => {
            this.dialogRef.close({
                data
            });
        });
    }
}
