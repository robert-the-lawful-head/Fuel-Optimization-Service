import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { AuthenticationService } from '../services/authentication.service'
import { User } from '../models/User';

export interface ActiveUser {
    fboId: number;
    groupId: number;
    icao: string;
}

@Injectable()
export class SharedService {
  //Public Members
    currentUser: User;

  // Observable string sources
  private emitChangeSource = new Subject();

    constructor(private authenticationService: AuthenticationService) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    }

  // Observable string streams
  changeEmitted$ = this.emitChangeSource.asObservable();

  // Service message commands
  emitChange(change: string) {
    this.emitChangeSource.next(change);
  }
}
