import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { JetNetInformationComponent } from '../jetnet-information/jetnet-information.component';

@Component({
    selector: 'app-jetnet-search',
    styleUrls: ['./jetnet-search.component.scss'],
    templateUrl: './jetnet-search.component.html',
})
export class JetNetSearchComponent { 
    constructor(private jetNetInformationDialog: MatDialog,) { }

    public tailNumberSearchChanged(tailNumber: any) {
        if (tailNumber.currentTarget.value.trim() != "") {
            const dialogRef = this.jetNetInformationDialog.open(JetNetInformationComponent, {
                width: '1100px',
                data: tailNumber.currentTarget.value.trim()
            });
            dialogRef
                .afterClosed()
                .subscribe((result: any) => {

                });
        }
    }
}
