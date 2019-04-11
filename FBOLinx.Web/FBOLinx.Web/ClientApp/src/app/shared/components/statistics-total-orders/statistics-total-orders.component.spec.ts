/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { StatisticsTotalOrdersComponent } from './statistics-total-orders.component';

let component: StatisticsTotalOrdersComponent;
let fixture: ComponentFixture<StatisticsTotalOrdersComponent>;

describe('statistics-total-orders component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ StatisticsTotalOrdersComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(StatisticsTotalOrdersComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});