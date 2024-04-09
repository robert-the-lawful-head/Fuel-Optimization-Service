import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared-service';

@Component({
  selector: 'app-public-view',
  templateUrl: './public-view.component.html',
  styleUrls: ['./public-view.component.scss']
})
export class PublicViewComponent implements OnInit {
    layoutClasses: any;
    menuStyle: string;
    boxed: boolean;
    compress: boolean;
    pageTitle: string = "Lobby View";
    openedSidebar = false;
    isPublicView = true;


    constructor(private sharedService: SharedService) {
        this.boxed = false;
        this.compress = false;
        this.menuStyle = 'style-3';
    }

    ngOnInit() {
        this.layoutClasses = this.getClasses();
    }
    getClasses() {
        const menu: string = this.menuStyle;

        return {
            ['menu-' + menu]: menu,
            boxed: this.boxed,
            'compress-vertical-navbar': this.compress,
            'open-sidebar': this.openedSidebar,
            rtl: false,
        };
    }

}
