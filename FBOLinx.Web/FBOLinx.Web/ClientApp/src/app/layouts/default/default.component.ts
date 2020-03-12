import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from '../shared-service';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';

//Services
import {FbopricesService} from '../../services/fboprices.service';
import { PricingtemplatesService } from '../../services/pricingtemplates.service';
//Components
import {PricingExpiredNotificationComponent} from
    '../../shared/components/pricing-expired-notification/pricing-expired-notification.component';
import * as moment from 'moment';

@Component({
    moduleId: module.id,
    selector: 'default-layout',
    templateUrl: 'default.component.html',
    styleUrls: ['../layouts.scss'],
    providers: [SharedService]
})
export class DefaultLayoutComponent implements OnInit {
    pageTitle: any;
    boxed: boolean;
    compress: boolean;
    menuStyle: string;
    rtl: boolean;
    @Input()
    openedSidebar: boolean;
    public pricingTemplatesData: any[];

    constructor(private _sharedService: SharedService,
        private fboPricesService: FbopricesService,
        private pricingTemplatesService: PricingtemplatesService,
        public expiredPricingDialog: MatDialog) {
        this.openedSidebar = false;
        this.boxed = false;
        this.compress = false;
        this.menuStyle = 'style-3';
        this.rtl = false;

        if (this._sharedService.currentUser.fboId) {
            this.pricingTemplatesService.getByFbo(this._sharedService.currentUser.fboId).subscribe((data: any) => this.pricingTemplatesData = data);
            _sharedService.changeEmitted$.subscribe(
                title => {
                    this.pageTitle = title;
                }
            );
        }
        
    }

    ngOnInit() {
        this.checkCurrentPrices();
        //this.callTemporaryAddOn();
    }

    getClasses() {
        let menu: string = (this.menuStyle);

        return {
            ['menu-' + menu]: menu,
            'boxed': this.boxed,
            'compress-vertical-navbar': this.compress,
            'open-sidebar': this.openedSidebar,
            'rtl': this.rtl
        };
    }

    sidebarState() {
        this.openedSidebar = !this.openedSidebar;
    }

    //Private Methods

    private checkCurrentPrices() {
        var remindMeLaterFlag = localStorage.getItem('pricingExpiredNotification');
        var noThanksFlag = sessionStorage.getItem('pricingExpiredNotification');
        if (noThanksFlag) {
            return;
        }
        if (remindMeLaterFlag && moment(moment().format('L')).isAfter(moment(remindMeLaterFlag))) {
            return;
        }
        this.fboPricesService.checkFboExpiredPricing(this._sharedService.currentUser.fboId).subscribe(
            (data: any) => {
                if(data){
                    if (data.price > 0) {
                        return;
                    }
                }
                
                const dialogRef = this.expiredPricingDialog.open(PricingExpiredNotificationComponent,
                    {
                        data: {}
                    });
                dialogRef.afterClosed().subscribe(result => {});
            });
    }
}
