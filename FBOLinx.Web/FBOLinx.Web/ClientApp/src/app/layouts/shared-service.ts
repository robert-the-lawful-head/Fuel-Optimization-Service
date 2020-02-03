import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { AuthenticationService } from '../services/authentication.service'
import { User } from '../models/User';

//Components
import * as moment from 'moment';

export interface ActiveUser {
    fboId: number;
    groupId: number;
    icao: string;
}

export class DashboardSettings {
    filterStartDate: Date;
    filterEndDate: Date;
}

@Injectable()
export class SharedService {
  //Public Members
    currentUser: User;
    dashboardSettings: DashboardSettings = new DashboardSettings();

  // Observable string sources
  private emitChangeSource = new Subject();

    constructor(private authenticationService: AuthenticationService) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
        var storedDashboardSettings = localStorage.getItem('dashboardSetings');
        if (!storedDashboardSettings) {
            this.dashboardSettings.filterStartDate = new Date(moment().add(-6, 'M').format('MM/DD/YYYY'));
            this.dashboardSettings.filterEndDate = new Date(moment().format('MM/DD/YYYY'));
        } else {
            this.dashboardSettings = JSON.parse(storedDashboardSettings);
        }
    }

  // Observable string streams
  changeEmitted$ = this.emitChangeSource.asObservable();

  // Service message commands
  emitChange(change: string) {
    this.emitChangeSource.next(change);
  }
}
