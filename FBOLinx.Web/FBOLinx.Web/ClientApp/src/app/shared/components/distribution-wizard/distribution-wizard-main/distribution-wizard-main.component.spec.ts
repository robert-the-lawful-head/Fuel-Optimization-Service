/// <reference path="../../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { DistributionWizardMainComponent } from './distribution-wizard-main.component';

let component: DistributionWizardMainComponent;
let fixture: ComponentFixture<DistributionWizardMainComponent>;

describe('distribution-wizard-main component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ DistributionWizardMainComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DistributionWizardMainComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});