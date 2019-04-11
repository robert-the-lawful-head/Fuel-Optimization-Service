/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { LandingSiteOverviewComponent } from './landing-site-overview.component';

let component: LandingSiteOverviewComponent;
let fixture: ComponentFixture<LandingSiteOverviewComponent>;

describe('landing-site-overview component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ LandingSiteOverviewComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(LandingSiteOverviewComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});