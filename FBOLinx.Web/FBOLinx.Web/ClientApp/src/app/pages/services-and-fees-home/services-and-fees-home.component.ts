import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-services-and-fees-home',
  templateUrl: './services-and-fees-home.component.html',
  styleUrls: ['./services-and-fees-home.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ServicesAndFeesHomeComponent implements OnInit {
    breadcrumb = [
        {
            link: '/default-layout',
            title: 'Main',
        },
        {
            link: '/default-layout/services-and-fees',
            title: 'Services & Fees Admin',
        },
    ];
    ;

    constructor() { }

    ngOnInit() {
    }

}
