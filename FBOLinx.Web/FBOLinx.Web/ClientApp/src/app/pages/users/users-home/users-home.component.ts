import { Component, Input, Output, OnInit, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

// Services
import { UserService } from "../../../services/user.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-users-home",
    templateUrl: "./users-home.component.html",
    styleUrls: ["./users-home.component.scss"],
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
    ) {}

    ngOnInit() {
        this.loadInitialData();
    }

    public editUserClicked(record) {
        this.userService
            .get(record)
            .subscribe((data: any) => (this.currentUser = data));
    }

    public deleteUserClicked(record) {
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
