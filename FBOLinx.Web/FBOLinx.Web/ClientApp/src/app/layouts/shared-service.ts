import { Injectable } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";

import { AuthenticationService } from "../services/authentication.service";
import { User } from "../models/User";

// Components
import * as moment from "moment";

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
    constructor(private authenticationService: AuthenticationService) {
        this.authenticationService.currentUser.subscribe(
            (x) => (this.currentUser = x)
        );
        const storedDashboardSettings = localStorage.getItem(
            "dashboardSetings"
        );
        if (!storedDashboardSettings) {
            this.dashboardSettings.filterStartDate = new Date(
                moment().add(-6, "M").format("MM/DD/YYYY")
            );
            this.dashboardSettings.filterEndDate = new Date(
                moment().format("MM/DD/YYYY")
            );
        } else {
            this.dashboardSettings = JSON.parse(storedDashboardSettings);
        }
    }
    // Public Members
    currentUser: User;
    dashboardSettings: DashboardSettings = new DashboardSettings();

    public priceTemplateMessageSource = new BehaviorSubject(
        "Update Pricing Template"
    );
    currentMessage = this.priceTemplateMessageSource.asObservable();

    public priceUpdateMessage = new BehaviorSubject("Enable button");
    priceMessage = this.priceUpdateMessage.asObservable();

    // Observable string sources
    public emitChangeSource = new Subject();
    public emitLoadedSource = new Subject();

    // Observable string streams
    changeEmitted$ = this.emitChangeSource.asObservable();

    // Observable string streams
    loadedEmitted$ = this.emitLoadedSource.asObservable();

    NotifyPricingTemplateComponent(message: string) {
        this.priceTemplateMessageSource.next(message);
    }

    NotifyPricingSavedComponent(message: string) {
        this.priceUpdateMessage.next(message);
    }

    // Service message commands
    emitChange(change: string) {
        this.emitChangeSource.next(change);
    }

    // Service message commands
    loadedChange(change: string) {
        this.emitLoadedSource.next(change);
    }
}
