/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PricingTemplatesHomeComponent } from './pricing-templates-home.component';

let component: PricingTemplatesHomeComponent;
let fixture: ComponentFixture<PricingTemplatesHomeComponent>;

describe('pricing-templates-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PricingTemplatesHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PricingTemplatesHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});