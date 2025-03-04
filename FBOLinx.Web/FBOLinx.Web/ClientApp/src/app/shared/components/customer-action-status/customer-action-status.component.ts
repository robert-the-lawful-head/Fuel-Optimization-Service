import { Component, Input } from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';


@Component({
    selector: 'app-customer-action-status',
    templateUrl: './customer-action-status.component.html',
    styleUrls: ['./customer-action-status.component.scss'],
})
export class CustomerActionStatusComponent {
    @Input() customerActionStatusEmailRequired: string;
    @Input() customerActionStatusSetupRequired: string;
    @Input() customerActionStatusTopCustomer: string;
    @Input() toolTipEmailRequired: string;
    @Input() toolTipSetupRequired: string;
    @Input() toolTipTopCustomer: string;
    @Input() moreThan2Badges: string;

    constructor(
    ) { }
  
}
