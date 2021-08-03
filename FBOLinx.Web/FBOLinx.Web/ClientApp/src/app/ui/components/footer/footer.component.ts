import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';

import { AppService } from '../../../services/app.service';

@Component({
    host: { class: 'app-footer' },
    selector: 'app-footer',
    styleUrls: ['./footer.component.scss'],
    templateUrl: './footer.component.html',
})
export class FooterComponent implements OnInit {
    public version: string;
    public year: string;

    constructor(private appService: AppService) {
        this.getAppVersion();
    }

    ngOnInit() {
        this.year = moment().format('YYYY');
    }

    private getAppVersion() {
        this.appService.getVersion().subscribe((data: any) => {
            this.version = data.version;
        });
    }
}
