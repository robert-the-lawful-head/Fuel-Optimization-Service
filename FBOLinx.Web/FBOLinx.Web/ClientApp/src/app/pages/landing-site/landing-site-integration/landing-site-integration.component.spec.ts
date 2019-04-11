/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { LandingSiteIntegrationComponent } from './landing-site-integration.component';

let component: LandingSiteIntegrationComponent;
let fixture: ComponentFixture<LandingSiteIntegrationComponent>;

describe('landing-site-integration component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ LandingSiteIntegrationComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(LandingSiteIntegrationComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});