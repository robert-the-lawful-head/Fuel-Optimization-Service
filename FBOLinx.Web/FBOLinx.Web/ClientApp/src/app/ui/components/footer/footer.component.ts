import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import * as moment from 'moment';
import { PolicyAndAgreementDocuments } from 'src/app/models/PolicyAndAgreementDocuments';
import { DocumentService } from 'src/app/services/documents.service';

import { AppService } from '../../../services/app.service';

@Component({
    host: { class: 'app-footer' },
    selector: 'app-footer',
    styleUrls: ['./footer.component.scss'],
    templateUrl: './footer.component.html',
})
export class FooterComponent implements OnInit {
    @Input() isLandingPage: boolean = false;
    public version: string;
    public year: string;
    public termsOfService: PolicyAndAgreementDocuments;
    public eulaLink: string = '';

    constructor(private appService: AppService,private documentService: DocumentService) {
        this.getAppVersion();
        this.getEULALastVersion();
    }

    ngOnInit() {
        this.year = moment().format('YYYY');
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.isLandingPage && changes.isLandingPage.currentValue) {
          this.updateCssVariable();
        }
      }

      private updateCssVariable(): void {
        // Update the CSS variable value
        const host = document.querySelector('app-footer') as HTMLElement;
        if (host) {
          host.style.setProperty('--custom-position', 'relative');
        }
      }

    private getAppVersion() {
        this.appService.getVersion().subscribe((data: any) => {
            this.version = data.version;
        });
    }

    private getEULALastVersion() {
        this.documentService.getLastEulaVersion().subscribe((data: any) => {
            this.eulaLink = data?.document;
        });
    }
    public hasEulaLink(): boolean {
        return this.eulaLink !== '';
    }
}
