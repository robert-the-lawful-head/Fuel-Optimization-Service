import { Component, Input, OnInit } from '@angular/core';
import { MatDialog, } from '@angular/material/dialog';

// Services
import { UserService } from '../../../services/user.service';

@Component({
    selector: 'app-users-home',
    templateUrl: './users-home.component.html',
    styleUrls: ['./users-home.component.scss'],
})
export class UsersHomeComponent implements OnInit {
    // Input/Output Bindings
    @Input() fboInfo: any;
    @Input() groupInfo: any;

    // Public Members
    public usersData: Array<any>;
    public currentUser: any;

    constructor(
        public newUserDialog: MatDialog,
        private userService: UserService
    ) {
    }

    ngOnInit() {
        this.loadInitialData();
    }

    public editUserClicked(record) {
        this.userService
            .get(record)
            .subscribe((data: any) => (this.currentUser = data));
    }

    public deleteUserClicked() {
        this.loadInitialData();
    }

    public saveUserEditClicked() {
        this.currentUser = null;
        this.loadInitialData();
    }

    public cancelUserEditClicked() {
        this.currentUser = null;
    }

    // Private Methods
    private loadInitialData() {
        if (this.fboInfo) {
            this.loadAllUsersForFbo();
        } else if (this.groupInfo) {
            this.loadAllUsersForGroup();
        }
    }

    private loadAllUsersForFbo() {
        this.userService
            .getForFboId(this.fboInfo.oid)
            .subscribe((data: any) => (this.usersData = data));
    }

    private loadAllUsersForGroup() {
        this.userService
            .getForGroupId(this.groupInfo.oid)
            .subscribe((data: any) => (this.usersData = data));
    }
}
