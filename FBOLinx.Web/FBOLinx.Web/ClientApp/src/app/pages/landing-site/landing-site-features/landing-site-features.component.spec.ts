/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { LandingSiteFeaturesComponent } from './landing-site-features.component';

let component: LandingSiteFeaturesComponent;
let fixture: ComponentFixture<LandingSiteFeaturesComponent>;

describe('landing-site-features component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ LandingSiteFeaturesComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(LandingSiteFeaturesComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});