/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { LandingSiteContactComponent } from './landing-site-contact.component';

let component: LandingSiteContactComponent;
let fixture: ComponentFixture<LandingSiteContactComponent>;

describe('landing-site-contact component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ LandingSiteContactComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(LandingSiteContactComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});