/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AnalysisFuelreqsTopCustomersFboComponent } from './analysis-fuelreqs-top-customers-fbo.component';

let component: AnalysisFuelreqsTopCustomersFboComponent;
let fixture: ComponentFixture<AnalysisFuelreqsTopCustomersFboComponent>;

describe('analysis-fuelreqs-top-customers-fbo component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AnalysisFuelreqsTopCustomersFboComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AnalysisFuelreqsTopCustomersFboComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});