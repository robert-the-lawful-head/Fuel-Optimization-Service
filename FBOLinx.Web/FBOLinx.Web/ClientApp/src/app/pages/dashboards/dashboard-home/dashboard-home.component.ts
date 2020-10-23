import { Component } from '@angular/core';
import { Router } from '@angular/router';

// Services
import { SharedService } from '../../../layouts/shared-service';

@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss'],
})
export class DashboardHomeComponent {
  constructor(
    private router: Router,
    private sharedService: SharedService
  ) {
    if (this.sharedService.currentUser.role === 3) {
      if (!this.sharedService.currentUser.impersonatedRole) {
        this.router.navigate(['/default-layout/groups/']);
      }
      if (this.sharedService.currentUser.impersonatedRole === 2) {
        this.router.navigate(['/default-layout/fbos/']);
      }
      if (this.sharedService.currentUser.impersonatedRole === 1) {
        this.router.navigate(['/default-layout/dashboard-fbo/']);
      }
    } else if (this.sharedService.currentUser.role === 2) {
      if (!this.sharedService.currentUser.impersonatedRole) {
        this.router.navigate(['/default-layout/fbos/']);
      }
      if (this.sharedService.currentUser.impersonatedRole === 1) {
        this.router.navigate(['/default-layout/dashboard-fbo/']);
      }
    } else if (this.sharedService.currentUser.role === 5) {
      this.router.navigate(['/default-layout/dashboard-csr/']);
    } else {
      this.router.navigate(['/default-layout/dashboard-fbo/']);
    }
  }
}
