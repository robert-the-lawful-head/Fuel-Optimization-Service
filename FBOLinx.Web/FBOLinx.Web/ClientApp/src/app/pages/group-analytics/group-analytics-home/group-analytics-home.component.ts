import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FbosService } from '../../../services/fbos.service';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';

import { GroupAnalyticsGenerateDialogComponent } from '../group-analytics-generate-dialog/group-analytics-generate-dialog.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'Group Analytics',
        link: '',
    },
];

@Component({
    selector: 'app-group-analytics-home',
    templateUrl: './group-analytics-home.component.html',
    styleUrls: [ './group-analytics-home.component.scss' ],
})
export class GroupAnalyticsHomeComponent implements OnInit {
    pageTitle = 'Group Analytics';
    breadcrumb: any[] = BREADCRUMBS;

    customers: any[];
    fbos: any[];

    constructor(
        private reportDialog: MatDialog,
        private sharedService: SharedService,
        private fbosService: FbosService,
        private customerInfoByGroupService: CustomerinfobygroupService
    ) {
        this.fbosService
            .getForGroup(this.sharedService.currentUser.groupId)
            .subscribe((fbos: any[]) => {
                this.fbos = fbos;
            });
    }

    ngOnInit() {
        this.loadCustomers();
    }

    loadCustomers() {
        this.customerInfoByGroupService.getCustomersByGroup(this.sharedService.currentUser.groupId)
            .subscribe((customers: any[]) => {
                this.customers = customers;
            });
    }

    onGenerate() {
        this.reportDialog.open(
            GroupAnalyticsGenerateDialogComponent,
            {
                data: {
                    customers: this.customers
                },
                width: '500px',
                autoFocus: false,
                panelClass: 'group-analytics-dialog'
            },
        );
    }
}
