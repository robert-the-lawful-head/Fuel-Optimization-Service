import { Component, Input, OnInit } from '@angular/core';

import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-vertical-navbar',
    styleUrls: ['vertical-navbar.component.scss'],
    templateUrl: 'vertical-navbar.component.html',
})
export class VerticalNavbarComponent {
    @Input() isMenuVisible: boolean;
}
