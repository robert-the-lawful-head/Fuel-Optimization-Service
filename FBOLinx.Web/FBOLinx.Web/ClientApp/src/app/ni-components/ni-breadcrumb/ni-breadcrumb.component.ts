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
    private defaultItem : Item  = { title: 'None', icon: '', link:'' };
    private _sectionTitle: Item  = this.defaultItem;
    isDashboard: boolean = false;

    private whitelist = [
        '/default-layout/dashboard',
        '/default-layout/dashboard-fbo',
        '/default-layout/dashboard-fbo-updated',
        '/default-layout/dashboard-csr'
    ];

    private breadcrumbs: any[] =  [
        {
            link: '/default-layout',
            title: 'Main',
        },
        {
            link: '/default-layout/analytics',
            title: 'Analytics',
        },
        {
            link: '/default-layout/customers',
            title: 'Customer Manager',
        },
        {
            link: '/default-layout/email-templates',
            title: 'Email Templates',
        },
        {
            link: '/default-layout/fuelreqs',
            title: 'Fuel & Service Orders',
        },
        {
            link: '/default-layout/customers',
            title: 'Customer Manager',
        },
        {
            link: '/default-layout/groups',
            title: 'Groups',
        },
        {
            link: '/default-layout/fuelreqs',
            title: 'Fuel Orders',
        },
        {
            link: '/default-layout/pricing-templates',
            title: 'ITP Margin Templates',
        },
        {
            link: '/default-layout/pricing-templates',
            title: 'ITP Margin Templates',
        },
        {
            link: '/default-layout/flight-watch',
            title: 'Flight Watch',
        },
        {
            link: '/default-layout/services-and-fees',
            title: 'Services & Fees Admin',
        },
        {
            link: '/default-layout/ramp-fees',
            title: 'Ramp Fees',
        },
        {
            link: '/default-layout/email-templates',
            title: 'Email Templates',
        },
        {
            link: '/default-layout/customers',
            title: 'Customers',
        },
        {
            link: '/default-layout/fbos',
            title: 'FBOs',
        },
        {
            link: '/default-layout/groups',
            title: 'Groups',
        },
        {
            link: '/default-layout/fbo-geofencing',
            title: 'FBO Geofencing',
        },
        {
            link: '/default-layout/dashboard-fbo-updated',
            title: 'Dashboard',
        },
        {
            link: '/default-layout/dashboard-fbo',
            title: 'Dashboard',
        },
        {
            link: '/default-layout/dashboard',
            title: 'Dashboard',
        },
        {
            link: '/default-layout/dashboard-csr',
            title: 'CSR Dashboard',
        }
    ];

    constructor(private router: Router) {
        router.events.subscribe((val) => {
            if(val instanceof NavigationEnd){
                if (this.whitelist.includes(val.url)) {
                    this.isDashboard = true;
                }
                else {
                    this.isDashboard = false;
                }

                var breadcrumb = this.breadcrumbs.find((item) => item.link === val.url);

                if(breadcrumb)
                    this._sectionTitle = breadcrumb;
                else
                    this._sectionTitle = this.defaultItem;
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
