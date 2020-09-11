import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";

import { State } from "../../../store/reducers";
import { breadcrumbSet } from "../../../store/actions";
// Services
import { GroupsService } from "../../../services/groups.service";
import { SharedService } from "../../../layouts/shared-service";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "/default-layout",
    },
    {
        title: "Groups",
        link: "",
    },
];

@Component({
    selector: "app-groups-home",
    templateUrl: "./groups-home.component.html",
    styleUrls: ["./groups-home.component.scss"],
})
export class GroupsHomeComponent implements OnInit {
    // Public Members
    public groupsData: any[];
    public currentGroup: any;

    constructor(
        private store: Store<State>,
        private router: Router,
        private groupsService: GroupsService,
        private sharedService: SharedService
    ) {
        this.store.dispatch(breadcrumbSet({ breadcrumbs: BREADCRUMBS }));
    }

    ngOnInit(): void {
        this.groupsService
            .getAllGroups()
            .subscribe((data: any) => (this.groupsData = data));
    }

    public editGroupClicked(record) {
        if (this.sharedService.currentUser.role === 3) {
            this.sharedService.currentUser.groupId = record.oid;
        }
        this.router.navigate(["/default-layout/groups/" + record.oid]);
    }

    public deleteFboClicked() {}

    public saveGroupEditClicked() {
        if (this.sharedService.currentUser.role === 3) {
            this.sharedService.currentUser.groupId = 0;
        }
        this.currentGroup = null;
    }

    public cancelGroupEditClicked() {
        if (this.sharedService.currentUser.role === 3) {
            this.sharedService.currentUser.groupId = 0;
        }
        this.currentGroup = null;
    }
}
