import { Component, OnInit } from '@angular/core';

@Component({
    host: { class: 'app-logo' },
    selector: 'app-logo',
    styleUrls: [ 'logo.component.scss' ],
    templateUrl: 'logo.component.html',
})
export class LogoComponent implements OnInit {
    constructor() {
    }

    ngOnInit() {
    }
}
