import { Component, OnInit } from '@angular/core';
import { AppService } from '../../../services/app.service';
import * as moment from 'moment';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: [ './footer.component.scss' ],
    host: { class: 'app-footer' },
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
