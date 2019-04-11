import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { GroupsService } from '../../../services/groups.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-groups-home',
    templateUrl: './groups-home.component.html',
    styleUrls: ['./groups-home.component.scss']
})
/** groups-home component*/
export class GroupsHomeComponent implements OnInit {

    //Public Members
    public groupsData: any[];
    public currentGroup: any;

    /** groups-home ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private groupsService: GroupsService,
        private sharedService: SharedService    ) {

    }

    ngOnInit(): void {
        this.groupsService.getAllGroups().subscribe((data: any) => this.groupsData = data);
    }

    /** Public Methods */
    public editGroupClicked = function (record) {
        if (this.sharedService.currentUser.role == 3)
            this.sharedService.currentUser.groupId = record.oid;
        this.router.navigate(['/default-layout/groups/' + record.oid]);
    }

    public deleteFboClicked = function (record) {
    };

    public saveGroupEditClicked = function () {
        if (this.sharedService.currentUser.role == 3)
            this.sharedService.currentUser.groupId = 0;
        this.currentGroup = null;
    }

    public cancelGroupEditClicked = function () {
        if (this.sharedService.currentUser.role == 3)
            this.sharedService.currentUser.groupId = 0;
        this.currentGroup = null;
    }
}
