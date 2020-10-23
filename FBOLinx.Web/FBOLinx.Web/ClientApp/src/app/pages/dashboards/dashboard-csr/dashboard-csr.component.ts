import { Component } from '@angular/core';

// Services
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'CSR Dashboard',
        link: '/default-layout/dashboard-csr',
    },
];

@Component({
    selector: 'app-dashboard-csr',
    templateUrl: './dashboard-csr.component.html',
    styleUrls: ['./dashboard-csr.component.scss'],
})
export class DashboardCsrComponent {
    public breadcrumb: any[] = BREADCRUMBS;
    public pageTitle = 'CSR Dashboard';

    constructor(private sharedService: SharedService) {
        this.sharedService.titleChange(this.pageTitle);
    }
}
