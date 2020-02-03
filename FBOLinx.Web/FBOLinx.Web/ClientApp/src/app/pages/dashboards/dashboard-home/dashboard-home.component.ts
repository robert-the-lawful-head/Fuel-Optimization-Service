import { Component } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-dashboard-home',
    templateUrl: './dashboard-home.component.html',
    styleUrls: ['./dashboard-home.component.scss']
})
/** dashboard-home component*/
export class DashboardHomeComponent {
    /** dashboard-home ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private sharedService: SharedService) {
        if (this.sharedService.currentUser.role == 3 || this.sharedService.currentUser.role == 2)
            this.router.navigate(['/default-layout/fbos/']);
        else
            this.router.navigate(['/default-layout/dashboard-fbo/']);
    }
}
