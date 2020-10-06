import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import * as _ from 'lodash';

// Services
import { FbosService } from '../../../services/fbos.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
  {
    title: 'Main',
    link: '/default-layout',
  },
  {
    title: 'FBOs',
    link: '',
  },
];

@Component({
  selector: 'app-fbos-home',
  templateUrl: './fbos-home.component.html',
  styleUrls: ['./fbos-home.component.scss'],
})
export class FbosHomeComponent implements OnInit {
  @Input() groupInfo: any;
  @Input() embed: boolean;

  // Public Members
  public breadcrumb: any[];
  public fbosData: Array < any > ;
  public currentFbo: any;
  public currentFboAirport: any;

  constructor(
    private router: Router,
    private fboService: FbosService,
    private fboAirportsService: FboairportsService,
    private sharedService: SharedService
  ) {
    this.currentFbo = null;
    this.currentFboAirport = null;
  }

  ngOnInit() {
    this.loadInitialData();

    if (!this.embed) {
      this.breadcrumb = BREADCRUMBS;
    }
  }

  public editFboClicked(record) {
    if (!this.groupInfo) {
      this.router.navigate(['/default-layout/fbos/' + record.oid]);
    } else {
      this.fboService
        .get(record)
        .subscribe((data: any) => (this.currentFbo = data));
      this.fboAirportsService
        .getForFbo(record)
        .subscribe((data: any) => (this.currentFboAirport = data));
    }
  }

  public saveFboEditClicked() {
    this.currentFboAirport = null;
    this.currentFbo = null;
    this.fbosData = null;
    this.loadInitialData();
  }

  public cancelFboEditClicked() {
    this.currentFbo = null;
  }

  // Private Methods
  private loadInitialData() {
    if (!this.groupInfo && this.sharedService.currentUser.impersonatedRole !== 2) {
      this.loadAllFbos();
    } else {
      this.loadAllFbosForGroup();
    }
  }

  private loadAllFbos() {
    this.fboService
      .getAllFbos()
      .subscribe((data: any) => (this.fbosData = data));
  }

  private loadAllFbosForGroup() {
    if (!this.groupInfo) {
      this.fboService
        .getForGroup(this.sharedService.currentUser.groupId)
        .subscribe((data: any) => (this.fbosData = data));
    } else {
      this.fboService
        .getForGroup(this.groupInfo.oid)
        .subscribe((data: any) => (this.fbosData = data));
    }
  }
}
