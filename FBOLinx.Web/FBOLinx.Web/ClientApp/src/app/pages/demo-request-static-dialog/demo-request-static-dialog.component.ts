import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountType } from 'src/app/enums/user-role';
import { SharedService } from 'src/app/layouts/shared-service';
import { fboChangedEvent } from 'src/app/constants/sharedEvents';

@Component({
  selector: 'app-demo-request-static-dialog',
  templateUrl: './demo-request-static-dialog.component.html',
  styleUrls: ['./demo-request-static-dialog.component.scss']
})
export class DemoRequestStaticDialogComponent implements OnInit {
    public isStaticModalVisibleVisible: boolean = false;
    private whitelistUrl = ['/default-layout/dashboard-fbo-updated'];
    constructor(private router: Router,
        private sharedService: SharedService) { }

    ngOnInit() {
        this.sharedService.changeEmitted$.subscribe((message) => {
            if (message === fboChangedEvent) {
                this.isStaticModalVisibleVisible = this.setIsStaticModalVisibleVisible();
            }
        });
    }

    openRequestDemo() {
        window.open('https://outlook.office365.com/owa/calendar/FBOLinxSales@fuelerlinx.com/bookings/', '_blank').focus();
    }
    setIsStaticModalVisibleVisible(): boolean {
        if (this.whitelistUrl.includes(this.router.url) &&
        this.sharedService.currentUser.accountType !=  AccountType.Freemium) return true;
        return false;
    }
}
