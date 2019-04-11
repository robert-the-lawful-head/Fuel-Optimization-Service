/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { StatisticsTotalAircraftComponent } from './statistics-total-aircraft.component';

let component: StatisticsTotalAircraftComponent;
let fixture: ComponentFixture<StatisticsTotalAircraftComponent>;

describe('statistics-total-aircraft component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ StatisticsTotalAircraftComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(StatisticsTotalAircraftComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});