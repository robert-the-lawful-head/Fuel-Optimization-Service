/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { LandingSiteHomeComponent } from './landing-site-home.component';

let component: LandingSiteHomeComponent;
let fixture: ComponentFixture<LandingSiteHomeComponent>;

describe('landing-site-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ LandingSiteHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(LandingSiteHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});