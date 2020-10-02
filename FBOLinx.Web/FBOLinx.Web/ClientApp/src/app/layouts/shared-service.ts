import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import * as moment from 'moment';

import { AuthenticationService } from '../services/authentication.service';
import { User } from '../models/User';

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
    // Private members
    private _currentUser: User;

    constructor(private authenticationService: AuthenticationService) {
        this.authenticationService.currentUser.subscribe(
            (x) => (this.currentUser = x)
        );
        const storedDashboardSettings = localStorage.getItem(
            'dashboardSetings'
        );
        if (!storedDashboardSettings) {
            this.dashboardSettings.filterStartDate = new Date(
                moment().add(-6, 'M').format('MM/DD/YYYY')
            );
            this.dashboardSettings.filterEndDate = new Date(
                moment().format('MM/DD/YYYY')
            );
        } else {
            this.dashboardSettings = JSON.parse(storedDashboardSettings);
        }
    }
    // Public Members
    get currentUser(): User {
        if (!this._currentUser.fboId && sessionStorage.getItem('fboId')) {
            this._currentUser.fboId = Number(sessionStorage.getItem('fboId'));
      }
      if (!this._currentUser.managerGroupId && sessionStorage.getItem('managerGroupId')) {
        this._currentUser.managerGroupId = Number(sessionStorage.getItem('managerGroupId'));
      }
      if (sessionStorage.getItem('groupId')) {
        this._currentUser.groupId = Number(sessionStorage.getItem('groupId'));
      }
        return this._currentUser;
    }

    set currentUser(user: User) {
        this._currentUser = user;
    }

    dashboardSettings: DashboardSettings = new DashboardSettings();

    public priceTemplateMessageSource = new BehaviorSubject(
        'Update Pricing Template'
    );
    currentMessage = this.priceTemplateMessageSource.asObservable();

    public priceUpdateMessage = new BehaviorSubject('Enable button');
    priceMessage = this.priceUpdateMessage.asObservable();

    // Observable string sources
    public titleChangeSource = new Subject();
    public emitChangeSource = new Subject();
    public valueChangeSource = new Subject();

    // Observable string streams
    titleChanged$ = this.titleChangeSource.asObservable();
    changeEmitted$ = this.emitChangeSource.asObservable();
    valueChanged$ = this.valueChangeSource.asObservable();

    NotifyPricingTemplateComponent(message: string) {
        this.priceTemplateMessageSource.next(message);
    }

    titleChange(title: string) {
        this.titleChangeSource.next(title);
    }

    // Service message commands
    emitChange(change: string) {
        this.emitChangeSource.next(change);
    }

    valueChange(change: any) {
        this.valueChangeSource.next(change);
    }
}
