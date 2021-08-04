import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

// Services
import { UserService } from '../../../services/user.service';

@Component({
    selector: 'app-users-edit',
    styleUrls: ['./users-edit.component.scss'],
    templateUrl: './users-edit.component.html',
})
export class UsersEditComponent implements OnInit {
    @Output() saveClicked = new EventEmitter<any>();
    @Output() cancelClicked = new EventEmitter<any>();
    @Input() userInfo: any;
    @Input() fboInfo: any;
    @Input() groupInfo: any;

    // Public Members
    public availableroles: any[];

    // Private Members
    private requiresRouting = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService
    ) {}

    ngOnInit() {
        if (this.userInfo) {
            this.loadAvailableRoles();
        } else {
            const id = this.route.snapshot.paramMap.get('id');
            this.requiresRouting = true;
            this.userService.get({ oid: id }).subscribe((data: any) => {
                this.userInfo = data;
                this.loadAvailableRoles();
            });
        }
    }

    public saveEdit() {
        this.userService.update(this.userInfo).subscribe(() => {
            if (this.userInfo.newPassword && this.userInfo.newPassword !== '') {
                this.userService
                    .updatePassword({
                        newPassword: this.userInfo.newPassword,
                        user: this.userInfo,
                    })
                    .subscribe((newPass: any) => {
                        this.userInfo.password = newPass;
                    });
            }
        });
        this.saveClicked.emit(this.userInfo);
    }

    public cancelEdit() {
        if (this.requiresRouting) {
            this.router.navigate(['/default-layout/fbos/']);
        } else {
            this.cancelClicked.emit();
        }
    }

    // Private Methods
    private loadAvailableRoles() {
        this.userService.getRoles().subscribe((data: any) => {
            let supportedRoleValues = [4];
            this.availableroles = [];
            if (this.userInfo.fboId > 0) {
                supportedRoleValues = [1, 4, 5];
            } else if (this.userInfo.groupId > 0) {
                supportedRoleValues = [2];
            }
            for (const role of data) {
                if (supportedRoleValues.indexOf(role.value) > -1) {
                    this.availableroles.push(role);
                }
            }
            if (this.userInfo.role > 0) {
                return;
            }
            this.userInfo.role = this.availableroles[0].Value;
        });
    }
}
