/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PricingTemplatesGridComponent } from './pricing-templates-grid.component';

let component: PricingTemplatesGridComponent;
let fixture: ComponentFixture<PricingTemplatesGridComponent>;

describe('pricing-templates-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PricingTemplatesGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PricingTemplatesGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});