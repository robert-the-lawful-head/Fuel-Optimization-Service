import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { UserRole } from 'src/app/enums/user-role';

import { SharedService } from '../../../layouts/shared-service';
import { accountTypeChangedEvent, fboChangedEvent } from '../../../constants/sharedEvents';
import { FbopricesService } from '../../../services/fboprices.service';
// Services
import { FbosService } from '../../../services/fbos.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { ManageConfirmationComponent } from '../../../shared/components/manage-confirmation/manage-confirmation.component';
import { PricingExpiredNotificationGroupComponent } from '../../../shared/components/pricing-expired-notification-group/pricing-expired-notification-group.component';
// Components
import { FbosDialogNewFboComponent } from '../fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
import { FbosGridNewFboDialogComponent } from '../fbos-grid-new-fbo-dialog/fbos-grid-new-fbo-dialog.component';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';
import { GroupsService } from 'src/app/services/groups.service';
import { ManageFboGroupsService } from 'src/app/services/managefbo.service';
import { GroupFboViewModel } from 'src/app/models/groups';

@Component({
    selector: 'app-fbos-grid',
    styleUrls: ['./fbos-grid.component.scss'],
    templateUrl: './fbos-grid.component.html',
})
export class FbosGridComponent implements OnInit {
    // Input/Output Bindings
    @Output() editFboClicked = new EventEmitter<any>();
    @Input() fbosData: Array<any>;
    @Input() groupInfo: any;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    // Public Members
    public pageTitle = 'FBOs';
    public fbosDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['icao', 'fbo', 'active', 'manage'];
    public airportData: Array<any>;
    public resultsLength = 0;
    public canManageFbo = false;
    public isDeleting: boolean;
    public searchValue = '';

    public pageIndexFbos = 0;
    public pageSizeFbos = 25;

    public tableSortFbos = 'icao';
    public tableSortOrderFbos = 'asc';

    public groupsFbosData: GroupFboViewModel;

    constructor(
        private newFboDialog: MatDialog,
        private fboService: FbosService,
        private fboPricesService: FbopricesService,
        private sharedService: SharedService,
        private deleteFboDialog: MatDialog,
        private manageFboDialog: MatDialog,
        private snackBar: MatSnackBar,
        private router: Router,
        private checkPricingDialog: MatDialog,
        private groupsService: GroupsService,
        private manageFboGroupsService: ManageFboGroupsService
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.canManageFbo = [UserRole.Conductor, UserRole.GroupAdmin].includes(
            this.sharedService.currentUser.role
        );

        if (this.sharedService.currentUser.role === UserRole.Conductor) {
            this.displayedColumns = [
                'icao',
                'fbo',
                'price',
                'active',
                'edit',
                'delete',
            ];
        } else {
            this.displayedColumns = ['icao', 'fbo', 'price', 'active', 'edit'];
        }

        this.groupsService
            .groupsAndFbos()
            .subscribe((data: GroupFboViewModel) => { this.groupsFbosData = data; });
    }

    ngOnInit() {
        if (!this.fbosData) {
            return;
        }
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.refreshTable();
        if (this.sharedService.currentUser.role !== 3) {
            const remindMeLaterFlag = localStorage.getItem(
                'pricingExpiredNotification'
            );
            const noThanksFlag = sessionStorage.getItem(
                'pricingExpiredNotification'
            );
            if (noThanksFlag) {
                return;
            }

            if (
                remindMeLaterFlag &&
                moment(new Date(moment().format('L'))) <=
                    moment(new Date(remindMeLaterFlag))
            ) {
                return;
            }

            this.checkExistingPrices();
        }

        this.paginator.pageIndex = 0;
    }

