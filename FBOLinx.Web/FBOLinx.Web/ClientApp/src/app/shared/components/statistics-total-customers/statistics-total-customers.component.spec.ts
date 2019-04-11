/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { StatisticsTotalCustomersComponent } from './statistics-total-customers.component';

let component: StatisticsTotalCustomersComponent;
let fixture: ComponentFixture<StatisticsTotalCustomersComponent>;

describe('statisticsTotalCustomers component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ StatisticsTotalCustomersComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(StatisticsTotalCustomersComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});