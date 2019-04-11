/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PricingTemplatesEditComponent } from './pricing-templates-edit.component';

let component: PricingTemplatesEditComponent;
let fixture: ComponentFixture<PricingTemplatesEditComponent>;

describe('pricing-templates-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PricingTemplatesEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PricingTemplatesEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});