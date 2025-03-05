import { Component, Input, OnInit } from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { Router } from '@angular/router';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';
import {
    accountTypeChangedEvent,
    fboChangedEvent,
} from 'src/app/constants/sharedEvents';
import { UserRole } from 'src/app/enums/user-role';
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

    ngOnInit() {}
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

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.managerGroupId,
                    this.sharedService.currentUser.groupId
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.groupId,
                    fbo.groupId
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.impersonatedrole,
                    UserRole.Primary
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.conductorFbo,
                    true
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.fboId,
                    fbo.oid
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.icao,
                    fbo.icao
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.accountType,
                    fbo.accountType
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.isNetworkFbo,
                    this.isNetworkFbo
                );

                var isSingleSourceFbo = await this.groupsService
                    .isGroupFboSingleSource(fbo.icao)
                    .toPromise();

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.isSingleSourceFbo,
                    isSingleSourceFbo
                );

                this.sharedService.emitChange(fboChangedEvent);
                this.sharedService.emitChange(accountTypeChangedEvent);

                this.router.navigate([
                    '/default-layout/dashboard-fbo-updated/',
                ]);
            });
        }
    }
}
