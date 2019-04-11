/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { StatisticsOrdersByLocationComponent } from './statistics-orders-by-location.component';

let component: StatisticsOrdersByLocationComponent;
let fixture: ComponentFixture<StatisticsOrdersByLocationComponent>;

describe('statistics-orders-by-location component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ StatisticsOrdersByLocationComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(StatisticsOrdersByLocationComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});