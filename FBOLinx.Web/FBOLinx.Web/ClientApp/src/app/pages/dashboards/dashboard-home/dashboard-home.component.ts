import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

// Services
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-dashboard-home',
    styleUrls: ['./dashboard-home.component.scss'],
    templateUrl: './dashboard-home.component.html',
})
export class DashboardHomeComponent {
    idParam: string;

    constructor(private router: Router, private sharedService: SharedService, private route: ActivatedRoute) {
        this.route.queryParamMap.subscribe((params) => {
            this.idParam = params.get('id');
        });

        if (this.sharedService.currentUser.role === 3) {
            if (!this.sharedService.currentUser.impersonatedRole) {
                this.router.navigate(['/default-layout/groups/']);
            }
            if (this.sharedService.currentUser.impersonatedRole === 2) {
                this.router.navigate(['/default-layout/fbos/']);
            }
            if (this.sharedService.currentUser.impersonatedRole === 1) {
                if (this.idParam != "")
                    this.router.navigate(['/default-layout/fuelreqs'], { queryParams: { id: this.idParam } });
                else
                    this.router.navigate(['/default-layout/dashboard-fbo-updated/']);
            }
        } else if (this.sharedService.currentUser.role === 2) {
            if (!this.sharedService.currentUser.impersonatedRole) {
                this.router.navigate(['/default-layout/fbos/']);
            }
            if (this.sharedService.currentUser.impersonatedRole === 1) {
                this.router.navigate(['/default-layout/dashboard-fbo-updated/']);
            }
        } else if (this.sharedService.currentUser.role === 5) {
            this.router.navigate(['/default-layout/dashboard-csr/']);
        } else if (this.sharedService.currentUser.role === 6) {
            if (this.idParam != "")
                this.router.navigate(['/default-layout/fuelreqs'], { queryParams: { id: this.idParam } });
            else
                this.router.navigate(['/default-layout/fuelreqs']);
        } else {
            this.router.navigate(['/default-layout/dashboard-fbo-updated/']);
        }
    }
}
