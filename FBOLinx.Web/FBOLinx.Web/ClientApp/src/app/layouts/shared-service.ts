import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { BehaviorSubject, Subject } from 'rxjs';

import { User } from '../models/User';
import { AuthenticationService } from '../services/authentication.service';
import { localStorageAccessConstant } from '../constants/LocalStorageAccessConstant';
import { UserRole } from '../enums/user-role';

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

    priceTemplateMessageSource = new BehaviorSubject('Update Pricing Template');
    currentMessage = this.priceTemplateMessageSource.asObservable();

    priceUpdateMessage = new BehaviorSubject('Enable button');
    priceMessage = this.priceUpdateMessage.asObservable();

    //for Log History
    updatedHistory = new BehaviorSubject<boolean>(false);

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
        const storedDashboardSettings =
            localStorage.getItem('dashboardSetings');
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
    private _title: string;
    get title(): string {
        return this._title;
    }
    set title(title: string) {
        this._title = title;
    }
    // Public Members
    get currentUser(): User {
        // const error = new Error();
        // const stackTrace = error.stack;
        // if(stackTrace.includes('currentUser')) return;


        // console.log("🚀 ~ file: shared-service.ts:72 ~ SharedService ~ getcurrentUser ~ this._currentUser == null:", this._currentUser)

        // for (const key in localStorageAccessConstant) {
        //     if (!this._currentUser[key] && localStorage.getItem(key)) {
        //         this._currentUser[key] = (this._currentUser[key] instanceof Number) ? Number(localStorage.getItem(localStorageAccessConstant.fboId)) : localStorage.getItem(localStorageAccessConstant.fboId);
        //     }
        // }

        if (this._currentUser == null) return;

        if (!this._currentUser.accountType && localStorage.getItem(localStorageAccessConstant.accountType)) {
            this._currentUser.accountType = Number(localStorage.getItem(localStorageAccessConstant.accountType));
        }
        if (!this._currentUser.icao && localStorage.getItem(localStorageAccessConstant.icao)) {
            this._currentUser.icao = localStorage.getItem(localStorageAccessConstant.icao);
        }
        if (!this._currentUser.fboId && localStorage.getItem(localStorageAccessConstant.fboId)) {
            this._currentUser.fboId = Number(localStorage.getItem(localStorageAccessConstant.fboId));
        }
        if (
            !this._currentUser.managerGroupId &&
            localStorage.getItem(localStorageAccessConstant.managerGroupId)
        ) {
            this._currentUser.managerGroupId = Number(
                localStorage.getItem(localStorageAccessConstant.managerGroupId)
            );
        }
        if (
            !this._currentUser.impersonatedRole &&
            localStorage.getItem(localStorageAccessConstant.impersonatedrole)
        ) {
            this._currentUser.impersonatedRole = Number(
                localStorage.getItem(localStorageAccessConstant.impersonatedrole)
            );
        }
        const sessionGroupId = localStorage.getItem(localStorageAccessConstant.groupId);
        if (
            sessionGroupId &&
            (!this._currentUser.groupId ||
                this._currentUser.groupId !== Number(sessionGroupId))
        ) {
            this._currentUser.groupId = Number(sessionGroupId);
        }
        if (!this._currentUser.groupId && localStorage.getItem(localStorageAccessConstant.groupId)) {
            this._currentUser.groupId = Number(localStorage.getItem(localStorageAccessConstant.groupId));
        }
        if (
            !this._currentUser.conductorFbo &&
            localStorage.getItem(localStorageAccessConstant.conductorFbo)
        ) {
            this._currentUser.conductorFbo = Boolean(
                localStorage.getItem(localStorageAccessConstant.conductorFbo)
            );
        }
        return this._currentUser;
    }
    get isCsr() {
        return this.currentUser.role === UserRole.CSR;
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

    isManagingGroup(): boolean {
        return (this.currentUser.groupId > 0 &&
            this.currentUser.managerGroupId > 0 &&
            this.currentUser.groupId != this.currentUser.managerGroupId);
    }
    setCurrentUserPropertyValue(property: string, value: string): void{
        this.currentUser[property] = value;
        localStorage.setItem(localStorageAccessConstant[property],value);
    }
    resetCurrentUserPropertyValue(property: string, resetvalue: any =  null): void{
        this.currentUser[property] = resetvalue;
        localStorage.removeItem(property);
    }
    getCurrentUserPropertyValue(property: string): string{
        return (this.currentUser[property]) ? this.currentUser[property] : localStorage.getItem(property);
    }
}
