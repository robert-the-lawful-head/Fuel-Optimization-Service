/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PricingTemplatesDialogNewTemplateComponent } from './pricing-templates-dialog-new-template.component';

let component: PricingTemplatesDialogNewTemplateComponent;
let fixture: ComponentFixture<PricingTemplatesDialogNewTemplateComponent>;

describe('pricing-templates-dialog-new-template component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PricingTemplatesDialogNewTemplateComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PricingTemplatesDialogNewTemplateComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});