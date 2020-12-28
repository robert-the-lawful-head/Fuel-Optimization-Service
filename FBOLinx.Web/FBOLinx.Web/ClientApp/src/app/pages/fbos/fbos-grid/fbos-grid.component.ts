import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import * as moment from 'moment';

// Services
import { FbosService } from '../../../services/fbos.service';
import { SharedService } from '../../../layouts/shared-service';
import { FbopricesService } from '../../../services/fboprices.service';

// Components
import { FbosDialogNewFboComponent } from '../fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
import { FbosGridNewFboDialogComponent } from '../fbos-grid-new-fbo-dialog/fbos-grid-new-fbo-dialog.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { ManageConfirmationComponent } from '../../../shared/components/manage-confirmation/manage-confirmation.component';
import { PricingExpiredNotificationGroupComponent } from '../../../shared/components/pricing-expired-notification-group/pricing-expired-notification-group.component';

import { fboChangedEvent } from '../../../models/sharedEvents';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';

@Component({
    selector: 'app-fbos-grid',
    templateUrl: './fbos-grid.component.html',
    styleUrls: ['./fbos-grid.component.scss'],
})
export class FbosGridComponent implements OnInit {
    // Input/Output Bindings
    @Output() editFboClicked = new EventEmitter<any>();
    @Input() fbosData: Array<any>;
    @Input() groupInfo: any;
    @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
    @ViewChild(MatSort, {static: true}) sort: MatSort;

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
        private notification: MatDialog
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.canManageFbo = this.sharedService.currentUser.role === 3 || this.sharedService.currentUser.role === 2;

        if (this.sharedService.currentUser.role === 3) {
            this.displayedColumns = [
                'icao',
                'fbo',
                'active',
                'manage',
                'delete',
            ];
        } else {
            this.displayedColumns = [
                'icao',
                'fbo',
                'active',
                'manage',
            ];
        }
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

            if (remindMeLaterFlag &&
                (moment(new Date(moment().format('L'))) !== moment(new Date(remindMeLaterFlag)))
            ) {
                return;
            }

            this.checkExistingPrices();
        }

        if (localStorage.getItem('pageIndexFbos')) {
            this.paginator.pageIndex = localStorage.getItem('pageIndexFbos') as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        if (sessionStorage.getItem('pageSizeFbos')) {
            this.pageSizeFbos = sessionStorage.getItem('pageSizeFbos') as any;
        } else {
            this.pageSizeFbos = 25;
        }

        if (sessionStorage.getItem('tableSortValueFbos')) {
            this.tableSortFbos = sessionStorage.getItem('tableSortValueFbos') as any;
        }

        if (sessionStorage.getItem('tableSortValueDirectionFbos')) {
            this.tableSortOrderFbos = sessionStorage.getItem(
                'tableSortValueDirectionFbos'
            ) as any;
        }

        if (sessionStorage.getItem('searchValueFbos')) {
            this.searchValue = sessionStorage.getItem('searchValueFbos').trim().toLowerCase();
            this.fbosDataSource.filter = sessionStorage.getItem('searchValueFbos').trim().toLowerCase();
        }
    }

    public deleteRecord(record) {
        const dialogRef = this.deleteFboDialog.open(
            DeleteConfirmationComponent,
            {
                data: {item: record, description: 'FBO'},
                autoFocus: false,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.isDeleting = true;
            this.fboService.remove(record).subscribe(() => {
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
            }, () => {
                this.isDeleting = false;
            });
        });
    }

    public editRecord(record, $event) {
        if (
            $event !== null &&
            ($event.srcElement.nodeName.toLowerCase() === 'button' ||
                $event.srcElement.nodeName.toLowerCase() === 'select' ||
                ($event.srcElement.nodeName.toLowerCase() === 'input' &&
                    $event.srcElement.getAttribute('type') === 'checkbox'))
        ) {
            $event.stopPropagation();
            return;
        }
        const clonedRecord = Object.assign({}, record);
        this.editFboClicked.emit(clonedRecord);
    }

    public newRecord() {
        if (this.groupInfo) {
            const dialogRef = this.newFboDialog.open(FbosDialogNewFboComponent, {
                width: '450px',
                data: {
                    groupId: this.groupInfo.oid,
                    initialSetupPhase: true
                },
            });

            dialogRef.afterClosed().subscribe((result) => {
                if (result) {
                    this.fbosData.push(result);

                    this.refreshTable();
                    this.snackBar.open(result.fbo + ' is created', '', {
                        duration: 3000,
                        panelClass: ['blue-snackbar'],
                    });
                    sessionStorage.setItem('isNewFbo', 'yes');
                    this.editRecord(result, null);
                }
            });
        } else {
            const dialogRef = this.newFboDialog.open(FbosGridNewFboDialogComponent, {
                width: '450px',
                data: {},
            });

            dialogRef.afterClosed().subscribe((result) => {
                if (result) {
                    this.fbosData.push(result);
                    this.refreshTable();
                    this.snackBar.open(result.fbo + ' is created', '', {
                        duration: 3000,
                        panelClass: ['blue-snackbar'],
                    });
                    sessionStorage.setItem('isNewFbo', 'yes');
                    this.router.navigate(['/default-layout/fbos/' + result.oid]);
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
        sessionStorage.setItem('searchValueFbos', filterValue);
        if (!filterValue) {
            sessionStorage.removeItem('searchValueFbos');
        }
    }

    public manageFBO(fbo) {
        if (!fbo.active) {
            this.notification.open(
                NotificationComponent,
                {
                    data: {
                        title: 'Inactive fbo',
                        text: 'You can\'t manage an inactive fbo!',
                    },
                }
            );
        } else {
            const dialogRef = this.manageFboDialog.open(
                ManageConfirmationComponent,
                {
                    width: '450px',
                    data: {
                        title: 'Manage FBO?',
                        description: 'This will temporarily switch your account to a primary user for this FBO.  Would you like to continue?',
                        fboId: fbo.oid,
                    },
                    autoFocus: false,
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }
                this.sharedService.currentUser.managerGroupId = this.sharedService.currentUser.groupId;
                localStorage.setItem('managerGroupId', this.sharedService.currentUser.managerGroupId.toString());
                this.sharedService.currentUser.groupId = fbo.groupId;
                localStorage.setItem('groupId', this.sharedService.currentUser.groupId.toString());
                this.sharedService.currentUser.impersonatedRole = 1;
                localStorage.setItem('impersonatedrole', '1');
                this.sharedService.currentUser.fboId = fbo.oid;
                localStorage.setItem('fboId', this.sharedService.currentUser.fboId.toString());
                this.sharedService.emitChange(fboChangedEvent);
                this.router.navigate(['/default-layout/dashboard-fbo/']);
            });
        }
    }

    public checkExistingPrices() {

        this.fboPricesService
            .checkFboExpiredPricingGroup(this.sharedService.currentUser.groupId)
            .subscribe((data: any) => {
                if (data && data.length) {
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
                            localStorage.setItem('fboId', this.sharedService.currentUser.fboId.toString());
                            this.sharedService.emitChange(fboChangedEvent);
                            this.router.navigate(['/default-layout/dashboard-fbo/']);
                        }
                    });
                }
            });
    }

    onPageChanged(event: any) {
        localStorage.setItem('pageIndexFbos', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeFbosValue',
            this.paginator.pageSize.toString()
        );
    }

    public saveHeader(value) {
        if (value) {
            sessionStorage.setItem('tableSortValueFbos', value);
        }

        if (this.sort.direction) {
            sessionStorage.setItem(
                'tableSortValueDirectionFbos',
                this.sort.direction
            );
        }
    }
}
