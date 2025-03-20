import { Component, Input, OnInit } from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { SharedService } from 'src/app/layouts/shared-service';
import { GroupsService } from 'src/app/services/groups.service';
import { ManageConfirmationComponent } from 'src/app/shared/components/manage-confirmation/manage-confirmation.component';
import { NotificationComponent } from 'src/app/shared/components/notification/notification.component';

@Component({
    selector: 'app-group-fbos-grid',
    templateUrl: './group-fbos-grid.component.html',
    styleUrls: ['./group-fbos-grid.component.scss'],
})
export class GroupFbosGridComponent implements OnInit {
    @Input() dataSource: any;
    @Input() isNetworkFbo: boolean;    

    displayedColumns = [
        'icao',
        'fbo',
        'pricingExpired',
        'needAttentionCustomers',
        'lastLogin',
        'users',
        'quotes30Days',
        'orders30Days',
        'accountExpired',
        'accountType',
        'actions',
    ];

    searchValue = '';
    tableLocalStorageFilterKey = 'conductor-group-grid-filter';

    constructor(
        private sharedService: SharedService,
        private notification: MatDialog,
        private manageDialog: MatDialog,
        private groupsService: GroupsService,
        private router: Router,

    ) {}

    ngOnInit()
    {
        
        
    }

    editFBO(fbo: any) {
        this.router.navigate(['/default-layout/fbos/' + fbo.oid]);
    }
    manageFBO(fbo: any) {
        if (!fbo.active) {
            this.notification.open(NotificationComponent, {
                data: {
                    text: "You can't manage an inactive fbo!",
                    title: 'Inactive fbo',
                },
            });
        } else {
            const dialogRef = this.manageDialog.open(
                ManageConfirmationComponent,
                {
                    autoFocus: false,
                    data: {
                        description:
                            'This will temporarily switch your account to a primary user for this FBO.  Would you like to continue?',
                        fboId: fbo.oid,
                        title: 'Manage FBO?',
                    },
                    width: '450px',
                }
            );

            dialogRef.afterClosed().subscribe(async (result) => {
                if (!result) {
                    return;
                }

                localStorage.setItem(
                    this.tableLocalStorageFilterKey,
                    this.searchValue
                );

                const isSingleSourceFboResponse = this.groupsService.isGroupFboSingleSource(fbo.icao);
                var isSingleSourceFbo = await lastValueFrom(isSingleSourceFboResponse);

                var hostName = window.location.hostname;
                var href = window.location.href;

                if (hostName === 'support.fbolinx.com') {
                    href = href.replace(hostName, "www.fbolinx.com");
                }
                else if (hostName === 'support-qa-app.fbolinx.com') {
                    href = href.replace(hostName, "qa-app.fbolinx.com");
                }
                //else if (hostName === 'st-support.fbolinx.com') {
                //    href = href.replace(hostName, "st-app.fbolinx.com");
                //}

                href = href.replace(window.location.pathname, "/outside-the-gate-layout/auth?token=" + this.sharedService.currentUser.authToken);

                const newUrl = href + "&groupId=" + fbo.groupId + "&fboId=" + fbo.oid + "&accountType=" + fbo.accountType + "&icao=" + fbo.icao + "&isSingleSourceFbo=" + isSingleSourceFbo + "&isNetworkFbo=" + this.isNetworkFbo;

                if (window.location.href !== newUrl) {
                    this.router.navigate([]).then(result => {
                        window.open(newUrl, '_blank');
                    });
                }
                //this.router.navigate([]).then(result => { window.open(window.location.href + "?conductor=1&groupId=" + fbo.groupId + "&fboiId=" + fbo.oid + "&accountType=" + fbo.accountType + "&icao=" + fbo.icao + "&isSingleSourceFbo=" + isSingleSourceFbo, '_blank'); });
            });
        }
    }
}
