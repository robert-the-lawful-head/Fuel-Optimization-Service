import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { UserService } from '../../../services/user.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { UsersDialogNewUserComponent } from '../users-dialog-new-user/users-dialog-new-user.component';

@Component({
    selector: 'app-users-grid',
    templateUrl: './users-grid.component.html',
    styleUrls: ['./users-grid.component.scss']
})
/** users-grid component*/
export class UsersGridComponent {

    @Output() userDeleted = new EventEmitter<any>();
    @Output() newUserClicked = new EventEmitter<any>();
    @Output() editUserClicked = new EventEmitter<any>();
    @Input() usersData: Array<any>;
    @Input() fboInfo: any;
    @Input() groupInfo: any;

    public usersDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['firstName', 'lastName', 'username', 'roleDescription', 'edit', 'delete'];
    public resultsLength: number = 0;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** users-grid ctor */
    constructor(private userService: UserService,
        public newUserDialog: MatDialog,
        public deleteUserDialog: MatDialog) {
        
    }

    ngOnInit() {
        if (!this.usersData)
            return;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.usersDataSource = new MatTableDataSource(this.usersData);
        this.usersDataSource.sort = this.sort;
        this.usersDataSource.paginator = this.paginator;
        this.resultsLength = this.usersData.length;
    }

    //Public Methods
    public deleteRecord(record) {
        const dialogRef = this.deleteUserDialog.open(DeleteConfirmationComponent, {
            data: { item: record, description: 'user' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            const deleteIndex = this.usersData.indexOf(record);
            this.userService.remove(record).subscribe(
                result => { this.usersData.splice(deleteIndex, 1); }
            );
            this.userDeleted.emit(record);
        });
    }

    public editRecord(record) {
        const clonedRecord = Object.assign({}, record);
        console.log(clonedRecord);
        this.editUserClicked.emit(clonedRecord);
    }

    public newRecord() {
        var newUser = { oid: 0, fboId: 0, groupId: 0 };
        if (this.fboInfo) {
            newUser.fboId = this.fboInfo.oid;
            newUser.groupId = this.fboInfo.groupId;
        }
        else if (this.groupInfo) {
            newUser.groupId = this.groupInfo.oid;
        }
        const dialogRef = this.newUserDialog.open(UsersDialogNewUserComponent, {
            width: '450px',
            data: newUser
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            console.log('Dialog data: ', result);
            this.userService.add(result).subscribe((data: any) => {
                result = data;
                if (result.newPassword && result.newPassword != '') {
                    this.userService.updatePassword({ user: data, newPassword: result.newPassword }).subscribe((newPass:
                        any) => {
                        result.password = newPass;
                        this.usersData.push(result);
                    });
                } else {
                    this.usersData.push(result);
                }
            });
        });
        this.newUserClicked.emit();
    }
}
