import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { groupGridSet } from 'src/app/store/actions';

import { SharedService } from '../../../layouts/shared-service';
import {
    deleteGroupEvent
} from '../../../models/sharedEvents';

// Services
import { GroupsService } from '../../../services/groups.service';
import { State } from '../../../store/reducers';
import { GroupGridState } from '../../../store/reducers/group';
import { getGroupGridState } from '../../../store/selectors';

@Component({
    selector: 'app-groups-home',
    styleUrls: ['./groups-home.component.scss'],
    templateUrl: './groups-home.component.html',
})
export class GroupsHomeComponent implements OnInit {
    // Members
    breadcrumb : any[] = [
        {
            link: '/default-layout',
            title: 'Main',
        },
        {
            link: '',
            title: 'Groups',
        },
    ];
    groupsFbosData: any;
    currentGroup: any;
    groupGridState: GroupGridState;
    isDeletingGroup: boolean = false;
    subscription: any;

    constructor(
        private router: Router,
        private groupsService: GroupsService,
        private sharedService: SharedService,
        private store: Store<State>
    ) {}

    ngOnInit(): void {
        this.loadGroupsFbos();
        this.store.select(getGroupGridState).subscribe((state) => {
            this.groupGridState = state;
        });

        this.subscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === deleteGroupEvent) {
                    this.deleteGroup({ isDeletingGroup: false });
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    editGroupClicked(event) {
        const { group, searchValue } = event;
        this.store.dispatch(
            groupGridSet({
                filter: searchValue,
            })
        );

        if (this.sharedService.currentUser.role === 3) {
            this.sharedService.currentUser.groupId = group.oid;
        }
        this.router.navigate(['/default-layout/groups/' + group.oid]);
    }

    editFboClicked(event) {
        const { fbo, searchValue } = event;
        this.store.dispatch(
            groupGridSet({
                filter: searchValue,
            })
        );
        this.router.navigate(['/default-layout/fbos/' + fbo.oid]);
    }

    deleteFboClicked() { }

    deleteGroup(event) {
        const { isDeletingGroup } = event;
        this.isDeletingGroup = isDeletingGroup;
    }

    saveGroupEditClicked() {
        if (this.sharedService.currentUser.role === 3) {
            this.sharedService.currentUser.groupId = 0;
        }
        this.currentGroup = null;
    }

    cancelGroupEditClicked() {
        if (this.sharedService.currentUser.role === 3) {
            this.sharedService.currentUser.groupId = 0;
        }
        this.currentGroup = null;
    }

    private loadGroupsFbos() {
        this.groupsService
            .groupsAndFbos()
            .subscribe((data: any) => (this.groupsFbosData = data));
    }
}
