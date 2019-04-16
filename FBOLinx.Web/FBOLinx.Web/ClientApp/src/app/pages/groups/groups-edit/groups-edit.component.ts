import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { GroupsService } from '../../../services/groups.service';
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Groups',
        link: '#/default-layout/groups'
    },
    {
        title: 'Edit Group',
        link: ''
    }
];

@Component({
    selector: 'app-groups-edit',
    templateUrl: './groups-edit.component.html',
    styleUrls: ['./groups-edit.component.scss']
})
/** groups-edit component*/
export class GroupsEditComponent implements OnInit {

    @Output() saveClicked = new EventEmitter<any>();
    @Output() cancelClicked = new EventEmitter<any>();
    @Input() groupInfo: any;

    //Public Members
    public pageTitle: string = 'Edit FBO';
    public breadcrumb: any[] = BREADCRUMBS;
    public currentContact: any;
    public contactsData: any;

    //Private Members
    private selectedContactRecord: any;
    private _RequiresRouting: boolean = false;

    /** groups-edit ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private groupsService: GroupsService,
        private sharedService: SharedService    ) {

    }

    ngOnInit() {
        let id = this.route.snapshot.paramMap.get('id');
        if (!id)
            this._RequiresRouting = false;
        else {
            this._RequiresRouting = true;
            this.groupsService.get({ oid: id }).subscribe((data: any) => {
                this.groupInfo = data;
            });
        }
    }

    //Public Methods
    public saveEdit() {
        this.groupsService.update(this.groupInfo).subscribe(() => {
        });
        this.saveClicked.emit(this.groupInfo);
    }

    public cancelEdit() {
        if (this._RequiresRouting)
            this.router.navigate(['/default-layout/groups/']);
        else
            this.cancelClicked.emit();
    }
}
