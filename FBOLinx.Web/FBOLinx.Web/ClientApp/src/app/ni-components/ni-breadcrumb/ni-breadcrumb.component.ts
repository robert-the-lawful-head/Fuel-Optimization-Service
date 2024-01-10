import { Component, Input, SimpleChanges } from '@angular/core';

import { Item } from './item';
import { NavigationEnd, Router } from '@angular/router';

@Component({
    selector: 'ni-breadcrumb',
    styleUrls: ['./ni-breadcrumb.component.scss'],
    templateUrl: './ni-breadcrumb.component.html',
})
export class NiBreadcrumbComponent {
    @Input() menu: Item[] = [];
    @Input() separator = '/';

    private _sectionTitle: Item  = { title: 'None', icon: '', link:'' };
    isDashboard: boolean = false;

    whitelist = [
        '/default-layout/dashboard',
        '/default-layout/dashboard-fbo',
        '/default-layout/dashboard-fbo-updated'
    ];

    constructor(private router: Router) {
        router.events.subscribe((val) => {
            if(val instanceof NavigationEnd){
                if (this.whitelist.includes(val.url)) {
                    console.log("🚀 ~ file: ni-breadcrumb.component.ts:25 ~ NiBreadcrumbComponent ~ router.events.subscribe ~ val:", val.url)
                    this.isDashboard = true;
                }
                else {
                    this.isDashboard = false;
                }
            }
        });
      }
    ngOnChanges(changes: SimpleChanges): void {
        if(changes.menu) {
            this._sectionTitle = changes.menu.currentValue.pop();
        }
    }
    get sectionTitle(): Item {
        return this._sectionTitle;

    }
    set sectionTitle(title: Item) {
        this._sectionTitle = title;
    }
}
