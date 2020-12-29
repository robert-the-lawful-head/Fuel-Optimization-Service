import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-vertical-navbar',
    templateUrl: 'vertical-navbar.component.html',
    styleUrls: [ 'vertical-navbar.component.scss' ]
})
export class VerticalNavbarComponent implements OnInit {
    constructor(
        private sharedService: SharedService,
    ) {
    }

    menuVisible() {
        return this.sharedService.currentUser.role !== 3 || this.sharedService.currentUser.impersonatedRole;
    }

    ngOnInit() {
    }
}
