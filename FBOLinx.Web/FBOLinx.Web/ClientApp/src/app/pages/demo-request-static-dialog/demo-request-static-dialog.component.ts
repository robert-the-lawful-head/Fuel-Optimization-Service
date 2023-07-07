import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountType } from 'src/app/enums/user-role';
import { SharedService } from 'src/app/layouts/shared-service';
import { fboChangedEvent } from 'src/app/constants/sharedEvents';
import { urls } from 'src/app/constants/externalUrlsConstants';

@Component({
  selector: 'app-demo-request-static-dialog',
  templateUrl: './demo-request-static-dialog.component.html',
  styleUrls: ['./demo-request-static-dialog.component.scss']
})
export class DemoRequestStaticDialogComponent implements OnInit {
    public isStaticModalVisible: boolean;
    private whitelistUrl = ['/default-layout/dashboard-fbo-updated'];
    constructor(private router: Router,
        private sharedService: SharedService) { }

    ngOnInit() {
        this.isStaticModalVisible = this.getIsStaticModalVisible();
        this.sharedService.changeEmitted$.subscribe((message) => {
            if (message === fboChangedEvent) {
                this.isStaticModalVisible = this.getIsStaticModalVisible();
            }
        });
    }

    openRequestDemo() {
        window.open(urls.demoRequestUrl, '_blank').focus();
    }
    getIsStaticModalVisible(): boolean {
        if (this.whitelistUrl.includes(this.router.url) &&
        this.sharedService.currentUser.accountType ==  AccountType.Freemium) return true;
        return false;
    }
}
