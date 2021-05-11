import { Component, EventEmitter, Input, OnInit, Output, ViewChild, } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

// Services
import { UserService } from '../../../services/user.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { UsersDialogNewUserComponent } from '../users-dialog-new-user/users-dialog-new-user.component';

@Component({
    selector: 'app-users-grid',
    templateUrl: './users-grid.component.html',
    styleUrls: [ './users-grid.component.scss' ],
})
export class UsersGridComponent implements OnInit {
    @Output() userDeleted = new EventEmitter<any>();
    @Output() newUserClicked = new EventEmitter<any>();
    @Output() editUserClicked = new EventEmitter<any>();
    @Input() usersData: Array<any>;
    @Input() fboInfo: any;
    @Input() groupInfo: any;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    public usersDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        'firstName',
        'lastName',
        'username',
        'roleDescription',
        'copyAlerts',
        'copyOrders',
        'delete',
    ];
    public resultsLength = 0;

    constructor(
        private userService: UserService,
        public newUserDialog: MatDialog,
        public deleteUserDialog: MatDialog
    ) {
    }

    ngOnInit() {
        if (!this.usersData) {
            return;
        }
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.usersDataSource = new MatTableDataSource(this.usersData);
        this.usersDataSource.sort = this.sort;
        this.usersDataSource.paginator = this.paginator;
        this.resultsLength = this.usersData.length;
    }

    // Public Methods
    public deleteRecord(record) {
        const dialogRef = this.deleteUserDialog.open(
            DeleteConfirmationComponent,
            {
                data: { item: record, description: 'user' },
                autoFocus: false,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            const deleteIndex = this.usersData.indexOf(record);
            this.userService.remove(record).subscribe(() => {
                const userArray = this.usersDataSource.data;
                userArray.splice(deleteIndex, 1);
                this.usersDataSource.data = userArray;
            });
            this.userDeleted.emit(record);
        });
    }

    public UpdateCopyAlertsValue(value) {
        if (value.copyAlerts) {
            value.copyAlerts = !value.copyAlerts;
        } else {
            value.copyAlerts = true;
        }

        if (this.fboInfo == null)
            value.GroupId = this.groupInfo.oid;
        else
            value.GroupId = this.fboInfo.groupId;
        this.userService.update(value).subscribe((data: any) => {
        });
    }

    public UpdateCopyOrdersValue(value) {
        if (value.copyOrders) {
            value.copyOrders = !value.copyOrders;
        } else {
            value.copyOrders = true;
        }

        if (this.fboInfo == null)
            value.GroupId = this.groupInfo.oid;
        else
            value.GroupId = this.fboInfo.groupId;
        this.userService.update(value).subscribe((data: any) => {
        });
    }

    public editRecord(record, $event) {
        if ($event.target) {
            if ($event.target.className.indexOf('mat-slide-toggle') > -1) {
                $event.stopPropagation();
                return false;
            } else {
                const clonedRecord = Object.assign({}, record);
                this.editUserClicked.emit(clonedRecord);
            }
        } else {
            const clonedRecord = Object.assign({}, record);
            this.editUserClicked.emit(clonedRecord);
        }
    }

    public newRecord() {
        const newUser = { oid: 0, fboId: 0, groupId: 0, copyAlerts: true, copyOrders: true };
        if (this.fboInfo) {
            newUser.fboId = this.fboInfo.oid;
            newUser.groupId = this.fboInfo.groupId;
        } else if (this.groupInfo) {
            newUser.groupId = this.groupInfo.oid;
        }
        const dialogRef = this.newUserDialog.open(UsersDialogNewUserComponent, {
            width: '450px',
            data: newUser,
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            if (this.fboInfo) {
                result.active = this.fboInfo.active;
            }

            this.userService.add(result).subscribe((data: any) => {
                const savedUser = data;
                const userArray = this.usersDataSource.data;
                if (result.newPassword && result.newPassword !== '') {
                    this.userService
                        .updatePassword({
                            user: data,
                            newPassword: result.newPassword,
                        })
                        .subscribe(
                            (newPassData: any) => {
                                savedUser.password = newPassData.password;
                                userArray.push(savedUser);
                                this.usersDataSource.data = userArray;
                            },
                            (err: any) => {
                                console.log(err);
                            }
                        );
                } else {
                    userArray.push(newUser);
                    this.usersDataSource.data = userArray;
                }
            });
        });
        this.newUserClicked.emit();
    }

    public applyFilter(filterValue: string) {
        this.usersDataSource.filter = filterValue.trim().toLowerCase();
    }
}
