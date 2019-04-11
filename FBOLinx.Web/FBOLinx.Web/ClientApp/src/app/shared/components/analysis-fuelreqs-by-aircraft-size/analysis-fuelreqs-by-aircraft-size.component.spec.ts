/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AnalysisFuelreqsByAircraftSizeComponent } from './analysis-fuelreqs-by-aircraft-size.component';

let component: AnalysisFuelreqsByAircraftSizeComponent;
let fixture: ComponentFixture<AnalysisFuelreqsByAircraftSizeComponent>;

describe('analysis-fuelreqs-by-aircraft-size component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AnalysisFuelreqsByAircraftSizeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AnalysisFuelreqsByAircraftSizeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});