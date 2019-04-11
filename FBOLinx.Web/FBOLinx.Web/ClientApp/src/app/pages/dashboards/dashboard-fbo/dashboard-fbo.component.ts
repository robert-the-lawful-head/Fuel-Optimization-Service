import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/'
    },
    {
        title: 'Dashboard',
        link: '#/default-layout/dashboard-fbo'
    }
];

@Component({
    selector: 'app-dashboard-fbo',
    templateUrl: './dashboard-fbo.component.html',
    styleUrls: ['./dashboard-fbo.component.scss']
})
/** dashboard-fbo component*/
export class DashboardFboComponent {

    public pageTitle: string = 'Dashboard';
    public breadcrumb: any[] = BREADCRUMBS;
    public statisticsOptions: any = {useCard: true};

    /** dashboard-fbo ctor */
    constructor(private sharedService: SharedService) {
        this.sharedService.emitChange(this.pageTitle);
    }
}
