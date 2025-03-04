import { Component } from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { JetNetInformationComponent } from '../jetnet-information/jetnet-information.component';

@Component({
    selector: 'app-jetnet-search',
    styleUrls: ['./jetnet-search.component.scss'],
    templateUrl: './jetnet-search.component.html',
})
export class JetNetSearchComponent { 
    constructor(private jetNetInformationDialog: MatDialog,) { }
    private isLoading: boolean = false;

    public tailNumberSearchChanged(tailNumber: any) {
        if (!this.isLoading) {
            this.isLoading = true;
        if (tailNumber.toString() == "[object PointerEvent]")
            tailNumber.currentTarget.value = (document.getElementById("tailNumber") as HTMLInputElement).value;

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
            this.isLoading = false;
        }
    }
}
