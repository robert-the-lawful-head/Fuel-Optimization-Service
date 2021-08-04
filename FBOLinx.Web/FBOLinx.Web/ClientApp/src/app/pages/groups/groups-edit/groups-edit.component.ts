import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

// Services
import { GroupsService } from '../../../services/groups.service';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/groups',
        title: 'Groups',
    },
    {
        link: '',
        title: 'Edit Group',
    },
];

@Component({
    selector: 'app-groups-edit',
    styleUrls: ['./groups-edit.component.scss'],
    templateUrl: './groups-edit.component.html',
})
export class GroupsEditComponent implements OnInit {
    @Output() cancelClicked = new EventEmitter<any>();
    @Input() groupInfo: any;

    // Public Members
    public pageTitle = 'Edit FBO';
    public breadcrumb: any[] = BREADCRUMBS;
    public currentContact: any;
    public contactsData: any;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private groupsService: GroupsService,
        private snackBar: MatSnackBar
    ) {}

    ngOnInit() {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.groupsService.get({ oid: id }).subscribe((data: any) => {
                this.groupInfo = data;
            });
        }
        if (sessionStorage.getItem('isNewFbo')) {
            sessionStorage.removeItem('isNewFbo');
        }
    }

    // Public Methods
    public saveEdit() {
        this.groupsService.update(this.groupInfo).subscribe(() => {
            this.snackBar.open('Successfully updated!', '', {
                duration: 2000,
                panelClass: ['blue-snackbar'],
            });
            this.router.navigate(['/default-layout/groups/']);
        });
    }

    public cancelEdit() {
        this.router.navigate(['/default-layout/groups/']);
    }
}
