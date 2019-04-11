import { Component, Input, Output, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { UserService } from '../../../services/user.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { UsersDialogNewUserComponent } from '../users-dialog-new-user/users-dialog-new-user.component';

@Component({
    selector: 'app-users-home',
    templateUrl: './users-home.component.html',
    styleUrls: ['./users-home.component.scss']
})
/** users-home component*/
export class UsersHomeComponent implements OnInit {

    //Input/Output Bindings
    @Input() fboInfo: any;
    @Input() groupInfo: any;

    //Public Members
    public usersData: Array<any>;
    public currentUser: any;

    /** users-home ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        public newUserDialog: MatDialog,
        private userService: UserService,
        private sharedService: SharedService) {

        
    }

    ngOnInit() {
        this.loadInitialData();
    }

    /** Public Methods */
    public editUserClicked (record) {
        this.userService.get(record).subscribe((data: any) => this.currentUser = data);
    }

    public deleteUserClicked (record) {
        this.loadInitialData();
    };

    public saveUserEditClicked () {
        this.currentUser = null;
    }

    public cancelFboEditClicked () {
        this.currentUser = null;
    }

    //Private Methods
    private loadInitialData() {
        if (this.fboInfo)
            this.loadAllUsersForFbo();
        else if (this.groupInfo)
            this.loadAllUsersForGroup();
    }

    private loadAllUsersForFbo() {
        this.userService.getForFboId(this.fboInfo.oid).subscribe((data: any) => this.usersData = data);
    }

    private loadAllUsersForGroup() {
        this.userService.getForGroupId(this.groupInfo.oid).subscribe((data: any) => this.usersData = data);
    }
}
