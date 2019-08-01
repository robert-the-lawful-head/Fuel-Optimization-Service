import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-needs-attention',
    templateUrl: './needs-attention.component.html',
    styleUrls: ['./needs-attention.component.scss']
})
/** needs-attention component*/
export class NeedsAttentionComponent {
    public customersWithoutMargins: any[];

    /** needs-attention ctor */
    constructor(private sharedService: SharedService, private customerInfoByGroupService: CustomerinfobygroupService) {
        this.loadCustomersWithoutMargins();
    }

    //Private Methods
    private loadCustomersWithoutMargins() {
        this.customerInfoByGroupService
            .getCustomersWithoutMargins(this.sharedService.currentUser.groupId, this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                    this.customersWithoutMargins = data;
                });
    }
}
