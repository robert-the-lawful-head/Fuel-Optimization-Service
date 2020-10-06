import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

// Services
import { GroupsService } from '../../../services/groups.service';
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
  {
    title: 'Main',
    link: '/default-layout',
  },
  {
    title: 'Groups',
    link: '',
  },
];

@Component({
  selector: 'app-groups-home',
  templateUrl: './groups-home.component.html',
  styleUrls: ['./groups-home.component.scss'],
})
export class GroupsHomeComponent implements OnInit {
    // Members
  breadcrumb = BREADCRUMBS;
  groupsFbosData: any;
  currentGroup: any;

  constructor(
    private router: Router,
    private groupsService: GroupsService,
    private sharedService: SharedService
  ) {}

  ngOnInit(): void {
    this.loadGroupsFbos();
  }

  editGroupClicked(record) {
    if (this.sharedService.currentUser.role === 3) {
      this.sharedService.currentUser.groupId = record.oid;
    }
    this.router.navigate(['/default-layout/groups/' + record.oid]);
  }

  editFboClicked(record) {
    this.router.navigate(['/default-layout/fbos/' + record.oid]);
  }

  deleteFboClicked() {}

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
