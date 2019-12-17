import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { UserService } from '../../../services/user.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-users-edit',
    templateUrl: './users-edit.component.html',
    styleUrls: ['./users-edit.component.scss']
})
/** users-edit component*/
export class UsersEditComponent implements OnInit  {

    @Output() saveClicked = new EventEmitter<any>();
    @Output() cancelClicked = new EventEmitter<any>();
    @Input() userInfo: any;
    @Input() fboInfo: any;
    @Input() groupInfo: any;

    //Public Members
    public availableroles: any[];

    //Private Members
    private _RequiresRouting: boolean = false;

    /** users-edit ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private userService: UserService,
        private sharedService: SharedService    ) {

    }

    ngOnInit() {
        if (this.userInfo) {
            this.loadAvailableRoles();
        } else {
            let id = this.route.snapshot.paramMap.get('id');
            this._RequiresRouting = true;
            this.userService.get({ oid: id }).subscribe((data: any) => {
                this.userInfo = data;
                this.loadAvailableRoles();
            });
        }
    }

    /** Public Methods */
    public saveEdit() {
        this.userService.update(this.userInfo).subscribe(() => {
            if (this.userInfo.newPassword && this.userInfo.newPassword != '') {
                this.userService.updatePassword({ user: this.userInfo, newPassword: this.userInfo.newPassword }).subscribe(
                    (newPass:
                        any) => {
                        this.userInfo.password = newPass;
                    });
            }
        });
        this.saveClicked.emit(this.userInfo);
    }

    public cancelEdit() {
        if (this._RequiresRouting)
            this.router.navigate(['/default-layout/fbos/']);
        else
            this.cancelClicked.emit();
    }

    //Private Methods
    private loadAvailableRoles() {
        this.userService.getRoles().subscribe((data: any) => {
            var supportedRoleValues = [4];
            this.availableroles = [];
            if (this.userInfo.fboId > 0) {
                supportedRoleValues = [1, 4];
            }
            else if (this.userInfo.groupId > 0) {
                supportedRoleValues = [2];
            }
            for (let role of data) {
                if (supportedRoleValues.indexOf(role.value) > -1)
                    this.availableroles.push(role);
            }
            if (this.userInfo.role > 0)
                return;
            this.userInfo.role = this.availableroles[this.availableroles.length - 1].Value;
        });
    }
}