    public deleteRecord(record) {
        const dialogRef = this.deleteFboDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'FBO', item: record, includeThis: true },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.isDeleting = true;
            this.fboService.remove(record).subscribe(
                () => {
                    this.fbosData.splice(this.fbosData.indexOf(record), 1);
                    this.sort.sortChange.subscribe(() => {
                        this.paginator.pageIndex = 0;
                    });
                    this.refreshTable();
                    this.isDeleting = false;
                    this.snackBar.open(record.fbo + ' is deleted', '', {
                        duration: 2000,
                        panelClass: ['blue-snackbar'],
                    });
                },
                () => {
                    this.isDeleting = false;
                }
            );
        });
    }

    public editRecord(record) {
        this.editFboClicked.emit(record);
    }

    public newRecord() {
        var groupId = 0;
        if (this.sharedService.isManagingGroup())
            groupId = this.sharedService.currentUser.groupId;
        else if (this.groupInfo)
            groupId = this.groupInfo.oid;
        if (groupId > 0) {
            const dialogRef = this.newFboDialog.open(
                FbosDialogNewFboComponent,
                {
                    data: {
                        groupId: groupId,
                        initialSetupPhase: true,
                    },
                    width: '450px',
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (result) {
                    this.fbosData.push(result);

                    this.refreshTable();
                    this.snackBar.open(result.fbo + ' is created', '', {
                        duration: 3000,
                        panelClass: ['blue-snackbar'],
                    });
                    sessionStorage.setItem('isNewFbo', 'yes');
                    this.editRecord(result);
                }
            });
        } else {
            const dialogRef = this.newFboDialog.open(
                FbosGridNewFboDialogComponent,
                {
                    data: {},
                    width: '450px',
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (result) {
                    this.fbosData.push(result);
                    this.refreshTable();
                    this.snackBar.open(result.fbo + ' is created', '', {
                        duration: 3000,
                        panelClass: ['blue-snackbar'],
                    });
                    sessionStorage.setItem('isNewFbo', 'yes');
                    this.router.navigate([
                        '/default-layout/fbos/' + result.oid,
                    ]);
                }
            });
        }
    }

    public refreshTable() {
        this.fbosDataSource = new MatTableDataSource(this.fbosData);
        this.fbosDataSource.sort = this.sort;
        this.fbosDataSource.paginator = this.paginator;
        this.resultsLength = this.fbosData.length;
    }

    public applyFilter(filterValue: string) {
        this.fbosDataSource.filter = filterValue.trim().toLowerCase();
    }

    public manageFBO(fbo, $event) {
        if (
            ($event !== null &&
                ($event.srcElement.nodeName.toLowerCase() === 'button' ||
                    $event.srcElement.nodeName.toLowerCase() === 'select' ||
                    ($event.srcElement.nodeName.toLowerCase() === 'input' &&
                        $event.srcElement.getAttribute('type') ===
                            'checkbox'))) ||
            !this.canManageFbo
        ) {
            $event.stopPropagation();
            return;
        }

        const dialogRef = this.manageFboDialog.open(
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

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            if (this.sharedService.currentUser.role != 3) {
                this.fboService.updateLastLogin(fbo.oid).subscribe((data: any) => {
                    this.completeManageProcess(fbo);
                });
            }
            else
                this.completeManageProcess(fbo);
        });
    }

    public checkExistingPrices() {
        this.fboPricesService
            .checkFboExpiredPricingGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: any) => {
                if (this.sharedService.currentUser.role != 6 && this.sharedService.currentUser.fboId > 0 && data && data.length) {
                    const dialogRef = this.checkPricingDialog.open(
                        PricingExpiredNotificationGroupComponent,
                        {
                            data,
                        }
                    );
                    dialogRef.afterClosed().subscribe((result) => {
                        if (result && result.fboId) {
                            this.sharedService.currentUser.impersonatedRole = 1;
                            this.sharedService.currentUser.fboId = result.fboId;
                            localStorage.setItem(
                                'fboId',
                                this.sharedService.currentUser.fboId.toString()
                            );
                            this.sharedService.emitChange(fboChangedEvent);
                            this.router.navigate([
                                '/default-layout/dashboard-fbo-updated/',
                            ]);
                        }
                    });
                }
            });
    }

    async completeManageProcess(fbo) {
        localStorage.setItem(
            'managerGroupId',
            this.sharedService.currentUser.groupId.toString()
        );
        this.sharedService.currentUser.managerGroupId =
            this.sharedService.currentUser.groupId;

        localStorage.setItem('groupId', fbo.groupId.toString());
        this.sharedService.currentUser.groupId = fbo.groupId;

        localStorage.setItem('impersonatedrole', '1');
        this.sharedService.currentUser.impersonatedRole = 1;

        localStorage.setItem('fboId', fbo.oid.toString());
        this.sharedService.currentUser.fboId = fbo.oid;

        this.sharedService.currentUser.icao = fbo.icao;

        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.accountType,fbo.accountType);

        if (this.groupsFbosData == undefined) {
            this.groupsService
                .groupsAndFbos()
                .subscribe((data: GroupFboViewModel) => {
                    this.groupsFbosData = data;
                    this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.isNetworkFbo, this.manageFboGroupsService.isNetworkFbo(this.groupsFbosData, fbo.groupId).toString());                });
        }

        var isSingleSourceFbo = await this.groupsService.isGroupFboSingleSource(fbo.icao).toPromise();

        this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.isSingleSourceFbo,isSingleSourceFbo.toString());

        this.sharedService.emitChange(fboChangedEvent);
        this.sharedService.emitChange(accountTypeChangedEvent);
        this.router.navigate(['/default-layout/dashboard-fbo-updated/']);
    }
}
