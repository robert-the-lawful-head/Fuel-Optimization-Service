import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';


@Component({
    selector: 'app-customer-action-status',
    templateUrl: './customer-action-status.component.html',
    styleUrls: ['./customer-action-status.component.scss'],
})
export class CustomerActionStatusComponent {
    @Input() customerActionStatusEmailRequired: string;
    @Input() customerActionStatusSetupRequired: string;
    @Input() customerActionStatusTopCustomer: string;

    constructor(
    ) { }
  
}
