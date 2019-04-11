/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AnalysisPriceOrdersChartComponent } from './analysis-price-orders-chart.component';

let component: AnalysisPriceOrdersChartComponent;
let fixture: ComponentFixture<AnalysisPriceOrdersChartComponent>;

describe('analysis-price-orders-chart component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AnalysisPriceOrdersChartComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AnalysisPriceOrdersChartComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});