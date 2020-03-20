/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AnalyticsOrdersQuoteChartComponent } from './analytics-orders-quote-chart.component';

let component: AnalyticsOrdersQuoteChartComponent;
let fixture: ComponentFixture<AnalyticsOrdersQuoteChartComponent>;

describe('analysis-price-orders-chart component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AnalyticsOrdersQuoteChartComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AnalyticsOrdersQuoteChartComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});