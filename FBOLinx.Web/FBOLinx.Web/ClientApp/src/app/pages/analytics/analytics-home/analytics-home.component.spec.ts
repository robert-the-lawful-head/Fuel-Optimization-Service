/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AnalyticsHomeComponent } from './analytics-home.component';

let component: AnalyticsHomeComponent;
let fixture: ComponentFixture<AnalyticsHomeComponent>;

describe('ramp-fees-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AnalyticsHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AnalyticsHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});