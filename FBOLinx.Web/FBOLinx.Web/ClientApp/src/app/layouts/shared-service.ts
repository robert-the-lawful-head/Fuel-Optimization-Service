import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { BehaviorSubject, Subject } from 'rxjs';

import { User } from '../models/User';
import { AuthenticationService } from '../services/authentication.service';

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
    dashboardSettings: DashboardSettings = new DashboardSettings();

    priceTemplateMessageSource = new BehaviorSubject(
        'Update Pricing Template'
    );
    currentMessage = this.priceTemplateMessageSource.asObservable();

    priceUpdateMessage = new BehaviorSubject('Enable button');
    priceMessage = this.priceUpdateMessage.asObservable();

    // Observable string sources
    titleChangeSource = new Subject();
    emitChangeSource = new Subject();
    valueChangeSource = new Subject();

    // Observable string streams
    titleChanged$ = this.titleChangeSource.asObservable();
    changeEmitted$ = this.emitChangeSource.asObservable();
    valueChanged$ = this.valueChangeSource.asObservable();

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

    // Private members
    private _currentUser: User;

    // Public Members
    get currentUser(): User {
        if (!this._currentUser.fboId && localStorage.getItem('fboId')) {
            this._currentUser.fboId = Number(localStorage.getItem('fboId'));
        }
        if (!this._currentUser.managerGroupId && localStorage.getItem('managerGroupId')) {
            this._currentUser.managerGroupId = Number(localStorage.getItem('managerGroupId'));
        }
        if (!this._currentUser.impersonatedRole && localStorage.getItem('impersonatedrole')) {
            this._currentUser.impersonatedRole = Number(localStorage.getItem('impersonatedrole'));
        }
        const sessionGroupId = localStorage.getItem('groupId');
        if (sessionGroupId && (!this._currentUser.groupId || this._currentUser.groupId !== Number(sessionGroupId))) {
            this._currentUser.groupId = Number(sessionGroupId);
        }
        if (!this._currentUser.groupId && localStorage.getItem('groupId')) {
            this._currentUser.groupId = Number(localStorage.getItem('groupId'));
        }
        if (!this._currentUser.conductorFbo && localStorage.getItem('conductorFbo')) {
            this._currentUser.conductorFbo = Boolean(localStorage.getItem('conductorFbo'));
        }
        return this._currentUser;
    }

    set currentUser(user: User) {
        this._currentUser = user;
    }

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
